using System;

namespace RaidClears.Features.Shared.Enums.Extensions
{
	public static class DungeonPathsExtensions
	{
		public static string GetLabel(this Encounters.DungeonPaths value)
		{
			return value switch
			{
				Encounters.DungeonPaths.AscalonianCatacombsStory => "Story", 
				Encounters.DungeonPaths.AscalonianCatacombsHodgins => "hodgins", 
				Encounters.DungeonPaths.AscalonianCatacombsDetha => "detha", 
				Encounters.DungeonPaths.AscalonianCatacombsTzark => "tzark", 
				Encounters.DungeonPaths.CaudecusManorStory => "Story", 
				Encounters.DungeonPaths.CaudecusManorAsura => "asura", 
				Encounters.DungeonPaths.CaudecusManorSeraph => "seraph", 
				Encounters.DungeonPaths.CaudecusManorButler => "butler", 
				Encounters.DungeonPaths.TwilightArborStory => "Story", 
				Encounters.DungeonPaths.TwilightArborLeurent => "leurent (Up)", 
				Encounters.DungeonPaths.TwilightArborVevina => "vevina (Forward)", 
				Encounters.DungeonPaths.TwilightArborAetherPath => "aetherpath", 
				Encounters.DungeonPaths.SorrowsEmbraceStory => "Story", 
				Encounters.DungeonPaths.SorrowsEmbraceFergg => "fergg", 
				Encounters.DungeonPaths.SorrowsEmbraceRasalov => "rasalov", 
				Encounters.DungeonPaths.SorrowsEmbraceKoptev => "koptev", 
				Encounters.DungeonPaths.CitadelOfFlameStory => "Story", 
				Encounters.DungeonPaths.CitadelOfFlameFerrah => "ferrah", 
				Encounters.DungeonPaths.CitadelOfFlameMagg => "magg", 
				Encounters.DungeonPaths.CitadelOfFlameRhiannon => "rhiannon", 
				Encounters.DungeonPaths.HonorOfTheWavesStory => "Story", 
				Encounters.DungeonPaths.HonorOfTheWavesButcher => "butcher", 
				Encounters.DungeonPaths.HonorOfTheWavesPlunderer => "plunderer", 
				Encounters.DungeonPaths.HonorOfTheWavesZealot => "zealot", 
				Encounters.DungeonPaths.CrucibleOfEternityStory => "Story", 
				Encounters.DungeonPaths.CrucibleOfEternitySubmarine => "submarine", 
				Encounters.DungeonPaths.CrucibleOfEternityTeleporter => "teleporter", 
				Encounters.DungeonPaths.CrucibleOfEternityFrontDoor => "front_door", 
				Encounters.DungeonPaths.RuinedCityOfArahStory => "Story", 
				Encounters.DungeonPaths.RuinedCityOfArahJotun => "jotun", 
				Encounters.DungeonPaths.RuinedCityOfArahMursaat => "mursaat", 
				Encounters.DungeonPaths.RuinedCityOfArahForgotten => "forgotten", 
				Encounters.DungeonPaths.RuinedCityOfArahSeer => "seer", 
				_ => throw new InvalidOperationException(), 
			};
		}

		public static string GetLabelShort(this Encounters.DungeonPaths value)
		{
			return value switch
			{
				Encounters.DungeonPaths.AscalonianCatacombsStory => "S", 
				Encounters.DungeonPaths.AscalonianCatacombsHodgins => "H", 
				Encounters.DungeonPaths.AscalonianCatacombsDetha => "D", 
				Encounters.DungeonPaths.AscalonianCatacombsTzark => "T", 
				Encounters.DungeonPaths.CaudecusManorStory => "S", 
				Encounters.DungeonPaths.CaudecusManorAsura => "A", 
				Encounters.DungeonPaths.CaudecusManorSeraph => "S", 
				Encounters.DungeonPaths.CaudecusManorButler => "B", 
				Encounters.DungeonPaths.TwilightArborStory => "S", 
				Encounters.DungeonPaths.TwilightArborLeurent => "Up", 
				Encounters.DungeonPaths.TwilightArborVevina => "Fwd", 
				Encounters.DungeonPaths.TwilightArborAetherPath => "Ae", 
				Encounters.DungeonPaths.SorrowsEmbraceStory => "S", 
				Encounters.DungeonPaths.SorrowsEmbraceFergg => "F", 
				Encounters.DungeonPaths.SorrowsEmbraceRasalov => "R", 
				Encounters.DungeonPaths.SorrowsEmbraceKoptev => "K", 
				Encounters.DungeonPaths.CitadelOfFlameStory => "S", 
				Encounters.DungeonPaths.CitadelOfFlameFerrah => "F", 
				Encounters.DungeonPaths.CitadelOfFlameMagg => "M", 
				Encounters.DungeonPaths.CitadelOfFlameRhiannon => "R", 
				Encounters.DungeonPaths.HonorOfTheWavesStory => "S", 
				Encounters.DungeonPaths.HonorOfTheWavesButcher => "B", 
				Encounters.DungeonPaths.HonorOfTheWavesPlunderer => "P", 
				Encounters.DungeonPaths.HonorOfTheWavesZealot => "Z", 
				Encounters.DungeonPaths.CrucibleOfEternityStory => "S", 
				Encounters.DungeonPaths.CrucibleOfEternitySubmarine => "S", 
				Encounters.DungeonPaths.CrucibleOfEternityTeleporter => "T", 
				Encounters.DungeonPaths.CrucibleOfEternityFrontDoor => "F", 
				Encounters.DungeonPaths.RuinedCityOfArahStory => "S", 
				Encounters.DungeonPaths.RuinedCityOfArahJotun => "J", 
				Encounters.DungeonPaths.RuinedCityOfArahMursaat => "M", 
				Encounters.DungeonPaths.RuinedCityOfArahForgotten => "F", 
				Encounters.DungeonPaths.RuinedCityOfArahSeer => "S", 
				_ => throw new ArgumentOutOfRangeException("value", value, null), 
			};
		}

		public static string GetApiLabel(this Encounters.DungeonPaths value)
		{
			return value switch
			{
				Encounters.DungeonPaths.AscalonianCatacombsStory => "ac_story", 
				Encounters.DungeonPaths.AscalonianCatacombsHodgins => "hodgins", 
				Encounters.DungeonPaths.AscalonianCatacombsDetha => "detha", 
				Encounters.DungeonPaths.AscalonianCatacombsTzark => "tzark", 
				Encounters.DungeonPaths.CaudecusManorStory => "cm_story", 
				Encounters.DungeonPaths.CaudecusManorAsura => "asura", 
				Encounters.DungeonPaths.CaudecusManorSeraph => "seraph", 
				Encounters.DungeonPaths.CaudecusManorButler => "butler", 
				Encounters.DungeonPaths.TwilightArborStory => "ta_story", 
				Encounters.DungeonPaths.TwilightArborLeurent => "leurent", 
				Encounters.DungeonPaths.TwilightArborVevina => "vevina", 
				Encounters.DungeonPaths.TwilightArborAetherPath => "aetherpath", 
				Encounters.DungeonPaths.SorrowsEmbraceStory => "se_story", 
				Encounters.DungeonPaths.SorrowsEmbraceFergg => "fergg", 
				Encounters.DungeonPaths.SorrowsEmbraceRasalov => "rasalov", 
				Encounters.DungeonPaths.SorrowsEmbraceKoptev => "koptev", 
				Encounters.DungeonPaths.CitadelOfFlameStory => "cof_story", 
				Encounters.DungeonPaths.CitadelOfFlameFerrah => "ferrah", 
				Encounters.DungeonPaths.CitadelOfFlameMagg => "magg", 
				Encounters.DungeonPaths.CitadelOfFlameRhiannon => "rhiannon", 
				Encounters.DungeonPaths.HonorOfTheWavesStory => "hotw_story", 
				Encounters.DungeonPaths.HonorOfTheWavesButcher => "butcher", 
				Encounters.DungeonPaths.HonorOfTheWavesPlunderer => "plunderer", 
				Encounters.DungeonPaths.HonorOfTheWavesZealot => "zealot", 
				Encounters.DungeonPaths.CrucibleOfEternityStory => "coe_story", 
				Encounters.DungeonPaths.CrucibleOfEternitySubmarine => "submarine", 
				Encounters.DungeonPaths.CrucibleOfEternityTeleporter => "teleporter", 
				Encounters.DungeonPaths.CrucibleOfEternityFrontDoor => "front_door", 
				Encounters.DungeonPaths.RuinedCityOfArahStory => "arah_story", 
				Encounters.DungeonPaths.RuinedCityOfArahJotun => "jotun", 
				Encounters.DungeonPaths.RuinedCityOfArahMursaat => "mursaat", 
				Encounters.DungeonPaths.RuinedCityOfArahForgotten => "forgotten", 
				Encounters.DungeonPaths.RuinedCityOfArahSeer => "seer", 
				_ => throw new ArgumentOutOfRangeException("value", value, null), 
			};
		}
	}
}
