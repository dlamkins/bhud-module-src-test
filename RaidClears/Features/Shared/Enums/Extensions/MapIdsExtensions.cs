namespace RaidClears.Features.Shared.Enums.Extensions
{
	public static class MapIdsExtensions
	{
		public static Encounters.StrikeMission GetStrikeMission(this MapIds.StrikeMaps value)
		{
			return value switch
			{
				MapIds.StrikeMaps.ColdWar => Encounters.StrikeMission.ColdWar, 
				MapIds.StrikeMaps.ColdWarPublic => Encounters.StrikeMission.ColdWar, 
				MapIds.StrikeMaps.Fraenir => Encounters.StrikeMission.Fraenir, 
				MapIds.StrikeMaps.FraenirPublic => Encounters.StrikeMission.Fraenir, 
				MapIds.StrikeMaps.ShiverpeaksPass => Encounters.StrikeMission.ShiverpeaksPass, 
				MapIds.StrikeMaps.ShiverpeaksPassPublic => Encounters.StrikeMission.ShiverpeaksPass, 
				MapIds.StrikeMaps.VoiceAndClaw => Encounters.StrikeMission.VoiceAndClaw, 
				MapIds.StrikeMaps.VoiceAndClawPublic => Encounters.StrikeMission.VoiceAndClaw, 
				MapIds.StrikeMaps.Whisper => Encounters.StrikeMission.Whisper, 
				MapIds.StrikeMaps.WhisperPublic => Encounters.StrikeMission.Whisper, 
				MapIds.StrikeMaps.Boneskinner => Encounters.StrikeMission.Boneskinner, 
				MapIds.StrikeMaps.BoneskinnerPublic => Encounters.StrikeMission.Boneskinner, 
				MapIds.StrikeMaps.DragonStorm => Encounters.StrikeMission.DragonStorm, 
				MapIds.StrikeMaps.DragonStormPublic => Encounters.StrikeMission.DragonStorm, 
				MapIds.StrikeMaps.AetherbladeHideout => Encounters.StrikeMission.AetherbladeHideout, 
				MapIds.StrikeMaps.Junkyard => Encounters.StrikeMission.Junkyard, 
				MapIds.StrikeMaps.Overlook => Encounters.StrikeMission.Overlook, 
				MapIds.StrikeMaps.HarvestTemple => Encounters.StrikeMission.HarvestTemple, 
				MapIds.StrikeMaps.OldLionsCourt => Encounters.StrikeMission.OldLionsCourt, 
				MapIds.StrikeMaps.CosmicObservatory => Encounters.StrikeMission.CosmicObservatory, 
				MapIds.StrikeMaps.TempleOfFebe => Encounters.StrikeMission.TempleOfFebe, 
				_ => Encounters.StrikeMission.ShiverpeaksPass, 
			};
		}

		public static string GetApiLabel(this MapIds.StrikeMaps value)
		{
			return value.GetStrikeMission().GetApiLabel();
		}

		public static string GetLabel(this MapIds.StrikeMaps value)
		{
			return value.GetStrikeMission().GetLabel();
		}

		public static string GetLabelShort(this MapIds.StrikeMaps value)
		{
			return value.GetStrikeMission().GetLabelShort();
		}
	}
}
