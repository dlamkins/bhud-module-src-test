using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models
{
	public sealed class Token
	{
		public static Token Empty = new Token
		{
			Name = string.Empty
		};

		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set; }
	}
}
