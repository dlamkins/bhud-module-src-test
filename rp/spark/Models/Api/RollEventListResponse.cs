using System.Collections.Generic;

namespace rp.spark.Models.Api
{
	public class RollEventListResponse
	{
		public long Revision { get; set; }

		public long LastSequence { get; set; }

		public bool GroupChanged { get; set; }

		public List<RollEvent> Events { get; set; } = new List<RollEvent>();

	}
}
