using System;

namespace rp.spark.Models
{
	public class SavedProfileSummary
	{
		public string CacheKey { get; set; } = string.Empty;


		public string ProfileId { get; set; } = string.Empty;


		public string ProfileName { get; set; } = string.Empty;


		public bool IsMature { get; set; }

		public string AccountName { get; set; } = string.Empty;


		public string OfficialCharacterName { get; set; } = string.Empty;


		public string DisplayCharacterName { get; set; } = string.Empty;


		public string Race { get; set; } = string.Empty;


		public string Profession { get; set; } = string.Empty;


		public string CustomProfession { get; set; } = string.Empty;


		public string ActiveProfileId { get; set; } = string.Empty;


		public string ActiveProfileName { get; set; } = string.Empty;


		public RPStatus Status { get; set; }

		public string Currently { get; set; } = string.Empty;


		public string OutOfCharacterInfo { get; set; } = string.Empty;


		public string LocationName { get; set; } = string.Empty;


		public ProfileRegion Region { get; set; }

		public DateTime LastSeen { get; set; }

		public DateTime CachedAt { get; set; }

		public bool IsBookmarked { get; set; }

		public DateTime? BookmarkedAt { get; set; }
	}
}
