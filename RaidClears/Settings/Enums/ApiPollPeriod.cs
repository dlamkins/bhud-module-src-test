using System.ComponentModel;

namespace RaidClears.Settings.Enums
{
	public enum ApiPollPeriod
	{
		[Description("3 minutes")]
		MINUTES_3 = 3,
		[Description("5 minutes")]
		MINUTES_5 = 5,
		[Description("10 minutes")]
		MINUTES_10 = 10,
		[Description("15 minutes")]
		MINUTES_15 = 0xF,
		[Description("30 minutes")]
		MINUTES_30 = 30
	}
}
