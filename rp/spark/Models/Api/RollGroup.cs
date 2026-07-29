using System;
using System.Collections.Generic;

namespace rp.spark.Models.Api
{
	public class RollGroup
	{
		public string GroupId { get; set; } = string.Empty;


		public string Code { get; set; } = string.Empty;


		public string OwnerAccountName { get; set; } = string.Empty;


		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


		public DateTime ExpiresAt { get; set; } = DateTime.UtcNow;


		public bool JoinLocked { get; set; }

		public bool HasPassword { get; set; }

		public long Revision { get; set; }

		public long LastSequence { get; set; }

		public List<RollMember> Members { get; set; } = new List<RollMember>();


		public List<RollEvent> History { get; set; } = new List<RollEvent>();


		public bool IsOwner(string accountName)
		{
			if (!string.IsNullOrWhiteSpace(accountName))
			{
				return string.Equals(OwnerAccountName?.Trim(), accountName.Trim(), StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}
	}
}
