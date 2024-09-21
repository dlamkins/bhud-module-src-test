using Atzie.MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Models
{
	public class TradingPostSource : ItemSource
	{
		public Item Item { get; set; }

		public TradingPostSource(string uniqueId)
			: base(uniqueId)
		{
			Icon = ServiceContainer.TextureRepository.Textures.TpIcon;
		}
	}
}
