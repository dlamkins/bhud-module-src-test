using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Discovery.ItemList
{
	public class ItemListView : View<IItemListPresenter>
	{
		private Panel _noResultsPanel;

		private Label _nothingLabel;

		private Image _nothingImage;

		private Label _limitLabel;

		private ContextMenuStripItem _hideLockedSkinsStripItem;

		private ContextMenuStripItem _hideUnlockedSkinsStripItem;

		private ContextMenuStripItem _hideMaxCollectedStripItem;

		public ViewContainer Container { get; set; }

		public FlowPanel Panel { get; set; }

		public ItemListModel Model { get; set; }

		public Dropdown RarityDropdown { get; set; }

		public Dropdown LegendaryTypeDropdown { get; set; }

		public Dropdown WeightDropdown { get; set; }

		public ContextMenuStrip SettingsMenu { get; set; }

		public bool LimitReached
		{
			get
			{
				return _limitLabel?.Visible ?? false;
			}
			set
			{
				if (_limitLabel != null)
				{
					_limitLabel.Visible = value;
				}
			}
		}

		public bool NoResults
		{
			get
			{
				return _noResultsPanel?.Visible ?? false;
			}
			set
			{
				if (_noResultsPanel != null)
				{
					_noResultsPanel.Visible = value;
					if (_noResultsPanel.Parent == null)
					{
						_noResultsPanel.Parent = Panel;
					}
				}
			}
		}

		public Container BuildPanel { get; set; }

		public GlowButton SettingsMenuButton { get; set; }

		public Label FiltersLabel { get; set; }

		public ItemListView(ItemListModel itemListModel)
		{
			Model = itemListModel;
			WithPresenter(new ItemListPresenter(this, itemListModel));
		}

		protected override void Build(Container buildPanel)
		{
			BuildPanel = buildPanel;
			Container = new ViewContainer
			{
				FadeView = true,
				Parent = buildPanel,
				Size = new Point(buildPanel.Width, buildPanel.Height),
				Padding = new Thickness(0f),
				Location = new Point(0, 0)
			};
			BuildRarityFilter();
			BuildAmorWeightFilter();
			BuildMoreFilters();
			BuildLegendaryTypeFilter();
			Panel = new FlowPanel
			{
				Parent = Container,
				Location = new Point(0, 40),
				Width = Container.Width - 5,
				Height = Container.Height - 50,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				CanScroll = true
			};
			BuildNoResultsPanel();
		}

		public void UpdateFilterLabel()
		{
			int enabledCount = 0;
			if (Model.Filter.HideSkinLocked)
			{
				enabledCount++;
			}
			if (Model.Filter.HideSkinUnlocked)
			{
				enabledCount++;
			}
			if (Model.Filter.HideMaxItemsCollected)
			{
				enabledCount++;
			}
			FiltersLabel.Text = $"{Common.MoreFilters} ({enabledCount})";
		}

		public void BuildRarityFilter()
		{
			RarityDropdown = new Dropdown
			{
				Parent = Container,
				Width = 120,
				Location = new Point(5, 5),
				Visible = true
			};
			RarityDropdown.Items.Add("All rarities");
			RarityDropdown.Items.Add("Exotic");
			RarityDropdown.Items.Add("Ascended");
			RarityDropdown.Items.Add("Legendary");
			if (string.IsNullOrEmpty(Model?.Filter?.Rarity))
			{
				RarityDropdown.SelectedItem = "All rarities";
			}
			else
			{
				RarityDropdown.SelectedItem = Model?.Filter.Rarity;
			}
			RarityDropdown.ValueChanged += base.Presenter.RarityDropdown_ValueChanged;
		}

		public void BuildLegendaryTypeFilter()
		{
			LegendaryTypeDropdown = new Dropdown
			{
				Parent = Container,
				Width = 150,
				Location = new Point(130, 5),
				Visible = true
			};
			LegendaryTypeDropdown.Items.Add("All types");
			LegendaryTypeDropdown.Items.Add("Perfected Envoy (Raid)");
			LegendaryTypeDropdown.Items.Add("Triumphant (WvW)");
			LegendaryTypeDropdown.Items.Add("Glorious (PvP)");
			LegendaryTypeDropdown.Items.Add("Obsidian (PvE)");
			if (string.IsNullOrEmpty(Model?.Filter?.LegendaryType))
			{
				LegendaryTypeDropdown.SelectedItem = "All types";
			}
			else
			{
				LegendaryTypeDropdown.SelectedItem = Model?.Filter.LegendaryType;
			}
			LegendaryTypeDropdown.ValueChanged += delegate(object sender, ValueChangedEventArgs args)
			{
				Dropdown dropdown = sender as Dropdown;
				if (dropdown != null)
				{
					if (dropdown.SelectedItem.Equals("All types", StringComparison.InvariantCultureIgnoreCase))
					{
						Model.Filter.LegendaryType = null;
					}
					else
					{
						Model.Filter.LegendaryType = dropdown.SelectedItem;
					}
					base.Presenter.DoUpdateView();
				}
			};
		}

		public void BuildAmorWeightFilter()
		{
			WeightDropdown = new Dropdown
			{
				Parent = Container,
				Width = 100,
				Location = new Point(480, 5),
				Visible = false
			};
			WeightDropdown.Items.Add("All weights");
			WeightDropdown.Items.Add("Light");
			WeightDropdown.Items.Add("Medium");
			WeightDropdown.Items.Add("Heavy");
			WeightDropdown.SelectedItem = "All weights";
			WeightDropdown.ValueChanged += delegate(object sender, ValueChangedEventArgs args)
			{
				Dropdown dropdown = sender as Dropdown;
				if (dropdown != null)
				{
					if (!Enum.IsDefined(typeof(WeightClass), dropdown.SelectedItem))
					{
						Model.Filter.Weight = WeightClass.Unknown;
					}
					else
					{
						Model.Filter.Weight = (WeightClass)Enum.Parse(typeof(WeightClass), dropdown.SelectedItem);
					}
					base.Presenter.DoUpdateView();
				}
			};
		}

		public void BuildMoreFilters()
		{
			SettingsMenuButton = new GlowButton
			{
				Location = new Point(Container.Width - 60, 5),
				Icon = AsyncTexture2D.FromAssetId(157109),
				ActiveIcon = AsyncTexture2D.FromAssetId(157110),
				Visible = true,
				BasicTooltipText = Common.MoreFilters,
				Parent = Container
			};
			FiltersLabel = new Label
			{
				Parent = Container,
				Location = new Point(SettingsMenuButton.Location.X - 90, 10),
				Text = Common.MoreFilters,
				AutoSizeWidth = true
			};
			SettingsMenu = new ContextMenuStrip();
			_hideLockedSkinsStripItem?.Dispose();
			_hideLockedSkinsStripItem = SettingsMenu.AddMenuItem(new ContextMenuStripItem(Common.HideLockedSkins));
			_hideLockedSkinsStripItem.CanCheck = true;
			_hideLockedSkinsStripItem.Checked = Model.Filter.HideSkinLocked;
			_hideLockedSkinsStripItem.Click += delegate(object sender, MouseEventArgs args)
			{
				ContextMenuStripItem contextMenuStripItem3 = sender as ContextMenuStripItem;
				if (contextMenuStripItem3 != null)
				{
					Model.Filter.HideSkinLocked = !contextMenuStripItem3.Checked;
					base.Presenter.DoUpdateView();
				}
			};
			_hideUnlockedSkinsStripItem?.Dispose();
			_hideUnlockedSkinsStripItem = SettingsMenu.AddMenuItem(new ContextMenuStripItem(Common.HideUnlockedSkins));
			_hideUnlockedSkinsStripItem.CanCheck = true;
			_hideUnlockedSkinsStripItem.Checked = Model.Filter.HideSkinUnlocked;
			_hideUnlockedSkinsStripItem.Click += delegate(object sender, MouseEventArgs args)
			{
				ContextMenuStripItem contextMenuStripItem2 = sender as ContextMenuStripItem;
				if (contextMenuStripItem2 != null)
				{
					Model.Filter.HideSkinUnlocked = !contextMenuStripItem2.Checked;
					base.Presenter.DoUpdateView();
				}
			};
			_hideMaxCollectedStripItem?.Dispose();
			_hideMaxCollectedStripItem = SettingsMenu.AddMenuItem(new ContextMenuStripItem(Common.HideMaxCollectedItems));
			_hideMaxCollectedStripItem.CanCheck = true;
			_hideMaxCollectedStripItem.Checked = Model.Filter.HideMaxItemsCollected;
			_hideMaxCollectedStripItem.Click += delegate(object sender, MouseEventArgs args)
			{
				ContextMenuStripItem contextMenuStripItem = sender as ContextMenuStripItem;
				if (contextMenuStripItem != null)
				{
					Model.Filter.HideMaxItemsCollected = !contextMenuStripItem.Checked;
					base.Presenter.DoUpdateView();
				}
			};
			SettingsMenuButton.Click += delegate(object sender, MouseEventArgs args)
			{
				SettingsMenu.Show((Control)sender);
			};
		}

		public void BuildNoResultsPanel()
		{
			_noResultsPanel?.ClearChildren();
			_noResultsPanel?.Dispose();
			_noResultsPanel = new Panel
			{
				Parent = Panel,
				Size = Panel.Size,
				Visible = false
			};
			_nothingLabel = new Label
			{
				Text = Common.FavoritesEmpty,
				Parent = _noResultsPanel,
				Font = GameService.Content.DefaultFont32,
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				StrokeText = true,
				Location = new Point(120, 200)
			};
			_nothingImage = new Image
			{
				Texture = ServiceContainer.TextureRepository.Textures.HeartDisabled,
				Parent = _noResultsPanel,
				Size = new Point(50, 50),
				Location = new Point(350, 300)
			};
		}

		public void SetItemRows(IList<ItemRowView> rows)
		{
			Panel.ClearChildren();
			foreach (ItemRowView row in rows)
			{
				ViewContainer viewContainer = new ViewContainer();
				viewContainer.Size = new Point(Panel.Width - 25, 60);
				viewContainer.Parent = Panel;
				viewContainer.Show(row);
			}
		}

		public void SetLimitLabelParent(Container parent)
		{
			if (_limitLabel != null)
			{
				_limitLabel.Parent = parent;
			}
		}
	}
}
