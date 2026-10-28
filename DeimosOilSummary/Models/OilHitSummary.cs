using System;
using System.Collections.Generic;

namespace DeimosOilSummary.Models
{
	public sealed class OilHitSummary
	{
		public string LogPath { get; set; }

		public DateTime LogTime { get; set; }

		public bool Success { get; set; }

		public string ErrorMessage { get; set; }

		public List<OilFirstTouch> FirstTouches { get; set; } = new List<OilFirstTouch>();


		public List<PlayerOilCount> Players { get; set; } = new List<PlayerOilCount>();


		public int TotalHits { get; set; }
	}
}
