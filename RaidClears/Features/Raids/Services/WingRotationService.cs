using System;

namespace RaidClears.Features.Raids.Services
{
	public static class WingRotationService
	{
		private const int EMBOLDEN_START_TIMESTAMP = 1656315000;

		private const int WEEKLY_SECONDS = 604800;

		private const int NUMBER_OF_WINGS = 7;

		public static WeeklyWings GetWeeklyWings()
		{
			int num = (int)Math.Floor((decimal)(((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() - 1656315000) / 604800m) % 7;
			return new WeeklyWings(num, (num + 1) % 7, 7);
		}
	}
}
