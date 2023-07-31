using Blish_HUD;
using Blish_HUD.Content;
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

		public AsyncTexture2D Icon
		{
			get
			{
				if (string.IsNullOrWhiteSpace(IconUrl))
				{
					return ProofLogix.Instance.Resources.GetApiIcon(Id);
				}
				return GameService.Content.get_DatAssetCache().GetTextureFromAssetId(AssetUtil.GetId(IconUrl));
			}
		}
	}
}
