namespace SLCASGetTokenExportLCA.Dataclasses
{
	using System;
	using System.Net.Http;
	using System.Text;
	using System.Threading.Tasks;

	using Newtonsoft.Json;

	using Skyline.DataMiner.Analytics.GenericInterface;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;

	internal sealed class WebAPI
	{
		private readonly HttpClient _httpClient = new HttpClient();
		private readonly GeneralInfoEventMessage _localAgentInfo;
		private readonly string _connectionTicket;
		private readonly IEngine _engine;

		public WebAPI(IConnection connection, IEngine engine)
		{
			_engine = engine;
			_localAgentInfo = GetLocalAgentInfo(connection);
			WebAPIOrigin = GetWebAPIOrigin(_localAgentInfo);
			_connectionTicket = GetConnectionTicket(connection);

			_engine.GenerateInformation($"WebAPIOrigin: {WebAPIOrigin}");
		}

		public string WebAPIOrigin { get; }

		private static JsonSerializerSettings JsonSerializerSettings => new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.None,
		};

		public async Task<string> GetWebAPIConnectionId()
		{
			var request = new ConnectWithTicketRequest
			{
				ConnectionTicket = _connectionTicket,
				Config = new ConnectionConfig { AppName = "Get Token to Export LCA" },
			};
			var endpoint = $"{WebAPIOrigin}/api/v1/internal.asmx/ConnectWithTicket";
			var response = await SendWebAPIRequest<ConnectionInfo>(endpoint, request);

			return response.Connection;
		}

		public async Task<string> GetTokenToExportLCA(string connectionID, string lcaID, int lcaVersion)
		{
			var request = new ExportApplicationRequest
			{
				Connection = connectionID,
				ApplicationId = lcaID,
				Options = new ExportOptions
				{
					Version = lcaVersion,
					IsDraft = false,
				},
			};
			var endpoint = $"{WebAPIOrigin}/api/v1/internal.asmx/ExportApplication";
			var response = await SendWebAPIRequest<TokenInfo>(endpoint, request);

			return response.Token;
		}

		public async Task<T> SendWebAPIRequest<T>(string endpoint, object request)
		{
			var jsonRequest = JsonConvert.SerializeObject(request, JsonSerializerSettings);
			var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
			_engine.GenerateInformation($"Sending WebAPI request to \"{endpoint}\": {jsonRequest}");

			var httpResponse = await _httpClient.PostAsync(endpoint, content);

			if (!httpResponse.IsSuccessStatusCode)
			{
				var errorContent = await httpResponse.Content.ReadAsStringAsync();
				throw new GenIfException(
					$"WebAPI request to \"{endpoint}\" failed with status code {(int)httpResponse.StatusCode} ({httpResponse.StatusCode}).{Environment.NewLine}" +
					$"Reason: {httpResponse.ReasonPhrase}.{Environment.NewLine}" +
					$"Response: {errorContent}");
			}

			var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
			_engine.GenerateInformation($"Response from WebAPI request \"{endpoint}\": {jsonResponse}");

			WebAPIResponse<T> response;
			try
			{
				response = SecureNewtonsoftDeserialization.DeserializeObject<WebAPIResponse<T>>(jsonResponse, JsonSerializerSettings);
			}
			catch (JsonException)
			{
				response = new WebAPIResponse<T> { Data = default };
			}

			return response.Data;
		}

		private static string GetWebAPIOrigin(GeneralInfoEventMessage localInfo)
		{
			if (localInfo is null || !localInfo.HTTPS || string.IsNullOrWhiteSpace(localInfo.CertificateAddressName))
				return "http://localhost";

			return $"https://{localInfo.CertificateAddressName}";
		}

		private static GeneralInfoEventMessage GetLocalAgentInfo(IConnection connection)
		{
			try
			{
				var request = new GetInfoMessage(InfoType.LocalGeneralInfoMessage);
				return (GeneralInfoEventMessage)connection.HandleSingleResponseMessage(request);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Failed to retrieve local agent info.", ex);
			}
		}

		private static string GetConnectionTicket(IConnection connection)
		{
			try
			{
				var request = new RequestTicketMessage(TicketType.Authentication, Array.Empty<byte>());
				var response = (TicketResponseMessage)connection.HandleSingleResponseMessage(request);
				return response.Ticket;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Failed to retrieve connection ticket.", ex);
			}
		}
	}

	internal sealed class ConnectionInfo
	{
		public string Connection { get; set; }
	}

	internal sealed class TokenInfo
	{
		public string Token { get; set; }
	}
}