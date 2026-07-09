using System;
using System.Collections.Generic;

namespace rp.spark.Models.Api
{
	public class ProfileData
	{
		public int SchemaVersion { get; set; } = 1;


		public string ProfileId { get; set; } = string.Empty;


		public bool IsMature { get; set; }

		public string ProfileName { get; set; } = "Default";


		public string DisplayCharacterName { get; set; } = string.Empty;


		public string Pronouns { get; set; } = string.Empty;


		public string CustomProfession { get; set; } = string.Empty;


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


		public static ProfileData FromProfile(CharacterProfile profile)
		{
			return new ProfileData
			{
				ProfileId = (profile?.ProfileId?.Trim() ?? string.Empty),
				IsMature = (profile?.IsMature ?? false),
				ProfileName = (string.IsNullOrWhiteSpace(profile?.ProfileName) ? "Default" : profile.ProfileName.Trim()),
				DisplayCharacterName = (profile?.DisplayName?.Trim() ?? string.Empty),
				Pronouns = (profile?.Pronouns?.Trim() ?? string.Empty),
				CustomProfession = (profile?.CustomProfession?.Trim() ?? string.Empty),
				Currently = (profile?.Currently?.Trim() ?? string.Empty),
				OutOfCharacterInfo = (profile?.OutOfCharacterInfo?.Trim() ?? string.Empty),
				AtAGlance = (profile?.AtAGlance ?? new List<AtAGlanceEntry>()),
				Experience = (profile?.Experience ?? ProfileExperience.Hidden),
				Preferences = (profile?.Preferences ?? ProfilePreferenceFlags.None),
				Themes = (profile?.Themes ?? ProfileThemeFlags.None),
				Styles = (profile?.Styles ?? ProfileStyleFlags.None),
				KnownFor = (profile?.KnownFor?.Trim() ?? string.Empty),
				Description = (profile?.Description?.Trim() ?? string.Empty),
				CreatedAt = (profile?.CreatedAt ?? DateTime.UtcNow),
				UpdatedAt = (profile?.UpdatedAt ?? DateTime.UtcNow)
			};
		}

		public CharacterProfile ToProfile(ProfileOwner identity, PlayerPresence presence = null)
		{
			CharacterProfile characterProfile = new CharacterProfile();
			characterProfile.ProfileId = ProfileId;
			characterProfile.IsMature = IsMature;
			characterProfile.ProfileName = ProfileName;
			characterProfile.AccountName = TextUtil.FirstNonEmpty(identity?.AccountName, presence?.AccountName);
			characterProfile.CharacterName = TextUtil.FirstNonEmpty(identity?.OfficialCharacterName, presence?.OfficialCharacterName);
			characterProfile.DisplayName = DisplayCharacterName;
			characterProfile.Pronouns = Pronouns;
			characterProfile.Race = TextUtil.FirstNonEmpty(identity?.Race, presence?.Race);
			characterProfile.Profession = TextUtil.FirstNonEmpty(identity?.Profession, presence?.Profession);
			characterProfile.Specialization = identity?.Specialization?.Trim() ?? string.Empty;
			characterProfile.CustomProfession = CustomProfession;
			characterProfile.IsCharacterVerified = identity?.IsCharacterApiVerified ?? presence?.IsVerified ?? false;
			characterProfile.Region = identity?.Region ?? presence?.Region ?? ProfileRegion.NA;
			characterProfile.Currently = Currently;
			characterProfile.OutOfCharacterInfo = OutOfCharacterInfo;
			characterProfile.AtAGlance = AtAGlance ?? new List<AtAGlanceEntry>();
			characterProfile.Experience = Experience;
			characterProfile.Preferences = Preferences;
			characterProfile.Themes = Themes;
			characterProfile.Styles = Styles;
			characterProfile.KnownFor = KnownFor;
			characterProfile.Description = Description;
			characterProfile.CreatedAt = CreatedAt;
			characterProfile.UpdatedAt = UpdatedAt;
			return characterProfile;
		}
	}
}
