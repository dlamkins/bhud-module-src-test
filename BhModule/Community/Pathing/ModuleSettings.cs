using System;
using Blish_HUD.Common.Gw2;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;

namespace BhModule.Community.Pathing
{
	public class ModuleSettings
	{
		private readonly PathingModule _module;

		private const string GLOBAL_SETTINGS = "global-settings";

		private const string PACK_SETTINGS = "pack-settings";

		private const string MAP_SETTINGS = "map-settings";

		private const string SCRIPT_SETTINGS = "script-settings";

		private const string KEYBIND_SETTINGS = "keybind-settings";

		public SettingCollection GlobalSettings { get; private set; }

		public SettingEntry<bool> GlobalPathablesEnabled { get; private set; }

		public SettingCollection PackSettings { get; private set; }

		public SettingEntry<bool> PackWorldPathablesEnabled { get; private set; }

		public SettingEntry<float> PackMaxOpacityOverride { get; private set; }

		public SettingEntry<float> PackMaxViewDistance { get; private set; }

		public SettingEntry<float> PackMaxTrailAnimationSpeed { get; private set; }

		public SettingEntry<float> PackMarkerScale { get; private set; }

		public SettingEntry<bool> PackFadeTrailsAroundCharacter { get; private set; }

		public SettingEntry<bool> PackFadePathablesDuringCombat { get; private set; }

		public SettingEntry<bool> PackFadeMarkersBetweenCharacterAndCamera { get; private set; }

		public SettingEntry<bool> PackAllowMarkersToAutomaticallyHide { get; private set; }

		public SettingEntry<MarkerClipboardConsentLevel> PackMarkerConsentToClipboard { get; private set; }

		public SettingEntry<MarkerInfoDisplayMode> PackInfoDisplayMode { get; private set; }

		public SettingEntry<bool> PackAllowInfoText { get; private set; }

		public SettingEntry<bool> PackAllowInteractIcon { get; private set; }

		public SettingEntry<bool> PackAllowMarkersToAnimate { get; private set; }

		public SettingEntry<bool> PackEnableSmartCategoryFilter { get; private set; }

		public SettingEntry<bool> PackShowWhenCategoriesAreFiltered { get; private set; }

		public SettingEntry<bool> PackTruncateLongCategoryNames { get; private set; }

		public SettingEntry<bool> PackShowHiddenMarkersReducedOpacity { get; private set; }

		public SettingEntry<bool> PackShowTooltipsOnAchievements { get; private set; }

		public SettingCollection MapSettings { get; private set; }

		public SettingEntry<bool> MapPathablesEnabled { get; private set; }

		public SettingEntry<MapVisibilityLevel> MapMarkerVisibilityLevel { get; private set; }

		public SettingEntry<MapVisibilityLevel> MapTrailVisibilityLevel { get; private set; }

		public SettingEntry<float> MapDrawOpacity { get; private set; }

		public SettingEntry<MapVisibilityLevel> MiniMapMarkerVisibilityLevel { get; private set; }

		public SettingEntry<MapVisibilityLevel> MiniMapTrailVisibilityLevel { get; private set; }

		public SettingEntry<float> MiniMapDrawOpacity { get; private set; }

		public SettingEntry<bool> MapShowAboveBelowIndicators { get; private set; }

		public SettingEntry<bool> MapFadeVerticallyDistantTrailSegments { get; private set; }

		public SettingEntry<float> MapTrailWidth { get; private set; }

		public SettingEntry<bool> MapShowTooltip { get; private set; }

		public SettingCollection ScriptSettings { get; private set; }

		public SettingEntry<bool> ScriptsEnabled { get; private set; }

		public SettingEntry<bool> ScriptsConsoleEnabled { get; private set; }

		public SettingCollection KeyBindSettings { get; private set; }

		public SettingEntry<KeyBinding> KeyBindTogglePathables { get; private set; }

		public SettingEntry<KeyBinding> KeyBindToggleWorldPathables { get; private set; }

		public SettingEntry<KeyBinding> KeyBindToggleMapPathables { get; private set; }

		public SettingEntry<KeyBinding> KeyBindReloadMarkerPacks { get; private set; }

		public ModuleSettings(PathingModule module, SettingCollection settings)
		{
			_module = module;
			InitGlobalSettings(settings);
			InitPackSettings(settings);
			InitMapSettings(settings);
			InitScriptSettings(settings);
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
			PackMarkerScale = PackSettings.DefineSetting<float>("PackMarkerScale", 1f, (Func<string>)(() => "Marker Scale"), (Func<string>)(() => "Modifies the size of markers in the world."));
			PackFadeTrailsAroundCharacter = PackSettings.DefineSetting<bool>("PackFadeTrailsAroundCharacter", true, (Func<string>)(() => Strings.Setting_PackFadeTrailsAroundCharacter), (Func<string>)(() => "If enabled, trails will be faded out around your character to make it easier to see your character."));
			PackFadePathablesDuringCombat = PackSettings.DefineSetting<bool>("PackFadePathablesDuringCombat", true, (Func<string>)(() => Strings.Setting_PackFadePathablesDuringCombat), (Func<string>)(() => "If enabled, markers and trails will be hidden while you're in combat to avoid obscuring the fight."));
			PackFadeMarkersBetweenCharacterAndCamera = PackSettings.DefineSetting<bool>("PackFadeMarkersBetweenCharacterAndCamera", true, (Func<string>)(() => Strings.Setting_PackFadeMarkersBetweenCharacterAndCamera), (Func<string>)(() => "If enabled, markers will be drawn with less opacity if they are directly between your character and the camera to avoid obscuring your vision."));
			PackAllowMarkersToAutomaticallyHide = PackSettings.DefineSetting<bool>("PackAllowMarkersToAutomaticallyHide", true, (Func<string>)(() => Strings.Setting_PackAllowMarkersToAutomaticallyHide), (Func<string>)(() => "If enabled, markers and trails may hide themselves as a result of interactions, API data, current festival, etc.  Disabling this feature forces all markers on the map to be shown."));
			PackMarkerConsentToClipboard = PackSettings.DefineSetting<MarkerClipboardConsentLevel>("PackMarkerConsentToClipboard", MarkerClipboardConsentLevel.Always, (Func<string>)(() => Strings.Setting_PackMarkerConsentToClipboard), (Func<string>)(() => string.Format(Strings.Setting_PackMarkerConsentToClipboardDescription, KeyBindings.Interact.GetBindingDisplayText())));
			PackInfoDisplayMode = PackSettings.DefineSetting<MarkerInfoDisplayMode>("PackInfoDisplayMode", MarkerInfoDisplayMode.Default, (Func<string>)(() => "Marker Info Display Mode"), (Func<string>)(() => "Default - Popups with extra info will show when you are near certain markers.\r\n\r\nWithout Background - Extra info will show when you are near certain markers, but there won't be a background behind the text.\r\n\r\nNever Display - Markers will not show popup info text on your screen."));
			PackAllowInteractIcon = PackSettings.DefineSetting<bool>("PackAllowInteractIcon", true, (Func<string>)(() => "Allow Markers to Show Interact Gear On-Screen"), (Func<string>)(() => "If enabled, interactable markers will show a small gear icon on-screen to show what the interaction will do."));
			PackAllowMarkersToAnimate = PackSettings.DefineSetting<bool>("PackAllowMarkersToAnimate", true, (Func<string>)(() => Strings.Setting_PackAllowMarkersToAnimate), (Func<string>)(() => "Allows animations such as 'bounce' and trail movements."));
			PackEnableSmartCategoryFilter = PackSettings.DefineSetting<bool>("PackEnableSmartCategoryFilter", true, (Func<string>)(() => "Enable Smart Categories"), (Func<string>)(() => "If a category doesn't contain markers or trails relevant to the current map, the category is hidden."));
			PackShowWhenCategoriesAreFiltered = PackSettings.DefineSetting<bool>("PackShowWhenCategoriesAreFiltered", true, (Func<string>)(() => "Indicate When Categories Are Hidden"), (Func<string>)(() => "Shows a note at the bottom of the menu indicating if categories have been hidden.  Clicking the note will show the hidden categories temporarily."));
			PackTruncateLongCategoryNames = PackSettings.DefineSetting<bool>("PackTruncateLongCategoryNames", false, (Func<string>)(() => "Truncate Long Category Names"), (Func<string>)(() => "Shortens long category names so that more nested menus can be shown on screen."));
			PackShowHiddenMarkersReducedOpacity = PackSettings.DefineSetting<bool>("PackShowHiddenMarkersReducedOpacity", false, (Func<string>)(() => "Temporarily Show Ghost Markers"), (Func<string>)(() => "Shows hidden markers with a reduced opacity allowing you to unhide them.  This setting automatically disables on startup."));
			PackShowTooltipsOnAchievements = PackSettings.DefineSetting<bool>("PackShowTooltipsOnAchievements", false, (Func<string>)(() => "Show Tooltips for Achievements"), (Func<string>)(() => "Warning: This can cause performance issues when browsing categories."));
			SettingComplianceExtensions.SetRange(PackMaxOpacityOverride, 0f, 1f);
			SettingComplianceExtensions.SetRange(PackMaxViewDistance, 25f, 50000f);
			SettingComplianceExtensions.SetRange(PackMaxTrailAnimationSpeed, 0f, 10f);
			SettingComplianceExtensions.SetRange(PackMarkerScale, 0.1f, 4f);
			PackShowHiddenMarkersReducedOpacity.set_Value(false);
		}

		private void InitMapSettings(SettingCollection settings)
		{
			MapSettings = settings.AddSubCollection("map-settings", false);
			MapPathablesEnabled = MapSettings.DefineSetting<bool>("MapPathablesEnabled", true, (Func<string>)(() => "Show Markers on Maps"), (Func<string>)null);
			MapMarkerVisibilityLevel = MapSettings.DefineSetting<MapVisibilityLevel>("MapMarkerVisibilityLevel", MapVisibilityLevel.Default, (Func<string>)(() => Strings.Setting_MapShowMarkersOnFullscreen), (Func<string>)(() => ""));
			MapTrailVisibilityLevel = MapSettings.DefineSetting<MapVisibilityLevel>("MapTrailVisibilityLevel", MapVisibilityLevel.Default, (Func<string>)(() => Strings.Setting_MapShowTrailsOnFullscreen), (Func<string>)(() => ""));
			MapDrawOpacity = MapSettings.DefineSetting<float>("MapDrawOpacity", 1f, (Func<string>)(() => "Opacity on Fullscreen Map"), (Func<string>)(() => ""));
			MiniMapMarkerVisibilityLevel = MapSettings.DefineSetting<MapVisibilityLevel>("MiniMapMarkerVisibilityLevel", MapVisibilityLevel.Default, (Func<string>)(() => Strings.Setting_MapShowMarkersOnCompass), (Func<string>)(() => ""));
			MiniMapTrailVisibilityLevel = MapSettings.DefineSetting<MapVisibilityLevel>("MiniMapTrailVisibilityLevel", MapVisibilityLevel.Default, (Func<string>)(() => Strings.Setting_MapShowTrailsOnCompass), (Func<string>)(() => ""));
			MiniMapDrawOpacity = MapSettings.DefineSetting<float>("MiniMapDrawOpacity", 1f, (Func<string>)(() => "Opacity on the Minimap"), (Func<string>)(() => ""));
			MapShowAboveBelowIndicators = MapSettings.DefineSetting<bool>("MapShowAboveBelowIndicators", true, (Func<string>)(() => Strings.Setting_MapShowAboveBelowIndicators), (Func<string>)(() => ""));
			MapFadeVerticallyDistantTrailSegments = MapSettings.DefineSetting<bool>("MapFadeVerticallyDistantTrailSegments", true, (Func<string>)(() => "Fade Trail Segments Which Are High Above or Below"), (Func<string>)(() => ""));
			MapShowTooltip = MapSettings.DefineSetting<bool>("MapShowTooltip", true, (Func<string>)(() => "Show Tooltips on Map"), (Func<string>)(() => "If enabled, tooltips will be shown on the map when the cursor is over a marker."));
			MapTrailWidth = MapSettings.DefineSetting<float>("MapTrailWidth", 2f, (Func<string>)(() => "Trail Width on Maps"), (Func<string>)(() => "The thickness of trails shown on the map."));
			SettingComplianceExtensions.SetRange(MapDrawOpacity, 0f, 1f);
			SettingComplianceExtensions.SetRange(MiniMapDrawOpacity, 0f, 1f);
			SettingComplianceExtensions.SetRange(MapTrailWidth, 0.5f, 4.5f);
		}

		private void InitScriptSettings(SettingCollection settings)
		{
			ScriptSettings = settings.AddSubCollection("script-settings", false);
			ScriptsEnabled = ScriptSettings.DefineSetting<bool>("ScriptsEnabled", true, (Func<string>)(() => "Enable Lua Scripts"), (Func<string>)(() => "If enabled, marker packs may load Lua scripts to provide custom functionality."));
			ScriptsConsoleEnabled = ScriptSettings.DefineSetting<bool>("ScriptsConsoleEnabled", false, (Func<string>)(() => "Enable Script Console"), (Func<string>)(() => "If enabled, the Script Console can be accessed from the Pathing module menu to debug scripts."));
		}

		private void InitKeyBindSettings(SettingCollection settings)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Expected O, but got Unknown
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Expected O, but got Unknown
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Expected O, but got Unknown
			KeyBindSettings = settings.AddSubCollection("keybind-settings", false);
			KeyBindTogglePathables = KeyBindSettings.DefineSetting<KeyBinding>("KeyBindTogglePathables", new KeyBinding((ModifierKeys)6, (Keys)220), (Func<string>)(() => "Toggle Markers"), (Func<string>)(() => ""));
			KeyBindToggleWorldPathables = KeyBindSettings.DefineSetting<KeyBinding>("KeyBindToggleWorldPathables", new KeyBinding((ModifierKeys)6, (Keys)219), (Func<string>)(() => "Toggle Markers in World"), (Func<string>)(() => ""));
			KeyBindToggleMapPathables = KeyBindSettings.DefineSetting<KeyBinding>("KeyBindToggleMapPathables", new KeyBinding((ModifierKeys)6, (Keys)221), (Func<string>)(() => "Toggle Markers on Map"), (Func<string>)(() => ""));
			KeyBindReloadMarkerPacks = KeyBindSettings.DefineSetting<KeyBinding>("KeyBindReloadMarkerPacks", new KeyBinding((ModifierKeys)6, (Keys)82), (Func<string>)(() => "Reload Marker Packs"), (Func<string>)(() => ""));
			HandleInternalKeyBinds();
		}

		private void HandleInternalKeyBinds()
		{
			KeyBindTogglePathables.get_Value().set_BlockSequenceFromGw2(true);
			KeyBindTogglePathables.get_Value().set_Enabled(true);
			KeyBindToggleWorldPathables.get_Value().set_BlockSequenceFromGw2(true);
			KeyBindToggleWorldPathables.get_Value().set_Enabled(true);
			KeyBindToggleMapPathables.get_Value().set_BlockSequenceFromGw2(true);
			KeyBindToggleMapPathables.get_Value().set_Enabled(true);
			KeyBindReloadMarkerPacks.get_Value().set_BlockSequenceFromGw2(true);
			KeyBindReloadMarkerPacks.get_Value().set_Enabled(true);
			KeyBindTogglePathables.get_Value().add_Activated((EventHandler<EventArgs>)ToggleGlobalPathablesEnabled);
			KeyBindToggleWorldPathables.get_Value().add_Activated((EventHandler<EventArgs>)TogglePackWorldPathablesEnabled);
			KeyBindToggleMapPathables.get_Value().add_Activated((EventHandler<EventArgs>)ToggleMapPathablesEnabled);
			KeyBindReloadMarkerPacks.get_Value().add_Activated((EventHandler<EventArgs>)ReloadMarkerPacks);
		}

		private void ReloadMarkerPacks(object sender, EventArgs e)
		{
			if (_module.PackInitiator != null)
			{
				_module.PackInitiator.ReloadPacks();
			}
		}

		private void ToggleGlobalPathablesEnabled(object sender, EventArgs e)
		{
			GlobalPathablesEnabled.set_Value(!GlobalPathablesEnabled.get_Value());
		}

		private void TogglePackWorldPathablesEnabled(object sender, EventArgs e)
		{
			PackWorldPathablesEnabled.set_Value(!PackWorldPathablesEnabled.get_Value());
		}

		private void ToggleMapPathablesEnabled(object sender, EventArgs e)
		{
			MapPathablesEnabled.set_Value(!MapPathablesEnabled.get_Value());
		}

		public void Unload()
		{
			KeyBindTogglePathables.get_Value().set_Enabled(false);
			KeyBindToggleWorldPathables.get_Value().set_Enabled(false);
			KeyBindToggleMapPathables.get_Value().set_Enabled(false);
			KeyBindReloadMarkerPacks.get_Value().set_Enabled(false);
			KeyBindTogglePathables.get_Value().remove_Activated((EventHandler<EventArgs>)ToggleGlobalPathablesEnabled);
			KeyBindToggleWorldPathables.get_Value().remove_Activated((EventHandler<EventArgs>)TogglePackWorldPathablesEnabled);
			KeyBindToggleMapPathables.get_Value().remove_Activated((EventHandler<EventArgs>)ToggleMapPathablesEnabled);
			KeyBindReloadMarkerPacks.get_Value().remove_Activated((EventHandler<EventArgs>)ReloadMarkerPacks);
		}
	}
}