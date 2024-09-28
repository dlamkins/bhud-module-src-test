using System;

namespace FarmingTracker
{
	public class Stat
	{
		public int ApiId { get; set; }

		public StatType StatType { get; set; }

		public long Count { get; set; }

		public long CountSign => Math.Sign(Count);

		public bool IsSingleItem => Math.Abs(Count) == 1;

		public ApiStatDetails Details { get; set; } = new ApiStatDetails();


		public Profits Profits { get; set; } = new Profits();


		public bool IsCoin => ApiId == 1;

		public bool IsCoinOrCustomCoin
		{
			get
			{
				if (!IsCoin)
				{
					return Details.IsCustomCoinStat;
				}
				return true;
			}
		}
	}
}
