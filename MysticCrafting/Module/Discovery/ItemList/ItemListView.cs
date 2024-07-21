using System;
using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Discovery.ItemList
{
	public class ItemListView : View<IItemListPresenter>
	{
		private Panel _noResultsPanel;

		private Label _nothingLabel;

		private Image _nothingImage;

		public EventHandler<EventArgs> ItemClicked;

		private Label _limitLabel;

		private ContextMenuStripItem _hideLockedSkinsStripItem;

		private ContextMenuStripItem _hideUnlockedSkinsStripItem;

		private ContextMenuStripItem _hideMaxCollectedStripItem;

		public ViewContainer Container { get; set; }

		public ViewContainer ParentContainer { get; set; }

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
				Label limitLabel = _limitLabel;
				if (limitLabel == null)
				{
					return false;
				}
				return ((Control)limitLabel).get_Visible();
			}
			set
			{
				if (_limitLabel != null)
				{
					((Control)_limitLabel).set_Visible(value);
				}
			}
		}

		public bool NoResults
		{
			get
			{
				Panel noResultsPanel = _noResultsPanel;
				if (noResultsPanel == null)
				{
					return false;
				}
				return ((Control)noResultsPanel).get_Visible();
			}
			set
			{
				if (_noResultsPanel != null)
				{
					((Control)_noResultsPanel).set_Visible(value);
					if (((Control)_noResultsPanel).get_Parent() == null)
					{
						((Control)_noResultsPanel).set_Parent((Container)(object)Panel);
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
			base.WithPresenter((IItemListPresenter)new ItemListPresenter(this, itemListModel));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Expected O, but got Unknown
			BuildPanel = buildPanel;
			ViewContainer val = new ViewContainer();
			val.set_FadeView(true);
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(new Point(((Control)buildPanel).get_Width(), ((Control)buildPanel).get_Height()));
			((Control)val).set_Padding(new Thickness(0f));
			((Control)val).set_Location(new Point(0, 0));
			Container = val;
			BuildRarityFilter();
			BuildAmorWeightFilter();
			BuildMoreFilters();
			BuildLegendaryTypeFilter();
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)Container);
			((Control)val2).set_Location(new Point(0, 40));
			((Control)val2).set_Width(((Control)Container).get_Width() - 5);
			((Control)val2).set_Height(((Control)Container).get_Height() - 50);
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val2).set_CanScroll(true);
			Panel = val2;
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
			FiltersLabel.set_Text($"{Common.MoreFilters} ({enabledCount})");
		}

		public void BuildRarityFilter()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			Dropdown val = new Dropdown();
			((Control)val).set_Parent((Container)(object)Container);
			((Control)val).set_Width(120);
			((Control)val).set_Location(new Point(5, 5));
			((Control)val).set_Visible(true);
			RarityDropdown = val;
			RarityDropdown.get_Items().Add("All rarities");
			RarityDropdown.get_Items().Add("Exotic");
			RarityDropdown.get_Items().Add("Ascended");
			RarityDropdown.get_Items().Add("Legendary");
			if (Model?.Filter == null || Model.Filter.Rarity == ItemRarity.Unknown)
			{
				RarityDropdown.set_SelectedItem("All rarities");
			}
			else
			{
				RarityDropdown.set_SelectedItem(Model?.Filter.Rarity.ToString());
			}
			RarityDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)base.get_Presenter().RarityDropdown_ValueChanged);
		}

		public void BuildLegendaryTypeFilter()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			Dropdown val = new Dropdown();
			((Control)val).set_Parent((Container)(object)Container);
			((Control)val).set_Width(150);
			((Control)val).set_Location(new Point(130, 5));
			((Control)val).set_Visible(true);
			LegendaryTypeDropdown = val;
			LegendaryTypeDropdown.get_Items().Add("All types");
			LegendaryTypeDropdown.get_Items().Add("Perfected Envoy (Raid)");
			LegendaryTypeDropdown.get_Items().Add("Triumphant (WvW)");
			LegendaryTypeDropdown.get_Items().Add("Glorious (PvP)");
			LegendaryTypeDropdown.get_Items().Add("Obsidian (PvE)");
			if (string.IsNullOrEmpty(Model?.Filter?.LegendaryType))
			{
				LegendaryTypeDropdown.set_SelectedItem("All types");
			}
			else
			{
				LegendaryTypeDropdown.set_SelectedItem(Model?.Filter.LegendaryType);
			}
			LegendaryTypeDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object sender, ValueChangedEventArgs args)
			{
				Dropdown val2 = (Dropdown)((sender is Dropdown) ? sender : null);
				if (val2 != null)
				{
					if (val2.get_SelectedItem().Equals("All types", StringComparison.InvariantCultureIgnoreCase))
					{
						Model.Filter.LegendaryType = null;
					}
					else
					{
						Model.Filter.LegendaryType = val2.get_SelectedItem();
					}
					((IPresenter)base.get_Presenter()).DoUpdateView();
				}
			});
		}

		public void BuildAmorWeightFilter()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			Dropdown val = new Dropdown();
			((Control)val).set_Parent((Container)(object)Container);
			((Control)val).set_Width(100);
			((Control)val).set_Location(new Point(480, 5));
			((Control)val).set_Visible(false);
			WeightDropdown = val;
			WeightDropdown.get_Items().Add("All weights");
			WeightDropdown.get_Items().Add("Light");
			WeightDropdown.get_Items().Add("Medium");
			WeightDropdown.get_Items().Add("Heavy");
			WeightDropdown.set_SelectedItem("All weights");
			WeightDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object sender, ValueChangedEventArgs args)
			{
				Dropdown val2 = (Dropdown)((sender is Dropdown) ? sender : null);
				if (val2 != null)
				{
					if (!Enum.IsDefined(typeof(WeightClass), val2.get_SelectedItem()))
					{
						Model.Filter.Weight = WeightClass.Unknown;
					}
					else
					{
						Model.Filter.Weight = (WeightClass)Enum.Parse(typeof(WeightClass), val2.get_SelectedItem());
					}
					((IPresenter)base.get_Presenter()).DoUpdateView();
				}
			});
		}

		public void BuildMoreFilters()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected O, but got Unknown
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Expected O, but got Unknown
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Expected O, but got Unknown
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Expected O, but got Unknown
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Expected O, but got Unknown
			GlowButton val = new GlowButton();
			((Control)val).set_Location(new Point(((Control)Container).get_Width() - 60, 5));
			val.set_Icon(AsyncTexture2D.FromAssetId(157109));
			val.set_ActiveIcon(AsyncTexture2D.FromAssetId(157110));
			((Control)val).set_Visible(true);
			((Control)val).set_BasicTooltipText(Common.MoreFilters);
			((Control)val).set_Parent((Container)(object)Container);
			SettingsMenuButton = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)Container);
			((Control)val2).set_Location(new Point(((Control)SettingsMenuButton).get_Location().X - 90, 10));
			val2.set_Text(Common.MoreFilters);
			val2.set_AutoSizeWidth(true);
			FiltersLabel = val2;
			SettingsMenu = new ContextMenuStrip();
			ContextMenuStripItem hideLockedSkinsStripItem = _hideLockedSkinsStripItem;
			if (hideLockedSkinsStripItem != null)
			{
				((Control)hideLockedSkinsStripItem).Dispose();
			}
			_hideLockedSkinsStripItem = SettingsMenu.AddMenuItem(new ContextMenuStripItem(Common.HideLockedSkins));
			_hideLockedSkinsStripItem.set_CanCheck(true);
			_hideLockedSkinsStripItem.set_Checked(Model.Filter.HideSkinLocked);
			((Control)_hideLockedSkinsStripItem).add_Click((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs args)
			{
				ContextMenuStripItem val5 = (ContextMenuStripItem)((sender is ContextMenuStripItem) ? sender : null);
				if (val5 != null)
				{
					Model.Filter.HideSkinLocked = !val5.get_Checked();
					((IPresenter)base.get_Presenter()).DoUpdateView();
				}
			});
			ContextMenuStripItem hideUnlockedSkinsStripItem = _hideUnlockedSkinsStripItem;
			if (hideUnlockedSkinsStripItem != null)
			{
				((Control)hideUnlockedSkinsStripItem).Dispose();
			}
			_hideUnlockedSkinsStripItem = SettingsMenu.AddMenuItem(new ContextMenuStripItem(Common.HideUnlockedSkins));
			_hideUnlockedSkinsStripItem.set_CanCheck(true);
			_hideUnlockedSkinsStripItem.set_Checked(Model.Filter.HideSkinUnlocked);
			((Control)_hideUnlockedSkinsStripItem).add_Click((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs args)
			{
				ContextMenuStripItem val4 = (ContextMenuStripItem)((sender is ContextMenuStripItem) ? sender : null);
				if (val4 != null)
				{
					Model.Filter.HideSkinUnlocked = !val4.get_Checked();
					((IPresenter)base.get_Presenter()).DoUpdateView();
				}
			});
			ContextMenuStripItem hideMaxCollectedStripItem = _hideMaxCollectedStripItem;
			if (hideMaxCollectedStripItem != null)
			{
				((Control)hideMaxCollectedStripItem).Dispose();
			}
			_hideMaxCollectedStripItem = SettingsMenu.AddMenuItem(new ContextMenuStripItem(Common.HideMaxCollectedItems));
			_hideMaxCollectedStripItem.set_CanCheck(true);
			_hideMaxCollectedStripItem.set_Checked(Model.Filter.HideMaxItemsCollected);
			((Control)_hideMaxCollectedStripItem).add_Click((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs args)
			{
				ContextMenuStripItem val3 = (ContextMenuStripItem)((sender is ContextMenuStripItem) ? sender : null);
				if (val3 != null)
				{
					Model.Filter.HideMaxItemsCollected = !val3.get_Checked();
					((IPresenter)base.get_Presenter()).DoUpdateView();
				}
			});
			((Control)SettingsMenuButton).add_Click((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs args)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				SettingsMenu.Show((Control)sender);
			});
		}

		public void BuildNoResultsPanel()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Expected O, but got Unknown
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Expected O, but got Unknown
			Panel noResultsPanel = _noResultsPanel;
			if (noResultsPanel != null)
			{
				((Container)noResultsPanel).ClearChildren();
			}
			Panel noResultsPanel2 = _noResultsPanel;
			if (noResultsPanel2 != null)
			{
				((Control)noResultsPanel2).Dispose();
			}
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)Panel);
			((Control)val).set_Size(((Control)Panel).get_Size());
			((Control)val).set_Visible(false);
			_noResultsPanel = val;
			Label val2 = new Label();
			val2.set_Text(Common.FavoritesEmpty);
			((Control)val2).set_Parent((Container)(object)_noResultsPanel);
			val2.set_Font(GameService.Content.get_DefaultFont32());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			val2.set_StrokeText(true);
			((Control)val2).set_Location(new Point(120, 200));
			_nothingLabel = val2;
			Image val3 = new Image();
			val3.set_Texture(ServiceContainer.TextureRepository.Textures.HeartDisabled);
			((Control)val3).set_Parent((Container)(object)_noResultsPanel);
			((Control)val3).set_Size(new Point(50, 50));
			((Control)val3).set_Location(new Point(350, 300));
			_nothingImage = val3;
		}

		public void SetItemRows(IList<ItemRowView> rows)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			((Container)Panel).ClearChildren();
			foreach (ItemRowView row in rows)
			{
				ViewContainer val = new ViewContainer();
				((Control)val).set_Size(new Point(((Control)Panel).get_Width() - 25, 60));
				((Control)val).set_Parent((Container)(object)Panel);
				val.Show((IView)(object)row);
			}
		}

		public void SetLimitLabelParent(Container parent)
		{
			if (_limitLabel != null)
			{
				((Control)_limitLabel).set_Parent(parent);
			}
		}
	}
}
