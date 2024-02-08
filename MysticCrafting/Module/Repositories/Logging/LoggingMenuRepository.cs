using System.Collections.Generic;
using MysticCrafting.Models.Menu;

namespace MysticCrafting.Module.Repositories.Logging
{
	public class LoggingMenuRepository : LoggingRepository, IMenuRepository, IRepository
	{
		private readonly IMenuRepository _menuRepository;

		public LoggingMenuRepository(IMenuRepository menuRepository)
			: base(menuRepository)
		{
			_menuRepository = menuRepository;
		}

		public IList<MysticMenuItem> GetMenuItems()
		{
			return _menuRepository.GetMenuItems();
		}
	}
}
