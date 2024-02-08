using MysticCrafting.Models.TradingPost;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Models
{
	public class TradingPostSource : ItemSource
	{
		public TradingPostPrice BuyPrice { get; set; }

		public TradingPostPrice SellPrice { get; set; }

		public TradingPostSource(string uniqueId)
			: base(uniqueId)
		{
			Icon = ServiceContainer.TextureRepository.Textures.TpIcon;
		}
	}
}
