using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class FilterTabView : View
	{
		private readonly Services _services;

		private FlowPanel? _rootFlowPanel;

		private const string MATCH_MULTIPLE_OPTION_HINT = "Some items match several of these options. These items are only hidden\nif all matching options are unselected.";

		public FilterTabView(Services services)
			: this()
		{
			_services = services;
		}

		protected override void Unload()
		{
			FlowPanel? rootFlowPanel = _rootFlowPanel;
			if (rootFlowPanel != null)
			{
				((Control)rootFlowPanel).Dispose();
			}
			_rootFlowPanel = null;
			((View<IPresenter>)this).Unload();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected O, but got Unknown
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Expected O, but got Unknown
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			val.set_ControlPadding(new Vector2(20f, 10f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Control)val).set_Parent(buildPanel);
			_rootFlowPanel = val;
			CollapsibleHelp collapsibleHelp = new CollapsibleHelp("- Checked = visible.\n- Unchecked = hidden by filter.\n- Items hidden by filters are still included in the profit calculation and are still shown in the 'Favorite Items' panel.\n- A filter, e.g. rarity filter, will not be applied if all its checkboxes are unchecked. In this case no items will be hidden by the filter.\n- filter icon on filter panel header:\nTRANSPARENT: filter wont hide stats.\nOPAQUE: filter will hide stats.\n- expand/collapse panels: for a better overview expand/collapse the filter panels by using the expand/collapse-all-buttons or by clicking on the filter panel headers.", buildPanel.get_ContentRegion().Width - 30, (Container)(object)_rootFlowPanel);
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object s, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				collapsibleHelp.UpdateSize(e.get_CurrentRegion().Width - 30);
			});
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)2);
			val2.set_ControlPadding(new Vector2(5f, 0f));
			((Container)val2).set_WidthSizingMode((SizingMode)1);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Parent((Container)(object)_rootFlowPanel);
			FlowPanel buttonFlowPanel = val2;
			List<FlowPanel> filterPanels = new List<FlowPanel>();
			StandardButton val3 = new StandardButton();
			val3.set_Text("Expand all");
			((Control)val3).set_Width(90);
			((Control)val3).set_Parent((Container)(object)buttonFlowPanel);
			StandardButton expandAllButton = val3;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Collapse all");
			((Control)val4).set_Width(90);
			((Control)val4).set_Parent((Container)(object)buttonFlowPanel);
			((Control)expandAllButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				foreach (FlowPanel item in filterPanels)
				{
					((Panel)item).Expand();
				}
			});
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				foreach (FlowPanel item2 in filterPanels)
				{
					((Panel)item2).Collapse();
				}
			});
			filterPanels.Add(CreateFilterSettingPanel("Count (items & currencies)", Constants.ALL_COUNTS, _services.SettingService.CountFilterSetting, _services, (Container)(object)_rootFlowPanel, "Coin will never be hidden."));
			filterPanels.Add(CreateFilterSettingPanel("Sell Methods (items & currencies)", Constants.ALL_SELL_METHODS, _services.SettingService.SellMethodFilterSetting, _services, (Container)(object)_rootFlowPanel, "Some items match several of these options. These items are only hidden\nif all matching options are unselected.\nRaw gold (coin) will always be visible regardless of which of these filters\nare active or not."));
			filterPanels.Add(CreateFilterSettingPanel("Rarity (items)", Constants.ALL_ITEM_RARITIES, _services.SettingService.RarityStatsFilterSetting, _services, (Container)(object)_rootFlowPanel));
			filterPanels.Add(CreateFilterSettingPanel("Type (items)", Constants.ALL_ITEM_TYPES, _services.SettingService.TypeStatsFilterSetting, _services, (Container)(object)_rootFlowPanel));
			filterPanels.Add(CreateFilterSettingPanel("Flag (items)", Constants.ALL_ITEM_FLAGS, _services.SettingService.FlagStatsFilterSetting, _services, (Container)(object)_rootFlowPanel, "Some items match several of these options. These items are only hidden\nif all matching options are unselected."));
			filterPanels.Add(CreateFilterSettingPanel("Currencies", Constants.ALL_CURRENCIES, _services.SettingService.CurrencyFilterSetting, _services, (Container)(object)_rootFlowPanel));
			filterPanels.Add(CreateFilterSettingPanel("GW2 API (items & currencies)", Constants.ALL_KNOWN_BY_API, _services.SettingService.KnownByApiFilterSetting, _services, (Container)(object)_rootFlowPanel, "Coin will never be hidden. Some items like the lvl-80-boost or\ncertain reknown heart items are not known by the GW2 API."));
		}

		private static FlowPanel CreateFilterSettingPanel<T>(string panelTitel, T[] allPossibleFilterElements, SettingEntry<List<T>> filterSettingEntry, Services services, Container parent, string hintText = "") where T : notnull
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Expected O, but got Unknown
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Expected O, but got Unknown
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Expected O, but got Unknown
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Expected O, but got Unknown
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Expected O, but got Unknown
			T[] allPossibleFilterElements2 = allPossibleFilterElements;
			SettingEntry<List<T>> filterSettingEntry2 = filterSettingEntry;
			Services services2 = services;
			Panel val = new Panel();
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
			Panel filterIconPanel = val;
			FlowPanel val2 = new FlowPanel();
			((Panel)val2).set_Title(panelTitel);
			((Panel)val2).set_ShowBorder(true);
			((Panel)val2).set_CanCollapse(true);
			((Control)val2).set_BackgroundColor(Color.get_Black() * 0.5f);
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_OuterControlPadding(new Vector2(10f, 10f));
			val2.set_ControlPadding(new Vector2(0f, 10f));
			((Control)val2).set_Width(500);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Parent((Container)(object)filterIconPanel);
			FlowPanel filterFlowPanel = val2;
			ClickThroughImage filterIcon = new ClickThroughImage(services2.TextureService.FilterTabIconTexture, new Point(380, 3), (Container)(object)filterIconPanel);
			if (!string.IsNullOrWhiteSpace(hintText))
			{
				new HintLabel((Container?)(object)filterFlowPanel, hintText);
			}
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)2);
			val3.set_ControlPadding(new Vector2(10f, 0f));
			((Container)val3).set_WidthSizingMode((SizingMode)1);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Parent((Container)(object)filterFlowPanel);
			FlowPanel buttonFlowPanel = val3;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Show all");
			((Control)val4).set_Width(80);
			((Control)val4).set_Parent((Container)(object)buttonFlowPanel);
			StandardButton showAllButton = val4;
			StandardButton val5 = new StandardButton();
			val5.set_Text("Hide all");
			((Control)val5).set_Width(80);
			((Control)val5).set_Parent((Container)(object)buttonFlowPanel);
			StandardButton hideAllButton = val5;
			List<Checkbox> filterCheckboxes = new List<Checkbox>();
			List<T> selectedFilterElements = filterSettingEntry2.get_Value().ToList();
			UpdateOpacity(filterIcon, selectedFilterElements, allPossibleFilterElements2);
			T[] array = allPossibleFilterElements2;
			for (int i = 0; i < array.Length; i++)
			{
				T filterElement = array[i];
				Checkbox val6 = new Checkbox();
				val6.set_Text(Helper.ConvertEnumValueToTextWithBlanks(filterElement.ToString()));
				val6.set_Checked(selectedFilterElements.Contains(filterElement));
				((Control)val6).set_Parent((Container)(object)filterFlowPanel);
				Checkbox filterCheckbox = val6;
				filterCheckboxes.Add(filterCheckbox);
				filterCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
				{
					if (filterCheckbox.get_Checked())
					{
						if (!selectedFilterElements.Contains(filterElement))
						{
							selectedFilterElements.Add(filterElement);
						}
					}
					else if (selectedFilterElements.Contains(filterElement))
					{
						selectedFilterElements.Remove(filterElement);
					}
					UpdateOpacity(filterIcon, selectedFilterElements, allPossibleFilterElements2);
					filterSettingEntry2.set_Value(selectedFilterElements);
					services2.UpdateLoop.TriggerUpdateUi();
				});
			}
			((Control)hideAllButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				foreach (Checkbox item in filterCheckboxes)
				{
					item.set_Checked(false);
				}
			});
			((Control)showAllButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				foreach (Checkbox item2 in filterCheckboxes)
				{
					item2.set_Checked(true);
				}
			});
			return filterFlowPanel;
		}

		private static void UpdateOpacity<T>(ClickThroughImage filterIcon, List<T> selectedFilterElements, T[] allPossibleFilterElements)
		{
			bool num = !selectedFilterElements.Any();
			bool allSelected = selectedFilterElements.Count == allPossibleFilterElements.Count();
			bool filterIsInactive = num || allSelected;
			filterIcon.SetOpacity(filterIsInactive);
		}
	}
}
