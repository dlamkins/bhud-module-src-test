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

		public static Encounters.Fractal GetFractal(this MapIds.FractalMaps value)
		{
			return value switch
			{
				MapIds.FractalMaps.AetherbladeFractal => Encounters.Fractal.AetherbladeFractal, 
				MapIds.FractalMaps.AquaticRuinsFractal => Encounters.Fractal.AquaticRuinsFractal, 
				MapIds.FractalMaps.CaptainMaiTrinBossFractal => Encounters.Fractal.CaptainMaiTrinBossFractal, 
				MapIds.FractalMaps.ChaosFractal => Encounters.Fractal.ChaosFractal, 
				MapIds.FractalMaps.CliffsideFractal => Encounters.Fractal.CliffsideFractal, 
				MapIds.FractalMaps.DeepstoneFractal => Encounters.Fractal.DeepstoneFractal, 
				MapIds.FractalMaps.MoltenBoss => Encounters.Fractal.MoltenBossFractal, 
				MapIds.FractalMaps.MoltenFurnace => Encounters.Fractal.MoltenFurnaceFractal, 
				MapIds.FractalMaps.Nightmare => Encounters.Fractal.NightmareFractal, 
				MapIds.FractalMaps.ShatteredObservatory => Encounters.Fractal.ShatteredObservatoryFractal, 
				MapIds.FractalMaps.SirensReef => Encounters.Fractal.SirensReefFractal, 
				MapIds.FractalMaps.SilentSurf => Encounters.Fractal.SilentSurfFractal, 
				MapIds.FractalMaps.SnowblindFractal => Encounters.Fractal.SnowblindFractal, 
				MapIds.FractalMaps.SolidOceanFractal => Encounters.Fractal.SolidOceanFractal, 
				MapIds.FractalMaps.SunquaPeak => Encounters.Fractal.SunquaPeakFractal, 
				MapIds.FractalMaps.SwamplandFractal => Encounters.Fractal.SwamplandFractal, 
				MapIds.FractalMaps.ThaumanovaReactor => Encounters.Fractal.ThaumanovaReactorFractal, 
				MapIds.FractalMaps.TwilightOasis => Encounters.Fractal.TwilightOasisFractal, 
				MapIds.FractalMaps.UncategorizedFractal => Encounters.Fractal.UncategorizedFractal, 
				MapIds.FractalMaps.UndergroundFacilityFractal => Encounters.Fractal.UndergroundFacilityFractal, 
				MapIds.FractalMaps.UrbanBattlegroundFractal => Encounters.Fractal.UrbanBattlegroundFractal, 
				MapIds.FractalMaps.VolcanicFractal => Encounters.Fractal.VolcanicFractal, 
				_ => Encounters.Fractal.VolcanicFractal, 
			};
		}

		public static string GetApiLabel(this MapIds.FractalMaps value)
		{
			return value.GetFractal().GetApiLabel();
		}

		public static string GetLabel(this MapIds.FractalMaps value)
		{
			return value.GetFractal().GetLabel();
		}

		public static string GetLabelShort(this MapIds.FractalMaps value)
		{
			return value.GetFractal().GetLabelShort();
		}
	}
}
