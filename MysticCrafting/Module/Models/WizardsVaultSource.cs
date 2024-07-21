using Atzie.MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Models
{
	public class WizardsVaultSource : ItemSource
	{
		public Item Item { get; set; }

		public WizardsVaultSource(string uniqueId)
			: base(uniqueId)
		{
			Icon = ServiceContainer.TextureRepository.GetRefTexture("soto_icon.png");
		}
	}
}
