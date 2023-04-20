using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Models;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Raids.Models
{
	public static class WingFactory
	{
		public static IEnumerable<Wing> Create(RaidPanel panel, WeeklyWings weekly)
		{
			RaidSettings settings = Service.Settings.RaidSettings;
			List<Wing> wings = GetWingMetaData();
			if (DateTime.Now.Month == 4 && DateTime.Now.Day == 1)
			{
				wings.Add(new Wing("The Wait for Wing 8", 7, "W8", new BoxModel[4]
				{
					new BoxModel("w8-1", "Not", "N"),
					new BoxModel("w8-2", "Our", "O"),
					new BoxModel("w8-3", "Priority", "P"),
					new BoxModel("w8-4", "Ever", "E")
				}));
			}
			foreach (Wing wing in wings)
			{
				GridGroup group = new GridGroup((Container)(object)panel, settings.Style.Layout);
				((FlowPanel)(object)group).VisiblityChanged(GetWingSelectionByIndex(wing.index, settings));
				wing.SetGridGroupReference(group);
				GridBox labelBox = new GridBox((Container)(object)group, wing.shortName, wing.name, settings.Style.LabelOpacity, settings.Style.FontSize);
				wing.SetGroupLabelReference(labelBox);
				labelBox.LayoutChange(settings.Style.Layout);
				ApplyConditionalTextColoring(labelBox, wing.index, weekly, settings);
				labelBox.LabelDisplayChange(settings.Style.LabelDisplay, (wing.index + 1).ToString(), wing.shortName);
				foreach (BoxModel encounter in wing.boxes)
				{
					GridBox encounterBox = new GridBox((Container)(object)group, encounter.shortName, encounter.name, settings.Style.GridOpacity, settings.Style.FontSize);
					encounter.SetGridBoxReference(encounterBox);
					encounter.WatchColorSettings(settings.Style.Color.Cleared, settings.Style.Color.NotCleared);
					ApplyConditionalTextColoring(encounterBox, wing.index, weekly, settings);
				}
			}
			return wings;
		}

		private static void ApplyConditionalTextColoring(GridBox box, int index, WeeklyWings weekly, RaidSettings settings)
		{
			if (index == weekly.Emboldened)
			{
				box.ConditionalTextColorSetting(settings.RaidPanelHighlightEmbolden, settings.RaidPanelColorEmbolden, settings.Style.Color.Text);
			}
			else if (index == weekly.CallOfTheMist)
			{
				box.ConditionalTextColorSetting(settings.RaidPanelHighlightCotM, settings.RaidPanelColorCotm, settings.Style.Color.Text);
			}
			else
			{
				box.TextColorSetting(settings.Style.Color.Text);
			}
		}

		private static SettingEntry<bool> GetWingSelectionByIndex(int index, RaidSettings settings)
		{
			return settings.RaidWings.ElementAt(index);
		}

		private static List<Wing> GetWingMetaData()
		{
			List<Wing> list = new List<Wing>();
			list.Add(new Wing(Strings.Raid_Wing_1, 0, Strings.Raid_Wing_1_Short, new BoxModel[4]
			{
				new Encounter(Encounters.RaidBosses.ValeGuardian),
				new Encounter(Encounters.RaidBosses.SpiritWoods),
				new Encounter(Encounters.RaidBosses.Gorseval),
				new Encounter(Encounters.RaidBosses.Sabetha)
			}));
			list.Add(new Wing(Strings.Raid_Wing_2, 1, Strings.Raid_Wing_2_Short, new BoxModel[3]
			{
				new Encounter(Encounters.RaidBosses.Slothasor),
				new Encounter(Encounters.RaidBosses.BanditTrio),
				new Encounter(Encounters.RaidBosses.Matthias)
			}));
			list.Add(new Wing(Strings.Raid_Wing_3, 2, Strings.Raid_Wing_3_Short, new BoxModel[4]
			{
				new Encounter(Encounters.RaidBosses.Escort),
				new Encounter(Encounters.RaidBosses.KeepConstruct),
				new Encounter(Encounters.RaidBosses.TwistedCastle),
				new Encounter(Encounters.RaidBosses.Xera)
			}));
			list.Add(new Wing(Strings.Raid_Wing_4, 3, Strings.Raid_Wing_4_Short, new BoxModel[4]
			{
				new Encounter(Encounters.RaidBosses.Cairn),
				new Encounter(Encounters.RaidBosses.MursaatOverseer),
				new Encounter(Encounters.RaidBosses.Samarog),
				new Encounter(Encounters.RaidBosses.Deimos)
			}));
			list.Add(new Wing(Strings.Raid_Wing_5, 4, Strings.Raid_Wing_5_Short, new BoxModel[4]
			{
				new Encounter(Encounters.RaidBosses.SoulessHorror),
				new Encounter(Encounters.RaidBosses.RiverOfSouls),
				new Encounter(Encounters.RaidBosses.StatuesOfGrenth),
				new Encounter(Encounters.RaidBosses.VoiceInTheVoid)
			}));
			list.Add(new Wing(Strings.Raid_Wing_6, 5, Strings.Raid_Wing_6_Short, new BoxModel[3]
			{
				new Encounter(Encounters.RaidBosses.ConjuredAmalgamate),
				new Encounter(Encounters.RaidBosses.TwinLargos),
				new Encounter(Encounters.RaidBosses.Qadim)
			}));
			list.Add(new Wing(Strings.Raid_Wing_7, 6, Strings.Raid_Wing_7_Short, new BoxModel[4]
			{
				new Encounter(Encounters.RaidBosses.Gate),
				new Encounter(Encounters.RaidBosses.Adina),
				new Encounter(Encounters.RaidBosses.Sabir),
				new Encounter(Encounters.RaidBosses.QadimThePeerless)
			}));
			return list;
		}
	}
}
