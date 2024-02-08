using MysticCrafting.Models.Items;

namespace MysticCrafting.Module.Repositories
{
	public interface IWikiLinkRepository : IRepository
	{
		MysticWikiLink GetLink(int itemId);
	}
}
