using System;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using MysticCrafting.Module.Discovery.ItemList.Controls;

namespace MysticCrafting.Module.Discovery.ItemList
{
	public class ItemListPresenter : Presenter<ItemListView, ItemListModel>, IItemListPresenter, IPresenter
	{
		private static readonly Logger Logger = Logger.GetLogger<ItemListPresenter>();

		public ItemListPresenter(ItemListView view, ItemListModel model)
			: base(view, model)
		{
			if (model != null)
			{
				model.FilterChanged = (EventHandler)Delegate.Combine(model.FilterChanged, (EventHandler)delegate
				{
					((Presenter<ItemListView, ItemListModel>)this).UpdateView();
				});
			}
		}

		protected override void UpdateView()
		{
			if (base.get_Model() == null)
			{
				throw new NullReferenceException("Model cannot be null.");
			}
			if (base.get_Model().Filter != null)
			{
				((Container)base.get_View().ResultsPanel).ClearChildren();
				List<Item> items = base.get_Model().GetFilteredItems().ToList();
				try
				{
					UpdateList(items);
				}
				catch (Exception ex)
				{
					Logger.Error("Item list view could not be updated: " + ex.Message);
				}
			}
		}

		public void UpdateList(List<Item> items)
		{
			if (!items.Any() && base.get_Model().Filter != null)
			{
				base.get_View().NoResults = true;
			}
			else
			{
				base.get_View().NoResults = false;
				List<ItemRowView> rows = new List<ItemRowView>();
				foreach (Item item in items)
				{
					rows.Add(new ItemRowView(item, displayDetailsType: true)
					{
						OnClick = ItemRow_Click
					});
				}
				base.get_View().SetItemRows(rows);
			}
			if (items.Count == base.get_Model().ItemLimit)
			{
				base.get_View().SetLimitLabel();
			}
		}

		private void ItemRow_Click(object sender, MouseEventArgs e)
		{
			if (sender is ItemButton)
			{
				base.get_View().ItemClicked?.Invoke(sender, (EventArgs)(object)e);
			}
		}

		public IList<string> BuildBreadcrumbs()
		{
			IList<string> breadcrumbs = base.get_Model().Breadcrumbs ?? new List<string>();
			string existingSearchCrumb = breadcrumbs.FirstOrDefault((string b) => b?.Contains("Search") ?? false);
			if (existingSearchCrumb != null)
			{
				breadcrumbs.Remove(existingSearchCrumb);
			}
			if (!string.IsNullOrWhiteSpace(base.get_Model().Filter.NameContainsText))
			{
				breadcrumbs.Add("Search: \"" + base.get_Model().Filter.NameContainsText + "\"");
			}
			return breadcrumbs;
		}

		public void Reload()
		{
			((Container)base.get_View().Container).ClearChildren();
			((View<IItemListPresenter>)base.get_View()).DoBuild((Container)(object)base.get_View().Container);
		}
	}
}
