using Blish_HUD.Content;

namespace MysticCrafting.Module.Models
{
	public interface IItemSource
	{
		string DisplayName { get; set; }

		string UniqueId { get; set; }

		AsyncTexture2D Icon { get; }
	}
}
