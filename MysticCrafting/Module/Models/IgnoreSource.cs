using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Models
{
	public class IgnoreSource : ItemSource
	{
		public IgnoreSource(string uniqueId)
			: base(uniqueId)
		{
			Icon = ServiceContainer.TextureRepository.Textures.IgnoreIcon;
		}
	}
}
