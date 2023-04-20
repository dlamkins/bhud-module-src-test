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

				public static Setting<Point> location = new Setting<Point>("RCDungeonLoc", new Point(250, 500));

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
					public static Setting<string> uncleared = new Setting<string>("DunColNotCleared", "#781414", () => Strings.Setting_Raid_ColNotClear_Label, () => Strings.Setting_Raid_ColNotClear_Tooltip);

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

				public static Setting<Point> location = new Setting<Point>("RCLocation", new Point(250, 210));

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
					public static Setting<string> uncleared = new Setting<string>("colNotCleared", "#781414", () => Strings.Setting_Raid_ColNotClear_Label, () => Strings.Setting_Raid_ColNotClear_Tooltip);

					public static Setting<string> cleared = new Setting<string>("colCleared", "#147814", () => Strings.Setting_Raid_ColClear_Label, () => Strings.Setting_Raid_ColClear_Tooltip);

					public static Setting<string> text = new Setting<string>("colText", "#FFFFFF", () => Strings.Setting_Raid_ColText_Label, () => Strings.Setting_Raid_ColText_Tooltip);

					public static Setting<string> cotm = new Setting<string>("colCotm", "#F3F527", () => Strings.Setting_Raid_ColCotm_Label, () => Strings.Setting_Raid_ColCotm_Tooltip);

					public static Setting<string> embolden = new Setting<string>("colEmbolden", "#5050FF", () => Strings.Setting_Raid_ColEmbolden_Label, () => Strings.Setting_Raid_ColEmbolden_Tooltip);

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

				public static Setting<bool>[] raidWings = new Setting<bool>[8]
				{
					new Setting<bool>("RCw1", DefaultValue: true, () => Strings.Setting_Raid_W1_Label, () => Strings.Setting_Raid_W1_Tooltip),
					new Setting<bool>("RCw2", DefaultValue: true, () => Strings.Setting_Raid_W2_Label, () => Strings.Setting_Raid_W2_Tooltip),
					new Setting<bool>("RCw3", DefaultValue: true, () => Strings.Setting_Raid_W3_Label, () => Strings.Setting_Raid_W3_Tooltip),
					new Setting<bool>("RCw4", DefaultValue: true, () => Strings.Setting_Raid_W4_Label, () => Strings.Setting_Raid_W4_Tooltip),
					new Setting<bool>("RCw5", DefaultValue: true, () => Strings.Setting_Raid_W5_Label, () => Strings.Setting_Raid_W5_Tooltip),
					new Setting<bool>("RCw6", DefaultValue: true, () => Strings.Setting_Raid_W6_Label, () => Strings.Setting_Raid_W6_Tooltip),
					new Setting<bool>("RCw7", DefaultValue: true, () => Strings.Setting_Raid_W7_Label, () => Strings.Setting_Raid_W7_Tooltip),
					new Setting<bool>("RCw8", DefaultValue: true, () => "Wing 8", () => "Happy April 1st")
				};
			}
		}

		public static class Strikes
		{
			public static class Style
			{
				public static class Color
				{
					public static Setting<string> uncleared = new Setting<string>("StkcolNotCleared", "#781414", () => Strings.Setting_Strike_ColNotClear_Label, () => Strings.Setting_Strike_ColNotClear_Tooltip);

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

				public static Setting<Point> location = new Setting<Point>("RCStkLocation", new Point(250, 370));

				public static Setting<bool> positionLock = new Setting<bool>("RCStkDrag", DefaultValue: true, () => Strings.Setting_Strike_Drag_Label, () => Strings.Setting_Strike_Drag_Tooltip);

				public static Setting<bool> tooltips = new Setting<bool>("RCStktooltips", DefaultValue: true, () => Strings.Setting_Strike_Tooltips_Label, () => Strings.Setting_Strike_Tooltips_Tooltip);

				public static Setting<bool> toolbarIcon = new Setting<bool>("RCStkCornerIcon", DefaultValue: true, () => Strings.Setting_Strike_Icon_Label, () => Strings.Setting_Strike_Icon_Tooltip);

				public static Setting<bool> visible = new Setting<bool>("RCStkActive", DefaultValue: true, () => Strings.Setting_Strike_Visible_Label, () => Strings.Setting_Strike_Visible_Tooltip);

				public static Setting<KeyBinding> keyBind = new Setting<KeyBinding>("RCStkkeybind", new KeyBinding((Keys)0), () => Strings.Setting_Strike_Keybind_Label, () => Strings.Setting_Strike_Keybind_Tooltip);
			}

			public static class Module
			{
				public static Setting<bool>[] ibsMissions = new Setting<bool>[6]
				{
					new StrikeSetting<bool>("StrikeVis_shiverpeak_pass", DefaultValue: true, () => Strings.Setting_Strike_SP_Label),
					new StrikeSetting<bool>("StrikeVis_fraenir_of_jormag", DefaultValue: true, () => Strings.Setting_Strike_FoJ_Label),
					new StrikeSetting<bool>("StrikeVis_voice_and_claw", DefaultValue: true, () => Strings.Setting_Strike_VandC_Label),
					new StrikeSetting<bool>("StrikeVis_whisper_of_jormag", DefaultValue: true, () => Strings.Setting_Strike_WoJ_Label),
					new StrikeSetting<bool>("StrikeVis_boneskinner", DefaultValue: true, () => Strings.Setting_Strike_BS_Label),
					new StrikeSetting<bool>("StrikeVis_cold_war", DefaultValue: true, () => Strings.Setting_Strike_CW_Label)
				};

				public static Setting<bool>[] eodMissions = new Setting<bool>[5]
				{
					new StrikeSetting<bool>("StrikeVis_aetherblade_hideout", DefaultValue: true, () => Strings.Setting_Strike_AH_Label),
					new StrikeSetting<bool>("StrikeVis_xunlai_jade_junkyard", DefaultValue: true, () => Strings.Setting_Strike_XJJ_Label),
					new StrikeSetting<bool>("StrikeVis_kaineng_overlook", DefaultValue: true, () => Strings.Setting_Strike_KO_Label),
					new StrikeSetting<bool>("StrikeVis_harvest_temple", DefaultValue: true, () => Strings.Setting_Strike_HT_Label),
					new StrikeSetting<bool>("StrikeVis_old_lion_court", DefaultValue: true, () => Strings.Setting_Strike_OLC_Label)
				};

				public static Setting<bool> showIbs = new Setting<bool>("StrikeVis_ibs", DefaultValue: true, () => Strings.Setting_Strike_IBS);

				public static Setting<bool> showEod = new Setting<bool>("StrikeVis_eod", DefaultValue: true, () => Strings.Setting_Strike_Eod);

				public static Setting<bool> showPriority = new Setting<bool>("StrikeVis_priority", DefaultValue: true, () => Strings.Setting_Stike_Priority);

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
					public static Setting<string> uncleared = new Setting<string>("FraccolNotCleared", "#781414", () => Strings.Setting_Fractals_ColNotClear_Label, () => Strings.Setting_Fractals_ColNotClear_Tooltip);

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

				public static Setting<Point> location = new Setting<Point>("RCFracLocation", new Point(250, 445));

				public static Setting<bool> positionLock = new Setting<bool>("RCFracDrag", DefaultValue: true, () => Strings.Setting_Fractals_Drag_Label, () => Strings.Setting_Fractals_Drag_Tooltip);

				public static Setting<bool> tooltips = new Setting<bool>("RCFractooltips", DefaultValue: true, () => Strings.Setting_Fractals_Tooltips_Label, () => Strings.Setting_Fractals_Tooltips_Tooltip);

				public static Setting<bool> toolbarIcon = new Setting<bool>("RCFracCornerIcon", DefaultValue: true, () => Strings.Setting_Fractals_Icon_Label, () => Strings.Setting_Fractals_Icon_Tooltip);

				public static Setting<bool> visible = new Setting<bool>("RCFracActive", DefaultValue: true, () => Strings.Setting_Fractals_Visible_Label, () => Strings.Setting_Fractals_Visible_Tooltip);

				public static Setting<KeyBinding> keyBind = new Setting<KeyBinding>("RCSFrackeybind", new KeyBinding((Keys)0), () => Strings.Setting_Fractals_Keybind_Label, () => Strings.Setting_Fractals_Keybind_Tooltip);
			}

			public static class Module
			{
				public static Setting<bool> showTierN = new Setting<bool>("FractalTierN", DefaultValue: true, () => Strings.Fractals_DailyTierN);

				public static Setting<bool> showRecs = new Setting<bool>("FractalRecs", DefaultValue: true, () => Strings.Fractals_DailyRecommended);

				public static Setting<bool> tomorrow = new Setting<bool>("FractalTierTomorrow", DefaultValue: false, () => "Tomorrow's Tier", () => "Show tomorow's TierN fractals. Useful for statics that pre-clear before reset");

				public static Setting<StrikeComplete> completionMethod = new Setting<StrikeComplete>("RCFractalComplete", StrikeComplete.MAP_CHANGE, () => Strings.Settings_Strike_Completion, () => Strings.Settings_Fractals_Completion);
			}
		}
	}
}
