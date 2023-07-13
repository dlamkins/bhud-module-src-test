using System.Collections.Generic;
using System.Linq;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Shared.Services;
using RaidClears.Localization;

namespace RaidClears.Features.Fractals.Services
{
	public static class DailyRecommendedFractalService
	{
		private static int DAILY_ROTATION_MAX_INDEX = 15;

		public static IEnumerable<BoxModel> GetRecommendedFractals()
		{
			return from e in GetDailyRecommendedFractals()
				select new BoxModel(e.Encounter.id ?? "", e.Encounter.name + "\n\n" + Strings.Strike_Tooltip_tomorrow + "\n" + e.TomorrowEncounter.GetLabel(), e.Encounter.shortName);
		}

		public static IEnumerable<FractalInfo> GetDailyRecommendedFractals()
		{
			int num = DayOfYearIndexService.DayOfYearIndex() % DAILY_ROTATION_MAX_INDEX;
			int tomorrow = (num + 1) % DAILY_ROTATION_MAX_INDEX;
			List<int> todayScales = DailyRecsRotation(num);
			List<int> tomorrowsScales = DailyRecsRotation(tomorrow);
			List<FractalInfo> resultList = new List<FractalInfo>();
			for (int i = 0; i < todayScales.Count(); i++)
			{
				Encounters.Fractal todayFrac = FractalExtensions.GetFractalForScale(todayScales[i]);
				Encounters.Fractal tomorrowFrac = FractalExtensions.GetFractalForScale(tomorrowsScales[i]);
				resultList.Add(new FractalInfo(todayFrac, tomorrowFrac));
			}
			return resultList;
		}

		public static List<int> DailyRecsRotation(int index)
		{
			return index switch
			{
				0 => new List<int> { 2, 37, 53 }, 
				1 => new List<int> { 6, 28, 61 }, 
				2 => new List<int> { 10, 32, 65 }, 
				3 => new List<int> { 14, 34, 74 }, 
				4 => new List<int> { 19, 37, 66 }, 
				5 => new List<int> { 15, 41, 60 }, 
				6 => new List<int> { 24, 35, 75 }, 
				7 => new List<int> { 25, 36, 69 }, 
				8 => new List<int> { 12, 40, 67 }, 
				9 => new List<int> { 8, 31, 54 }, 
				10 => new List<int> { 11, 39, 59 }, 
				11 => new List<int> { 18, 27, 64 }, 
				12 => new List<int> { 4, 30, 58 }, 
				13 => new List<int> { 16, 42, 62 }, 
				14 => new List<int> { 5, 47, 68 }, 
				_ => new List<int> { 97, 98, 99, 100 }, 
			};
		}
	}
}
