using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class DateTimeDebugPanelService
	{
		private const string DEBUG_DATE_FORMAT = "ddd dd:MM:yyyy HH:mm:ss";

		private const int DOES_NOT_MATTER = 60;

		public static void CreateDateTimeDebugPanel(Container parent, SettingEntry<bool> debugDateTimeEnabledSetting, SettingEntry<DateTime> debugDateTimeValueSetting)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Expected O, but got Unknown
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Panel)val).set_Title("DateTime Debug");
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(5f, 5f));
			((Panel)val).set_ShowBorder(true);
			((Control)val).set_BackgroundColor(Color.get_Black() * 0.3f);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
			FlowPanel debugDateTimeFlowPanel = val;
			new SettingControl((Container)(object)debugDateTimeFlowPanel, (SettingEntry)(object)debugDateTimeEnabledSetting);
			TextBox val2 = new TextBox();
			((TextInputBase)val2).set_Text(debugDateTimeValueSetting.get_Value().ToString());
			((Control)val2).set_BasicTooltipText("new debug UTC for date time mocking. mainly for reset");
			((Control)val2).set_Width(200);
			((Control)val2).set_Parent((Container)(object)debugDateTimeFlowPanel);
			TextBox dateTimeTextBox = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_BasicTooltipText("click to apply as new debug UTC");
			((Control)val3).set_Width(210);
			((Control)val3).set_Parent((Container)(object)debugDateTimeFlowPanel);
			StandardButton setDateTimeButton = val3;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Update textbox with real UTC");
			((Control)val4).set_BasicTooltipText("This just replaces the text in the textbox with the real current UTC. it does NOT start mocking with this time. press other button for that");
			((Control)val4).set_Width(210);
			((Control)val4).set_Parent((Container)(object)debugDateTimeFlowPanel);
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((TextInputBase)dateTimeTextBox).set_Text(DateTime.UtcNow.ToString());
			});
			((Control)setDateTimeButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				DateTimeService.UtcNow = debugDateTimeValueSetting.get_Value();
			});
			debugDateTimeEnabledSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				((Control)setDateTimeButton).set_Enabled(debugDateTimeEnabledSetting.get_Value());
			});
			((Control)setDateTimeButton).set_Enabled(debugDateTimeEnabledSetting.get_Value());
			Dictionary<AutomaticReset, Label> resetDateTimelabelDict = new Dictionary<AutomaticReset, Label>();
			List<AutomaticReset> dateTimeBasedAutomaticResets = (from AutomaticReset a in Enum.GetValues(typeof(AutomaticReset))
				where a != 0 && a != AutomaticReset.OnModuleStart
				select a).ToList();
			foreach (AutomaticReset automaticReset in dateTimeBasedAutomaticResets)
			{
				Dictionary<AutomaticReset, Label> dictionary = resetDateTimelabelDict;
				AutomaticReset key = automaticReset;
				Label val5 = new Label();
				val5.set_Text(automaticReset.ToString());
				val5.set_AutoSizeHeight(true);
				val5.set_AutoSizeWidth(true);
				((Control)val5).set_Parent((Container)(object)debugDateTimeFlowPanel);
				dictionary[key] = val5;
			}
			((TextInputBase)dateTimeTextBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				if (DateTime.TryParse(((TextInputBase)dateTimeTextBox).get_Text(), out var result))
				{
					debugDateTimeValueSetting.set_Value(result);
				}
				UpdateDateTimeButtonText(setDateTimeButton, ((TextInputBase)dateTimeTextBox).get_Text());
				UpdateResetDateTimeLabels(resetDateTimelabelDict, dateTimeBasedAutomaticResets, ((TextInputBase)dateTimeTextBox).get_Text());
			});
			UpdateDateTimeButtonText(setDateTimeButton, ((TextInputBase)dateTimeTextBox).get_Text());
			UpdateResetDateTimeLabels(resetDateTimelabelDict, dateTimeBasedAutomaticResets, ((TextInputBase)dateTimeTextBox).get_Text());
		}

		private static void UpdateResetDateTimeLabels(Dictionary<AutomaticReset, Label> resetDateTimelabelDict, List<AutomaticReset> dateTimeBasedAutomaticResets, string dateTimeTextBoxText)
		{
			if (!DateTime.TryParse(dateTimeTextBoxText, out var dateTimeUtc))
			{
				return;
			}
			foreach (AutomaticReset automaticReset in dateTimeBasedAutomaticResets)
			{
				string dateTimeText = NextAutomaticResetCalculator.GetNextResetDateTimeUtc(dateTimeUtc, automaticReset, 60).ToString("ddd dd:MM:yyyy HH:mm:ss");
				resetDateTimelabelDict[automaticReset].set_Text($"{dateTimeText} {automaticReset}");
			}
		}

		private static void UpdateDateTimeButtonText(StandardButton button, string dateTimeText)
		{
			if (DateTime.TryParse(dateTimeText, out var mockedDateTimeUtc))
			{
				button.set_Text("SET " + mockedDateTimeUtc.ToString("ddd dd:MM:yyyy HH:mm:ss"));
			}
		}
	}
}
