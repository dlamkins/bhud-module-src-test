using Blish_HUD;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Localization;
using RaidClears.Settings.Enums;

namespace RaidClears.Settings.Models
{
	public static class Settings
	{
		public static class Dungeons
		{
			public static class General
			{
				public static Setting<bool> enable = new Setting<bool>("RCDungeonEnable", DefaultValue: true, () => Strings.Setting_Dun_Enabled, () => Strings.Setting_Dun_Enabled_Tooltip);

				public static Setting<Point> location = new Setting<Point>("RCDungeonLoc", new Point(250, 605));

				public static Setting<bool> positionLock = new Setting<bool>("RCDunDrag", DefaultValue: true, () => Strings.Setting_Dun_Drag_Label, () => Strings.Setting_Dun_Drag_Tooltip);

				public static Setting<bool> tooltips = new Setting<bool>("RCDuntooltips", DefaultValue: true, () => Strings.Setting_Dun_Tooltips_Label, () => Strings.Setting_Dun_Tooltips_Tooltip);

				public static Setting<bool> toolbarIcon = new Setting<bool>("RCDungeonCornerIcon", DefaultValue: true, () => Strings.Setting_Dun_Icon_Label, () => Strings.Setting_Dun_Icon_Tooltip);

				public static Setting<bool> visible = new Setting<bool>("RCDungeonActive", DefaultValue: true, () => Strings.Setting_Dun_Visible_Label, () => Strings.Setting_Dun_Visible_Tooltip);

				public static Setting<KeyBinding> keyBind = new Setting<KeyBinding>("RCDungeonkeybind", new KeyBinding((Keys)0), () => Strings.Setting_Dun_Keybind_Label, () => Strings.Setting_Dun_Keybind_Tooltip);
			}

			public static class Style
			{
				public static class Color
				{
					public static Setting<string> uncleared = new Setting<string>("DunColNotCleared", "#4f4f4f", () => Strings.Setting_Raid_ColNotClear_Label, () => Strings.Setting_Raid_ColNotClear_Tooltip);

					public static Setting<string> cleared = new Setting<string>("DunColCleared", "#147814", () => Strings.Setting_Raid_ColClear_Label, () => Strings.Setting_Raid_ColClear_Tooltip);

					public static Setting<string> text = new Setting<string>("DunColText", "#FFFFFF", () => Strings.Setting_Raid_ColText_Label, () => Strings.Setting_Raid_ColText_Tooltip);

					public static Setting<string> frequenter = new Setting<string>("DunColFreq", "#F3F527", () => Strings.Setting_Dun_ColFreqText_Label, () => Strings.Setting_Dun_ColFreqText_Tooltip);

					public static Setting<string> background = new Setting<string>("DunColBG", "#000000", () => Strings.Setting_Raid_ColBG_Label, () => Strings.Setting_Raid_ColBG_Tooltip);
				}

				public static Setting<FontSize> fontSize = new Setting<FontSize>("RCDungeonFontSize", (FontSize)18, () => Strings.Setting_Raid_Font_Label, () => Strings.Setting_Raid_Font_Tooptip);

				public static Setting<LabelDisplay> labelDisplay = new Setting<LabelDisplay>("RCDungeonLabelDisplay", LabelDisplay.Abbreviation, () => Strings.Setting_Raid_LabelDisplay_Label, () => Strings.Setting_Raid_LabelDisplay_Tooltip);

				public static Setting<Layout> layout = new Setting<Layout>("RCDungeonOrientation", Layout.Vertical, () => Strings.Setting_Raid_Layout_Label);

				public static Setting<float> labelOpacity = new Setting<float>("RCDungeonOpacity", 1f, () => Strings.Setting_Raid_LabelOpacity_Label, () => Strings.Setting_Raid_LabelOpacity_Tooltip);

				public static Setting<float> gridOpacity = new Setting<float>("RCPathOpacity", 0.8f, () => Strings.Setting_Raid_GridOpacity_Label, () => Strings.Setting_Raid_GridOpactiy_Tooltip);

				public static Setting<float> backgroundOpacity = new Setting<float>("RCDunBGOpacity", 0f, () => Strings.Setting_Raid_PanelOpacity_Label, () => Strings.Setting_Raid_PanelOpacity_Tooltip);
			}

			public static class Module
			{
				public static Setting<bool> highlightFrequenter = new Setting<bool>("RCDunFreqHighlight", DefaultValue: true, () => "Highlight Frequenter Paths");

				public static Setting<bool>[] dungeonPaths = new Setting<bool>[8]
				{
					new DungeonSetting<bool>("RCd1", DefaultValue: true, () => Encounters.Dungeons.AscalonianCatacombs.GetLabel()),
					new DungeonSetting<bool>("RCd2", DefaultValue: true, () => Encounters.Dungeons.CaudecusManor.GetLabel()),
					new DungeonSetting<bool>("RCd3", DefaultValue: true, () => Encounters.Dungeons.TwilightArbor.GetLabel()),
					new DungeonSetting<bool>("RCd4", DefaultValue: true, () => Encounters.Dungeons.SorrowsEmbrace.GetLabel()),
					new DungeonSetting<bool>("RCd5", DefaultValue: true, () => Encounters.Dungeons.CitadelOfFlame.GetLabel()),
					new DungeonSetting<bool>("RCd6", DefaultValue: true, () => Encounters.Dungeons.HonorOfTheWaves.GetLabel()),
					new DungeonSetting<bool>("RCd7", DefaultValue: true, () => Encounters.Dungeons.CrucibleOfEternity.GetLabel()),
					new DungeonSetting<bool>("RCd8", DefaultValue: true, () => Encounters.Dungeons.RuinedCityOfArah.GetLabel())
				};

				public static Setting<bool> dungeonFrequenterEnabled = new Setting<bool>("RCdf", DefaultValue: true, () => "Dungeon Frequenter Summary", () => "Enable a dungeon frequenter achievement summary");
			}
		}

		public static class Raids
		{
			public static class General
			{
				public static Setting<bool> enabled = new Setting<bool>("RCRaidEnabled", DefaultValue: true, () => "Enable Raids Feature");

				public static Setting<Point> location = new Setting<Point>("RCLocation", new Point(250, 185));

				public static Setting<bool> positionLock = new Setting<bool>("RCDrag", DefaultValue: true, () => Strings.Setting_Raid_Drag_Label, () => Strings.Setting_Raid_Drag_Tooltip);

				public static Setting<bool> tooltips = new Setting<bool>("RCtooltips", DefaultValue: true, () => Strings.Setting_Raid_Tooltips_Label, () => Strings.Setting_Raid_Tooltips_Tooltip);

				public static Setting<bool> toolbarIcon = new Setting<bool>("RCCornerIcon", DefaultValue: true, () => Strings.Setting_Raid_Icon_Label, () => Strings.Setting_Raid_Icon_Tooltip);

				public static Setting<bool> visible = new Setting<bool>("RCActive", DefaultValue: true, () => Strings.Setting_Raid_Visible_Label, () => Strings.Setting_Raid_Visible_Tooltip);

				public static Setting<KeyBinding> keyBind = new Setting<KeyBinding>("RCkeybind", new KeyBinding((Keys)0), () => Strings.Setting_Raid_Keybind_Label, () => Strings.Setting_Raid_Keybind_Tooltip);
			}

			public static class Style
			{
				public static class Color
				{
					public static Setting<string> uncleared = new Setting<string>("colNotCleared", "#4f4f4f", () => Strings.Setting_Raid_ColNotClear_Label, () => Strings.Setting_Raid_ColNotClear_Tooltip);

					public static Setting<string> cleared = new Setting<string>("colCleared", "#147814", () => Strings.Setting_Raid_ColClear_Label, () => Strings.Setting_Raid_ColClear_Tooltip);

					public static Setting<string> text = new Setting<string>("colText", "#FFFFFF", () => Strings.Setting_Raid_ColText_Label, () => Strings.Setting_Raid_ColText_Tooltip);

					public static Setting<string> cotm = new Setting<string>("colCotm", "#F3F527", () => Strings.Setting_Raid_ColCotm_Label, () => Strings.Setting_Raid_ColCotm_Tooltip);

					public static Setting<string> embolden = new Setting<string>("colEmbolden", "#202090", () => Strings.Setting_Raid_ColEmbolden_Label, () => Strings.Setting_Raid_ColEmbolden_Tooltip);

					public static Setting<string> nonWeeklyBounty = new Setting<string>("colNonWeeklyBounty", "#CC8800", () => Strings.Setting_Raid_ColNonWeeklyBounty_Label, () => Strings.Setting_Raid_ColNonWeeklyBounty_Tooltip);

					public static Setting<string> background = new Setting<string>("colRaidBG", "#000000", () => Strings.Setting_Raid_ColBG_Label, () => Strings.Setting_Raid_ColBG_Tooltip);
				}

				public static Setting<FontSize> fontSize = new Setting<FontSize>("RCFontSize", (FontSize)18, () => Strings.Setting_Raid_Font_Label, () => Strings.Setting_Raid_Font_Tooptip);

				public static Setting<LabelDisplay> labelDisplay = new Setting<LabelDisplay>("RCLabelDisplay", LabelDisplay.Abbreviation, () => Strings.Setting_Raid_LabelDisplay_Label, () => Strings.Setting_Raid_LabelDisplay_Tooltip);

				public static Setting<Layout> layout = new Setting<Layout>("RCOrientation", Layout.Vertical, () => Strings.Setting_Raid_Layout_Label, () => Strings.Setting_Raid_Layout_Tooltip);

				public static Setting<float> labelOpacity = new Setting<float>("RCWingOpacity", 1f, () => Strings.Setting_Raid_LabelOpacity_Label, () => Strings.Setting_Raid_LabelOpacity_Tooltip);

				public static Setting<float> gridOpacity = new Setting<float>("RCEncOpacity", 0.8f, () => Strings.Setting_Raid_GridOpacity_Label, () => Strings.Setting_Raid_GridOpactiy_Tooltip);

				public static Setting<float> backgroundOpacity = new Setting<float>("RCRaidBgOpacity", 0f, () => Strings.Setting_Raid_PanelOpacity_Label, () => Strings.Setting_Raid_PanelOpacity_Tooltip);
			}

			public static class Module
			{
				public static Setting<bool> highlightEmbolden = new Setting<bool>("RCEmbolden", DefaultValue: true, () => Strings.Setting_Raid_Embolden_Label, () => Strings.Setting_Raid_Embolden_Tooltip);

				public static Setting<bool> highlightCotm = new Setting<bool>("RCCotM", DefaultValue: true, () => Strings.Setting_Raid_Cotm_Label, () => Strings.Setting_Raid_Cotm_Tooltip);

				public static Setting<bool> highlightNonWeeklyBounty = new Setting<bool>("RCNonWeeklyBounty", DefaultValue: true, () => Strings.Setting_Raid_NonWeeklyBounty_Label, () => Strings.Setting_Raid_NonWeeklyBounty_Tooltip);

				public static Setting<bool> omitEventEncounters = new Setting<bool>("RCOmitEventEncounters", DefaultValue: true, () => Strings.Setting_Raid_OmitEventEncounters_Label, () => Strings.Setting_Raid_OmitEventEncounters_Tooltip);

				public static Setting<bool> mentorProgress = new Setting<bool>("RCMentorProgress", DefaultValue: true, () => Strings.Setting_Raid_MentorProgress_Label, () => Strings.Setting_Raid_MentorProgress_Tooltip);

				public static Setting<bool> mentorProgressPopup = new Setting<bool>("RCMentorProgressPopup", DefaultValue: false, () => Strings.Setting_Raid_MentorProgressPopup_Label, () => Strings.Setting_Raid_MentorProgressPopup_Tooltip);

				public static Setting<bool> mentorProgressPopupReposition = new Setting<bool>("RCMentorProgressPopupReposition", DefaultValue: false, () => Strings.Setting_Raid_MentorProgressPopupReposition_Label, () => Strings.Setting_Raid_MentorProgressPopupReposition_Tooltip);

				public static Setting<Point> mentorProgressPopupPosition = new Setting<Point>("RCMentorProgressPopupPosition", new Point(-1, -1));
			}
		}

		public static class Strikes
		{
			public static class Style
			{
				public static class Color
				{
					public static Setting<string> uncleared = new Setting<string>("StkcolNotCleared", "#4f4f4f", () => Strings.Setting_Strike_ColNotClear_Label, () => Strings.Setting_Strike_ColNotClear_Tooltip);

					public static Setting<string> cleared = new Setting<string>("StkColCleared", "#147814", () => Strings.Setting_Strike_ColClear_Label, () => Strings.Setting_Strike_ColClear_Tooltip);

					public static Setting<string> text = new Setting<string>("StkColText", "#FFFFFF", () => Strings.Setting_Strike_ColText_Label, () => Strings.Setting_Strike_ColText_Tooltip);

					public static Setting<string> background = new Setting<string>("colStrikeBG", "#000000", () => Strings.Setting_Strike_ColBG_Label, () => Strings.Setting_Strike_ColBG_Tooltip);
				}

				public static Setting<FontSize> fontSize = new Setting<FontSize>("RCStkFontSize", (FontSize)18, () => Strings.Setting_Strike_Font_Label, () => Strings.Setting_Strike_Font_Tooptip);

				public static Setting<LabelDisplay> labelDisplay = new Setting<LabelDisplay>("RCStkLabelDisplay", LabelDisplay.Abbreviation, () => Strings.Setting_Strike_LabelDisplay_Label, () => Strings.Setting_Strike_LabelDisplay_Tooltip);

				public static Setting<Layout> layout = new Setting<Layout>("RCStkOrientation", Layout.Vertical, () => Strings.Setting_Strike_Layout_Label, () => Strings.Setting_Strike_Layout_Tooltip);

				public static Setting<float> labelOpacity = new Setting<float>("RCStkLabelOpacity", 1f, () => Strings.Setting_Strike_LabelOpacity_Label, () => Strings.Setting_Strike_LabelOpacity_Tooltip);

				public static Setting<float> gridOpacity = new Setting<float>("RCStkOpacity", 0.8f, () => Strings.Setting_Strike_GridOpacity_Label, () => Strings.Setting_Strike_GridOpactiy_Tooltip);

				public static Setting<float> backgroundOpacity = new Setting<float>("RCStrikeBgOpacity", 0f, () => Strings.Setting_Strike_PanelOpacity_Label, () => Strings.Setting_Strike_PanelOpacity_Tooltip);
			}

			public static class General
			{
				public static Setting<bool> enabled = new Setting<bool>("RCStkEnabled", DefaultValue: true, () => "Enable strikes Feature");

				public static Setting<Point> location = new Setting<Point>("RCStkLocation", new Point(250, 380));

				public static Setting<bool> positionLock = new Setting<bool>("RCStkDrag", DefaultValue: true, () => Strings.Setting_Strike_Drag_Label, () => Strings.Setting_Strike_Drag_Tooltip);

				public static Setting<bool> tooltips = new Setting<bool>("RCStktooltips", DefaultValue: true, () => Strings.Setting_Strike_Tooltips_Label, () => Strings.Setting_Strike_Tooltips_Tooltip);

				public static Setting<bool> toolbarIcon = new Setting<bool>("RCStkCornerIcon", DefaultValue: true, () => Strings.Setting_Strike_Icon_Label, () => Strings.Setting_Strike_Icon_Tooltip);

				public static Setting<bool> visible = new Setting<bool>("RCStkActive", DefaultValue: true, () => Strings.Setting_Strike_Visible_Label, () => Strings.Setting_Strike_Visible_Tooltip);

				public static Setting<KeyBinding> keyBind = new Setting<KeyBinding>("RCStkkeybind", new KeyBinding((Keys)0), () => Strings.Setting_Strike_Keybind_Label, () => Strings.Setting_Strike_Keybind_Tooltip);
			}

			public static class Module
			{
				public static Setting<bool> anchorToRaids = new Setting<bool>("RCAnchorToRaids", DefaultValue: false, () => Strings.Settings_Strike_AnchorToRaidLabel, () => Strings.Settings_Strike_AnchorToRaidTooltip);

				public static Setting<StrikeComplete> strikeCompletion = new Setting<StrikeComplete>("RCStrikeComplete", StrikeComplete.MAP_CHANGE, () => Strings.Settings_Strike_Completion, () => Strings.Settings_Strike_CompletionTooltip);
			}
		}

		public static class Fractal
		{
			public static class Style
			{
				public static class Color
				{
					public static Setting<string> uncleared = new Setting<string>("FraccolNotCleared", "#4f4f4f", () => Strings.Setting_Fractals_ColNotClear_Label, () => Strings.Setting_Fractals_ColNotClear_Tooltip);

					public static Setting<string> cleared = new Setting<string>("FracColCleared", "#147814", () => Strings.Setting_Fractals_ColClear_Label, () => Strings.Setting_Fractals_ColClear_Tooltip);

					public static Setting<string> text = new Setting<string>("FracColText", "#FFFFFF", () => Strings.Setting_Fractals_ColText_Label, () => Strings.Setting_Fractals_ColText_Tooltip);

					public static Setting<string> background = new Setting<string>("colFracBG", "#000000", () => Strings.Setting_Fractals_ColBG_Label, () => Strings.Setting_Fractals_ColBG_Tooltip);
				}

				public static Setting<FontSize> fontSize = new Setting<FontSize>("RCFracFontSize", (FontSize)18, () => Strings.Setting_Fractals_Font_Label, () => Strings.Setting_Fractals_Font_Tooptip);

				public static Setting<LabelDisplay> labelDisplay = new Setting<LabelDisplay>("RCFracLabelDisplay", LabelDisplay.Abbreviation, () => Strings.Setting_Fractals_LabelDisplay_Label, () => Strings.Setting_Fractals_LabelDisplay_Tooltip);

				public static Setting<Layout> layout = new Setting<Layout>("RCFracOrientation", Layout.Vertical, () => Strings.Setting_Fractals_Layout_Label, () => Strings.Setting_Fractals_Layout_Tooltip);

				public static Setting<float> labelOpacity = new Setting<float>("RCFracLabelOpacity", 1f, () => Strings.Setting_Fractals_LabelOpacity_Label, () => Strings.Setting_Fractals_LabelOpacity_Tooltip);

				public static Setting<float> gridOpacity = new Setting<float>("RCFracOpacity", 0.8f, () => Strings.Setting_Fractals_GridOpacity_Label, () => Strings.Setting_Fractals_GridOpactiy_Tooltip);

				public static Setting<float> backgroundOpacity = new Setting<float>("RCSFracBgOpacity", 0f, () => Strings.Setting_Fractals_PanelOpacity_Label, () => Strings.Setting_Fractals_PanelOpacity_Tooltip);
			}

			public static class General
			{
				public static Setting<bool> enabled = new Setting<bool>("RCFracEnabled", DefaultValue: true, () => "Enable Fractals Feature");

				public static Setting<Point> location = new Setting<Point>("RCFracLocation", new Point(250, 525));

				public static Setting<bool> positionLock = new Setting<bool>("RCFracDrag", DefaultValue: true, () => Strings.Setting_Fractals_Drag_Label, () => Strings.Setting_Fractals_Drag_Tooltip);

				public static Setting<bool> tooltips = new Setting<bool>("RCFractooltips", DefaultValue: true, () => Strings.Setting_Fractals_Tooltips_Label, () => Strings.Setting_Fractals_Tooltips_Tooltip);

				public static Setting<bool> toolbarIcon = new Setting<bool>("RCFracCornerIcon", DefaultValue: true, () => Strings.Setting_Fractals_Icon_Label, () => Strings.Setting_Fractals_Icon_Tooltip);

				public static Setting<bool> visible = new Setting<bool>("RCFracActive", DefaultValue: true, () => Strings.Setting_Fractals_Visible_Label, () => Strings.Setting_Fractals_Visible_Tooltip);

				public static Setting<KeyBinding> keyBind = new Setting<KeyBinding>("RCSFrackeybind", new KeyBinding((Keys)0), () => Strings.Setting_Fractals_Keybind_Label, () => Strings.Setting_Fractals_Keybind_Tooltip);
			}

			public static class Module
			{
				public static Setting<bool> showCMs = new Setting<bool>("FractalCM", DefaultValue: true, () => Strings.Settings_Fractals_ShowChallengeMotes, () => Strings.Settings_Fractals_ShowChallengeMotes_Tooltip);

				public static Setting<bool> showTierN = new Setting<bool>("FractalTierN", DefaultValue: true, () => Strings.Fractals_DailyTierN);

				public static Setting<bool> showRecs = new Setting<bool>("FractalRecs", DefaultValue: true, () => Strings.Fractals_DailyRecommended);

				public static Setting<bool> tomorrow = new Setting<bool>("FractalTierTomorrow", DefaultValue: false, () => Strings.Settings_Fractals_TomorrowTier, () => Strings.Settings_Fractals_TomorrowTier_Tooltip);

				public static Setting<StrikeComplete> completionMethod = new Setting<StrikeComplete>("RCFractalComplete", StrikeComplete.MAP_CHANGE, () => Strings.Settings_Strike_Completion, () => Strings.Settings_Fractals_Completion);
			}
		}
	}
}
