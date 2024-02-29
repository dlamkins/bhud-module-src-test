using Atzie.MysticCrafting.Models.Items;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Models
{
	public class ItemContainerSource : ItemSource
	{
		public MysticVaultContainer Container { get; set; }

		public MysticItem ContainerItem { get; set; }

		public ItemContainerSource(string uniqueId)
			: base(uniqueId)
		{
			Icon = ServiceContainer.TextureRepository.GetRefTexture("784330_big.png");
		}
	}
}
