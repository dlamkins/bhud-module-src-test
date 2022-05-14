using System;
using System.Collections.Generic;
using Blish_HUD;
using Eclipse1807.BlishHUD.FishingBuddy.Properties;
using Gw2Sharp.WebApi.V2.Models;

namespace Eclipse1807.BlishHUD.FishingBuddy.Utils
{
	public static class TyriaTime
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(TyriaTime));

		public static readonly DateTime CanthaDawnStart = new DateTime(2000, 1, 1, 7, 0, 0);

		public static readonly DateTime CanthaDayStart = new DateTime(2000, 1, 1, 8, 0, 0);

		public static readonly DateTime CanthaDuskStart = new DateTime(2000, 1, 1, 19, 0, 0);

		public static readonly DateTime CanthaNightStart = new DateTime(2000, 1, 1, 20, 0, 0);

		public static readonly DateTime CentralDawnStart = new DateTime(2000, 1, 1, 5, 0, 0);

		public static readonly DateTime CentralDayStart = new DateTime(2000, 1, 1, 6, 0, 0);

		public static readonly DateTime CentralDuskStart = new DateTime(2000, 1, 1, 20, 0, 0);

		public static readonly DateTime CentralNightStart = new DateTime(2000, 1, 1, 21, 0, 0);

		public static readonly int CanthaDayLength = 55;

		public static readonly int CanthaNightLength = 55;

		public static readonly int CentralDayLength = 70;

		public static readonly int CentralNightLength = 40;

		public static readonly int DuskDawnLength = 5;

		public static readonly DateTime CanthaDawnStartUTC = new DateTime(2000, 1, 1, 0, 35, 0);

		public static readonly DateTime CanthaDayStartUTC = new DateTime(2000, 1, 1, 0, 40, 0);

		public static readonly DateTime CanthaDuskStartUTC = new DateTime(2000, 1, 1, 1, 35, 0);

		public static readonly DateTime CanthaNightStartUTC = new DateTime(2000, 1, 1, 1, 40, 0);

		public static readonly DateTime CentralDawnStartUTC = new DateTime(2000, 1, 1, 0, 25, 0);

		public static readonly DateTime CentralDayStartUTC = new DateTime(2000, 1, 1, 0, 30, 0);

		public static readonly DateTime CentralDuskStartUTC = new DateTime(2000, 1, 1, 1, 40, 0);

		public static readonly DateTime CentralNightStartUTC = new DateTime(2000, 1, 1, 1, 45, 0);

		public static readonly List<int> AlwaysDayMaps = new List<int> { 1195, 1465, 1206, 968 };

		public static readonly List<int> AlwaysNightMaps = new List<int> { 1361, 1413, 1414, 862, 863, 864, 865, 866, 1304, 1316 };

		public static DateTime CalcTyriaTime()
		{
			try
			{
				DateTime UTC = DateTime.UtcNow;
				int TyrianSec = (UTC.Hour * 3600 + UTC.Minute * 60 + UTC.Second) * 12 - 60;
				TyrianSec %= 86400;
				int TyrianHour = TyrianSec / 3600;
				TyrianSec %= 3600;
				int TyrianMin = TyrianSec / 60;
				TyrianSec %= 60;
				return new DateTime(2000, 1, 1, TyrianHour, TyrianMin, TyrianSec);
			}
			catch
			{
				return new DateTime(2000, 1, 1, 0, 0, 0);
			}
		}

		public static string CurrentMapPhase(Map map)
		{
			DateTime TyriaTime = CalcTyriaTime();
			if (AlwaysDayMaps.Contains(map.get_Id()))
			{
				return Strings.Day;
			}
			if (AlwaysNightMaps.Contains(map.get_Id()))
			{
				return Strings.Night;
			}
			if (map.get_RegionId() == FishingMaps.CanthaRegionId)
			{
				if (TyriaTime >= CanthaDawnStart && TyriaTime < CanthaDayStart)
				{
					return Strings.Dawn;
				}
				if (TyriaTime >= CanthaDayStart && TyriaTime < CanthaDuskStart)
				{
					return Strings.Day;
				}
				if (TyriaTime >= CanthaDuskStart && TyriaTime < CanthaNightStart)
				{
					return Strings.Dusk;
				}
				return Strings.Night;
			}
			if (TyriaTime >= CentralDawnStart && TyriaTime < CentralDayStart)
			{
				return Strings.Dawn;
			}
			if (TyriaTime >= CentralDayStart && TyriaTime < CentralDuskStart)
			{
				return Strings.Day;
			}
			if (TyriaTime >= CentralDuskStart && TyriaTime < CentralNightStart)
			{
				return Strings.Dusk;
			}
			return Strings.Night;
		}

		public static TimeSpan TimeTilNextPhase(Map map)
		{
			DateTime tyriaTime = CalcTyriaTime();
			DateTime now = DateTime.UtcNow;
			DateTime nowish = new DateTime(2000, 1, 1, now.Hour % 2, now.Minute, now.Second);
			if (map == null || AlwaysDayMaps.Contains(map.get_Id()) || AlwaysNightMaps.Contains(map.get_Id()))
			{
				return TimeSpan.Zero;
			}
			DateTime currentPhaseEnd;
			if (map.get_RegionId() == FishingMaps.CanthaRegionId)
			{
				if (tyriaTime >= CanthaDawnStart && tyriaTime < CanthaDayStart)
				{
					currentPhaseEnd = CanthaDawnStartUTC.AddMinutes(DuskDawnLength);
				}
				else if (tyriaTime >= CanthaDayStart && tyriaTime < CanthaDuskStart)
				{
					currentPhaseEnd = CanthaDayStartUTC.AddMinutes(CanthaDayLength);
				}
				else if (tyriaTime >= CanthaDuskStart && tyriaTime < CanthaNightStart)
				{
					currentPhaseEnd = CanthaDuskStartUTC.AddMinutes(DuskDawnLength);
				}
				else
				{
					currentPhaseEnd = CanthaNightStartUTC.AddMinutes(CanthaNightLength);
					if (nowish.Hour == 0)
					{
						nowish = nowish.AddHours(2.0);
					}
				}
			}
			else if (tyriaTime >= CentralDawnStart && tyriaTime < CentralDayStart)
			{
				currentPhaseEnd = CentralDawnStartUTC.AddMinutes(DuskDawnLength);
			}
			else if (tyriaTime >= CentralDayStart && tyriaTime < CentralDuskStart)
			{
				currentPhaseEnd = CentralDayStartUTC.AddMinutes(CentralDayLength);
			}
			else if (tyriaTime >= CentralDuskStart && tyriaTime < CentralNightStart)
			{
				currentPhaseEnd = CentralDuskStartUTC.AddMinutes(DuskDawnLength);
			}
			else
			{
				currentPhaseEnd = CentralNightStartUTC.AddMinutes(CentralNightLength);
				if (nowish.Hour == 0)
				{
					nowish = nowish.AddHours(2.0);
				}
			}
			return currentPhaseEnd.Subtract(nowish);
		}
	}
}
