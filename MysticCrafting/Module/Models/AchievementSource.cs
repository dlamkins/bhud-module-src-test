using Atzie.MysticCrafting.Models.Account;
using Atzie.MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Models
{
	public class AchievementSource : ItemSource
	{
		public Achievement Achievement { get; set; }

		public Item Item { get; set; }

		public AchievementSource(string uniqueId)
			: base(uniqueId)
		{
			Icon = ServiceContainer.TextureRepository.Textures.Achievement;
		}
	}
}
