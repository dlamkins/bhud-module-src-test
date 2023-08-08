using Blish_HUD.Content;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models
{
	public class Resource
	{
		public static Resource Empty = new Resource
		{
			IconUrl = string.Empty,
			Name = string.Empty
		};

		[JsonProperty("icon")]
		public string IconUrl { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonIgnore]
		public ItemRarity Rarity { get; set; }

		[JsonIgnore]
		public AsyncTexture2D Icon { get; set; }
	}
}
