using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Strikes.Models
{
	public static class StrikeMetaData
	{
		private static StrikeSettings Settings => Service.Settings.StrikeSettings;

		public static IEnumerable<Strike> Create(StrikesPanel panel)
		{
			StrikeSettings settings = Service.Settings.StrikeSettings;
			List<Strike> strikes = GetStrikeMetaData().ToList();
			foreach (Strike strike in strikes)
			{
				GridGroup group = new GridGroup((Container)(object)panel, settings.Style.Layout);
				((FlowPanel)(object)group).VisiblityChanged(Service.StrikeData.GetExpansionVisible(Service.StrikeData.GetExpansionStrikesByName(strike.name)));
				strike.SetGridGroupReference(group);
				GridBox labelBox = new GridBox((Container)(object)group, strike.shortName, strike.name, settings.Style.LabelOpacity, settings.Style.FontSize);
				strike.SetGroupLabelReference(labelBox);
				labelBox.LayoutChange(settings.Style.Layout);
				labelBox.LabelDisplayChange(settings.Style.LabelDisplay, strike.shortName, strike.shortName);
				foreach (int index in Enumerable.Range(0, strike.boxes.Count()))
				{
					BoxModel encounter = strike.boxes.ToArray()[index];
					GridBox encounterBox = new GridBox((Container)(object)group, encounter.shortName, encounter.name, settings.Style.GridOpacity, settings.Style.FontSize);
					encounterBox.VisiblityChanged(Service.StrikeData.GetMissionVisible(Service.StrikeData.GetBossEncounterByName(encounter.name)));
					encounterBox.TextColorSetting(settings.Style.Color.Text);
					encounter.SetGridBoxReference(encounterBox);
					encounter.WatchColorSettings(settings.Style.Color.Cleared, settings.Style.Color.NotCleared);
				}
			}
			if (Service.DailyBountyData.Enabled)
			{
				ExpansionStrikes priorityMeta = Service.StrikeData.Priority;
				string todayShortName = Service.StrikeSettings.GetEncounterLabel(priorityMeta.Id);
				strikes.Add(new DailyBounty(priorityMeta.Name, priorityMeta.Id, 12, todayShortName, new List<BoxModel>(), (Container)(object)panel));
				ExpansionStrikes tomorrowMeta = Service.StrikeData.PriorityTomorrow;
				string tomorrowShortName = Service.StrikeSettings.GetEncounterLabel(tomorrowMeta.Id);
				strikes.Add(new DailyBountyTomorrow(tomorrowMeta.Name, tomorrowMeta.Id, 13, tomorrowShortName, new List<BoxModel>(), (Container)(object)panel));
			}
			return strikes;
		}

		private static IEnumerable<Strike> GetStrikeMetaData()
		{
			List<Strike> strikes = new List<Strike>();
			foreach (ExpansionStrikes expansion in Service.StrikeData.Expansions)
			{
				strikes.Add(new Strike(expansion));
			}
			return strikes;
		}
	}
}
