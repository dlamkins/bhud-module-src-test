using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Services;
using RaidClears.Localization;
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
					encounterBox.VisiblityChanged(Service.StrikeData.GetMissionVisible(Service.StrikeData.GetStrikeMissionByName(encounter.name)));
					encounterBox.TextColorSetting(settings.Style.Color.Text);
					encounter.SetGridBoxReference(encounterBox);
					encounter.WatchColorSettings(settings.Style.Color.Cleared, settings.Style.Color.NotCleared);
				}
			}
			strikes.Add(new PriorityStrikes(Strings.StrikeGroup_Priority, 11, Strings.StrikeGroup_Priority_abbr, new List<BoxModel>(), (Container)(object)panel));
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
