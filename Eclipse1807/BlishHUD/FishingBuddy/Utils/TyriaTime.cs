using System;
using System.Collections.Generic;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Models;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	public static class TyriaTime
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(TyriaTime));

		public static readonly DateTime canthaDawnStart = new DateTime(2000, 1, 1, 7, 0, 0);

		public static readonly DateTime canthaDayStart = new DateTime(2000, 1, 1, 8, 0, 0);

		public static readonly DateTime canthaDuskStart = new DateTime(2000, 1, 1, 19, 0, 0);

		public static readonly DateTime canthaNightStart = new DateTime(2000, 1, 1, 20, 0, 0);

		public static readonly DateTime centralDawnStart = new DateTime(2000, 1, 1, 5, 0, 0);

		public static readonly DateTime centralDayStart = new DateTime(2000, 1, 1, 6, 0, 0);

		public static readonly DateTime centralDuskStart = new DateTime(2000, 1, 1, 20, 0, 0);

		public static readonly DateTime centralNightStart = new DateTime(2000, 1, 1, 21, 0, 0);

		public static readonly int canthaDayLength = 55;

		public static readonly int canthaNightLength = 55;

		public static readonly int centralDayLength = 70;

		public static readonly int centralNightLength = 40;

		public static readonly int DuskDawnLength = 5;

		public static readonly DateTime canthaDawnStartUTC = new DateTime(2000, 1, 1, 0, 35, 0);

		public static readonly DateTime canthaDayStartUTC = new DateTime(2000, 1, 1, 0, 40, 0);

		public static readonly DateTime canthaDuskStartUTC = new DateTime(2000, 1, 1, 1, 35, 0);

		public static readonly DateTime canthaNightStartUTC = new DateTime(2000, 1, 1, 1, 40, 0);

		public static readonly DateTime centralDawnStartUTC = new DateTime(2000, 1, 1, 0, 25, 0);

		public static readonly DateTime centralDayStartUTC = new DateTime(2000, 1, 1, 0, 30, 0);

		public static readonly DateTime centralDuskStartUTC = new DateTime(2000, 1, 1, 1, 40, 0);

		public static readonly DateTime centralNightStartUTC = new DateTime(2000, 1, 1, 1, 45, 0);

		public static readonly List<int> AlwaysDayMaps = new List<int> { 1195, 1465, 1206, 968 };

		public static readonly List<int> AlwaysNightMaps = new List<int> { 1361, 1413, 1414, 862, 863, 864, 865, 866, 1304, 1316 };

		public static string CurrentMapPhase(Map map)
		{
			DateTime TyriaTime = CalcTyriaTime();
			if (AlwaysDayMaps.Contains(map.get_Id()))
			{
				return "Day";
			}
			if (AlwaysNightMaps.Contains(map.get_Id()))
			{
				return "Night";
			}
			if (map.get_RegionId() == FishingMaps.CanthaRegionId)
			{
				if (TyriaTime >= canthaDawnStart && TyriaTime < canthaDayStart)
				{
					return "Dawn";
				}
				if (TyriaTime >= canthaDayStart && TyriaTime < canthaDuskStart)
				{
					return "Day";
				}
				if (TyriaTime >= canthaDuskStart && TyriaTime < canthaNightStart)
				{
					return "Dusk";
				}
				return "Night";
			}
			if (TyriaTime >= centralDawnStart && TyriaTime < centralDayStart)
			{
				return "Dawn";
			}
			if (TyriaTime >= centralDayStart && TyriaTime < centralDuskStart)
			{
				return "Day";
			}
			if (TyriaTime >= centralDuskStart && TyriaTime < centralNightStart)
			{
				return "Dusk";
			}
			return "Night";
		}

		public static DateTime CalcTyriaTime()
		{
			try
			{
				DateTime UTC = DateTime.UtcNow;
				int tyriasec = (UTC.Hour * 3600 + UTC.Minute * 60 + UTC.Second) * 12 - 60;
				tyriasec %= 86400;
				int tyrianhour = tyriasec / 3600;
				tyriasec %= 3600;
				int tyrianmin = tyriasec / 60;
				tyriasec %= 60;
				return new DateTime(2000, 1, 1, tyrianhour, tyrianmin, tyriasec);
			}
			catch
			{
				return new DateTime(2000, 1, 1, 0, 0, 0);
			}
		}

		public static TimeSpan TimeTilNextPhase(Map map)
		{
			DateTime TyriaTime = CalcTyriaTime();
			DateTime now = DateTime.UtcNow;
			DateTime nowish = new DateTime(2000, 1, 1, now.Hour % 2, now.Minute, now.Second);
			if (map == null || AlwaysDayMaps.Contains(map.get_Id()) || AlwaysNightMaps.Contains(map.get_Id()))
			{
				return TimeSpan.Zero;
			}
			DateTime currentPhaseEnd;
			if (map.get_RegionId() == FishingMaps.CanthaRegionId)
			{
				if (TyriaTime >= canthaDawnStart && TyriaTime < canthaDayStart)
				{
					currentPhaseEnd = canthaDawnStartUTC.AddMinutes(DuskDawnLength);
				}
				else if (TyriaTime >= canthaDayStart && TyriaTime < canthaDuskStart)
				{
					currentPhaseEnd = canthaDayStartUTC.AddMinutes(canthaDayLength);
				}
				else if (TyriaTime >= canthaDuskStart && TyriaTime < canthaNightStart)
				{
					currentPhaseEnd = canthaDuskStartUTC.AddMinutes(DuskDawnLength);
				}
				else
				{
					currentPhaseEnd = canthaNightStartUTC.AddMinutes(canthaNightLength);
					if (nowish.Hour == 0)
					{
						nowish = nowish.AddHours(2.0);
					}
				}
			}
			else if (TyriaTime >= centralDawnStart && TyriaTime < centralDayStart)
			{
				currentPhaseEnd = centralDawnStartUTC.AddMinutes(DuskDawnLength);
			}
			else if (TyriaTime >= centralDayStart && TyriaTime < centralDuskStart)
			{
				currentPhaseEnd = centralDayStartUTC.AddMinutes(centralDayLength);
			}
			else if (TyriaTime >= centralDuskStart && TyriaTime < centralNightStart)
			{
				currentPhaseEnd = centralDuskStartUTC.AddMinutes(DuskDawnLength);
			}
			else
			{
				currentPhaseEnd = centralNightStartUTC.AddMinutes(centralNightLength);
				if (nowish.Hour == 0)
				{
					nowish = nowish.AddHours(2.0);
				}
			}
			return currentPhaseEnd.Subtract(nowish);
		}
	}
}
