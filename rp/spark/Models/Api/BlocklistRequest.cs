using System.Collections.Generic;

namespace rp.spark.Models.Api
{
	public class BlocklistRequest
	{
		public List<string> AccountNames { get; set; } = new List<string>();

	}
}
