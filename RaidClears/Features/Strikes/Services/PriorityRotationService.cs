using System;
using System.Collections.Generic;
using System.Linq;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Features.Shared.Models;
using RaidClears.Localization;

namespace RaidClears.Features.Strikes.Services
{
	public static class PriorityRotationService
	{
		private static int BOTH_AT_INDEX_0_TIMESTAMP = 1675123200;

		private static int DAILY_SECONDS = 86400;

		private static int NUMBER_OF_IBS_STRIKES = 6;

		private static int NUMBER_OF_EOD_STRIKES = 5;

		public static IEnumerable<BoxModel> GetPriorityEncounters()
		{
			return from e in GetPriorityStrikes()
				select new BoxModel("priority_" + e.Encounter.id, e.Encounter.name + "\n\n" + Strings.Strike_Tooltip_tomorrow + "\n" + e.TomorrowEncounter.GetLabel(), e.Encounter.shortName);
		}

		public static IEnumerable<StrikeInfo> GetPriorityStrikes()
		{
			int num = (int)Math.Floor((decimal)((((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() - BOTH_AT_INDEX_0_TIMESTAMP) / DAILY_SECONDS));
			int ibs_index = num % NUMBER_OF_IBS_STRIKES;
			int eod_index = num % NUMBER_OF_EOD_STRIKES;
			return new List<StrikeInfo>
			{
				IcebroodStrikeInfo(ibs_index),
				EndOfDragonsStrikeInfo(eod_index)
			};
		}

		public static StrikeInfo IcebroodStrikeInfo(int index)
		{
			return index switch
			{
				0 => new StrikeInfo(Encounters.StrikeMission.ColdWar, new List<int> { 1374, 1376 }, Encounters.StrikeMission.Fraenir), 
				1 => new StrikeInfo(Encounters.StrikeMission.Fraenir, new List<int> { 1341, 1344 }, Encounters.StrikeMission.ShiverpeaksPass), 
				2 => new StrikeInfo(Encounters.StrikeMission.ShiverpeaksPass, new List<int> { 1331, 1332 }, Encounters.StrikeMission.VoiceAndClaw), 
				3 => new StrikeInfo(Encounters.StrikeMission.VoiceAndClaw, new List<int> { 1340, 1346 }, Encounters.StrikeMission.Whisper), 
				4 => new StrikeInfo(Encounters.StrikeMission.Whisper, new List<int> { 1357, 1359 }, Encounters.StrikeMission.Boneskinner), 
				5 => new StrikeInfo(Encounters.StrikeMission.Boneskinner, new List<int> { 1339, 1351 }, Encounters.StrikeMission.ColdWar), 
				_ => new StrikeInfo(Encounters.StrikeMission.ShiverpeaksPass, new List<int>(), Encounters.StrikeMission.ShiverpeaksPass), 
			};
		}

		public static StrikeInfo EndOfDragonsStrikeInfo(int index)
		{
			return index switch
			{
				0 => new StrikeInfo(Encounters.StrikeMission.AetherbladeHideout, new List<int> { 1432 }, Encounters.StrikeMission.Junkyard), 
				1 => new StrikeInfo(Encounters.StrikeMission.Junkyard, new List<int> { 1450 }, Encounters.StrikeMission.Overlook), 
				2 => new StrikeInfo(Encounters.StrikeMission.Overlook, new List<int> { 1451 }, Encounters.StrikeMission.HarvestTemple), 
				3 => new StrikeInfo(Encounters.StrikeMission.HarvestTemple, new List<int> { 1437 }, Encounters.StrikeMission.OldLionsCourt), 
				4 => new StrikeInfo(Encounters.StrikeMission.OldLionsCourt, new List<int> { 1485 }, Encounters.StrikeMission.AetherbladeHideout), 
				_ => new StrikeInfo(Encounters.StrikeMission.AetherbladeHideout, new List<int>(), Encounters.StrikeMission.AetherbladeHideout), 
			};
		}
	}
}
