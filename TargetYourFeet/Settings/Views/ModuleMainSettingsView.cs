using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using TargetYourFeet.Localization;
using TargetYourFeet.Settings.Services;
using TargetYourFeet.Utils;

namespace TargetYourFeet.Settings.Views
{
	public class ModuleMainSettingsView : View
	{
		private SettingService _settings;

		public ModuleMainSettingsView(SettingService settings)
			: this()
		{
			_settings = settings;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			((View<IPresenter>)this).Build(buildPanel);
			((Panel)FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddString(Strings.DisableDisclaimer, Color.get_Red()).AddSpace()
				.AddSetting((SettingEntry)(object)_settings.KeybindBehaviour)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings.TargetFeetKeybind)
				.AddSpace()
				.AddSpace()
				.AddString("Action cam is NOT supported with \"Single Press\" mode.")
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings.ActionCamInUse)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings.ActionCamKeybind)).set_ShowBorder(false);
		}
	}
}
