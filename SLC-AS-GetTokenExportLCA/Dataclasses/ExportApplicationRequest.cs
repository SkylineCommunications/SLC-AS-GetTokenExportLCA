namespace SLCASGetTokenExportLCA.Dataclasses
{
	using Newtonsoft.Json;

	internal sealed class ExportApplicationRequest
	{
		[JsonProperty("connection")]
		public string Connection { get; set; }

		[JsonProperty("applicationID")]
		public string ApplicationId { get; set; }

		[JsonProperty("options")]
		public ExportOptions Options { get; set; }
	}

	internal sealed class ExportOptions
	{
		[JsonProperty("version")]
		public int Version { get; set; }

		[JsonProperty("isDraft")]
		public bool IsDraft { get; set; }
	}
}
