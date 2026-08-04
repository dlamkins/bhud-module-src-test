using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Oberyn.AnglerAssociate.Models;

namespace Oberyn.AnglerAssociate.Services
{
	public static class BaitIcons
	{
		public static readonly Dictionary<Bait, int> AssetIds = new Dictionary<Bait, int>
		{
			{
				Bait.FishEgg,
				2594404
			},
			{
				Bait.Leech,
				2594406
			},
			{
				Bait.LightningBug,
				2594408
			},
			{
				Bait.FreshwaterMinnow,
				2594410
			},
			{
				Bait.HaijuMinnow,
				2594410
			},
			{
				Bait.RamshornSnail,
				2594412
			},
			{
				Bait.Shrimpling,
				2594414
			},
			{
				Bait.SparkflyLarva,
				2594416
			},
			{
				Bait.GlowWorm,
				2594418
			},
			{
				Bait.LavaBeetle,
				2594420
			},
			{
				Bait.Sardine,
				2594422
			},
			{
				Bait.Scorpion,
				2594424
			},
			{
				Bait.Nightcrawler,
				2594426
			},
			{
				Bait.Mackerel,
				2594639
			}
		};

		public static int? GetAssetId(Bait bait)
		{
			if (!AssetIds.TryGetValue(bait, out var id))
			{
				return null;
			}
			return id;
		}

		public static AsyncTexture2D GetTexture(Bait bait)
		{
			int? assetId = GetAssetId(bait);
			if (!assetId.HasValue)
			{
				return null;
			}
			return GameService.Content.get_DatAssetCache().GetTextureFromAssetId(assetId.Value);
		}
	}
}
