namespace GetTokenExportLCA.Dataclasses
{
	using System.Collections.Generic;

	using Newtonsoft.Json;

	internal sealed class ExportDashboardsRequest
	{
		[JsonProperty("connection")]
		public string Connection { get; set; }

		[JsonProperty("dashboards")]
		public List<DashboardReference> Dashboards { get; set; } = new List<DashboardReference>();

		[JsonProperty("options")]
		public ExportDashboardsOptions Options { get; set; } = new ExportDashboardsOptions();
	}

	internal sealed class DashboardReference
	{
		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("Folder")]
		public string Folder { get; set; }
	}

	internal sealed class ExportDashboardsOptions
	{
	}
}
