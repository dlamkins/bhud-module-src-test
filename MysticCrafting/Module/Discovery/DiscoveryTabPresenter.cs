using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Discovery.ItemList;
using MysticCrafting.Module.Menu;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Discovery
{
	public class DiscoveryTabPresenter : Presenter<DiscoveryTabView, DiscoveryTabMenuModel>, IDiscoveryTabPresenter, IPresenter
	{
		public DiscoveryTabPresenter(DiscoveryTabView view, DiscoveryTabMenuModel model)
			: base(view, model)
		{
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			base.View.MenuItemSelected += OnMenuItemSelected;
			return base.Load(progress);
		}

		private void OnMenuItemSelected(object sender, ControlActivatedEventArgs e)
		{
			CategoryMenu menu = sender as CategoryMenu;
			if (menu != null)
			{
				CategoryMenuItem menuItem = menu.SelectedMenuItem;
				base.View.SearchText = string.Empty;
				base.View.NameFilter = string.Empty;
				if (menuItem.Text == "Home")
				{
					base.View.ItemListContainer.Show(base.View.HomeView);
				}
				else
				{
					UpdateItemList(menuItem);
				}
			}
		}

		protected override void UpdateView()
		{
			base.View.SetMenuItems(base.Model.GetMenuItems(base.View.Menu));
		}

		public void UpdateItemList(CategoryMenuItem menuItem)
		{
			if (menuItem != null && base.View.ItemList?.Presenter != null)
			{
				base.View.ReloadItemList(new ItemListModel(ServiceContainer.ItemRepository)
				{
					Filter = menuItem.ItemFilter,
					Breadcrumbs = GetBreadcrumbs(menuItem)
				});
			}
		}

		public void ShowItemList(MysticItemFilter filter, List<string> breadcrumbs)
		{
			if (filter != null)
			{
				base.View.ReloadItemList(new ItemListModel(ServiceContainer.ItemRepository)
				{
					Filter = filter,
					Breadcrumbs = breadcrumbs
				});
			}
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
					base.View.ReloadItemList(base.View.ItemListModel);
				}
			});
		}

		protected override void Unload()
		{
			base.View.MenuItemSelected -= OnMenuItemSelected;
		}
	}
}
