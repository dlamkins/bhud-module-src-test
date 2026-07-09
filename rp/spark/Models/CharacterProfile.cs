using System;
using System.Collections.Generic;

namespace rp.spark.Models
{
	public class CharacterProfile
	{
		public int SchemaVersion { get; set; } = 1;


		public string ProfileId { get; set; } = Guid.NewGuid().ToString();


		public string ProfileName { get; set; } = "Default";


		public bool IsMature { get; set; }

		public string AccountName { get; set; } = string.Empty;


		public string CharacterName { get; set; } = string.Empty;


		public string DisplayName { get; set; } = string.Empty;


		public string Pronouns { get; set; } = string.Empty;


		public string Race { get; set; } = string.Empty;


		public string Profession { get; set; } = string.Empty;


		public string Specialization { get; set; } = string.Empty;


		public string CustomProfession { get; set; } = string.Empty;


		public bool IsCharacterVerified { get; set; }

		public ProfileRegion Region { get; set; }

		public string Currently { get; set; } = string.Empty;


		public string OutOfCharacterInfo { get; set; } = string.Empty;


		public List<AtAGlanceEntry> AtAGlance { get; set; } = new List<AtAGlanceEntry>();


		public ProfileExperience Experience { get; set; }

		public ProfilePreferenceFlags Preferences { get; set; }

		public ProfileThemeFlags Themes { get; set; }

		public ProfileStyleFlags Styles { get; set; }

		public string KnownFor { get; set; } = string.Empty;


		public string Description { get; set; } = string.Empty;


		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

	}
}
