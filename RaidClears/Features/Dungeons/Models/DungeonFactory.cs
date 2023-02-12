using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Models;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Dungeons.Models
{
	public static class DungeonFactory
	{
		public const int FrequenterIndex = 8;

		public const string FrequenterID = "freq";

		private static DungeonSettings Settings => Service.Settings.DungeonSettings;

		public static IEnumerable<Dungeon> Create(DungeonPanel panel)
		{
			Dungeon[] dungeons = GetDungeonMetaData();
			Dungeon[] array = dungeons;
			foreach (Dungeon dungeon in array)
			{
				GridGroup group = new GridGroup((Container)(object)panel, Settings.Style.Layout);
				if (dungeon.index == 8)
				{
					((FlowPanel)(object)group).VisiblityChanged(Settings.DungeonFrequenterVisible);
				}
				else
				{
					((FlowPanel)(object)group).VisiblityChanged(Settings.DungeonPaths.ElementAt(dungeon.index));
				}
				dungeon.SetGridGroupReference(group);
				GridBox labelBox = new GridBox((Container)(object)group, dungeon.shortName, dungeon.name, Settings.Style.LabelOpacity, Settings.Style.FontSize);
				labelBox.LayoutChange(Settings.Style.Layout);
				dungeon.SetGroupLabelReference(labelBox);
				labelBox.LabelDisplayChange(Settings.Style.LabelDisplay, (dungeon.index + 1).ToString(), dungeon.shortName);
				foreach (Path encounter in dungeon.boxes.OfType<Path>())
				{
					GridBox encounterBox = new GridBox((Container)(object)group, encounter.shortName, encounter.name, Settings.Style.GridOpacity, Settings.Style.FontSize);
					encounter.SetGridBoxReference(encounterBox);
					encounter.WatchColorSettings(Settings.Style.Color.Cleared, Settings.Style.Color.NotCleared);
					encounter.RegisterFrequenterSettings(Settings.DungeonHighlightFrequenter, Settings.DungeonPanelColorFreq, Settings.Style.Color.Text);
				}
			}
			return dungeons;
		}

		private static SettingEntry<bool> GetDungeonSelectionByIndex(int index, DungeonSettings settings)
		{
			return settings.DungeonPaths.ElementAt(index);
		}

		private static Dungeon[] GetDungeonMetaData()
		{
			return new Dungeon[9]
			{
				new Dungeon($"Ascalonian Catacombs\nStory {30}, Explore {35}", 0, "AC", new BoxModel[4]
				{
					new Path(Encounters.DungeonPaths.AscalonianCatacombsStory),
					new Path(Encounters.DungeonPaths.AscalonianCatacombsHodgins),
					new Path(Encounters.DungeonPaths.AscalonianCatacombsDetha),
					new Path(Encounters.DungeonPaths.AscalonianCatacombsTzark)
				}),
				new Dungeon($"Caudecus Manor\nStory {40}, Explore {45}", 1, "CM", new BoxModel[4]
				{
					new Path(Encounters.DungeonPaths.CaudecusManorStory),
					new Path(Encounters.DungeonPaths.CaudecusManorAsura),
					new Path(Encounters.DungeonPaths.CaudecusManorSeraph),
					new Path(Encounters.DungeonPaths.CaudecusManorButler)
				}),
				new Dungeon($"Twilight Arbor\nStory {50}, Explore {55}", 2, "TA", new BoxModel[4]
				{
					new Path(Encounters.DungeonPaths.TwilightArborStory),
					new Path(Encounters.DungeonPaths.TwilightArborLeurent),
					new Path(Encounters.DungeonPaths.TwilightArborVevina),
					new Path(Encounters.DungeonPaths.TwilightArborAetherPath)
				}),
				new Dungeon($"Sorrows Embrace\nStory {60}, Explore {65}", 3, "SE", new BoxModel[4]
				{
					new Path(Encounters.DungeonPaths.SorrowsEmbraceStory),
					new Path(Encounters.DungeonPaths.SorrowsEmbraceFergg),
					new Path(Encounters.DungeonPaths.SorrowsEmbraceRasalov),
					new Path(Encounters.DungeonPaths.SorrowsEmbraceKoptev)
				}),
				new Dungeon($"Citadel of Flame\nStory {70}, Explore {75}", 4, "CoF", new BoxModel[4]
				{
					new Path(Encounters.DungeonPaths.CitadelOfFlameStory),
					new Path(Encounters.DungeonPaths.CitadelOfFlameFerrah),
					new Path(Encounters.DungeonPaths.CitadelOfFlameMagg),
					new Path(Encounters.DungeonPaths.CitadelOfFlameRhiannon)
				}),
				new Dungeon($"Honor of the Waves\nStory {76}, Explore {80}", 5, "HW", new BoxModel[4]
				{
					new Path(Encounters.DungeonPaths.HonorOfTheWavesStory),
					new Path(Encounters.DungeonPaths.HonorOfTheWavesButcher),
					new Path(Encounters.DungeonPaths.HonorOfTheWavesPlunderer),
					new Path(Encounters.DungeonPaths.HonorOfTheWavesZealot)
				}),
				new Dungeon($"Crucible of Eternity\nStory {78}, Explore {80}", 6, "CoE", new BoxModel[4]
				{
					new Path(Encounters.DungeonPaths.CrucibleOfEternityStory),
					new Path(Encounters.DungeonPaths.CrucibleOfEternitySubmarine),
					new Path(Encounters.DungeonPaths.CrucibleOfEternityTeleporter),
					new Path(Encounters.DungeonPaths.CrucibleOfEternityFrontDoor)
				}),
				new Dungeon($"Ruined City of Arah\nExplore {80}", 7, "Arah", new BoxModel[4]
				{
					new Path(Encounters.DungeonPaths.RuinedCityOfArahJotun),
					new Path(Encounters.DungeonPaths.RuinedCityOfArahMursaat),
					new Path(Encounters.DungeonPaths.RuinedCityOfArahForgotten),
					new Path(Encounters.DungeonPaths.RuinedCityOfArahSeer)
				}),
				new Dungeon("Frequenter Achievement Summary", 8, "Freq", new BoxModel[1]
				{
					new Path("freq", "Frequenter Achievement Paths Finished", "0/8")
				})
			};
		}
	}
}
