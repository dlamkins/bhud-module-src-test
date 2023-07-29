using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models
{
	public sealed class ProofBusy
	{
		[JsonProperty("busy")]
		public int Busy { get; set; }
	}
}
