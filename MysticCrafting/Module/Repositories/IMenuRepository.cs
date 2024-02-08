using System.Collections.Generic;
using MysticCrafting.Models.Menu;

namespace MysticCrafting.Module.Repositories
{
	public interface IMenuRepository : IRepository
	{
		IList<MysticMenuItem> GetMenuItems();
	}
}
