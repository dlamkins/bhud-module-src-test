using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Discovery.ItemList.Controls;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Recipe;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Discovery.ItemList
{
	public class ItemListPresenter : Presenter<ItemListView, ItemListModel>, IItemListPresenter, IPresenter
	{
		private static List<string> ArmorTypes = new List<string> { "Helm", "Shoulders", "Coat", "Gloves", "Leggings", "Boots", "Back" };

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
			if (base.Model.Filter == null)
			{
				return;
			}
			base.View.Panel.ClearChildren();
			if (!string.IsNullOrEmpty(base.Model.Filter?.Rarity) && base.View.RarityDropdown != null)
			{
				base.Model.Filter.Rarity = (base.View.RarityDropdown.SelectedItem.Equals("All rarities") ? null : base.View.RarityDropdown.SelectedItem);
			}
			if (base.Model.Filter != null)
			{
				if (base.Model.Filter.Type == "Armor" || ArmorTypes.Contains(base.Model.Filter.DetailsType))
				{
					base.View.WeightDropdown.Visible = true;
					base.View.LegendaryTypeDropdown.Visible = true;
					base.Model.Filter.WeightFilterDisabled = false;
				}
				else
				{
					base.View.WeightDropdown.Visible = false;
					base.View.LegendaryTypeDropdown.Visible = false;
					base.Model.Filter.WeightFilterDisabled = true;
				}
				if (base.Model.Filter.Weight != 0 && base.View.WeightDropdown.Visible)
				{
					base.View.WeightDropdown.SelectedItem = base.Model.Filter.Weight.ToString();
				}
			}
			List<MysticItem> items = base.Model.GetFilteredItems().ToList();
			if (items.Count == base.Model.ItemLimit)
			{
				base.View.LimitReached = true;
			}
			base.View.UpdateFilterLabel();
			UpdateList(items);
		}

		public void UpdateList(List<MysticItem> items)
		{
			if (!items.Any() && base.Model.Filter != null && base.Model.Filter.IsFavorite)
			{
				base.View.NoResults = true;
				return;
			}
			List<ItemRowView> rows = new List<ItemRowView>();
			items = items.Where(delegate(MysticItem i)
			{
				bool flag = ServiceContainer.PlayerUnlocksService.ItemUnlocked(i.DefaultSkin.GetValueOrDefault());
				if (base.Model.Filter.HideSkinUnlocked && flag)
				{
					return false;
				}
				if (base.Model.Filter.HideSkinLocked && !flag)
				{
					return false;
				}
				return (!base.Model.Filter.HideMaxItemsCollected || !(ServiceContainer.PlayerUnlocksService.LegendaryUnlockedCount(i.GameId) >= i.GetMaxCount())) ? true : false;
			}).ToList();
			foreach (MysticItem item in items.Take(100))
			{
				string.IsNullOrEmpty(base.Model.Filter?.DetailsType);
				rows.Add(new ItemRowView(item, displayDetailsType: true)
				{
					OnClick = ItemRow_Click
				});
			}
			base.View.SetItemRows(rows);
		}

		public void RarityDropdown_ValueChanged(object sender, ValueChangedEventArgs e)
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
				_recipeDetailsView?.RecipeItemList?.Dispose();
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
			string existingSearchCrumb = breadcrumbs.FirstOrDefault((string b) => b?.Contains("Search") ?? false);
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
			base.View.SettingsMenuButton.Parent = base.View.Container;
			base.View.FiltersLabel.Parent = base.View.Container;
			base.View.WeightDropdown.Parent = base.View.Container;
			base.View.LegendaryTypeDropdown.Parent = base.View.Container;
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
