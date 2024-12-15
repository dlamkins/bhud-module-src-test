using System;

namespace RaidClears.Features.Raids.Models
{
	public class WeeklyModifier
	{
		public bool Emboldened { get; }

		public bool CallOfTheMist { get; }

		public WeeklyModifier(RaidWing raidWing)
		{
			Emboldened = GetModifierActive(raidWing.EmboldenedTimestamp, raidWing.EmboldendedWeeks, Service.RaidData.SecondsInWeek);
			CallOfTheMist = GetModifierActive(raidWing.CallOfTheMistsTimestamp, raidWing.CallOfTheMistsWeeks, Service.RaidData.SecondsInWeek);
		}

		public bool GetModifierActive(int timestamp, int weeksBetween, int weeklySeconds)
		{
			if (weeksBetween <= 0)
			{
				return false;
			}
			return (int)Math.Floor((decimal)(((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() - timestamp) / (decimal)weeklySeconds) % weeksBetween == 0;
		}
	}
}
