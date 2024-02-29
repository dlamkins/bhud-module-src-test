using MysticCrafting.Models.Items;

namespace MysticCrafting.Module.Repositories
{
	public interface IWikiLinkRepository
	{
		MysticWikiLink GetLink(int itemId);
	}
}
