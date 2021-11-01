using System;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;

namespace BhModule.Community.Pathing
{
	public class ModuleSettings
	{
		private const string GLOBAL_SETTINGS = "global-settings";

		private const string PACK_SETTINGS = "pack-settings";

		private const string MAP_SETTINGS = "map-settings";

		private const string KEYBIND_SETTINGS = "keybind-settings";

		public SettingCollection GlobalSettings { get; private set; }

		public SettingEntry<bool> GlobalPathablesEnabled { get; private set; }

		public SettingCollection PackSettings { get; private set; }

		public SettingEntry<bool> PackWorldPathablesEnabled { get; private set; }

		public SettingEntry<float> PackMaxOpacityOverride { get; private set; }

		public SettingEntry<float> PackMaxViewDistance { get; private set; }

		public SettingEntry<float> PackMaxTrailAnimationSpeed { get; private set; }

		public SettingEntry<bool> PackFadeTrailsAroundCharacter { get; private set; }

		public SettingEntry<bool> PackFadePathablesDuringCombat { get; private set; }

		public SettingEntry<bool> PackFadeMarkersBetweenCharacterAndCamera { get; private set; }

		public SettingEntry<bool> PackAllowMarkersToAutomaticallyHide { get; private set; }

		public SettingEntry<MarkerClipboardConsentLevel> PackMarkerConsentToClipboard { get; private set; }

		public SettingEntry<bool> PackAllowMarkersToAnimate { get; private set; }

		public SettingEntry<bool> PackShowCategoriesFromAllMaps { get; private set; }

		public SettingEntry<bool> PackShowWhenCategoriesAreFiltered { get; private set; }

		public SettingCollection MapSettings { get; private set; }

		public SettingEntry<bool> MapPathablesEnabled { get; private set; }

		public SettingEntry<MapVisibilityLevel> MapMarkerVisibilityLevel { get; private set; }

		public SettingEntry<MapVisibilityLevel> MapTrailVisibilityLevel { get; private set; }

		public SettingEntry<MapVisibilityLevel> MiniMapMarkerVisibilityLevel { get; private set; }

		public SettingEntry<MapVisibilityLevel> MiniMapTrailVisibilityLevel { get; private set; }

		public SettingEntry<bool> MapShowAboveBelowIndicators { get; private set; }

		public SettingEntry<bool> MapFadeVerticallyDistantTrailSegments { get; private set; }

		public SettingCollection KeyBindSettings { get; private set; }

		public SettingEntry<KeyBinding> KeyBindTogglePathables { get; private set; }

		public SettingEntry<KeyBinding> KeyBindToggleWorldPathables { get; private set; }

		public SettingEntry<KeyBinding> KeyBindToggleMapPathables { get; private set; }

		public ModuleSettings(SettingCollection settings)
		{
			InitGlobalSettings(settings);
			InitPackSettings(settings);
			InitMapSettings(settings);
			InitKeyBindSettings(settings);
		}

		private void InitGlobalSettings(SettingCollection settings)
		{
			GlobalSettings = settings.AddSubCollection("global-settings", false);
			GlobalPathablesEnabled = GlobalSettings.DefineSetting<bool>("GlobalPathablesEnabled", true, (Func<string>)null, (Func<string>)null);
		}

		private void InitPackSettings(SettingCollection settings)
		{
			PackSettings = settings.AddSubCollection("pack-settings", false);
			PackWorldPathablesEnabled = PackSettings.DefineSetting<bool>("PackWorldPathablesEnabled", true, (Func<string>)(() => "Show Markers in World"), (Func<string>)null);
			PackMaxOpacityOverride = PackSettings.DefineSetting<float>("PackMaxOpacityOverride", 1f, (Func<string>)(() => Strings.Setting_PackMaxOpacityOverride), (Func<string>)(() => ""));
			PackMaxViewDistance = PackSettings.DefineSetting<float>("PackMaxViewDistance", 25000f, (Func<string>)(() => Strings.Setting_PackMaxViewDistance), (Func<string>)(() => ""));
			PackMaxTrailAnimationSpeed = PackSettings.DefineSetting<float>("PackMaxTrailAnimationSpeed", 10f, (Func<string>)(() => Strings.Setting_PackMaxTrailAnimationSpeed), (Func<string>)(() => ""));
			PackFadeTrailsAroundCharacter = PackSettings.DefineSetting<bool>("PackFadeTrailsAroundCharacter", true, (Func<string>)(() => Strings.Setting_PackFadeTrailsAroundCharacter), (Func<string>)(() => ""));
			PackFadePathablesDuringCombat = PackSettings.DefineSetting<bool>("PackFadePathablesDuringCombat", true, (Func<string>)(() => Strings.Setting_PackFadePathablesDuringCombat), (Func<string>)(() => ""));
			PackFadeMarkersBetweenCharacterAndCamera = PackSettings.DefineSetting<bool>("PackFadeMarkersBetweenCharacterAndCamera", true, (Func<string>)(() => Strings.Setting_PackFadeMarkersBetweenCharacterAndCamera), (Func<string>)(() => ""));
			PackAllowMarkersToAutomaticallyHide = PackSettings.DefineSetting<bool>("PackAllowMarkersToAutomaticallyHide", true, (Func<string>)(() => Strings.Setting_PackAllowMarkersToAutomaticallyHide), (Func<string>)(() => ""));
			PackMarkerConsentToClipboard = PackSettings.DefineSetting<MarkerClipboardConsentLevel>("PackMarkerConsentToClipboard", MarkerClipboardConsentLevel.Always, (Func<string>)(() => Strings.Setting_PackMarkerConsentToClipboard), (Func<string>)(() => ""));
			PackAllowMarkersToAnimate = PackSettings.DefineSetting<bool>("PackAllowMarkersToAnimate", true, (Func<string>)(() => Strings.Setting_PackAllowMarkersToAnimate), (Func<string>)(() => ""));
			PackShowCategoriesFromAllMaps = PackSettings.DefineSetting<bool>("PackShowCategoriesFromAllMaps", false, (Func<string>)(() => Strings.Setting_PackShowCategoriesFromAllMaps), (Func<string>)(() => ""));
			PackShowWhenCategoriesAreFiltered = PackSettings.DefineSetting<bool>("PackShowWhenCategoriesAreFiltered", true, (Func<string>)(() => "Indicate when categories are hidden"), (Func<string>)(() => ""));
			SettingComplianceExtensions.SetRange(PackMaxOpacityOverride, 0f, 1f);
			SettingComplianceExtensions.SetRange(PackMaxViewDistance, 25f, 50000f);
			SettingComplianceExtensions.SetRange(PackMaxTrailAnimationSpeed, 0f, 10f);
		}

		private void InitMapSettings(SettingCollection settings)
		{
			MapSettings = settings.AddSubCollection("map-settings", false);
			MapPathablesEnabled = MapSettings.DefineSetting<bool>("MapPathablesEnabled", true, (Func<string>)(() => "Show Markers on Maps"), (Func<string>)null);
			MapMarkerVisibilityLevel = MapSettings.DefineSetting<MapVisibilityLevel>("MapMarkerVisibilityLevel", MapVisibilityLevel.Default, (Func<string>)(() => Strings.Setting_MapShowMarkersOnFullscreen), (Func<string>)(() => ""));
			MapTrailVisibilityLevel = MapSettings.DefineSetting<MapVisibilityLevel>("MapTrailVisibilityLevel", MapVisibilityLevel.Default, (Func<string>)(() => Strings.Setting_MapShowTrailsOnFullscreen), (Func<string>)(() => ""));
			MiniMapMarkerVisibilityLevel = MapSettings.DefineSetting<MapVisibilityLevel>("MiniMapMarkerVisibilityLevel", MapVisibilityLevel.Default, (Func<string>)(() => Strings.Setting_MapShowMarkersOnCompass), (Func<string>)(() => ""));
			MiniMapTrailVisibilityLevel = MapSettings.DefineSetting<MapVisibilityLevel>("MiniMapTrailVisibilityLevel", MapVisibilityLevel.Default, (Func<string>)(() => Strings.Setting_MapShowTrailsOnCompass), (Func<string>)(() => ""));
			MapShowAboveBelowIndicators = MapSettings.DefineSetting<bool>("MapShowAboveBelowIndicators", true, (Func<string>)(() => Strings.Setting_MapShowAboveBelowIndicators), (Func<string>)(() => ""));
			MapFadeVerticallyDistantTrailSegments = MapSettings.DefineSetting<bool>("MapFadeVerticallyDistantTrailSegments", true, (Func<string>)(() => "Fade Trail Segments Which Are High Above or Below"), (Func<string>)null);
		}

		private void InitKeyBindSettings(SettingCollection settings)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			KeyBindSettings = settings.AddSubCollection("keybind-settings", false);
			KeyBindTogglePathables = KeyBindSettings.DefineSetting<KeyBinding>("KeyBindTogglePathables", new KeyBinding((ModifierKeys)6, (Keys)220), "Toggle Markers", "", (SettingTypeRendererDelegate)null);
			KeyBindToggleWorldPathables = KeyBindSettings.DefineSetting<KeyBinding>("KeyBindToggleWorldPathables", new KeyBinding((ModifierKeys)6, (Keys)219), "Toggle Markers in World", "", (SettingTypeRendererDelegate)null);
			KeyBindToggleMapPathables = KeyBindSettings.DefineSetting<KeyBinding>("KeyBindToggleMapPathables", new KeyBinding((ModifierKeys)6, (Keys)221), "Toggle Markers on Map", "", (SettingTypeRendererDelegate)null);
			HandleInternalKeyBinds();
		}

		private void HandleInternalKeyBinds()
		{
			KeyBindTogglePathables.get_Value().set_Enabled(true);
			KeyBindToggleWorldPathables.get_Value().set_Enabled(true);
			KeyBindToggleMapPathables.get_Value().set_Enabled(true);
			KeyBindTogglePathables.get_Value().add_Activated((EventHandler<EventArgs>)async delegate
			{
				GlobalPathablesEnabled.set_Value(!GlobalPathablesEnabled.get_Value());
			});
			KeyBindToggleWorldPathables.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				PackWorldPathablesEnabled.set_Value(!PackWorldPathablesEnabled.get_Value());
			});
			KeyBindToggleMapPathables.get_Value().add_Activated((EventHandler<EventArgs>)delegate
			{
				MapPathablesEnabled.set_Value(!MapPathablesEnabled.get_Value());
			});
		}
	}
}
