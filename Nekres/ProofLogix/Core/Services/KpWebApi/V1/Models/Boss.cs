using Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models.Converter;
using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models
{
	[JsonConverter(typeof(BossConverter))]
	public sealed class Boss
	{
		public string Name { get; set; }

		public bool Cleared { get; set; }
	}
}
