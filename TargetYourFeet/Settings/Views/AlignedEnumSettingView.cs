using System;
using System.Reflection;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace TargetYourFeet.Settings.Views
{
	public static class AlignedEnumSettingView
	{
		public static IView? FromEnum(SettingEntry setting, int definedWidth = -1)
		{
			object obj = Activator.CreateInstance(typeof(AlignedEnumSettingView<>).MakeGenericType(setting.get_SettingType()), setting, definedWidth);
			return (IView?)((obj is IView) ? obj : null);
		}
	}
	public class AlignedEnumSettingView<TEnum> : EnumSettingView<TEnum> where TEnum : struct, Enum
	{
		protected static int NUMERIC_SLIDER_WIDTH = 277;

		protected static int DROPDOWN_HEIGHT = 27;

		public AlignedEnumSettingView(SettingEntry<TEnum> setting, int definedWidth = -1)
			: base(setting, definedWidth)
		{
		}

		protected override void BuildSetting(Container buildPanel)
		{
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			base.BuildSetting(buildPanel);
			object obj = typeof(EnumSettingView<TEnum>).GetField("_displayNameLabel", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(this);
			Label label = (Label)((obj is Label) ? obj : null);
			if (label != null)
			{
				label.set_AutoSizeWidth(false);
				((Control)label).set_Width(175);
			}
			object obj2 = typeof(EnumSettingView<TEnum>).GetField("_enumDropdown", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(this);
			Dropdown dropdown = (Dropdown)((obj2 is Dropdown) ? obj2 : null);
			if (dropdown != null)
			{
				((Control)dropdown).set_Size(new Point(NUMERIC_SLIDER_WIDTH, DROPDOWN_HEIGHT));
			}
		}
	}
}
