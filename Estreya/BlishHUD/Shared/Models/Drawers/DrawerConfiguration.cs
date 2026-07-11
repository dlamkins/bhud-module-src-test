using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Estreya.BlishHUD.Shared.Extensions;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.Shared.Models.Drawers
{
	public class DrawerConfiguration
	{
		public string Name { get; set; }

		public SettingEntry<bool> Enabled { get; set; }

		public SettingEntry<KeyBinding> EnabledKeybinding { get; set; }

		public DrawerLocation Location { get; set; }

		public DrawerSize Size { get; set; }

		public SettingEntry<BuildDirection> BuildDirection { get; set; }

		public SettingEntry<float> Opacity { get; set; }

		public SettingEntry<Color> BackgroundColor { get; set; }

		public SettingEntry<Color> TextColor { get; set; }

		public SettingEntry<FontFace> FontFace { get; set; }

		public SettingEntry<string> CustomFontPath { get; set; }

		public SettingEntry<FontSize> FontSize { get; set; }

		public void CopyTo(DrawerConfiguration config)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			config.Enabled.set_Value(Enabled.get_Value());
			SettingEntry<KeyBinding> enabledKeybinding = config.EnabledKeybinding;
			KeyBinding val = new KeyBinding();
			val.set_BlockSequenceFromGw2(EnabledKeybinding.get_Value().get_BlockSequenceFromGw2());
			val.set_Enabled(EnabledKeybinding.get_Value().get_Enabled());
			val.set_IgnoreWhenInTextField(EnabledKeybinding.get_Value().get_IgnoreWhenInTextField());
			enabledKeybinding.set_Value(val);
			config.Location.X.set_Value(Location.X.get_Value());
			config.Location.Y.set_Value(Location.Y.get_Value());
			config.Size.X.set_Value(Size.X.get_Value());
			config.Size.Y.set_Value(Size.Y.get_Value());
			config.BuildDirection.set_Value(BuildDirection.get_Value());
			config.Opacity.set_Value(Opacity.get_Value());
			config.BackgroundColor.set_Value(BackgroundColor.get_Value().Copy<Color>());
			config.TextColor.set_Value(TextColor.get_Value().Copy<Color>());
			config.FontFace.set_Value(FontFace.get_Value());
			config.CustomFontPath.set_Value(CustomFontPath.get_Value());
			config.FontSize.set_Value(FontSize.get_Value());
		}
	}
}
