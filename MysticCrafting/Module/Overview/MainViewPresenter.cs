using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using MysticCrafting.Models.Items;
using MysticCrafting.Models.Menu;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.Items;
using MysticCrafting.Module.Menu;
using MysticCrafting.Module.Repositories;

namespace MysticCrafting.Module.Overview
{
	public class MainViewPresenter : Presenter<MainView, MainViewModel>, IMainViewPresenter, IPresenter
	{
		private readonly IMenuRepository _menuItemRepository;

		private CategoryMenuItem DefaultMenuItemSelection { get; set; }

		public MainViewPresenter(MainView view, MainViewModel model, IMenuRepository menuItemRepository)
			: base(view, model)
		{
			_menuItemRepository = menuItemRepository;
		}

		protected override void UpdateView()
		{
		}

		public void InitializeMenu()
		{
			IList<MysticMenuItem> menuItems = _menuItemRepository.GetMenuItems();
			InitializeMenuItems(menuItems, base.View.Menu);
			if (DefaultMenuItemSelection != null)
			{
				DefaultMenuItemSelection.Select();
				ReloadItemList(DefaultMenuItemSelection);
			}
		}

		private void InitializeMenuItems(IList<MysticMenuItem> menuItems, Container parent)
		{
			if (menuItems == null || parent == null)
			{
				return;
			}
			foreach (MysticMenuItem item in menuItems)
			{
				if (item.Filters == null)
				{
					item.Filters = new MysticItemFilter();
				}
				CategoryMenuItem menuItem = new CategoryMenuItem(LocalizationHelper.TranslateMenuItem(item.Name), item.Filters)
				{
					Parent = parent
				};
				if (item.Icon.HasValue && item.Icon != 0)
				{
					menuItem.Icon = AsyncTexture2D.FromAssetId(item.Icon.Value);
				}
				menuItem.ItemClicked += base.View.Menu_ItemClicked;
				if (item.SelectedByDefault)
				{
					DefaultMenuItemSelection = menuItem;
				}
				if (item.Children != null && item.Children.Any())
				{
					InitializeMenuItems(item.Children.ToList(), menuItem);
				}
			}
		}

		public void GoToMenuItem(CategoryMenuItem menuItem)
		{
			base.View.SearchText = string.Empty;
			base.View.NameFilter = string.Empty;
			if (menuItem.Selected)
			{
				menuItem = menuItem.Parent as CategoryMenuItem;
			}
			ReloadItemList(menuItem);
		}

		private void ReloadItemList(CategoryMenuItem menuItem)
		{
			if (menuItem != null && base.View.ItemList?.Presenter != null)
			{
				IItemListPresenter presenter = base.View.ItemList?.Presenter;
				if (presenter != null)
				{
					presenter.UpdateFilter(menuItem.ItemFilter);
					presenter.UpdateMenuBreadcrumbs(GetBreadcrumbs(menuItem));
					ReloadItemList();
				}
			}
		}

		public void ReloadItemList()
		{
			base.View.ItemList?.Presenter.Reload();
		}

		private List<string> GetBreadcrumbs(CategoryMenuItem menuItem)
		{
			List<string> breadcrumbs = new List<string>();
			CategoryMenuItem parent = menuItem.Parent as CategoryMenuItem;
			if (parent != null)
			{
				breadcrumbs.AddRange(GetBreadcrumbs(parent));
			}
			breadcrumbs.Add(menuItem.Text);
			return breadcrumbs;
		}

		public void SearchAsync(string text)
		{
			Task.Run(delegate
			{
				lock (base.View.ItemList)
				{
					base.View.NameFilter = text;
					ReloadItemList();
				}
			});
		}
	}
}
