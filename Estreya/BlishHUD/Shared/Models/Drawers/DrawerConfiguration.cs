using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
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
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			config.Enabled.set_Value(Enabled.get_Value());
			config.EnabledKeybinding.set_Value(EnabledKeybinding.get_Value());
			config.Location.X.set_Value(Location.X.get_Value());
			config.Location.Y.set_Value(Location.Y.get_Value());
			config.Size.X.set_Value(Size.X.get_Value());
			config.Size.Y.set_Value(Size.Y.get_Value());
			config.BuildDirection.set_Value(BuildDirection.get_Value());
			config.Opacity.set_Value(Opacity.get_Value());
			config.BackgroundColor.set_Value(BackgroundColor.get_Value());
			config.TextColor.set_Value(TextColor.get_Value());
			config.FontFace.set_Value(FontFace.get_Value());
			config.CustomFontPath.set_Value(CustomFontPath.get_Value());
			config.FontSize.set_Value(FontSize.get_Value());
		}
	}
}
