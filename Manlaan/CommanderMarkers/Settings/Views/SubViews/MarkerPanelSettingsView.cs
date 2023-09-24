using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.Markers;
using Manlaan.CommanderMarkers.Settings.Services;
using Manlaan.CommanderMarkers.Utils;

namespace Manlaan.CommanderMarkers.Settings.Views.SubViews
{
	public class MarkerPanelSettingsView : View
	{
		protected SettingService _settings;

		protected override void Build(Container buildPanel)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			_settings = Service.Settings;
			((View<IPresenter>)this).Build(buildPanel);
			((Container)(object)FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddSetting((SettingEntry)(object)_settings._settingShowMarkersPanel).AddSetting((SettingEntry)(object)_settings._settingOnlyWhenCommander)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings._settingDrag)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings._settingGroundMarkersEnabled)
				.AddSetting((SettingEntry)(object)_settings._settingTargetMarkersEnabled)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings._settingImgWidth)
				.AddSetting((SettingEntry)(object)_settings._settingOpacity)
				.AddSettingEnum((SettingEntry)(object)_settings._settingOrientation)
				.AddSpace()
				.AddString("Preview")
				.AddSpace()).AddControl((Control)(object)new MarkersPanel(Service.Settings, Service.Textures, mouseEventsEnabled: false));
		}

		public MarkerPanelSettingsView()
			: this()
		{
		}
	}
}
