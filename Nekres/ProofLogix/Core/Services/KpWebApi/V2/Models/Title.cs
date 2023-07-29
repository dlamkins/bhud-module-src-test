using Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models
{
	public sealed class Title
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("mode")]
		[JsonConverter(typeof(StringEnumConverter))]
		public Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models.Title.TitleMode Mode { get; set; }
	}
}
