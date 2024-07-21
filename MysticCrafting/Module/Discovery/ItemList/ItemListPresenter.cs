using System;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using MysticCrafting.Module.Discovery.ItemList.Controls;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.RecipeTree;
using MysticCrafting.Module.RecipeTree.TreeView;
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
			if (base.get_Model() == null)
			{
				throw new NullReferenceException("Model cannot be null.");
			}
			if (base.get_Model().Filter == null)
			{
				return;
			}
			((Container)base.get_View().Panel).ClearChildren();
			if (base.get_Model().Filter.Rarity != 0 && base.get_View().RarityDropdown != null)
			{
				base.get_Model().Filter.Rarity = ((!base.get_View().RarityDropdown.get_SelectedItem().Equals("All rarities")) ? ((ItemRarity)Enum.Parse(typeof(ItemRarity), base.get_View().RarityDropdown.get_SelectedItem())) : ItemRarity.Unknown);
			}
			if (base.get_Model().Filter != null)
			{
				if (base.get_Model().Filter.Type == ItemType.Armor || ArmorTypes.Contains(base.get_Model().Filter.DetailsType))
				{
					((Control)base.get_View().WeightDropdown).set_Visible(true);
					((Control)base.get_View().LegendaryTypeDropdown).set_Visible(true);
					base.get_Model().Filter.WeightFilterDisabled = false;
				}
				else
				{
					((Control)base.get_View().WeightDropdown).set_Visible(false);
					((Control)base.get_View().LegendaryTypeDropdown).set_Visible(false);
					base.get_Model().Filter.WeightFilterDisabled = true;
				}
				if (base.get_Model().Filter.Weight != 0 && ((Control)base.get_View().WeightDropdown).get_Visible())
				{
					base.get_View().WeightDropdown.set_SelectedItem(base.get_Model().Filter.Weight.ToString());
				}
			}
			List<Item> items = base.get_Model().GetFilteredItems().ToList();
			if (items.Count == base.get_Model().ItemLimit)
			{
				base.get_View().LimitReached = true;
			}
			base.get_View().UpdateFilterLabel();
			try
			{
				UpdateList(items);
			}
			catch (Exception)
			{
			}
		}

		public void UpdateList(List<Item> items)
		{
			if (!items.Any() && base.get_Model().Filter != null && base.get_Model().Filter.IsFavorite)
			{
				base.get_View().NoResults = true;
				return;
			}
			List<ItemRowView> rows = new List<ItemRowView>();
			items = items.Where(delegate(Item i)
			{
				bool flag = ServiceContainer.PlayerUnlocksService.ItemUnlocked(i.DefaultSkin);
				if (base.get_Model().Filter.HideSkinUnlocked && flag)
				{
					return false;
				}
				if (base.get_Model().Filter.HideSkinLocked && !flag)
				{
					return false;
				}
				return (!base.get_Model().Filter.HideMaxItemsCollected || !(ServiceContainer.PlayerUnlocksService.LegendaryUnlockedCount(i.Id) >= i.GetMaxCount())) ? true : false;
			}).ToList();
			foreach (Item item in items.Take(100))
			{
				rows.Add(new ItemRowView(item, displayDetailsType: true)
				{
					OnClick = ItemRow_Click
				});
			}
			base.get_View().SetItemRows(rows);
		}

		public void RarityDropdown_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			Dropdown dropdown = (Dropdown)((sender is Dropdown) ? sender : null);
			if (dropdown != null)
			{
				if (dropdown.get_SelectedItem().Equals("All rarities", StringComparison.InvariantCultureIgnoreCase))
				{
					base.get_Model().Filter.Rarity = ItemRarity.Unknown;
				}
				else
				{
					base.get_Model().Filter.Rarity = (ItemRarity)Enum.Parse(typeof(ItemRarity), dropdown.get_SelectedItem());
				}
				((Presenter<ItemListView, ItemListModel>)this).UpdateView();
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

		private void ItemDetails_BackButton_Click(object sender, MouseEventArgs e)
		{
			((Container)base.get_View().Container).ClearChildren();
			RecipeDetailsView recipeDetailsView = _recipeDetailsView;
			if (recipeDetailsView != null)
			{
				TreeView treeView = recipeDetailsView.TreeView;
				if (treeView != null)
				{
					((Control)treeView).Dispose();
				}
			}
			base.get_View().LimitReached = false;
			((Control)base.get_View().Panel).set_Parent((Container)(object)base.get_View().Container);
			((Control)base.get_View().RarityDropdown).set_Parent((Container)(object)base.get_View().Container);
			((Control)base.get_View().SettingsMenuButton).set_Parent((Container)(object)base.get_View().Container);
			((Control)base.get_View().FiltersLabel).set_Parent((Container)(object)base.get_View().Container);
			((Control)base.get_View().WeightDropdown).set_Parent((Container)(object)base.get_View().Container);
			((Control)base.get_View().LegendaryTypeDropdown).set_Parent((Container)(object)base.get_View().Container);
			base.get_View().SetLimitLabelParent((Container)(object)base.get_View().Container);
			ServiceContainer.TextureRepository.ClearTextures();
		}

		public void Reload()
		{
			((Container)base.get_View().Container).ClearChildren();
			base.get_View().LimitReached = false;
			((View<IItemListPresenter>)base.get_View()).DoBuild((Container)(object)base.get_View().Container);
		}
	}
}
