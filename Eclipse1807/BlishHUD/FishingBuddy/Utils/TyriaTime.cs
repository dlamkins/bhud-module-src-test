using System;
using System.Collections.Generic;
using Blish_HUD;
using Gw2Sharp.WebApi.V2.Models;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	internal class TyriaTime
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

		public static readonly DateTime canthaDawnStartUTC = new DateTime(2000, 1, 1, 0, 35, 0);

		public static readonly DateTime canthaDayStartUTC = new DateTime(2000, 1, 1, 0, 40, 0);

		public static readonly DateTime canthaDuskStartUTC = new DateTime(2000, 1, 1, 0, 35, 0);

		public static readonly DateTime canthaNightStartUTC = new DateTime(2000, 1, 1, 0, 40, 0);

		public static readonly DateTime centralDawnStartUTC = new DateTime(2000, 1, 1, 0, 25, 0);

		public static readonly DateTime centralDayStartUTC = new DateTime(2000, 1, 1, 0, 30, 0);

		public static readonly DateTime centralDuskStartUTC = new DateTime(2000, 1, 1, 0, 40, 0);

		public static readonly DateTime centralNightStartUTC = new DateTime(2000, 1, 1, 0, 45, 0);

		public static readonly DateTime _0h = new DateTime(2000, 1, 1, 0, 0, 0);

		public static readonly DateTime _1h = new DateTime(2000, 1, 1, 1, 0, 0);

		public static readonly List<int> CanthaMaps = new List<int> { 1442, 1419, 1444, 1462, 1438, 1452, 1428, 1422 };

		public static readonly int CanthaRegionId = 37;

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
			if (map.get_RegionId() == CanthaRegionId)
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

		public static DateTime NextPhaseTime(Map map)
		{
			return DateTime.Now + TimeTilNextPhase(map);
		}

		public static TimeSpan TimeTilNextPhase(Map map)
		{
			DateTime TyriaTime = CalcTyriaTime();
			DateTime nowish = new DateTime(2000, 1, 1, 0, DateTime.Now.Minute, DateTime.Now.Second);
			TimeSpan timeTilNextPhase = TimeSpan.Zero;
			if (map == null || AlwaysDayMaps.Contains(map.get_Id()) || AlwaysNightMaps.Contains(map.get_Id()))
			{
				return TimeSpan.Zero;
			}
			if (map.get_RegionId() == CanthaRegionId)
			{
				if (TyriaTime >= canthaDawnStart && TyriaTime < canthaDayStart)
				{
					return canthaDayStartUTC.Subtract(nowish);
				}
				if (TyriaTime >= canthaDayStart && TyriaTime < canthaDuskStart)
				{
					if (nowish >= canthaDayStartUTC && nowish < _1h)
					{
						timeTilNextPhase = _1h.Subtract(nowish);
						return timeTilNextPhase + canthaDuskStartUTC.Subtract(_0h);
					}
					return canthaDuskStartUTC.Subtract(nowish);
				}
				if (TyriaTime >= canthaDuskStart && TyriaTime < canthaNightStart)
				{
					return canthaNightStartUTC.Subtract(nowish);
				}
				if (nowish >= canthaNightStartUTC && nowish < _1h)
				{
					timeTilNextPhase = _1h.Subtract(nowish);
					return timeTilNextPhase + canthaDawnStartUTC.Subtract(_0h);
				}
				return canthaDuskStartUTC.Subtract(nowish);
			}
			if (TyriaTime >= centralDawnStart && TyriaTime < centralDayStart)
			{
				return centralDayStartUTC.Subtract(nowish);
			}
			if (TyriaTime >= centralDayStart && TyriaTime < centralDuskStart)
			{
				if (nowish >= centralDayStartUTC && nowish < _1h)
				{
					timeTilNextPhase = _1h.Subtract(nowish);
					return timeTilNextPhase + centralDuskStartUTC.Subtract(_0h);
				}
				return centralDuskStartUTC.Subtract(nowish);
			}
			if (TyriaTime >= centralDuskStart && TyriaTime < centralNightStart)
			{
				return centralNightStartUTC.Subtract(nowish);
			}
			if (nowish >= centralNightStartUTC && nowish < _1h)
			{
				timeTilNextPhase = _1h.Subtract(nowish);
				return timeTilNextPhase + centralDawnStartUTC.Subtract(_0h);
			}
			return centralDawnStartUTC.Subtract(nowish);
		}
	}
}
