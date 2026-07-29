using System;

namespace rp.spark.Models.Api
{
	public class RollMember
	{
		public string AccountName { get; set; } = string.Empty;


		public string CharacterName { get; set; } = string.Empty;


		public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

	}
}
