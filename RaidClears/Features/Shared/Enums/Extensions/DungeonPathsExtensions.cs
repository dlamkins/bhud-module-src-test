using System;

namespace RaidClears.Features.Shared.Enums.Extensions
{
	public static class DungeonPathsExtensions
	{
		public static string GetLabel(this Encounters.DungeonPaths value)
		{
			switch (value)
			{
			case Encounters.DungeonPaths.AscalonianCatacombsStory:
				return "Story";
			case Encounters.DungeonPaths.AscalonianCatacombsHodgins:
				return "hodgins";
			case Encounters.DungeonPaths.AscalonianCatacombsDetha:
				return "detha";
			case Encounters.DungeonPaths.AscalonianCatacombsTzark:
				return "tzark";
			case Encounters.DungeonPaths.CaudecusManorStory:
				return "Story";
			case Encounters.DungeonPaths.CaudecusManorAsura:
				return "asura";
			case Encounters.DungeonPaths.CaudecusManorSeraph:
				return "seraph";
			case Encounters.DungeonPaths.CaudecusManorButler:
				return "butler";
			case Encounters.DungeonPaths.TwilightArborStory:
				return "Story";
			case Encounters.DungeonPaths.TwilightArborLeurent:
				return "leurent (Up)";
			case Encounters.DungeonPaths.TwilightArborVevina:
				return "vevina (Forward)";
			case Encounters.DungeonPaths.TwilightArborAetherPath:
				return "aetherpath";
			case Encounters.DungeonPaths.SorrowsEmbraceStory:
				return "Story";
			case Encounters.DungeonPaths.SorrowsEmbraceFergg:
				return "fergg";
			case Encounters.DungeonPaths.SorrowsEmbraceRasalov:
				return "rasalov";
			case Encounters.DungeonPaths.SorrowsEmbraceKoptev:
				return "koptev";
			case Encounters.DungeonPaths.CitadelOfFlameStory:
				return "Story";
			case Encounters.DungeonPaths.CitadelOfFlameFerrah:
				return "ferrah";
			case Encounters.DungeonPaths.CitadelOfFlameMagg:
				return "magg";
			case Encounters.DungeonPaths.CitadelOfFlameRhiannon:
				return "rhiannon";
			case Encounters.DungeonPaths.HonorOfTheWavesStory:
				return "Story";
			case Encounters.DungeonPaths.HonorOfTheWavesButcher:
				return "butcher";
			case Encounters.DungeonPaths.HonorOfTheWavesPlunderer:
				return "plunderer";
			case Encounters.DungeonPaths.HonorOfTheWavesZealot:
				return "zealot";
			case Encounters.DungeonPaths.CrucibleOfEternityStory:
				return "Story";
			case Encounters.DungeonPaths.CrucibleOfEternitySubmarine:
				return "submarine";
			case Encounters.DungeonPaths.CrucibleOfEternityTeleporter:
				return "teleporter";
			case Encounters.DungeonPaths.CrucibleOfEternityFrontDoor:
				return "front_door";
			case Encounters.DungeonPaths.RuinedCityOfArahStory:
				return "Story";
			case Encounters.DungeonPaths.RuinedCityOfArahJotun:
				return "jotun";
			case Encounters.DungeonPaths.RuinedCityOfArahMursaat:
				return "mursaat";
			case Encounters.DungeonPaths.RuinedCityOfArahForgotten:
				return "forgotten";
			case Encounters.DungeonPaths.RuinedCityOfArahSeer:
				return "seer";
			default:
			{
				_003CPrivateImplementationDetails_003E.ThrowInvalidOperationException();
				string result = default(string);
				return result;
			}
			}
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
