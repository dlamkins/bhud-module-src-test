using System;

namespace Oberyn.AnglerAssociate.Services
{
	public static class DailyFisherRotation
	{
		private static readonly DateTime AnchorDate = new DateTime(2026, 7, 27);

		private static readonly string[] RotationOrder = new string[8] { "Daily Shiverpeaks Fisher", "Daily Desert Fisher", "Daily End of Dragons Fisher", "Daily Heart of Maguuma Fisher", "Daily Ascalon Fisher", "Daily Orr Fisher", "Daily Kryta Fisher", "Daily Maguuma Jungle Fisher" };

		public static string GetToday(DateTime? utcNow = null)
		{
			int index = (((utcNow ?? DateTime.UtcNow).Date - AnchorDate).Days % RotationOrder.Length + RotationOrder.Length) % RotationOrder.Length;
			return RotationOrder[index];
		}
	}
}
