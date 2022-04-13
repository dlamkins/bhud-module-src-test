using System;
using System.Collections.Generic;
using Blish_HUD;

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

		public static readonly List<int> CanthaMaps = new List<int> { 1442, 1419, 1444, 1462, 1438, 1452, 1428, 1422 };

		public static readonly List<int> AlwaysDayMaps = new List<int> { 1195, 1465, 1206, 968 };

		public static readonly List<int> AlwaysNightMaps = new List<int> { 1361, 1413, 1414, 862, 863, 864, 865, 866, 1304, 1316 };

		public static string CurrentMapPhase(int MapId)
		{
			DateTime TyriaTime = CalcTyriaTime();
			if (AlwaysDayMaps.Contains(MapId))
			{
				return "Day";
			}
			if (AlwaysDayMaps.Contains(MapId))
			{
				return "Night";
			}
			if (CanthaMaps.Contains(MapId))
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

		public static TimeSpan CalcTimeTilNextPhase(int MapId)
		{
			DateTime TyriaTime = CalcTyriaTime();
			if (AlwaysDayMaps.Contains(MapId))
			{
				return TimeSpan.Zero;
			}
			if (AlwaysDayMaps.Contains(MapId))
			{
				return TimeSpan.Zero;
			}
			if (CanthaMaps.Contains(MapId))
			{
				if (TyriaTime >= canthaDawnStart && TyriaTime < canthaDayStart)
				{
					return canthaDayStart - TyriaTime;
				}
				if (TyriaTime >= canthaDayStart && TyriaTime < canthaDuskStart)
				{
					return canthaDuskStart - TyriaTime;
				}
				if (TyriaTime >= canthaDuskStart && TyriaTime < canthaNightStart)
				{
					return canthaNightStart - TyriaTime;
				}
				return canthaDawnStart - TyriaTime;
			}
			if (TyriaTime >= centralDawnStart && TyriaTime < centralDayStart)
			{
				return centralDayStart - TyriaTime;
			}
			if (TyriaTime >= centralDayStart && TyriaTime < centralDuskStart)
			{
				return centralDuskStart - TyriaTime;
			}
			if (TyriaTime >= centralDuskStart && TyriaTime < centralNightStart)
			{
				return centralNightStart - TyriaTime;
			}
			return centralDawnStart - TyriaTime;
		}
	}
}
