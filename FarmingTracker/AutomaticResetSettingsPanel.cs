using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class AutomaticResetSettingsPanel : FlowPanel
	{
		private const int DOES_NOT_MATTER = 60;

		private readonly Services _services;

		private Panel _resetMinutesPanel;

		public AutomaticResetSettingsPanel(Container parent, Services services)
			: this()
		{
			_services = services;
			CreateAutomaticResetDropDown(parent, services.SettingService);
			CreateMinutesDropDown(parent, services.SettingService);
		}

		private static void CreateAutomaticResetDropDown(Container parent, SettingService settingService)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Expected O, but got Unknown
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_BasicTooltipText(((SettingEntry)settingService.AutomaticResetSetting).get_GetDescriptionFunc()());
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
			Panel automaticResetPanel = val;
			Label val2 = new Label();
			val2.set_Text(((SettingEntry)settingService.AutomaticResetSetting).get_GetDisplayNameFunc()());
			((Control)val2).set_BasicTooltipText(((SettingEntry)settingService.AutomaticResetSetting).get_GetDescriptionFunc()());
			((Control)val2).set_Location(new Point(5, 4));
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Parent((Container)(object)automaticResetPanel);
			Label automaticResetLabel = val2;
			Dropdown val3 = new Dropdown();
			((Control)val3).set_BasicTooltipText(((SettingEntry)settingService.AutomaticResetSetting).get_GetDescriptionFunc()());
			((Control)val3).set_Location(new Point(((Control)automaticResetLabel).get_Right() + 5, 0));
			((Control)val3).set_Width(370);
			((Control)val3).set_Parent((Container)(object)automaticResetPanel);
			Dropdown automaticResetDropDown = val3;
			Dictionary<AutomaticReset, string> dropDownTextDict = GetDropDownTextsForAutomaticResetSetting();
			foreach (string dropDownText in dropDownTextDict.Values)
			{
				automaticResetDropDown.get_Items().Add(dropDownText);
			}
			automaticResetDropDown.set_SelectedItem(dropDownTextDict[settingService.AutomaticResetSetting.get_Value()]);
			automaticResetDropDown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object s, ValueChangedEventArgs e)
			{
				settingService.AutomaticResetSetting.set_Value(dropDownTextDict.First((KeyValuePair<AutomaticReset, string> d) => d.Value == e.get_CurrentValue()).Key);
			});
		}

		private void CreateMinutesDropDown(Container parent, SettingService settingService)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_BasicTooltipText(((SettingEntry)settingService.MinutesUntilResetAfterModuleShutdownSetting).get_GetDescriptionFunc()());
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
			_resetMinutesPanel = val;
			Dropdown val2 = new Dropdown();
			((Control)val2).set_BasicTooltipText(((SettingEntry)settingService.MinutesUntilResetAfterModuleShutdownSetting).get_GetDescriptionFunc()());
			((Control)val2).set_Location(new Point(5, 0));
			((Control)val2).set_Width(60);
			((Control)val2).set_Parent((Container)(object)_resetMinutesPanel);
			Dropdown minutesDropDown = val2;
			Label val3 = new Label();
			val3.set_Text(((SettingEntry)settingService.MinutesUntilResetAfterModuleShutdownSetting).get_GetDisplayNameFunc()());
			((Control)val3).set_BasicTooltipText(((SettingEntry)settingService.MinutesUntilResetAfterModuleShutdownSetting).get_GetDescriptionFunc()());
			((Control)val3).set_Location(new Point(((Control)minutesDropDown).get_Right() + 5, 4));
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Parent((Container)(object)_resetMinutesPanel);
			foreach (string dropDownValue in new List<int>
			{
				15, 30, 45, 60, 90, 120, 180, 240, 360, 480,
				600, 720, 840, 960, 1080, 1200
			}.Select((int m) => m.ToString()))
			{
				minutesDropDown.get_Items().Add(dropDownValue);
			}
			minutesDropDown.set_SelectedItem(settingService.MinutesUntilResetAfterModuleShutdownSetting.get_Value().ToString());
			minutesDropDown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				settingService.MinutesUntilResetAfterModuleShutdownSetting.set_Value(int.Parse(minutesDropDown.get_SelectedItem()));
			});
			settingService.AutomaticResetSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<AutomaticReset>>)AutomaticResetSettingChanged);
			AutomaticResetSettingChanged();
		}

		protected override void DisposeControl()
		{
			_services.SettingService.AutomaticResetSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<AutomaticReset>>)AutomaticResetSettingChanged);
			((FlowPanel)this).DisposeControl();
		}

		private void AutomaticResetSettingChanged(object sender = null, ValueChangedEventArgs<AutomaticReset> e = null)
		{
			((Control)_resetMinutesPanel).set_Opacity((_services.SettingService.AutomaticResetSetting.get_Value() == AutomaticReset.MinutesAfterModuleShutdown) ? 1f : 0f);
		}

		private static Dictionary<AutomaticReset, string> GetDropDownTextsForAutomaticResetSetting()
		{
			return new Dictionary<AutomaticReset, string>
			{
				[AutomaticReset.Never] = "Never (click reset button for manual reset)",
				[AutomaticReset.OnModuleStart] = "On module start",
				[AutomaticReset.OnDailyReset] = "On daily reset (" + GetNextDailyResetLocalTime() + ")",
				[AutomaticReset.OnWeeklyReset] = "On weekly reset (" + GetNextWeeklyResetInLocalTime(AutomaticReset.OnWeeklyReset) + ")",
				[AutomaticReset.OnWeeklyNaWvwReset] = "On weekly NA WvW reset (" + GetNextWeeklyResetInLocalTime(AutomaticReset.OnWeeklyNaWvwReset) + ")",
				[AutomaticReset.OnWeeklyEuWvwReset] = "On weekly EU WvW reset (" + GetNextWeeklyResetInLocalTime(AutomaticReset.OnWeeklyEuWvwReset) + ")",
				[AutomaticReset.OnWeeklyMapBonusRewardsReset] = "On weekly map bonus rewards reset (" + GetNextWeeklyResetInLocalTime(AutomaticReset.OnWeeklyMapBonusRewardsReset) + ")",
				[AutomaticReset.MinutesAfterModuleShutdown] = "Minutes after module shutdown (change minutes below)"
			};
		}

		private static string GetNextWeeklyResetInLocalTime(AutomaticReset automaticReset)
		{
			return NextAutomaticResetCalculator.GetNextResetDateTimeUtc(DateTimeService.UtcNow, automaticReset, 60).ToLocalTime().ToString("dddd HH:mm");
		}

		private static string GetNextDailyResetLocalTime()
		{
			return NextAutomaticResetCalculator.GetNextResetDateTimeUtc(DateTimeService.UtcNow, AutomaticReset.OnDailyReset, 60).ToLocalTime().ToString("HH:mm");
		}
	}
}
