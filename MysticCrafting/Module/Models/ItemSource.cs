using Blish_HUD.Content;

namespace MysticCrafting.Module.Models
{
	public abstract class ItemSource : IItemSource
	{
		public string DisplayName { get; set; }

		public string UniqueId { get; set; }

		public virtual AsyncTexture2D Icon { get; protected set; }

		protected ItemSource(string uniqueId)
		{
			UniqueId = uniqueId;
		}
	}
}
