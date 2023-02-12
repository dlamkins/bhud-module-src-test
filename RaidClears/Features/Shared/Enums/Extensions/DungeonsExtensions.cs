using System;

namespace RaidClears.Features.Shared.Enums.Extensions
{
	public static class DungeonsExtensions
	{
		public static string GetLabel(this Encounters.Dungeons value)
		{
			return value switch
			{
				Encounters.Dungeons.AscalonianCatacombs => "Ascalonian Catacombs", 
				Encounters.Dungeons.CaudecusManor => "Caudecus Manor", 
				Encounters.Dungeons.TwilightArbor => "Twilight Arbor", 
				Encounters.Dungeons.SorrowsEmbrace => "Sorrows Embrace", 
				Encounters.Dungeons.CitadelOfFlame => "Citadel of Flame", 
				Encounters.Dungeons.HonorOfTheWaves => "Honor of the Waves", 
				Encounters.Dungeons.CrucibleOfEternity => "Crucible of Eternity", 
				Encounters.Dungeons.RuinedCityOfArah => "Ruined City of Arah", 
				_ => throw new ArgumentOutOfRangeException("value", value, null), 
			};
		}

		public static string GetLabelShort(this Encounters.Dungeons value)
		{
			return value switch
			{
				Encounters.Dungeons.AscalonianCatacombs => "AC", 
				Encounters.Dungeons.CaudecusManor => "CM", 
				Encounters.Dungeons.TwilightArbor => "TA", 
				Encounters.Dungeons.SorrowsEmbrace => "SE", 
				Encounters.Dungeons.CitadelOfFlame => "CoF", 
				Encounters.Dungeons.HonorOfTheWaves => "HW", 
				Encounters.Dungeons.CrucibleOfEternity => "CoE", 
				Encounters.Dungeons.RuinedCityOfArah => "Arah", 
				_ => throw new ArgumentOutOfRangeException("value", value, null), 
			};
		}

		public static string GetApiLabel(this Encounters.Dungeons value)
		{
			return value switch
			{
				Encounters.Dungeons.AscalonianCatacombs => string.Empty, 
				Encounters.Dungeons.CaudecusManor => string.Empty, 
				Encounters.Dungeons.TwilightArbor => string.Empty, 
				Encounters.Dungeons.SorrowsEmbrace => string.Empty, 
				Encounters.Dungeons.CitadelOfFlame => string.Empty, 
				Encounters.Dungeons.HonorOfTheWaves => string.Empty, 
				Encounters.Dungeons.CrucibleOfEternity => string.Empty, 
				Encounters.Dungeons.RuinedCityOfArah => string.Empty, 
				_ => throw new ArgumentOutOfRangeException("value", value, null), 
			};
		}
	}
}
