using System.Reflection;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;

namespace RaidClears.Settings.Views
{
	public class FixedWidthBoolSettingView : BoolSettingView
	{
		private readonly int _fixedWidth;

		public FixedWidthBoolSettingView(SettingEntry<bool> setting, int definedWidth = -1)
			: this(setting, definedWidth)
		{
			_fixedWidth = definedWidth;
		}

		protected override void BuildSetting(Container buildPanel)
		{
			((BoolSettingView)this).BuildSetting(buildPanel);
			Checkbox checkbox = GetCheckbox();
			if (checkbox != null)
			{
				((Control)checkbox).set_Width(_fixedWidth);
			}
		}

		protected override void RefreshDisplayName(string displayName)
		{
			Checkbox checkbox = GetCheckbox();
			if (checkbox != null)
			{
				checkbox.set_Text(displayName);
			}
		}

		private Checkbox? GetCheckbox()
		{
			object obj = typeof(BoolSettingView).GetField("_boolCheckbox", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(this);
			return (Checkbox?)((obj is Checkbox) ? obj : null);
		}

		public static IView FromSetting(SettingEntry<bool> setting, int definedWidth = -1)
		{
			return (IView)(object)new FixedWidthBoolSettingView(setting, definedWidth);
		}
	}
}
