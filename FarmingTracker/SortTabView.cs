using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class SortTabView : View
	{
		private readonly Services _services;

		private FlowPanel? _rootFlowPanel;

		public SortTabView(Services services)
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
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Expected O, but got Unknown
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			val.set_ControlPadding(new Vector2(20f, 10f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Control)val).set_Parent(buildPanel);
			_rootFlowPanel = val;
			CollapsibleHelp collapsibleHelp = new CollapsibleHelp("- only items are sorted, not currencies.\n- custom profit is not taken into account in the profit sorting for now. This may change in the future.\n- multiple sorts can be combined. Example:\n'sort by' positive/negative count, 'then by' rarity, 'then by' name.\nThis will first split the items into gained items (positive count) and lost items (negative count). Then it will sort the gained items by rarity. After that items of same rarity are sorted by name. This process is repeated for the lost items.\n- sort by API ID is similiar to sort by ITEM TYPE. It can sometimes help grouping similiar items (e.g. material storage items).\n- combining multiple sorts can sometimes not produce the expected result. e.g. do not sort by API ID or NAME first. Every item has an unique API ID and an unique NAME. Because of that it will create groups where each group consists of only 1 item. Single item groups cannot be further sorted. Because of that every 'then by' sort after an API ID / NAME sort, will have no effect.", buildPanel.get_ContentRegion().Width - 30, (Container)(object)_rootFlowPanel);
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object s, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				collapsibleHelp.UpdateSize(e.get_CurrentRegion().Width - 30);
			});
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_ControlPadding(new Vector2(0f, 5f));
			((Control)val2).set_Width(500);
			((Container)val2).set_WidthSizingMode((SizingMode)1);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Parent((Container)(object)_rootFlowPanel);
			FlowPanel allSortsFlowPanel = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("+ Add sort");
			((Control)val3).set_Width(90);
			((Control)val3).set_Parent((Container)(object)_rootFlowPanel);
			List<SortPanel> sortPanels = new List<SortPanel>();
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SortByWithDirection sortByWithDirection2 = SortByWithDirection.Name_Ascending;
				AddSingleSortPanel(allSortsFlowPanel, sortPanels, sortByWithDirection2);
				List<SortByWithDirection> list = _services.SettingService.SortByWithDirectionListSetting.get_Value().ToList();
				list.Add(sortByWithDirection2);
				_services.SettingService.SortByWithDirectionListSetting.set_Value(list);
				_services.UpdateLoop.TriggerUpdateUi();
			});
			foreach (SortByWithDirection sortByWithDirection in _services.SettingService.SortByWithDirectionListSetting.get_Value().ToList())
			{
				AddSingleSortPanel(allSortsFlowPanel, sortPanels, sortByWithDirection);
			}
		}

		private void AddSingleSortPanel(FlowPanel allSortsFlowPanel, List<SortPanel> sortPanels, SortByWithDirection sortByWithDirection)
		{
			List<SortPanel> sortPanels2 = sortPanels;
			SortPanel singleSortPanel = new SortPanel((Container)(object)allSortsFlowPanel, sortByWithDirection);
			sortPanels2.Add(singleSortPanel);
			SetThenByOrSortByLabels(sortPanels2);
			singleSortPanel.Dropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				int num2 = sortPanels2.IndexOf(singleSortPanel);
				if (num2 == -1)
				{
					Module.Logger.Error("Removing sort panel failed. Could not find sort panel.");
				}
				else
				{
					List<SortByWithDirection> list2 = _services.SettingService.SortByWithDirectionListSetting.get_Value().ToList();
					list2.RemoveAt(num2);
					list2.Insert(num2, singleSortPanel.GetSelectedSortBy());
					_services.SettingService.SortByWithDirectionListSetting.set_Value(list2);
					_services.UpdateLoop.TriggerUpdateUi();
				}
			});
			((Control)singleSortPanel.RemoveSortButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				int num = sortPanels2.IndexOf(singleSortPanel);
				if (num == -1)
				{
					Module.Logger.Error("Removing sort panel failed. Could not find sort panel.");
				}
				else
				{
					List<SortByWithDirection> list = _services.SettingService.SortByWithDirectionListSetting.get_Value().ToList();
					list.RemoveAt(num);
					_services.SettingService.SortByWithDirectionListSetting.set_Value(list);
					((Control)singleSortPanel).set_Parent((Container)null);
					sortPanels2.Remove(singleSortPanel);
					SetThenByOrSortByLabels(sortPanels2);
					_services.UpdateLoop.TriggerUpdateUi();
				}
			});
		}

		private static void SetThenByOrSortByLabels(List<SortPanel> sortPanels)
		{
			if (!sortPanels.Any())
			{
				return;
			}
			foreach (SortPanel sortPanel in sortPanels)
			{
				sortPanel.SetLabelText("Then by");
			}
			sortPanels[0].SetLabelText("Sort by");
		}
	}
}
