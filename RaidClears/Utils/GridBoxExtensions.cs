using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using RaidClears.Features.Shared.Controls;
using RaidClears.Settings.Enums;

namespace RaidClears.Utils
{
	public static class GridBoxExtensions
	{
		public static void LabelDisplayChange(this GridBox box, SettingEntry<LabelDisplay> labelDisplay, string shortText, string longText)
		{
			GridBox box2 = box;
			string shortText2 = shortText;
			string longText2 = longText;
			EventHandler<ValueChangedEventArgs<LabelDisplay>> settingChangedDelegate = delegate(object _, ValueChangedEventArgs<LabelDisplay> args)
			{
				if (args.get_NewValue() == LabelDisplay.NoLabel)
				{
					((Control)box2).Hide();
				}
				else
				{
					((Control)box2).Show();
					((Label)box2).set_Text(GetLabelText(args.get_NewValue(), shortText2, longText2));
				}
				((Control)((Control)box2).get_Parent()).Invalidate();
			};
			labelDisplay.add_SettingChanged(settingChangedDelegate);
			settingChangedDelegate(null, new ValueChangedEventArgs<LabelDisplay>(labelDisplay.get_Value(), labelDisplay.get_Value()));
		}

		public static void TextColorSetting(this GridBox box, SettingEntry<string> textColor)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			GridBox box2 = box;
			textColor.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object _, ValueChangedEventArgs<string> e)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				((Label)box2).set_TextColor(e.get_NewValue().HexToXnaColor());
			});
			((Label)box2).set_TextColor(textColor.get_Value().HexToXnaColor());
		}

		public static void ConditionalTextColorSetting(this GridBox box, SettingEntry<bool> condition, SettingEntry<string> trueColor, SettingEntry<string> falseColor)
		{
			GridBox box2 = box;
			SettingEntry<string> trueColor2 = trueColor;
			SettingEntry<string> falseColor2 = falseColor;
			SettingEntry<bool> condition2 = condition;
			CalculateConditionalTextColor((Label)(object)box2, condition2.get_Value(), trueColor2.get_Value(), falseColor2.get_Value());
			condition2.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				CalculateConditionalTextColor((Label)(object)box2, e.get_NewValue(), trueColor2.get_Value(), falseColor2.get_Value());
			});
			trueColor2.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object _, ValueChangedEventArgs<string> e)
			{
				CalculateConditionalTextColor((Label)(object)box2, condition2.get_Value(), e.get_NewValue(), falseColor2.get_Value());
			});
			falseColor2.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object _, ValueChangedEventArgs<string> e)
			{
				CalculateConditionalTextColor((Label)(object)box2, condition2.get_Value(), trueColor2.get_Value(), e.get_NewValue());
			});
		}

		public static void VisiblityChanged(this GridBox panel, SettingEntry<bool>? setting)
		{
			GridBox panel2 = panel;
			if (setting == null)
			{
				return;
			}
			setting!.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object _, ValueChangedEventArgs<bool> e)
			{
				((Control)panel2).set_Visible(e.get_NewValue());
				Container parent2 = ((Control)panel2).get_Parent();
				if (parent2 != null)
				{
					((Control)parent2).Invalidate();
				}
			});
			((Control)panel2).set_Visible(setting!.get_Value());
			Container parent = ((Control)panel2).get_Parent();
			if (parent != null)
			{
				((Control)parent).Invalidate();
			}
		}

		private static string GetLabelText(LabelDisplay labelDisplay, string shortText, string longText)
		{
			return labelDisplay switch
			{
				LabelDisplay.NoLabel => "", 
				LabelDisplay.WingNumber => shortText, 
				LabelDisplay.Abbreviation => longText, 
				_ => "-", 
			};
		}

		private static void CalculateConditionalTextColor(Label boxLabel, bool condition, string trueColor, string falseColor)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			boxLabel.set_TextColor(condition ? trueColor.HexToXnaColor() : falseColor.HexToXnaColor());
		}
	}
}
