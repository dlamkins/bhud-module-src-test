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
			base.get_View().MenuItemSelected += OnMenuItemSelected;
			return base.Load(progress);
		}

		private void OnMenuItemSelected(object sender, ControlActivatedEventArgs e)
		{
			CategoryMenu menu = sender as CategoryMenu;
			if (menu != null)
			{
				CategoryMenuItem menuItem = menu.SelectedMenuItem;
				base.get_View().SearchText = string.Empty;
				base.get_View().NameFilter = string.Empty;
				if (menuItem.Text == "Home")
				{
					base.get_View().ItemListContainer.Show((IView)(object)base.get_View().HomeView);
				}
				else
				{
					UpdateItemList(menuItem);
				}
			}
		}

		protected override void UpdateView()
		{
			base.get_View().SetMenuItems(base.get_Model().GetMenuItems((Container)(object)base.get_View().Menu));
		}

		public void UpdateItemList(CategoryMenuItem menuItem)
		{
			if (menuItem != null && ((View<IItemListPresenter>)base.get_View().ItemList)?.get_Presenter() != null)
			{
				base.get_View().ReloadItemList(new ItemListModel(ServiceContainer.ItemRepository)
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
				base.get_View().ReloadItemList(new ItemListModel(ServiceContainer.ItemRepository)
				{
					Filter = filter,
					Breadcrumbs = breadcrumbs
				});
			}
		}

		private List<string> GetBreadcrumbs(CategoryMenuItem menuItem)
		{
			List<string> breadcrumbs = new List<string>();
			CategoryMenuItem parent = ((Control)menuItem).get_Parent() as CategoryMenuItem;
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
				lock (base.get_View().ItemList)
				{
					base.get_View().NameFilter = text;
					base.get_View().ReloadItemList(base.get_View().ItemListModel);
				}
			});
		}

		protected override void Unload()
		{
			base.get_View().MenuItemSelected -= OnMenuItemSelected;
		}
	}
}
