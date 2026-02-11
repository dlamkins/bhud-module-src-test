using Blish_HUD.Settings;
using RaidClears.Settings.Enums;

namespace RaidClears.Settings.Models
{
	public class StrikeSettings
	{
		public SettingEntry<bool> AnchorToRaidPanel { get; set; }

		public SettingEntry<StrikeComplete> StrikeCompletion { get; set; }

		public DisplayStyle Style { get; set; }

		public GenericSettings Generic { get; set; }

		public StrikeSettings(SettingCollection settings)
		{
			Style = new DisplayStyle
			{
				Color = new DisplayColor
				{
					Background = settings.DefineSetting(Settings.Strikes.Style.Color.background),
					NotCleared = settings.DefineSetting(Settings.Strikes.Style.Color.uncleared),
					Cleared = settings.DefineSetting(Settings.Strikes.Style.Color.cleared),
					Text = settings.DefineSetting(Settings.Strikes.Style.Color.text)
				},
				FontSize = settings.DefineSetting(Settings.Strikes.Style.fontSize),
				BgOpacity = settings.DefineSetting(Settings.Strikes.Style.backgroundOpacity),
				GridOpacity = settings.DefineSetting(Settings.Strikes.Style.gridOpacity),
				LabelDisplay = settings.DefineSetting(Settings.Strikes.Style.labelDisplay),
				LabelOpacity = settings.DefineSetting(Settings.Strikes.Style.labelOpacity),
				Layout = settings.DefineSetting(Settings.Strikes.Style.layout)
			};
			SettingComplianceExtensions.SetRange(Style.GridOpacity, 0.1f, 1f);
			SettingComplianceExtensions.SetRange(Style.LabelOpacity, 0.1f, 1f);
			SettingComplianceExtensions.SetRange(Style.BgOpacity, 0f, 1f);
			SettingComplianceExtensions.SetExcluded<LabelDisplay>(Style.LabelDisplay, new LabelDisplay[1]);
			Generic = new GenericSettings
			{
				Location = settings.DefineSetting(Settings.Strikes.General.location),
				Enabled = settings.DefineSetting(Settings.Strikes.General.enabled),
				PositionLock = settings.DefineSetting(Settings.Strikes.General.positionLock),
				ShowHideKeyBind = settings.DefineSetting(Settings.Strikes.General.keyBind),
				ToolbarIcon = settings.DefineSetting(Settings.Strikes.General.toolbarIcon),
				Visible = settings.DefineSetting(Settings.Strikes.General.visible),
				Tooltips = settings.DefineSetting(Settings.Strikes.General.tooltips)
			};
			AnchorToRaidPanel = settings.DefineSetting(Settings.Strikes.Module.anchorToRaids);
			StrikeCompletion = settings.DefineSetting(Settings.Strikes.Module.strikeCompletion);
			CleanUpOldSettings(settings);
		}

		public void CleanUpOldSettings(SettingCollection settings)
		{
			settings.UndefineSetting("StrikeVis_ibs");
			settings.UndefineSetting("StrikeVis_eod");
			settings.UndefineSetting("StrikeVis_soto");
			settings.UndefineSetting("StrikeVis_priority");
			settings.UndefineSetting("StrikeVis_shiverpeak_pass");
			settings.UndefineSetting("StrikeVis_fraenir_of_jormag");
			settings.UndefineSetting("StrikeVis_voice_and_claw");
			settings.UndefineSetting("StrikeVis_whisper_of_jormag");
			settings.UndefineSetting("StrikeVis_boneskinner");
			settings.UndefineSetting("StrikeVis_cold_war");
			settings.UndefineSetting("StrikeVis_dragonstorm");
			settings.UndefineSetting("StrikeVis_aetherblade_hideout");
			settings.UndefineSetting("StrikeVis_xunlai_jade_junkyard");
			settings.UndefineSetting("StrikeVis_kaineng_overlook");
			settings.UndefineSetting("StrikeVis_harvest_temple");
			settings.UndefineSetting("StrikeVis_old_lion_court");
			settings.UndefineSetting("StrikeVis_cosmic_observatory");
			settings.UndefineSetting("StrikeVis_temple_of_febe");
		}
	}
}
