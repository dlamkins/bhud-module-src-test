using System.Collections.Generic;
using System.Linq;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Shared.Services;
using RaidClears.Localization;

namespace RaidClears.Features.Fractals.Services
{
	public static class DailyTierNFractalService
	{
		private static int DAILY_ROTATION_MAX_INDEX = 15;

		public static IEnumerable<BoxModel> GetDailyTierN()
		{
			return from e in GetDailyTierNFractals()
				select new BoxModel(e.Encounter.id ?? "", e.Encounter.name + "\n\n" + Strings.Strike_Tooltip_tomorrow + "\n" + e.TomorrowEncounter.GetLabel(), e.Encounter.shortName);
		}

		public static IEnumerable<BoxModel> GetCMFractals()
		{
			int today = DayOfYearIndexService.DayOfYearIndex();
			return new List<(Encounters.Fractal, int)>
			{
				(Encounters.Fractal.NightmareFractal, 97),
				(Encounters.Fractal.ShatteredObservatoryFractal, 98),
				(Encounters.Fractal.SunquaPeakFractal, 99),
				(Encounters.Fractal.SilentSurfFractal, 100)
			}.Select<(Encounters.Fractal, int), BoxModel>(((Encounters.Fractal fractal, int scale) e) => new BoxModel(e.fractal.GetApiLabel(), GetCMTooltip(e.fractal, e.scale, today), e.fractal.GetLabelShort()));
		}

		private static string GetCMTooltip(Encounters.Fractal fractal, int scale, int today)
		{
			string instab = string.Join("\n\t", Service.InstabilitiesData.GetInstabsForLevelOnDay(scale, today).ToArray());
			string tomInstab = string.Join("\n\t", Service.InstabilitiesData.GetInstabsForLevelOnDay(scale, (today + 1) % 366).ToArray());
			return fractal.GetLabel() + "\n\nInstabilities\n\t" + instab + "\n\nTomorrow's Instabilities\n\t" + tomInstab;
		}

		public static IEnumerable<BoxModel> GetTomorrowTierN()
		{
			return from e in GetTomorrowTierNFractals()
				select new BoxModel(e.GetApiLabel() ?? "", e.GetLabel() ?? "", e.GetLabelShort());
		}

		public static IEnumerable<FractalInfo> GetDailyTierNFractals()
		{
			int num = DayOfYearIndexService.DayOfYearIndex() % DAILY_ROTATION_MAX_INDEX;
			int tomorrow = (num + 1) % DAILY_ROTATION_MAX_INDEX;
			List<Encounters.Fractal> todayScales = DailyRecsRotation(num);
			List<Encounters.Fractal> tomorrowsScales = DailyRecsRotation(tomorrow);
			List<FractalInfo> resultList = new List<FractalInfo>();
			for (int i = 0; i < todayScales.Count(); i++)
			{
				resultList.Add(new FractalInfo(todayScales[i], tomorrowsScales[i]));
			}
			return resultList;
		}

		public static IEnumerable<Encounters.Fractal> GetTomorrowTierNFractals()
		{
			List<Encounters.Fractal> tomorrowsScales = DailyRecsRotation((DayOfYearIndexService.DayOfYearIndex() + 1) % DAILY_ROTATION_MAX_INDEX);
			List<Encounters.Fractal> resultList = new List<Encounters.Fractal>();
			for (int i = 0; i < tomorrowsScales.Count(); i++)
			{
				resultList.Add(tomorrowsScales[i]);
			}
			return resultList;
		}

		public static List<Encounters.Fractal> DailyRecsRotation(int index)
		{
			return index switch
			{
				0 => new List<Encounters.Fractal>
				{
					Encounters.Fractal.NightmareFractal,
					Encounters.Fractal.SnowblindFractal,
					Encounters.Fractal.VolcanicFractal
				}, 
				1 => new List<Encounters.Fractal>
				{
					Encounters.Fractal.AetherbladeFractal,
					Encounters.Fractal.UncategorizedFractal,
					Encounters.Fractal.ThaumanovaReactorFractal
				}, 
				2 => new List<Encounters.Fractal>
				{
					Encounters.Fractal.TwilightOasisFractal,
					Encounters.Fractal.CliffsideFractal,
					Encounters.Fractal.ChaosFractal
				}, 
				3 => new List<Encounters.Fractal>
				{
					Encounters.Fractal.DeepstoneFractal,
					Encounters.Fractal.CaptainMaiTrinBossFractal,
					Encounters.Fractal.SilentSurfFractal
				}, 
				4 => new List<Encounters.Fractal>
				{
					Encounters.Fractal.SnowblindFractal,
					Encounters.Fractal.SolidOceanFractal,
					Encounters.Fractal.NightmareFractal
				}, 
				5 => new List<Encounters.Fractal>
				{
					Encounters.Fractal.ChaosFractal,
					Encounters.Fractal.UncategorizedFractal,
					Encounters.Fractal.UrbanBattlegroundFractal
				}, 
				6 => new List<Encounters.Fractal>
				{
					Encounters.Fractal.SirensReefFractal,
					Encounters.Fractal.MoltenFurnaceFractal,
					Encounters.Fractal.DeepstoneFractal
				}, 
				7 => new List<Encounters.Fractal>
				{
					Encounters.Fractal.MoltenBossFractal,
					Encounters.Fractal.TwilightOasisFractal,
					Encounters.Fractal.UndergroundFacilityFractal
				}, 
				8 => new List<Encounters.Fractal>
				{
					Encounters.Fractal.VolcanicFractal,
					Encounters.Fractal.SwamplandFractal,
					Encounters.Fractal.SilentSurfFractal
				}, 
				9 => new List<Encounters.Fractal>
				{
					Encounters.Fractal.SnowblindFractal,
					Encounters.Fractal.ThaumanovaReactorFractal,
					Encounters.Fractal.AquaticRuinsFractal
				}, 
				10 => new List<Encounters.Fractal>
				{
					Encounters.Fractal.UndergroundFacilityFractal,
					Encounters.Fractal.UrbanBattlegroundFractal,
					Encounters.Fractal.SunquaPeakFractal
				}, 
				11 => new List<Encounters.Fractal>
				{
					Encounters.Fractal.AetherbladeFractal,
					Encounters.Fractal.ChaosFractal,
					Encounters.Fractal.NightmareFractal
				}, 
				12 => new List<Encounters.Fractal>
				{
					Encounters.Fractal.CliffsideFractal,
					Encounters.Fractal.MoltenBossFractal,
					Encounters.Fractal.SirensReefFractal
				}, 
				13 => new List<Encounters.Fractal>
				{
					Encounters.Fractal.SwamplandFractal,
					Encounters.Fractal.SolidOceanFractal,
					Encounters.Fractal.DeepstoneFractal
				}, 
				14 => new List<Encounters.Fractal>
				{
					Encounters.Fractal.CaptainMaiTrinBossFractal,
					Encounters.Fractal.ShatteredObservatoryFractal,
					Encounters.Fractal.MoltenBossFractal
				}, 
				_ => new List<Encounters.Fractal> { Encounters.Fractal.NightmareFractal }, 
			};
		}
	}
}
