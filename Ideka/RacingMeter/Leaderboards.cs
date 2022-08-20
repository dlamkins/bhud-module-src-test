using System.Collections.Generic;
using Ideka.RacingMeterLib;

namespace Ideka.RacingMeter
{
	public class Leaderboards
	{
		public IDictionary<string, Leaderboard> Boards { get; set; } = new Dictionary<string, Leaderboard>();

	}
}
