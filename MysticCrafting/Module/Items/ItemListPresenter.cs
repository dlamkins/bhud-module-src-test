using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Items.Controls;
using MysticCrafting.Module.Recipe;
using MysticCrafting.Module.Repositories;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Items
{
	public class ItemListPresenter : Presenter<ItemListView, ItemListModel>, IItemListPresenter, IPresenter
	{
		private readonly IItemRepository _itemRepository;

		private Panel EmptyResultsPanel;

		private Label _limitLabel;

		public ItemListPresenter(ItemListView view, ItemListModel model, IItemRepository itemRepository)
			: base(view, model)
		{
			_itemRepository = itemRepository;
		}

		public void UpdateFilter(MysticItemFilter filter)
		{
			base.View.ItemFilter = filter;
		}

		public void UpdateMenuBreadcrumbs(List<string> breadcrumbs)
		{
			base.View.MenuBreadcrumbs = breadcrumbs;
		}

		protected override void UpdateView()
		{
			if (base.Model == null)
			{
				throw new NullReferenceException("Model cannot be null.");
			}
			if (base.View.ItemFilter == null)
			{
				return;
			}
			base.View.Panel.ClearChildren();
			if (base.View.RarityDropdown != null)
			{
				base.View.RarityDropdown.ValueChanged += RarityDropdown_ValueChanged;
			}
			if (base.View.ItemFilter != null)
			{
				if (base.View.RarityDropdown.SelectedItem.Equals("All rarities"))
				{
					base.View.ItemFilter.Rarity = null;
				}
				else
				{
					base.View.ItemFilter.Rarity = base.View.RarityDropdown.SelectedItem;
				}
			}
			List<MysticItem> items = new List<MysticItem>();
			items = _itemRepository.FilterItems(base.View.ItemFilter).ToList();
			if (items.Count > base.View.ItemLimit)
			{
				ShowMaxItemsLabel();
				items = items.Take(base.View.ItemLimit).ToList();
			}
			UpdateList(items);
		}

		public void UpdateColumnNames()
		{
			new Label
			{
				Parent = base.View.Panel,
				Text = "Name",
				StrokeText = true,
				TextColor = Color.LightYellow,
				Location = new Point(100, 20)
			};
			new Label
			{
				Parent = base.View.Panel,
				Text = "Type",
				StrokeText = true,
				TextColor = Color.LightYellow,
				Location = new Point(base.View.Panel.Width - 350, 20)
			};
		}

		public void UpdateList(List<MysticItem> items)
		{
			EmptyResultsPanel?.ClearChildren();
			EmptyResultsPanel?.Dispose();
			if (!items.Any() && base.View.ItemFilter != null)
			{
				if (base.View.ItemFilter.IsFavorite)
				{
					EmptyResultsPanel = new Panel
					{
						Parent = base.View.Panel,
						Size = base.View.Panel.Size
					};
					new Label
					{
						Text = Common.FavoritesEmpty,
						Parent = EmptyResultsPanel,
						Font = GameService.Content.DefaultFont32,
						AutoSizeWidth = true,
						AutoSizeHeight = true,
						StrokeText = true,
						Location = new Point(120, 200)
					};
					new Image
					{
						Texture = ServiceContainer.TextureRepository.Textures.HeartDisabled,
						Parent = EmptyResultsPanel,
						Size = new Point(50, 50),
						Location = new Point(350, 300)
					};
				}
				return;
			}
			base.View.Panel.ClearChildren();
			foreach (MysticItem item in items)
			{
				ViewContainer viewContainer = new ViewContainer();
				viewContainer.Size = new Point(base.View.Panel.Width, 65);
				viewContainer.Parent = base.View.Panel;
				bool displayDetailsType = string.IsNullOrEmpty(base.View.ItemFilter?.DetailsType);
				ItemRowView itemRow = new ItemRowView(item, displayDetailsType)
				{
					OnClick = ItemRow_Click
				};
				viewContainer.Show(itemRow);
			}
		}

		public void ShowMaxItemsLabel()
		{
			if (_limitLabel != null)
			{
				_limitLabel.Dispose();
			}
			_limitLabel = new Label
			{
				Text = Common.MaximumResultsReached,
				Parent = base.View.BuildPanel,
				Width = 800,
				TextColor = Color.Orange,
				Font = GameService.Content.DefaultFont16,
				Top = 5,
				Left = 160,
				StrokeText = true,
				Visible = true
			};
		}

		private void RarityDropdown_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			Dropdown dropdown = sender as Dropdown;
			if (dropdown != null)
			{
				if (dropdown.SelectedItem.Equals("All rarities", StringComparison.InvariantCultureIgnoreCase))
				{
					base.View.ItemFilter.Rarity = null;
				}
				else
				{
					base.View.ItemFilter.Rarity = dropdown.SelectedItem;
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
				_limitLabel?.Dispose();
				RecipeDetailsView recipeListView = new RecipeDetailsView(listItem.Item, BuildBreadcrumbs());
				recipeListView.OnBackButtonClick = (EventHandler<MouseEventArgs>)Delegate.Combine(recipeListView.OnBackButtonClick, new EventHandler<MouseEventArgs>(ItemDetails_BackButton_Click));
				ServiceContainer.AudioService.PlayMenuItemClick();
				base.View.Container.Show(recipeListView);
			}
		}

		private List<string> BuildBreadcrumbs()
		{
			List<string> breadcrumbs = base.View.MenuBreadcrumbs ?? new List<string>();
			string existingSearchCrumb = breadcrumbs.FirstOrDefault((string b) => b.Contains("Search"));
			if (existingSearchCrumb != null)
			{
				breadcrumbs.Remove(existingSearchCrumb);
			}
			if (!string.IsNullOrWhiteSpace(base.View.ItemFilter.NameContainsText))
			{
				breadcrumbs.Add("Search: \"" + base.View.ItemFilter.NameContainsText + "\"");
			}
			return breadcrumbs;
		}

		private void ItemDetails_BackButton_Click(object sender, MouseEventArgs e)
		{
			base.View.Container.ClearChildren();
			_limitLabel?.Dispose();
			base.View.Panel.Parent = base.View.Container;
			base.View.RarityDropdown.Parent = base.View.Container;
			if (_limitLabel != null)
			{
				_limitLabel.Parent = base.View.Container;
			}
		}

		public void Reload()
		{
			base.View.Container.ClearChildren();
			_limitLabel?.Dispose();
			base.View.DoBuild(base.View.Container);
		}
	}
}
