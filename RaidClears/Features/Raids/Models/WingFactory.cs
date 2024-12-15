using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Raids.Models
{
	public static class WingFactory
	{
		public static IEnumerable<Wing> Create(RaidPanel panel)
		{
			RaidSettings settings = Service.Settings.RaidSettings;
			List<Wing> wings = GetWingMetaData();
			foreach (Wing wing in wings)
			{
				GridGroup group = new GridGroup((Container)(object)panel, settings.Style.Layout);
				WeeklyModifier weeklyModifier = new WeeklyModifier(Service.RaidData.GetRaidWingByZeroIndex(wing.index));
				((FlowPanel)(object)group).VisiblityChanged(GetWingSelectionByIndex(wing.index));
				wing.SetGridGroupReference(group);
				GridBox labelBox = new GridBox((Container)(object)group, wing.shortName, wing.name, settings.Style.LabelOpacity, settings.Style.FontSize);
				wing.SetGroupLabelReference(labelBox);
				labelBox.LayoutChange(settings.Style.Layout);
				ApplyConditionalTextColoring(labelBox, weeklyModifier, settings);
				labelBox.LabelDisplayChange(settings.Style.LabelDisplay, (wing.index + 1).ToString(), wing.shortName);
				foreach (BoxModel encounter in wing.boxes)
				{
					GridBox encounterBox = new GridBox((Container)(object)group, encounter.shortName, encounter.name, settings.Style.GridOpacity, settings.Style.FontSize);
					encounterBox.VisiblityChanged(Service.RaidSettings.GetEncounterVisibleByApiId(encounter.id));
					encounter.SetGridBoxReference(encounterBox);
					encounter.WatchColorSettings(settings.Style.Color.Cleared, settings.Style.Color.NotCleared);
					ApplyConditionalTextColoring(encounterBox, weeklyModifier, settings);
				}
			}
			return wings;
		}

		private static void ApplyConditionalTextColoring(GridBox box, WeeklyModifier weekly, RaidSettings settings)
		{
			if (weekly.Emboldened)
			{
				box.ConditionalTextColorSetting(settings.RaidPanelHighlightEmbolden, settings.RaidPanelColorEmbolden, settings.Style.Color.Text);
			}
			else if (weekly.CallOfTheMist)
			{
				box.ConditionalTextColorSetting(settings.RaidPanelHighlightCotM, settings.RaidPanelColorCotm, settings.Style.Color.Text);
			}
			else
			{
				box.TextColorSetting(settings.Style.Color.Text);
			}
		}

		private static SettingEntry<bool> GetWingSelectionByIndex(int index)
		{
			RaidWing raidWing = Service.RaidData.GetRaidWingByZeroIndex(index);
			return Service.RaidSettings.GetWingVisible(raidWing);
		}

		private static List<Wing> GetWingMetaData()
		{
			List<Wing> raids = new List<Wing>();
			foreach (ExpansionRaid expansion in Service.RaidData.Expansions)
			{
				foreach (RaidWing wing in expansion.Wings)
				{
					List<BoxModel> encounters = new List<BoxModel>();
					foreach (RaidEncounter encounter in wing.Encounters)
					{
						encounters.Add(new Encounter(encounter));
					}
					Wing wingModel = new Wing(wing.Name, wing.Id, wing.Number - 1, wing.Abbriviation, encounters.ToArray());
					raids.Add(wingModel);
				}
			}
			return raids;
		}
	}
}
