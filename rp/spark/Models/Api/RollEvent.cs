using System;
using System.Collections.Generic;

namespace rp.spark.Models.Api
{
	public class RollEvent
	{
		public long Sequence { get; set; }

		public DateTime Timestamp { get; set; } = DateTime.UtcNow;


		public string AccountName { get; set; } = string.Empty;


		public string CharacterName { get; set; } = string.Empty;


		public string Type { get; set; } = "roll";


		public string Expression { get; set; } = string.Empty;


		public string Tag { get; set; } = string.Empty;


		public string Text { get; set; } = string.Empty;


		public List<int> Rolls { get; set; } = new List<int>();


		public int Modifier { get; set; }

		public int Total { get; set; }
	}
}
