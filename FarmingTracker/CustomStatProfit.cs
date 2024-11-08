namespace FarmingTracker
{
	public class CustomStatProfit
	{
		public int ApiId { get; }

		public StatType StatType { get; }

		public long Unsigned_CustomProfitInCopper { get; set; }

		public CustomStatProfit(int apiId, StatType statType)
		{
			ApiId = apiId;
			StatType = statType;
		}

		public bool BelongsToStat(Stat stat)
		{
			if (ApiId == stat.ApiId)
			{
				return StatType == stat.StatType;
			}
			return false;
		}
	}
}
