using System;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RaidClears.Settings
{
	public class SettingService
	{
		public SettingEntry<Point> RaidPanelLocationPoint { get; }

		public SettingEntry<bool> RaidPanelIsVisible { get; }

		public SettingEntry<bool> RaidPanelAllowTooltipsSetting { get; }

		public SettingEntry<bool> RaidPanelDragWithMouseIsEnabledSetting { get; }

		public SettingEntry<FontSize> RaidPanelFontSizeSetting { get; }

		public SettingEntry<WingLabel> RaidPanelWingLabelsSetting { get; }

		public SettingEntry<Orientation> RaidPanelOrientationSetting { get; }

		public SettingEntry<float> RaidPanelWingLabelOpacity { get; }

		public SettingEntry<float> RaidPanelEncounterOpacity { get; }

		public SettingEntry<bool> W1IsVisibleSetting { get; }

		public SettingEntry<bool> W2IsVisibleSetting { get; }

		public SettingEntry<bool> W3IsVisibleSetting { get; }

		public SettingEntry<bool> W4IsVisibleSetting { get; }

		public SettingEntry<bool> W5IsVisibleSetting { get; }

		public SettingEntry<bool> W6IsVisibleSetting { get; }

		public SettingEntry<bool> W7IsVisibleSetting { get; }

		public SettingEntry<KeyBinding> RaidPanelIsVisibleKeyBind { get; }

		public SettingEntry<bool> ShowRaidsCornerIconSetting { get; }

		public SettingService(SettingCollection settings)
		{
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Expected O, but got Unknown
			//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_060f: Unknown result type (might be due to invalid IL or missing references)
			ShowRaidsCornerIconSetting = settings.DefineSetting<bool>("RCCornerIcon", false, (Func<string>)(() => "Display top left toggle button"), (Func<string>)(() => "Add a button next to Blish on the top left of screen that hides or shows the Raid Clears window."));
			RaidPanelIsVisible = settings.DefineSetting<bool>("RCActive", true, (Func<string>)(() => "Display on screen"), (Func<string>)(() => "Enable the Raid Clears grid."));
			RaidPanelIsVisibleKeyBind = settings.DefineSetting<KeyBinding>("RCkeybind", new KeyBinding((Keys)0), (Func<string>)(() => "Display on screen keybind"), (Func<string>)(() => "Reveal or hide the display from key press."));
			RaidPanelIsVisibleKeyBind.get_Value().set_Enabled(true);
			RaidPanelDragWithMouseIsEnabledSetting = settings.DefineSetting<bool>("RCDrag", false, (Func<string>)(() => "Enable Dragging"), (Func<string>)(() => "Click and drag to reposition the clears window."));
			RaidPanelAllowTooltipsSetting = settings.DefineSetting<bool>("RCtooltips", false, (Func<string>)(() => "Allow tooltips"), (Func<string>)(() => "Hovering the mouse over an encounter will display the full name"));
			RaidPanelFontSizeSetting = settings.DefineSetting<FontSize>("RCFontSize", (FontSize)11, (Func<string>)(() => "Font Size"), (Func<string>)(() => "Change the size of the grid"));
			RaidPanelWingLabelsSetting = settings.DefineSetting<WingLabel>("RCLabelDisplay", WingLabel.Abbreviation, (Func<string>)(() => "Wing Label"), (Func<string>)(() => "Display wing label as wing number or abbreviated name"));
			RaidPanelOrientationSetting = settings.DefineSetting<Orientation>("RCOrientation", Orientation.Vertical, (Func<string>)(() => "Orientation"), (Func<string>)(() => "Display the wings in a vertial column or horizontal row"));
			RaidPanelWingLabelOpacity = settings.DefineSetting<float>("RCWingOpacity", 1f, (Func<string>)(() => "Wing Label Opacity"), (Func<string>)(() => "Wing label transparency, Hidden <--> Full Visible"));
			SettingComplianceExtensions.SetRange(RaidPanelWingLabelOpacity, 0f, 1f);
			RaidPanelEncounterOpacity = settings.DefineSetting<float>("RCEncOpacity", 0.8f, (Func<string>)(() => "Encounter Opacity"), (Func<string>)(() => "Encounter label transparency, Hidden <--> Full Visible"));
			SettingComplianceExtensions.SetRange(RaidPanelEncounterOpacity, 0f, 1f);
			W1IsVisibleSetting = settings.DefineSetting<bool>("RCw1", true, (Func<string>)(() => "W1 / Spirit Vale"), (Func<string>)(() => "Enable Spirit Vale on the main display"));
			W2IsVisibleSetting = settings.DefineSetting<bool>("RCw2", true, (Func<string>)(() => "W2 / Salvation Pass"), (Func<string>)(() => "Enable Salvation Pass on the main display"));
			W3IsVisibleSetting = settings.DefineSetting<bool>("RCw3", true, (Func<string>)(() => "W3 / Stronghold of the Faithful"), (Func<string>)(() => "Enable Stronghold of the Faithful on the main display"));
			W4IsVisibleSetting = settings.DefineSetting<bool>("RCw4", true, (Func<string>)(() => "W4 / Bastion of the Penitent"), (Func<string>)(() => "Enable Bastion of the Penitent on the main display"));
			W5IsVisibleSetting = settings.DefineSetting<bool>("RCw5", true, (Func<string>)(() => "W5 / Hall of Chains"), (Func<string>)(() => "Enable Hall of Chains on the main display"));
			W6IsVisibleSetting = settings.DefineSetting<bool>("RCw6", true, (Func<string>)(() => "W6 / Mythwright Gambit"), (Func<string>)(() => "Enable Mythwright Gambit on the main display"));
			W7IsVisibleSetting = settings.DefineSetting<bool>("RCw7", true, (Func<string>)(() => "W7 / The Key of Ahdashim"), (Func<string>)(() => "Enable The Key of Ahdashim on the main display"));
			RaidPanelLocationPoint = settings.DefineSetting<Point>("RCLocation", new Point(100, 100), (Func<string>)(() => ""), (Func<string>)(() => ""));
			SettingCollection internalSettingSubCollection = settings.AddSubCollection("internal settings (not visible in UI)", false);
			RaidPanelLocationPoint = internalSettingSubCollection.DefineSetting<Point>("RCLocation", new Point(100, 100), (Func<string>)null, (Func<string>)null);
		}

		public void ToggleRaidPanelVisibility()
		{
			RaidPanelIsVisible.set_Value(!RaidPanelIsVisible.get_Value());
		}

		public bool[] GetWingVisibilitySettings()
		{
			return new bool[7]
			{
				W1IsVisibleSetting.get_Value(),
				W2IsVisibleSetting.get_Value(),
				W3IsVisibleSetting.get_Value(),
				W4IsVisibleSetting.get_Value(),
				W5IsVisibleSetting.get_Value(),
				W6IsVisibleSetting.get_Value(),
				W7IsVisibleSetting.get_Value()
			};
		}
	}
}
