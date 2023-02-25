using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Estreya.BlishHUD.Shared.Models;
using Estreya.BlishHUD.Shared.Models.Drawers;
using Estreya.BlishHUD.Shared.State;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Settings
{
	public abstract class BaseModuleSettings
	{
		public class ModuleSettingsChangedEventArgs
		{
			public string Name { get; set; }

			public object NewValue { get; set; }

			public object PreviousValue { get; set; }
		}

		protected Logger Logger;

		private KeyBinding _globalEnabledKeybinding;

		private Color _defaultColor;

		protected readonly SettingCollection _settings;

		private const string GLOBAL_SETTINGS = "global-settings";

		private const string DRAWER_SETTINGS = "drawer-settings";

		public Color DefaultGW2Color
		{
			get
			{
				return _defaultColor;
			}
			private set
			{
				_defaultColor = value;
			}
		}

		public SettingCollection GlobalSettings { get; private set; }

		public SettingEntry<bool> GlobalDrawerVisible { get; private set; }

		public SettingEntry<KeyBinding> GlobalDrawerVisibleHotkey { get; private set; }

		public SettingEntry<bool> RegisterCornerIcon { get; private set; }

		public SettingEntry<CornerIconClickAction> CornerIconLeftClickAction { get; private set; }

		public SettingEntry<CornerIconClickAction> CornerIconRightClickAction { get; private set; }

		public SettingEntry<bool> HideOnMissingMumbleTicks { get; private set; }

		public SettingEntry<bool> HideInCombat { get; private set; }

		public SettingEntry<bool> HideOnOpenMap { get; private set; }

		public SettingEntry<bool> HideInPvE_OpenWorld { get; private set; }

		public SettingEntry<bool> HideInPvE_Competetive { get; private set; }

		public SettingEntry<bool> HideInWvW { get; private set; }

		public SettingEntry<bool> HideInPvP { get; private set; }

		public SettingEntry<bool> DebugEnabled { get; private set; }

		public SettingCollection DrawerSettings { get; private set; }

		public event EventHandler<ModuleSettingsChangedEventArgs> ModuleSettingsChanged;

		public BaseModuleSettings(SettingCollection settings, KeyBinding globalEnabledKeybinding)
		{
			Logger = Logger.GetLogger(GetType());
			_settings = settings;
			_globalEnabledKeybinding = globalEnabledKeybinding;
			BuildDefaultColor();
			InitializeGlobalSettings(_settings);
			InitializeDrawerSettings(_settings);
			InitializeAdditionalSettings(_settings);
		}

		private void InitializeDrawerSettings(SettingCollection settings)
		{
			DrawerSettings = settings.AddSubCollection("drawer-settings", false);
		}

		private void BuildDefaultColor()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Expected O, but got Unknown
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Expected O, but got Unknown
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Expected O, but got Unknown
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Expected O, but got Unknown
			//IL_01ee: Expected O, but got Unknown
			Color val = new Color();
			val.set_Name("Dye Remover");
			val.set_Id(1);
			val.set_BaseRgb((IReadOnlyList<int>)new List<int> { 128, 26, 26 });
			ColorMaterial val2 = new ColorMaterial();
			val2.set_Brightness(15);
			val2.set_Contrast(1.25);
			val2.set_Hue(38);
			val2.set_Saturation(9.0 / 32.0);
			val2.set_Lightness(1.44531);
			val2.set_Rgb((IReadOnlyList<int>)new List<int> { 124, 108, 83 });
			val.set_Cloth(val2);
			ColorMaterial val3 = new ColorMaterial();
			val3.set_Brightness(-8);
			val3.set_Contrast(1.0);
			val3.set_Hue(34);
			val3.set_Saturation(0.3125);
			val3.set_Lightness(1.09375);
			val3.set_Rgb((IReadOnlyList<int>)new List<int> { 65, 49, 29 });
			val.set_Leather(val3);
			ColorMaterial val4 = new ColorMaterial();
			val4.set_Brightness(5);
			val4.set_Contrast(1.05469);
			val4.set_Hue(38);
			val4.set_Saturation(0.101563);
			val4.set_Lightness(1.36719);
			val4.set_Rgb((IReadOnlyList<int>)new List<int> { 96, 91, 83 });
			val.set_Metal(val4);
			ColorMaterial val5 = new ColorMaterial();
			val5.set_Brightness(15);
			val5.set_Contrast(1.25);
			val5.set_Hue(38);
			val5.set_Saturation(9.0 / 32.0);
			val5.set_Lightness(1.44531);
			val5.set_Rgb((IReadOnlyList<int>)new List<int> { 124, 108, 83 });
			val.set_Fur(val5);
			_defaultColor = val;
		}

		protected virtual void InitializeAdditionalSettings(SettingCollection settings)
		{
		}

		private void InitializeGlobalSettings(SettingCollection settings)
		{
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			GlobalSettings = settings.AddSubCollection("global-settings", false);
			GlobalDrawerVisible = GlobalSettings.DefineSetting<bool>("GlobalDrawerVisible", true, (Func<string>)(() => "Global Visible"), (Func<string>)(() => "Whether the modules drawers should be visible."));
			GlobalDrawerVisible.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			bool globalHotkeyEnabled = _globalEnabledKeybinding != null;
			if (_globalEnabledKeybinding == null)
			{
				_globalEnabledKeybinding = new KeyBinding((ModifierKeys)7, (Keys)13);
				Logger.Debug("No default keybinding defined. Building temp keybinding. Enabled = {0}", new object[1] { globalHotkeyEnabled });
			}
			GlobalDrawerVisibleHotkey = GlobalSettings.DefineSetting<KeyBinding>("GlobalDrawerVisibleHotkey", _globalEnabledKeybinding, (Func<string>)(() => "Global Visible Hotkey"), (Func<string>)(() => "Defines the hotkey used to toggle the global visibility."));
			GlobalDrawerVisibleHotkey.add_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)SettingChanged<KeyBinding>);
			GlobalDrawerVisibleHotkey.get_Value().set_Enabled(globalHotkeyEnabled);
			GlobalDrawerVisibleHotkey.get_Value().add_Activated((EventHandler<EventArgs>)GlobalEnabledHotkey_Activated);
			GlobalDrawerVisibleHotkey.get_Value().set_IgnoreWhenInTextField(true);
			GlobalDrawerVisibleHotkey.get_Value().set_BlockSequenceFromGw2(globalHotkeyEnabled);
			RegisterCornerIcon = GlobalSettings.DefineSetting<bool>("RegisterCornerIcon", true, (Func<string>)(() => "Register Corner Icon"), (Func<string>)(() => "Whether the module should register a corner icon."));
			RegisterCornerIcon.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			RegisterCornerIcon.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)RegisterCornerIcon_SettingChanged);
			CornerIconLeftClickAction = GlobalSettings.DefineSetting<CornerIconClickAction>("CornerIconLeftClickAction", CornerIconClickAction.Settings, (Func<string>)(() => "Corner Icon Left Click Action"), (Func<string>)(() => "Defines the action of the corner icon when left clicked."));
			CornerIconLeftClickAction.add_SettingChanged((EventHandler<ValueChangedEventArgs<CornerIconClickAction>>)SettingChanged<CornerIconClickAction>);
			CornerIconRightClickAction = GlobalSettings.DefineSetting<CornerIconClickAction>("CornerIconRightClickAction", CornerIconClickAction.None, (Func<string>)(() => "Corner Icon Right Click Action"), (Func<string>)(() => "Defines the action of the corner icon when right clicked."));
			CornerIconRightClickAction.add_SettingChanged((EventHandler<ValueChangedEventArgs<CornerIconClickAction>>)SettingChanged<CornerIconClickAction>);
			HideOnOpenMap = GlobalSettings.DefineSetting<bool>("HideOnOpenMap", true, (Func<string>)(() => "Hide on open Map"), (Func<string>)(() => "Whether the modules drawers should hide when the map is open."));
			HideOnOpenMap.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideOnMissingMumbleTicks = GlobalSettings.DefineSetting<bool>("HideOnMissingMumbleTicks", true, (Func<string>)(() => "Hide on Cutscenes"), (Func<string>)(() => "Whether the modules drawers should hide when cutscenes are played."));
			HideOnMissingMumbleTicks.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideInCombat = GlobalSettings.DefineSetting<bool>("HideInCombat", false, (Func<string>)(() => "Hide in Combat"), (Func<string>)(() => "Whether the modules drawers should hide when in combat."));
			HideInCombat.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideInPvE_OpenWorld = GlobalSettings.DefineSetting<bool>("HideInPvE_OpenWorld", false, (Func<string>)(() => "Hide in PvE (Open World)"), (Func<string>)(() => "Whether the drawers should hide when in PvE (Open World)."));
			HideInPvE_OpenWorld.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideInPvE_Competetive = GlobalSettings.DefineSetting<bool>("HideInPvE_Competetive", false, (Func<string>)(() => "Hide in PvE (Competetive)"), (Func<string>)(() => "Whether the drawers should hide when in PvE (Competetive)."));
			HideInPvE_Competetive.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideInWvW = GlobalSettings.DefineSetting<bool>("HideInWvW", false, (Func<string>)(() => "Hide in WvW"), (Func<string>)(() => "Whether the drawers should hide when in world vs. world."));
			HideInWvW.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideInPvP = GlobalSettings.DefineSetting<bool>("HideInPvP", false, (Func<string>)(() => "Hide in PvP"), (Func<string>)(() => "Whether the drawers should hide when in player vs. player."));
			HideInPvP.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			DebugEnabled = GlobalSettings.DefineSetting<bool>("DebugEnabled", false, (Func<string>)(() => "Debug Enabled"), (Func<string>)(() => "Whether the module runs in debug mode."));
			DebugEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HandleEnabledStates();
			DoInitializeGlobalSettings(GlobalSettings);
		}

		private void RegisterCornerIcon_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			HandleEnabledStates();
		}

		private void GlobalEnabledHotkey_Activated(object sender, EventArgs e)
		{
			GlobalDrawerVisible.set_Value(!GlobalDrawerVisible.get_Value());
		}

		private void HandleEnabledStates()
		{
			SettingComplianceExtensions.SetDisabled((SettingEntry)(object)CornerIconLeftClickAction, !RegisterCornerIcon.get_Value());
			SettingComplianceExtensions.SetDisabled((SettingEntry)(object)CornerIconRightClickAction, !RegisterCornerIcon.get_Value());
		}

		protected virtual void DoInitializeGlobalSettings(SettingCollection globalSettingCollection)
		{
		}

		protected virtual void DoInitializeLocationSettings(SettingCollection locationSettingCollection)
		{
		}

		public DrawerConfiguration AddDrawer(string name, BuildDirection defaultBuildDirection = BuildDirection.Top)
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected O, but got Unknown
			int maxHeight = 1080;
			int maxWidth = 1920;
			SettingEntry<bool> enabled = DrawerSettings.DefineSetting<bool>(name + "-enabled", true, (Func<string>)(() => "Enabled"), (Func<string>)(() => "Whether the drawer is enabled."));
			SettingEntry<KeyBinding> enabledKeybinding = DrawerSettings.DefineSetting<KeyBinding>(name + "-enabledKeybinding", new KeyBinding(), (Func<string>)(() => "Enabled Keybinding"), (Func<string>)(() => "Defines the keybinding to toggle this drawer on and off."));
			enabledKeybinding.get_Value().set_Enabled(true);
			enabledKeybinding.get_Value().set_IgnoreWhenInTextField(true);
			enabledKeybinding.get_Value().set_BlockSequenceFromGw2(true);
			SettingEntry<int> locationX = DrawerSettings.DefineSetting<int>(name + "-locationX", (int)((double)maxWidth * 0.1), (Func<string>)(() => "Location X"), (Func<string>)(() => "Defines the position on the x axis."));
			SettingComplianceExtensions.SetRange(locationX, 0, maxWidth);
			SettingEntry<int> locationY = DrawerSettings.DefineSetting<int>(name + "-locationY", (int)((double)maxHeight * 0.1), (Func<string>)(() => "Location Y"), (Func<string>)(() => "Defines the position on the y axis."));
			SettingComplianceExtensions.SetRange(locationY, 0, maxHeight);
			SettingEntry<int> width = DrawerSettings.DefineSetting<int>(name + "-width", (int)((double)maxWidth * 0.5), (Func<string>)(() => "Width"), (Func<string>)(() => "The width of the drawer."));
			SettingComplianceExtensions.SetRange(width, 0, maxWidth);
			SettingEntry<int> height = DrawerSettings.DefineSetting<int>(name + "-height", (int)((double)maxHeight * 0.25), (Func<string>)(() => "Height"), (Func<string>)(() => "The height of the drawer."));
			SettingComplianceExtensions.SetRange(height, 0, maxHeight);
			SettingEntry<BuildDirection> buildDirection = DrawerSettings.DefineSetting<BuildDirection>(name + "-buildDirection", defaultBuildDirection, (Func<string>)(() => "Build Direction"), (Func<string>)(() => "The build direction of the drawer."));
			SettingEntry<float> opacity = DrawerSettings.DefineSetting<float>(name + "-opacity", 1f, (Func<string>)(() => "Opacity"), (Func<string>)(() => "The opacity of the drawer."));
			SettingComplianceExtensions.SetRange(opacity, 0f, 1f);
			SettingEntry<Color> backgroundColor = DrawerSettings.DefineSetting<Color>(name + "-backgroundColor", DefaultGW2Color, (Func<string>)(() => "Background Color"), (Func<string>)(() => "The background color of the drawer."));
			SettingEntry<FontSize> fontSize = DrawerSettings.DefineSetting<FontSize>(name + "-fontSize", (FontSize)16, (Func<string>)(() => "Font Size"), (Func<string>)(() => "The font size of the drawer."));
			SettingEntry<Color> textColor = DrawerSettings.DefineSetting<Color>(name + "-textColor", DefaultGW2Color, (Func<string>)(() => "Text Color"), (Func<string>)(() => "The text color of the drawer."));
			return new DrawerConfiguration
			{
				Name = name,
				Enabled = enabled,
				EnabledKeybinding = enabledKeybinding,
				Location = new DrawerLocation
				{
					X = locationX,
					Y = locationY
				},
				Size = new DrawerSize
				{
					X = width,
					Y = height
				},
				BuildDirection = buildDirection,
				Opacity = opacity,
				BackgroundColor = backgroundColor,
				FontSize = fontSize,
				TextColor = textColor
			};
		}

		public void RemoveDrawer(string name)
		{
			DrawerSettings.UndefineSetting(name + "-enabled");
			DrawerSettings.UndefineSetting(name + "-enabledKeybinding");
			DrawerSettings.UndefineSetting(name + "-locationX");
			DrawerSettings.UndefineSetting(name + "-locationY");
			DrawerSettings.UndefineSetting(name + "-width");
			DrawerSettings.UndefineSetting(name + "-height");
			DrawerSettings.UndefineSetting(name + "-buildDirection");
			DrawerSettings.UndefineSetting(name + "-opacity");
			DrawerSettings.UndefineSetting(name + "-backgroundColor");
			DrawerSettings.UndefineSetting(name + "-fontSize");
			DrawerSettings.UndefineSetting(name + "-textColor");
		}

		public void CheckDrawerSizeAndPosition(DrawerConfiguration configuration)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			bool buildFromBottom = configuration.BuildDirection.get_Value() == BuildDirection.Bottom;
			int num = (int)((float)GameService.Graphics.get_Resolution().X / GameService.Graphics.get_UIScaleMultiplier());
			int maxResY = (int)((float)GameService.Graphics.get_Resolution().Y / GameService.Graphics.get_UIScaleMultiplier());
			int minLocationX = 0;
			int maxLocationX = num - configuration.Size.X.get_Value();
			int minLocationY = (buildFromBottom ? configuration.Size.Y.get_Value() : 0);
			int maxLocationY = (buildFromBottom ? maxResY : (maxResY - configuration.Size.Y.get_Value()));
			int minWidth = 0;
			int maxWidth = num - configuration.Location.X.get_Value();
			int minHeight = 0;
			int maxHeight = maxResY - configuration.Location.Y.get_Value();
			SettingComplianceExtensions.SetRange(configuration.Location.X, minLocationX, maxLocationX);
			SettingComplianceExtensions.SetRange(configuration.Location.Y, minLocationY, maxLocationY);
			SettingComplianceExtensions.SetRange(configuration.Size.X, minWidth, maxWidth);
			SettingComplianceExtensions.SetRange(configuration.Size.Y, minHeight, maxHeight);
		}

		public virtual void UpdateLocalization(TranslationState translationState)
		{
			string globalDrawerVisibleDisplayNameDefault = ((SettingEntry)GlobalDrawerVisible).get_DisplayName();
			string globalDrawerVisibleDescriptionDefault = ((SettingEntry)GlobalDrawerVisible).get_Description();
			((SettingEntry)GlobalDrawerVisible).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-globalDrawerVisible-name", globalDrawerVisibleDisplayNameDefault)));
			((SettingEntry)GlobalDrawerVisible).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-globalDrawerVisible-description", globalDrawerVisibleDescriptionDefault)));
			string globalDrawerVisibleHotkeyDisplayNameDefault = ((SettingEntry)GlobalDrawerVisibleHotkey).get_DisplayName();
			string globalDrawerVisibleHotkeyDescriptionDefault = ((SettingEntry)GlobalDrawerVisibleHotkey).get_Description();
			((SettingEntry)GlobalDrawerVisibleHotkey).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-globalDrawerVisibleHotkey-name", globalDrawerVisibleHotkeyDisplayNameDefault)));
			((SettingEntry)GlobalDrawerVisibleHotkey).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-globalDrawerVisibleHotkey-description", globalDrawerVisibleHotkeyDescriptionDefault)));
			string registerCornerIconDisplayNameDefault = ((SettingEntry)RegisterCornerIcon).get_DisplayName();
			string registerCornerIconDescriptionDefault = ((SettingEntry)RegisterCornerIcon).get_Description();
			((SettingEntry)RegisterCornerIcon).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-registerCornerIcon-name", registerCornerIconDisplayNameDefault)));
			((SettingEntry)RegisterCornerIcon).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-registerCornerIcon-description", registerCornerIconDescriptionDefault)));
			string hideOnOpenMapDisplayNameDefault = ((SettingEntry)HideOnOpenMap).get_DisplayName();
			string hideOnOpenMapDescriptionDefault = ((SettingEntry)HideOnOpenMap).get_Description();
			((SettingEntry)HideOnOpenMap).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-hideOnOpenMap-name", hideOnOpenMapDisplayNameDefault)));
			((SettingEntry)HideOnOpenMap).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-hideOnOpenMap-description", hideOnOpenMapDescriptionDefault)));
			string hideOnMissingMumbleTickDisplayNameDefault = ((SettingEntry)HideOnMissingMumbleTicks).get_DisplayName();
			string hideOnMissingMumbleTickDescriptionDefault = ((SettingEntry)HideOnMissingMumbleTicks).get_Description();
			((SettingEntry)HideOnMissingMumbleTicks).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-hideOnMissingMumbleTick-name", hideOnMissingMumbleTickDisplayNameDefault)));
			((SettingEntry)HideOnMissingMumbleTicks).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-hideOnMissingMumbleTick-description", hideOnMissingMumbleTickDescriptionDefault)));
			string hideInCombatDisplayNameDefault = ((SettingEntry)HideInCombat).get_DisplayName();
			string hideInCombatDescriptionDefault = ((SettingEntry)HideInCombat).get_Description();
			((SettingEntry)HideInCombat).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-hideInCombat-name", hideInCombatDisplayNameDefault)));
			((SettingEntry)HideInCombat).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-hideInCombat-description", hideInCombatDescriptionDefault)));
			string hideInPVEOpenWorldDisplayNameDefault = ((SettingEntry)HideInPvE_OpenWorld).get_DisplayName();
			string hideInPVEOpenWorldDescriptionDefault = ((SettingEntry)HideInPvE_OpenWorld).get_Description();
			((SettingEntry)HideInPvE_OpenWorld).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-hideInPVEOpenWorld-name", hideInPVEOpenWorldDisplayNameDefault)));
			((SettingEntry)HideInPvE_OpenWorld).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-hideInPVEOpenWorld-description", hideInPVEOpenWorldDescriptionDefault)));
			string hideInPVECompetetiveDisplayNameDefault = ((SettingEntry)HideInPvE_Competetive).get_DisplayName();
			string hideInPVECompetetiveDescriptionDefault = ((SettingEntry)HideInPvE_Competetive).get_Description();
			((SettingEntry)HideInPvE_Competetive).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-hideInPVECompetetive-name", hideInPVECompetetiveDisplayNameDefault)));
			((SettingEntry)HideInPvE_Competetive).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-hideInPVECompetetive-description", hideInPVECompetetiveDescriptionDefault)));
			string hideInWVWDisplayNameDefault = ((SettingEntry)HideInWvW).get_DisplayName();
			string hideInWVWDescriptionDefault = ((SettingEntry)HideInWvW).get_Description();
			((SettingEntry)HideInWvW).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-hideInWVW-name", hideInWVWDisplayNameDefault)));
			((SettingEntry)HideInWvW).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-hideInWVW-description", hideInWVWDescriptionDefault)));
			string hideInPVPDisplayNameDefault = ((SettingEntry)HideInPvP).get_DisplayName();
			string hideInPVPDescriptionDefault = ((SettingEntry)HideInPvP).get_Description();
			((SettingEntry)HideInPvP).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-hideInPVP-name", hideInPVPDisplayNameDefault)));
			((SettingEntry)HideInPvP).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-hideInPVP-description", hideInPVPDescriptionDefault)));
		}

		public void UpdateDrawerLocalization(DrawerConfiguration drawerConfiguration, TranslationState translationState)
		{
			string enabledDisplayNameDefault = ((SettingEntry)drawerConfiguration.Enabled).get_DisplayName();
			string enabledDescriptionDefault = ((SettingEntry)drawerConfiguration.Enabled).get_Description();
			((SettingEntry)drawerConfiguration.Enabled).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerEnabled-name", enabledDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.Enabled).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerEnabled-description", enabledDescriptionDefault)));
			string enabledKeybindingDisplayNameDefault = ((SettingEntry)drawerConfiguration.EnabledKeybinding).get_DisplayName();
			string enabledKeybindingDescriptionDefault = ((SettingEntry)drawerConfiguration.EnabledKeybinding).get_Description();
			((SettingEntry)drawerConfiguration.EnabledKeybinding).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerEnabledKeybinding-name", enabledKeybindingDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.EnabledKeybinding).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerEnabledKeybinding-description", enabledKeybindingDescriptionDefault)));
			string locationXDisplayNameDefault = ((SettingEntry)drawerConfiguration.Location.X).get_DisplayName();
			string locationXDescriptionDefault = ((SettingEntry)drawerConfiguration.Location.X).get_Description();
			((SettingEntry)drawerConfiguration.Location.X).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerLocationX-name", locationXDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.Location.X).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerLocationX-description", locationXDescriptionDefault)));
			string locationYDisplayNameDefault = ((SettingEntry)drawerConfiguration.Location.Y).get_DisplayName();
			string locationYDescriptionDefault = ((SettingEntry)drawerConfiguration.Location.Y).get_Description();
			((SettingEntry)drawerConfiguration.Location.Y).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerLocationY-name", locationYDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.Location.Y).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerLocationY-description", locationYDescriptionDefault)));
			string widthDisplayNameDefault = ((SettingEntry)drawerConfiguration.Size.X).get_DisplayName();
			string widthDescriptionDefault = ((SettingEntry)drawerConfiguration.Size.X).get_Description();
			((SettingEntry)drawerConfiguration.Size.X).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerWidth-name", widthDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.Size.X).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerWidth-description", widthDescriptionDefault)));
			string heightDisplayNameDefault = ((SettingEntry)drawerConfiguration.Size.Y).get_DisplayName();
			string heightDescriptionDefault = ((SettingEntry)drawerConfiguration.Size.Y).get_Description();
			((SettingEntry)drawerConfiguration.Size.Y).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerHeight-name", heightDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.Size.Y).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerHeight-description", heightDescriptionDefault)));
			string buildDirectionDisplayNameDefault = ((SettingEntry)drawerConfiguration.BuildDirection).get_DisplayName();
			string buildDirectionDescriptionDefault = ((SettingEntry)drawerConfiguration.BuildDirection).get_Description();
			((SettingEntry)drawerConfiguration.BuildDirection).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerBuildDirection-name", buildDirectionDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.BuildDirection).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerBuildDirection-description", buildDirectionDescriptionDefault)));
			string opacityDisplayNameDefault = ((SettingEntry)drawerConfiguration.Opacity).get_DisplayName();
			string opacityDescriptionDefault = ((SettingEntry)drawerConfiguration.Opacity).get_Description();
			((SettingEntry)drawerConfiguration.Opacity).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerOpacity-name", opacityDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.Opacity).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerOpacity-description", opacityDescriptionDefault)));
			string backgroundColorDisplayNameDefault = ((SettingEntry)drawerConfiguration.BackgroundColor).get_DisplayName();
			string backgroundColorDescriptionDefault = ((SettingEntry)drawerConfiguration.BackgroundColor).get_Description();
			((SettingEntry)drawerConfiguration.BackgroundColor).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerBackgroundColor-name", backgroundColorDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.BackgroundColor).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerBackgroundColor-description", backgroundColorDescriptionDefault)));
			string fontSizeDisplayNameDefault = ((SettingEntry)drawerConfiguration.FontSize).get_DisplayName();
			string fontSizeDescriptionDefault = ((SettingEntry)drawerConfiguration.FontSize).get_Description();
			((SettingEntry)drawerConfiguration.FontSize).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerFontSize-name", fontSizeDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.FontSize).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerFontSize-description", fontSizeDescriptionDefault)));
			string textColorDisplayNameDefault = ((SettingEntry)drawerConfiguration.TextColor).get_DisplayName();
			string textColorDescriptionDefault = ((SettingEntry)drawerConfiguration.TextColor).get_Description();
			((SettingEntry)drawerConfiguration.TextColor).set_GetDisplayNameFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerTextColor-name", textColorDisplayNameDefault)));
			((SettingEntry)drawerConfiguration.TextColor).set_GetDescriptionFunc((Func<string>)(() => translationState.GetTranslation("setting-drawerTextColor-description", textColorDescriptionDefault)));
		}

		public virtual void Unload()
		{
			GlobalDrawerVisible.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			GlobalDrawerVisibleHotkey.remove_SettingChanged((EventHandler<ValueChangedEventArgs<KeyBinding>>)SettingChanged<KeyBinding>);
			GlobalDrawerVisibleHotkey.get_Value().set_Enabled(false);
			GlobalDrawerVisibleHotkey.get_Value().remove_Activated((EventHandler<EventArgs>)GlobalEnabledHotkey_Activated);
			RegisterCornerIcon.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			RegisterCornerIcon.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)RegisterCornerIcon_SettingChanged);
			HideOnOpenMap.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideOnMissingMumbleTicks.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideInPvE_OpenWorld.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideInPvE_Competetive.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideInCombat.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			HideInPvP.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
			DebugEnabled.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged<bool>);
		}

		protected void SettingChanged<T>(object sender, ValueChangedEventArgs<T> e)
		{
			SettingEntry<T> settingEntry = (SettingEntry<T>)sender;
			string prevValue = ((e.get_PreviousValue().GetType() == typeof(string)) ? e.get_PreviousValue().ToString() : JsonConvert.SerializeObject(e.get_PreviousValue()));
			string newValue = ((e.get_NewValue().GetType() == typeof(string)) ? e.get_NewValue().ToString() : JsonConvert.SerializeObject(e.get_NewValue()));
			Logger.Debug("Changed setting \"" + ((SettingEntry)settingEntry).get_EntryKey() + "\" from \"" + prevValue + "\" to \"" + newValue + "\"");
			this.ModuleSettingsChanged?.Invoke(this, new ModuleSettingsChangedEventArgs
			{
				Name = ((SettingEntry)settingEntry).get_EntryKey(),
				NewValue = e.get_NewValue(),
				PreviousValue = e.get_PreviousValue()
			});
		}
	}
}
