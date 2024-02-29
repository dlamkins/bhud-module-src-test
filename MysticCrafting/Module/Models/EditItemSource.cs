using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Models
{
	public class EditItemSource : ItemSource
	{
		public EditItemSource(string uniqueId)
			: base(uniqueId)
		{
			Icon = ServiceContainer.TextureRepository.Textures.EditFeatherIcon;
		}
	}
}
