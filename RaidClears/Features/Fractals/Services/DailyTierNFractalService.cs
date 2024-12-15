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

		public static IEnumerable<(BoxModel box, FractalMap fractalMap, int scale)> GetCMFractals()
		{
			return BuildToolTipData(Service.FractalMapData.ChallengeMotes.Select(Service.FractalMapData.GetFractalForScale));
		}

		public static IEnumerable<(BoxModel box, FractalMap fractalMap, int scale)> GetTomorrowTierNForTooltip()
		{
			return BuildToolTipData(GetTomorrowTierNFractals());
		}

		public static IEnumerable<(BoxModel box, FractalMap fractalMap, int scale)> BuildToolTipData(IEnumerable<FractalMap> fractals)
		{
			DayOfYearIndexService.DayOfYearIndex();
			List<(BoxModel, FractalMap, int)> CMs = new List<(BoxModel, FractalMap, int)>();
			foreach (FractalMap map in fractals)
			{
				List<int> scales = map.Scales;
				CMs.Add((new BoxModel(map.ApiLabel, "", Service.FractalPersistance.GetEncounterLabel(map.ApiLabel)), map, scales.Last()));
			}
			return CMs;
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
