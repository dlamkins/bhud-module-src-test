using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Settings;
using RaidClears.Settings.Enums;

namespace RaidClears.Settings.Models
{
	public class StrikeSettings
	{
		public SettingEntry<bool> StrikeVisibleIbs { get; set; }

		public SettingEntry<bool> StrikeVisibleEod { get; set; }

		public SettingEntry<bool> StrikeVisibleSotO { get; set; }

		public SettingEntry<bool> StrikeVisiblePriority { get; set; }

		public SettingEntry<bool> AnchorToRaidPanel { get; set; }

		public SettingEntry<StrikeComplete> StrikeCompletion { get; set; }

		public IEnumerable<SettingEntry<bool>> IbsMissions { get; set; }

		public IEnumerable<SettingEntry<bool>> EodMissions { get; set; }

		public IEnumerable<SettingEntry<bool>> SotOMissions { get; set; }

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
			IbsMissions = ((IEnumerable<Setting<bool>>)Settings.Strikes.Module.ibsMissions).Select((Func<Setting<bool>, SettingEntry<bool>>)settings.DefineSetting);
			EodMissions = ((IEnumerable<Setting<bool>>)Settings.Strikes.Module.eodMissions).Select((Func<Setting<bool>, SettingEntry<bool>>)settings.DefineSetting);
			SotOMissions = ((IEnumerable<Setting<bool>>)Settings.Strikes.Module.sotoMissions).Select((Func<Setting<bool>, SettingEntry<bool>>)settings.DefineSetting);
			StrikeVisibleIbs = settings.DefineSetting(Settings.Strikes.Module.showIbs);
			StrikeVisibleEod = settings.DefineSetting(Settings.Strikes.Module.showEod);
			StrikeVisibleSotO = settings.DefineSetting(Settings.Strikes.Module.showSotO);
			StrikeVisiblePriority = settings.DefineSetting(Settings.Strikes.Module.showPriority);
			AnchorToRaidPanel = settings.DefineSetting(Settings.Strikes.Module.anchorToRaids);
			StrikeCompletion = settings.DefineSetting(Settings.Strikes.Module.strikeCompletion);
		}
	}
}
