namespace RaidClears.Features.Shared.Enums.Extensions
{
	public static class StrikeMissionTypeExtensions
	{
		public static string GetLabel(this StrikeMissionType type)
		{
			return type switch
			{
				StrikeMissionType.Ibs => "IBS", 
				StrikeMissionType.Eod => "EoD", 
				StrikeMissionType.Priority => "Priority", 
				_ => "unknown", 
			};
		}
	}
}
