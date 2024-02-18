using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Discovery.ItemList.Controls;
using MysticCrafting.Module.Recipe;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Discovery.ItemList
{
	public class ItemListPresenter : Presenter<ItemListView, ItemListModel>, IItemListPresenter, IPresenter
	{
		private RecipeDetailsView _recipeDetailsView;

		public ItemListPresenter(ItemListView view, ItemListModel model)
			: base(view, model)
		{
		}

		protected override void UpdateView()
		{
			if (base.Model == null)
			{
				throw new NullReferenceException("Model cannot be null.");
			}
			if (base.Model.Filter != null)
			{
				base.View.Panel.ClearChildren();
				if (base.View.RarityDropdown != null)
				{
					base.View.RarityDropdown.ValueChanged += RarityDropdown_ValueChanged;
				}
				if (base.Model.Filter != null && base.View.RarityDropdown != null)
				{
					base.Model.Filter.Rarity = (base.View.RarityDropdown.SelectedItem.Equals("All rarities") ? null : base.View.RarityDropdown.SelectedItem);
				}
				List<MysticItem> items = base.Model.GetFilteredItems().ToList();
				if (items.Count == base.Model.ItemLimit)
				{
					base.View.LimitReached = true;
				}
				UpdateList(items);
			}
		}

		public void UpdateList(List<MysticItem> items)
		{
			if (!items.Any() && base.Model.Filter != null && base.Model.Filter.IsFavorite)
			{
				base.View.NoResults = true;
				return;
			}
			List<ItemRowView> rows = new List<ItemRowView>();
			foreach (MysticItem item in items)
			{
				bool displayDetailsType = string.IsNullOrEmpty(base.Model.Filter?.DetailsType);
				rows.Add(new ItemRowView(item, displayDetailsType)
				{
					OnClick = ItemRow_Click
				});
			}
			base.View.SetItemRows(rows);
		}

		private void RarityDropdown_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			Dropdown dropdown = sender as Dropdown;
			if (dropdown != null)
			{
				if (dropdown.SelectedItem.Equals("All rarities", StringComparison.InvariantCultureIgnoreCase))
				{
					base.Model.Filter.Rarity = null;
				}
				else
				{
					base.Model.Filter.Rarity = dropdown.SelectedItem;
				}
				UpdateView();
			}
		}

		private void ItemRow_Click(object sender, MouseEventArgs e)
		{
			ItemButton listItem = sender as ItemButton;
			if (listItem != null)
			{
				base.View.Container.ClearChildren();
				base.View.LimitReached = false;
				_recipeDetailsView?.RecipeItemList.Dispose();
				_recipeDetailsView = new RecipeDetailsView(listItem.Item, BuildBreadcrumbs());
				RecipeDetailsView recipeDetailsView = _recipeDetailsView;
				recipeDetailsView.OnBackButtonClick = (EventHandler<MouseEventArgs>)Delegate.Combine(recipeDetailsView.OnBackButtonClick, new EventHandler<MouseEventArgs>(ItemDetails_BackButton_Click));
				ServiceContainer.AudioService.PlayMenuItemClick();
				base.View.Container.Show(_recipeDetailsView);
			}
		}

		private IList<string> BuildBreadcrumbs()
		{
			IList<string> breadcrumbs = base.Model.Breadcrumbs ?? new List<string>();
			string existingSearchCrumb = breadcrumbs.FirstOrDefault((string b) => b.Contains("Search"));
			if (existingSearchCrumb != null)
			{
				breadcrumbs.Remove(existingSearchCrumb);
			}
			if (!string.IsNullOrWhiteSpace(base.Model.Filter.NameContainsText))
			{
				breadcrumbs.Add("Search: \"" + base.Model.Filter.NameContainsText + "\"");
			}
			return breadcrumbs;
		}

		private void ItemDetails_BackButton_Click(object sender, MouseEventArgs e)
		{
			base.View.Container.ClearChildren();
			_recipeDetailsView?.RecipeItemList?.Dispose();
			base.View.LimitReached = false;
			base.View.Panel.Parent = base.View.Container;
			base.View.RarityDropdown.Parent = base.View.Container;
			base.View.SetLimitLabelParent(base.View.Container);
			ServiceContainer.TextureRepository.ClearTextures();
		}

		public void Reload()
		{
			base.View.Container.ClearChildren();
			base.View.LimitReached = false;
			base.View.DoBuild(base.View.Container);
		}
	}
}
