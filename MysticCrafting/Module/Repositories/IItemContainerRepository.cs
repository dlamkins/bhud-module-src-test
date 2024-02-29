using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Items;

namespace MysticCrafting.Module.Repositories
{
	public interface IItemContainerRepository : IRepository
	{
		IList<MysticItemContainer> GetItemContainers(int itemId);
	}
}
