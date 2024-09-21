using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using MysticCrafting.Models.Items;
using MysticCrafting.Models.Menu;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.Menu;
using MysticCrafting.Module.Repositories;

namespace MysticCrafting.Module.Discovery
{
	public class DiscoveryTabMenuModel : IDiscoveryTabMenuModel
	{
		private readonly IMenuRepository _menuRepository;

		public DiscoveryTabMenuModel(IMenuRepository menuRepository)
		{
			_menuRepository = menuRepository;
		}

		public IList<CategoryMenuItem> GetMenuItems(Container parent = null)
		{
			IList<MysticMenuItem> menuItems = _menuRepository.GetMenuItems();
			return GetMenuItemsRecursively(menuItems, parent);
		}

		private IList<CategoryMenuItem> GetMenuItemsRecursively(IList<MysticMenuItem> menuItems, Container parent = null)
		{
			List<CategoryMenuItem> categoryMenuItems = new List<CategoryMenuItem>();
			foreach (MysticMenuItem item in menuItems)
			{
				if (item.Filters == null)
				{
					item.Filters = new MysticItemFilter();
				}
				CategoryMenuItem categoryMenuItem = new CategoryMenuItem(LocalizationHelper.TranslateMenuItem(item.Name), item.Filters);
				((Control)categoryMenuItem).set_Parent(parent);
				CategoryMenuItem menuItem = categoryMenuItem;
				if (item.Icon.HasValue && item.Icon != 0)
				{
					menuItem.Icon = AsyncTexture2D.FromAssetId(item.Icon.Value);
				}
				categoryMenuItems.Add(menuItem);
				if (item.Children != null && item.Children.Any())
				{
					GetMenuItemsRecursively(item.Children.ToList(), (Container)(object)menuItem);
				}
				if (item.SelectedByDefault)
				{
					menuItem.Select();
				}
			}
			return categoryMenuItems;
		}
	}
}
