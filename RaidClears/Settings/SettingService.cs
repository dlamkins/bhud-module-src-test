using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Settings.Enums;

namespace RaidClears.Settings
{
	public class SettingService
	{
		public SettingEntry<ApiPollPeriod> RaidPanelApiPollingPeriod { get; }

		public SettingEntry<Point> RaidPanelLocationPoint { get; }

		public SettingEntry<bool> RaidPanelIsVisible { get; }

		public SettingEntry<bool> DungeonsEnabled { get; }

		public SettingEntry<bool> AllowTooltipsSetting { get; }

		public SettingEntry<bool> DragWithMouseIsEnabledSetting { get; }

		public SettingEntry<FontSize> RaidPanelFontSizeSetting { get; }

		public SettingEntry<WingLabel> RaidPanelWingLabelsSetting { get; }

		public SettingEntry<Orientation> RaidPanelOrientationSetting { get; }

		public SettingEntry<float> RaidPanelWingLabelOpacity { get; }

		public SettingEntry<float> RaidPanelEncounterOpacity { get; }

		public SettingEntry<bool> RaidPanelHighlightEmbolden { get; }

		public SettingEntry<bool> RaidPanelHighlightCotM { get; }

		public SettingEntry<bool> W1IsVisibleSetting { get; }

		public SettingEntry<bool> W2IsVisibleSetting { get; }

		public SettingEntry<bool> W3IsVisibleSetting { get; }

		public SettingEntry<bool> W4IsVisibleSetting { get; }

		public SettingEntry<bool> W5IsVisibleSetting { get; }

		public SettingEntry<bool> W6IsVisibleSetting { get; }

		public SettingEntry<bool> W7IsVisibleSetting { get; }

		public SettingEntry<KeyBinding> RaidPanelIsVisibleKeyBind { get; }

		public SettingEntry<bool> ShowRaidsCornerIconSetting { get; }

		public SettingEntry<Point> DungeonPanelLocationPoint { get; }

		public SettingEntry<bool> DungeonPanelIsVisible { get; }

		public SettingEntry<FontSize> DungeonPanelFontSizeSetting { get; }

		public SettingEntry<DungeonLabel> DungeonPanelWingLabelsSetting { get; }

		public SettingEntry<DungeonOrientation> DungeonPanelOrientationSetting { get; }

		public SettingEntry<float> DungeonPanelWingLabelOpacity { get; }

		public SettingEntry<float> DungeonPanelEncounterOpacity { get; }

		public SettingEntry<bool> D1IsVisibleSetting { get; }

		public SettingEntry<bool> D2IsVisibleSetting { get; }

		public SettingEntry<bool> D3IsVisibleSetting { get; }

		public SettingEntry<bool> D4IsVisibleSetting { get; }

		public SettingEntry<bool> D5IsVisibleSetting { get; }

		public SettingEntry<bool> D6IsVisibleSetting { get; }

		public SettingEntry<bool> D7IsVisibleSetting { get; }

		public SettingEntry<bool> D8IsVisibleSetting { get; }

		public SettingEntry<bool> DFIsVisibleSetting { get; }

		public SettingEntry<KeyBinding> DungeonPanelIsVisibleKeyBind { get; }

		public SettingEntry<bool> ShowDungeonCornerIconSetting { get; }

		public SettingService(SettingCollection settings)
		{
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Expected O, but got Unknown
			//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07eb: Expected O, but got Unknown
			//IL_0cc8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ce7: Unknown result type (might be due to invalid IL or missing references)
			RaidPanelApiPollingPeriod = settings.DefineSetting<ApiPollPeriod>("RCPoll", ApiPollPeriod.MINUTES_5, (Func<string>)(() => "Api Poll Frequency"), (Func<string>)(() => "How often should the GW2 API be checked for updated information"));
			DragWithMouseIsEnabledSetting = settings.DefineSetting<bool>("RCDrag", false, (Func<string>)(() => "Enable Dragging"), (Func<string>)(() => "Click and drag to reposition the clears window."));
			AllowTooltipsSetting = settings.DefineSetting<bool>("RCtooltips", false, (Func<string>)(() => "Allow tooltips"), (Func<string>)(() => "Hovering the mouse over an encounter will display the full name"));
			ShowRaidsCornerIconSetting = settings.DefineSetting<bool>("RCCornerIcon", false, (Func<string>)(() => "Display top left toggle button"), (Func<string>)(() => "Add a button next to Blish on the top left of screen that hides or shows the Raid Clears window."));
			RaidPanelIsVisible = settings.DefineSetting<bool>("RCActive", true, (Func<string>)(() => "Display on screen"), (Func<string>)(() => "Enable the Raid Clears grid."));
			RaidPanelIsVisibleKeyBind = settings.DefineSetting<KeyBinding>("RCkeybind", new KeyBinding((Keys)0), (Func<string>)(() => "Display on screen keybind"), (Func<string>)(() => "Reveal or hide the display from key press."));
			RaidPanelIsVisibleKeyBind.get_Value().set_Enabled(true);
			RaidPanelFontSizeSetting = settings.DefineSetting<FontSize>("RCFontSize", (FontSize)11, (Func<string>)(() => "Font Size   "), (Func<string>)(() => "Change the size of the grid (Weird sizes from available fonts)"));
			SettingEntry<FontSize> raidPanelFontSizeSetting = RaidPanelFontSizeSetting;
			FontSize[] array = new FontSize[5];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			SettingComplianceExtensions.SetExcluded<FontSize>(raidPanelFontSizeSetting, (FontSize[])(object)array);
			RaidPanelWingLabelsSetting = settings.DefineSetting<WingLabel>("RCLabelDisplay", WingLabel.Abbreviation, (Func<string>)(() => "Wing Label"), (Func<string>)(() => "Display wing label as wing number or abbreviated name"));
			RaidPanelOrientationSetting = settings.DefineSetting<Orientation>("RCOrientation", Orientation.Vertical, (Func<string>)(() => "Orientation"), (Func<string>)(() => "Display the wings in a vertial column or horizontal row"));
			RaidPanelWingLabelOpacity = settings.DefineSetting<float>("RCWingOpacity", 1f, (Func<string>)(() => "Wing Label Opacity"), (Func<string>)(() => "Wing label transparency, Hidden <--> Full Visible"));
			SettingComplianceExtensions.SetRange(RaidPanelWingLabelOpacity, 0f, 1f);
			RaidPanelEncounterOpacity = settings.DefineSetting<float>("RCEncOpacity", 0.8f, (Func<string>)(() => "Encounter Opacity"), (Func<string>)(() => "Encounter label transparency, Hidden <--> Full Visible"));
			SettingComplianceExtensions.SetRange(RaidPanelEncounterOpacity, 0f, 1f);
			RaidPanelHighlightEmbolden = settings.DefineSetting<bool>("RCEmbolden", true, (Func<string>)(() => "Highlight the weekly 'Emboldened' raid wing"), (Func<string>)(() => "Colors the text blue for the weekly Emboldened raid wing\nEmbolden mode increases player health, damage, and healing for each stack."));
			RaidPanelHighlightCotM = settings.DefineSetting<bool>("RCCotM", true, (Func<string>)(() => "Highlight the weekly 'Call of the Mist' raid wing"), (Func<string>)(() => "Colors the text golden for the weekly Call of the Mists raid wing\nCall of the Mists doubles all gold in the boss loot chest"));
			W1IsVisibleSetting = settings.DefineSetting<bool>("RCw1", true, (Func<string>)(() => "W1 / Spirit Vale"), (Func<string>)(() => "Enable Spirit Vale on the main display"));
			W2IsVisibleSetting = settings.DefineSetting<bool>("RCw2", true, (Func<string>)(() => "W2 / Salvation Pass"), (Func<string>)(() => "Enable Salvation Pass on the main display"));
			W3IsVisibleSetting = settings.DefineSetting<bool>("RCw3", true, (Func<string>)(() => "W3 / Stronghold of the Faithful"), (Func<string>)(() => "Enable Stronghold of the Faithful on the main display"));
			W4IsVisibleSetting = settings.DefineSetting<bool>("RCw4", true, (Func<string>)(() => "W4 / Bastion of the Penitent"), (Func<string>)(() => "Enable Bastion of the Penitent on the main display"));
			W5IsVisibleSetting = settings.DefineSetting<bool>("RCw5", true, (Func<string>)(() => "W5 / Hall of Chains"), (Func<string>)(() => "Enable Hall of Chains on the main display"));
			W6IsVisibleSetting = settings.DefineSetting<bool>("RCw6", true, (Func<string>)(() => "W6 / Mythwright Gambit"), (Func<string>)(() => "Enable Mythwright Gambit on the main display"));
			W7IsVisibleSetting = settings.DefineSetting<bool>("RCw7", true, (Func<string>)(() => "W7 / The Key of Ahdashim"), (Func<string>)(() => "Enable The Key of Ahdashim on the main display"));
			DungeonsEnabled = settings.DefineSetting<bool>("RCDungeonsEnabled", false, (Func<string>)(() => "Enable Dungeon Tracking Feature"), (Func<string>)(() => "Turn on the daily dungeon and dungeon frequenter feature."));
			ShowDungeonCornerIconSetting = settings.DefineSetting<bool>("RCDungeonCornerIcon", false, (Func<string>)(() => "Display top left toggle button"), (Func<string>)(() => "Add a button next to Blish on the top left of screen that hides or shows the Raid Clears window."));
			DungeonPanelIsVisible = settings.DefineSetting<bool>("RCDungeonActive", true, (Func<string>)(() => "Display on screen"), (Func<string>)(() => "Enable the Raid Clears grid."));
			DungeonPanelIsVisibleKeyBind = settings.DefineSetting<KeyBinding>("RCDungeonkeybind", new KeyBinding((Keys)0), (Func<string>)(() => "Display on screen keybind"), (Func<string>)(() => "Reveal or hide the display from key press."));
			DungeonPanelIsVisibleKeyBind.get_Value().set_Enabled(true);
			DungeonPanelFontSizeSetting = settings.DefineSetting<FontSize>("RCDungeonFontSize", (FontSize)11, (Func<string>)(() => "Font Size       "), (Func<string>)(() => "Change the size of the grid (Weird sizes from available fonts)"));
			SettingEntry<FontSize> dungeonPanelFontSizeSetting = DungeonPanelFontSizeSetting;
			FontSize[] array2 = new FontSize[5];
			RuntimeHelpers.InitializeArray(array2, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			SettingComplianceExtensions.SetExcluded<FontSize>(dungeonPanelFontSizeSetting, (FontSize[])(object)array2);
			DungeonPanelWingLabelsSetting = settings.DefineSetting<DungeonLabel>("RCDungeonLabelDisplay", DungeonLabel.Abbreviation, (Func<string>)(() => "Dungeon Label"), (Func<string>)(() => "Display wing label as wing number or abbreviated name"));
			DungeonPanelOrientationSetting = settings.DefineSetting<DungeonOrientation>("RCDungeonOrientation", DungeonOrientation.Vertical, (Func<string>)(() => "Orientation     "), (Func<string>)(() => "Display the dungeons in a vertial column or horizontal row"));
			DungeonPanelWingLabelOpacity = settings.DefineSetting<float>("RCDungeonOpacity", 1f, (Func<string>)(() => "Dungeon Label Opacity"), (Func<string>)(() => "Dungeon label transparency, Hidden <--> Full Visible"));
			SettingComplianceExtensions.SetRange(DungeonPanelWingLabelOpacity, 0f, 1f);
			DungeonPanelEncounterOpacity = settings.DefineSetting<float>("RCPathOpacity", 0.8f, (Func<string>)(() => "Path Opacity"), (Func<string>)(() => "Path label transparency, Hidden <--> Full Visible"));
			SettingComplianceExtensions.SetRange(DungeonPanelEncounterOpacity, 0f, 1f);
			D1IsVisibleSetting = settings.DefineSetting<bool>("RCd1", true, (Func<string>)(() => "Ascalonian Catacombs"), (Func<string>)(() => "Enable Ascalonian Catacombs on the dungeon display"));
			D2IsVisibleSetting = settings.DefineSetting<bool>("RCd2", true, (Func<string>)(() => "Caudecus Manor"), (Func<string>)(() => "Enable Caudecus Manor on the dungeon display"));
			D3IsVisibleSetting = settings.DefineSetting<bool>("RCd3", true, (Func<string>)(() => "Twilight Arbor"), (Func<string>)(() => "Enable Twilight Arbor on the dungeon display"));
			D4IsVisibleSetting = settings.DefineSetting<bool>("RCd4", true, (Func<string>)(() => "Sorrows Embrace"), (Func<string>)(() => "Enable Sorrows Embrace on the dungeon display"));
			D5IsVisibleSetting = settings.DefineSetting<bool>("RCd5", true, (Func<string>)(() => "Citadel of Flame"), (Func<string>)(() => "Enable Citadel of Flame on the dungeon display"));
			D6IsVisibleSetting = settings.DefineSetting<bool>("RCd6", true, (Func<string>)(() => "Honor of the Waves"), (Func<string>)(() => "Enable Honor of the Waves on the dungeon display"));
			D7IsVisibleSetting = settings.DefineSetting<bool>("RCd7", true, (Func<string>)(() => "Crucible of Eternity"), (Func<string>)(() => "Enable Crucible of Eternity on the dungeon display"));
			D8IsVisibleSetting = settings.DefineSetting<bool>("RCd8", true, (Func<string>)(() => "Ruined City of Arah"), (Func<string>)(() => "Enable Ruined City of Arah on the dungeon display"));
			DFIsVisibleSetting = settings.DefineSetting<bool>("RCdf", true, (Func<string>)(() => "Dungeon Frequenter Summary"), (Func<string>)(() => "Enable a dungeon frequenter achievement summary"));
			SettingCollection internalSettingSubCollection = settings.AddSubCollection("internal settings (not visible in UI)", false);
			RaidPanelLocationPoint = internalSettingSubCollection.DefineSetting<Point>("RCLocation", new Point(100, 100), (Func<string>)null, (Func<string>)null);
			DungeonPanelLocationPoint = internalSettingSubCollection.DefineSetting<Point>("RCDungeonLoc", new Point(200, 100), (Func<string>)null, (Func<string>)null);
		}

		public void ToggleRaidPanelVisibility()
		{
			RaidPanelIsVisible.set_Value(!RaidPanelIsVisible.get_Value());
		}

		public void ToggleDungeonPanelVisibility()
		{
			DungeonPanelIsVisible.set_Value(!DungeonPanelIsVisible.get_Value());
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

		public bool[] GetDungeonVisibilitySettings()
		{
			return new bool[9]
			{
				D1IsVisibleSetting.get_Value(),
				D2IsVisibleSetting.get_Value(),
				D3IsVisibleSetting.get_Value(),
				D4IsVisibleSetting.get_Value(),
				D5IsVisibleSetting.get_Value(),
				D6IsVisibleSetting.get_Value(),
				D7IsVisibleSetting.get_Value(),
				D8IsVisibleSetting.get_Value(),
				DFIsVisibleSetting.get_Value()
			};
		}
	}
}
