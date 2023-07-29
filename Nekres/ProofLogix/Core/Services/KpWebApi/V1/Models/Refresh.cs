using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models
{
	public sealed class Refresh
	{
		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("min")]
		public int Minutes { get; set; }
	}
}
