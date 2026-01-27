namespace GetTokenExportLCA.Dataclasses
{
	using Newtonsoft.Json;

	internal sealed class ConnectWithTicketRequest
	{
		[JsonProperty("connectionTicket")]
		public string ConnectionTicket { get; set; }

		[JsonProperty("config")]
		public ConnectionConfig Config { get; set; }
	}

	internal sealed class ConnectionConfig
	{
		public string AppName { get; set; }
	}
}