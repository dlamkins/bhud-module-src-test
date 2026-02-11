using System.Collections.Generic;
using Blish_HUD.Settings;

namespace RaidClears.Settings.Models
{
	public class RaidSettings
	{
		public IEnumerable<SettingEntry<bool>> RaidWings { get; set; }

		public DisplayStyle Style { get; set; }

		public GenericSettings Generic { get; set; }

		public SettingEntry<bool> RaidPanelHighlightEmbolden { get; set; }

		public SettingEntry<bool> RaidPanelHighlightCotM { get; set; }

		public SettingEntry<bool> RaidPanelMentorProgress { get; set; }

		public SettingEntry<bool> RaidPanelMentorProgressPopup { get; set; }

		public SettingEntry<string> RaidPanelColorEmbolden { get; set; }

		public SettingEntry<string> RaidPanelColorCotm { get; set; }

		public RaidSettings(SettingCollection settings)
		{
			Generic = new GenericSettings
			{
				Enabled = settings.DefineSetting(Settings.Raids.General.enabled),
				PositionLock = settings.DefineSetting(Settings.Raids.General.positionLock),
				Location = settings.DefineSetting(Settings.Raids.General.location),
				Tooltips = settings.DefineSetting(Settings.Raids.General.tooltips),
				ToolbarIcon = settings.DefineSetting(Settings.Raids.General.toolbarIcon),
				ShowHideKeyBind = settings.DefineSetting(Settings.Raids.General.keyBind),
				Visible = settings.DefineSetting(Settings.Raids.General.visible)
			};
			RaidPanelHighlightEmbolden = settings.DefineSetting(Settings.Raids.Module.highlightEmbolden);
			RaidPanelHighlightCotM = settings.DefineSetting(Settings.Raids.Module.highlightCotm);
			RaidPanelMentorProgress = settings.DefineSetting(Settings.Raids.Module.mentorProgress);
			RaidPanelMentorProgressPopup = settings.DefineSetting(Settings.Raids.Module.mentorProgressPopup);
			Style = new DisplayStyle
			{
				Color = new DisplayColor
				{
					Background = settings.DefineSetting(Settings.Raids.Style.Color.background),
					Cleared = settings.DefineSetting(Settings.Raids.Style.Color.cleared),
					Text = settings.DefineSetting(Settings.Raids.Style.Color.text),
					NotCleared = settings.DefineSetting(Settings.Raids.Style.Color.uncleared)
				},
				Layout = settings.DefineSetting(Settings.Raids.Style.layout),
				FontSize = settings.DefineSetting(Settings.Raids.Style.fontSize),
				BgOpacity = settings.DefineSetting(Settings.Raids.Style.backgroundOpacity),
				GridOpacity = settings.DefineSetting(Settings.Raids.Style.gridOpacity),
				LabelDisplay = settings.DefineSetting(Settings.Raids.Style.labelDisplay),
				LabelOpacity = settings.DefineSetting(Settings.Raids.Style.labelOpacity)
			};
			SettingComplianceExtensions.SetRange(Style.GridOpacity, 0.1f, 1f);
			SettingComplianceExtensions.SetRange(Style.LabelOpacity, 0.1f, 1f);
			SettingComplianceExtensions.SetRange(Style.BgOpacity, 0f, 1f);
			RaidPanelColorEmbolden = settings.DefineSetting(Settings.Raids.Style.Color.embolden);
			RaidPanelColorCotm = settings.DefineSetting(Settings.Raids.Style.Color.cotm);
			CleanUpOldSettings(settings);
		}

		public void CleanUpOldSettings(SettingCollection settings)
		{
			settings.UndefineSetting("RCw1");
			settings.UndefineSetting("RCw2");
			settings.UndefineSetting("RCw3");
			settings.UndefineSetting("RCw4");
			settings.UndefineSetting("RCw5");
			settings.UndefineSetting("RCw6");
			settings.UndefineSetting("RCw7");
			settings.UndefineSetting("RCw8");
		}
	}
}
