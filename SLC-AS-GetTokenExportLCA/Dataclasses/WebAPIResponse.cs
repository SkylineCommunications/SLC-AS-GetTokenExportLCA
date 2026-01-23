namespace SLCASGetTokenExportLCA.Dataclasses
{
	using Newtonsoft.Json;

	internal sealed class WebAPIResponse<T>
	{
		[JsonProperty("d")]
		public T Data { get; set; }
	}
}