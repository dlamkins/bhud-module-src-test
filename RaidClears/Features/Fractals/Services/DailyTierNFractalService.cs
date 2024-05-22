using System.Collections.Generic;
using System.Linq;
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
				select new BoxModel(e.Encounter.id ?? "", e.Encounter.name + "\n\n" + Strings.Strike_Tooltip_tomorrow + "\n" + e.TomorrowEncounter.Label, e.Encounter.shortName);
		}

		public static IEnumerable<BoxModel> GetCMFractals()
		{
			int today = DayOfYearIndexService.DayOfYearIndex();
			List<(FractalMap, int)> CMs = new List<(FractalMap, int)>();
			int[] challengeMotes = Service.FractalMapData.ChallengeMotes;
			foreach (int scale in challengeMotes)
			{
				CMs.Add((Service.FractalMapData.GetFractalForScale(scale), scale));
			}
			return CMs.Select<(FractalMap, int), BoxModel>(((FractalMap fractal, int scale) e) => new BoxModel(e.fractal.ApiLabel, GetCMTooltip(e.fractal, e.scale, today), e.fractal.ShortLabel));
		}

		private static string GetCMTooltip(FractalMap fractal, int scale, int today)
		{
			string instab = string.Join("\n\t", Service.InstabilitiesData.GetInstabsForLevelOnDay(scale, today).ToArray());
			string tomInstab = string.Join("\n\t", Service.InstabilitiesData.GetInstabsForLevelOnDay(scale, (today + 1) % 366).ToArray());
			return fractal.Label + "\n\nInstabilities\n\t" + instab + "\n\nTomorrow's Instabilities\n\t" + tomInstab;
		}

		public static IEnumerable<BoxModel> GetTomorrowTierN()
		{
			return from e in GetTomorrowTierNFractals()
				select new BoxModel(e.ApiLabel ?? "", e.Label ?? "", e.ShortLabel);
		}

		public static IEnumerable<FractalInfo> GetDailyTierNFractals()
		{
			int num = DayOfYearIndexService.DayOfYearIndex() % DAILY_ROTATION_MAX_INDEX;
			int tomorrow = (num + 1) % DAILY_ROTATION_MAX_INDEX;
			List<FractalMap> todayScales = DailyRecsRotation(num);
			List<FractalMap> tomorrowsScales = DailyRecsRotation(tomorrow);
			List<FractalInfo> resultList = new List<FractalInfo>();
			for (int i = 0; i < todayScales.Count(); i++)
			{
				resultList.Add(new FractalInfo(todayScales[i], tomorrowsScales[i]));
			}
			return resultList;
		}

		public static IEnumerable<FractalMap> GetTomorrowTierNFractals()
		{
			List<FractalMap> tomorrowsScales = DailyRecsRotation((DayOfYearIndexService.DayOfYearIndex() + 1) % DAILY_ROTATION_MAX_INDEX);
			List<FractalMap> resultList = new List<FractalMap>();
			for (int i = 0; i < tomorrowsScales.Count(); i++)
			{
				resultList.Add(tomorrowsScales[i]);
			}
			return resultList;
		}

		public static List<FractalMap> DailyRecsRotation(int index)
		{
			List<FractalMap> _list = new List<FractalMap>();
			if (Service.FractalMapData.DailyTier.Count < index)
			{
				return _list;
			}
			foreach (string fractalName in Service.FractalMapData.DailyTier[index])
			{
				_list.Add(Service.FractalMapData.GetFractalByName(fractalName));
			}
			return _list;
		}
	}
}
