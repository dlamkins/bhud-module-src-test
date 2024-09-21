using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using MysticCrafting.Module.Menu;

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
				if (base.get_View().ItemListModel != null)
				{
					base.get_View().ItemListModel.Filter.Type = menuItem.ItemFilter.Type;
					base.get_View().ItemListModel.Filter.Types = menuItem.ItemFilter.Types;
					base.get_View().ItemListModel.Filter.DetailsType = menuItem.ItemFilter.DetailsType;
					base.get_View().ItemListModel.Filter.Categories = menuItem.ItemFilter.Categories;
					base.get_View().ItemListModel.Filter.IsFavorite = menuItem.ItemFilter.IsFavorite;
					base.get_View().ItemListModel.Breadcrumbs = GetBreadcrumbs(menuItem);
					base.get_View().ItemListModel.InvokeFilterChanged();
				}
			}
		}

		protected override void UpdateView()
		{
			base.get_View().SetMenuItems(base.get_Model().GetMenuItems((Container)(object)base.get_View().Menu));
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
					base.get_View().ItemListModel.InvokeFilterChanged();
				}
			});
		}

		protected override void Unload()
		{
			base.get_View().MenuItemSelected -= OnMenuItemSelected;
		}
	}
}
