using System.Collections.Generic;
using System.Linq;
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
				select new BoxModel(e.Encounter.id ?? "", e.Encounter.name + "\n\n" + Strings.Strike_Tooltip_tomorrow + "\n" + e.TomorrowEncounter.Label, e.Encounter.shortName);
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
				FractalMap todayFrac = Service.FractalMapData.GetFractalForScale(todayScales[i]);
				FractalMap tomorrowFrac = Service.FractalMapData.GetFractalForScale(tomorrowsScales[i]);
				resultList.Add(new FractalInfo(todayFrac, tomorrowFrac));
			}
			return resultList;
		}

		public static List<int> DailyRecsRotation(int index)
		{
			if (Service.FractalMapData.Recs.Count >= index)
			{
				return Service.FractalMapData.Recs[index];
			}
			return new List<int> { 96, 97, 98, 99, 100 };
		}
	}
}
