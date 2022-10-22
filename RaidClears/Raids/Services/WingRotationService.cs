using System;

namespace RaidClears.Raids.Services
{
	public class WingRotationService : IDisposable
	{
		private const int EMBOLDEN_START_TIMESTAMP = 1656315000;

		private const int WEEKLY_SECONDS = 604800;

		private const int NUMBER_OF_WINGS = 7;

		public (int, int) getHighlightedWingIndices()
		{
			int num = (int)Math.Floor((decimal)((((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() - 1656315000) / 604800)) % 7;
			return (num, (num + 1) % 7);
		}

		public void Dispose()
		{
		}
	}
}
