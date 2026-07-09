using System;
using System.Collections.Generic;

namespace rp.spark.Models
{
	public static class ProfileLabels
	{
		public static readonly string[] RpStatusOptions = new string[4] { "Online", "Invisible", "Looking for RP", "Busy" };

		public static readonly string[] ExperienceOptions = new string[5] { "Don't show", "New", "Returning", "Proficient", "Experienced" };

		public static readonly KeyValuePair<ProfilePreferenceFlags, string>[] PreferenceOptions = new KeyValuePair<ProfilePreferenceFlags, string>[6]
		{
			new KeyValuePair<ProfilePreferenceFlags, string>(ProfilePreferenceFlags.Casual, "Casual"),
			new KeyValuePair<ProfilePreferenceFlags, string>(ProfilePreferenceFlags.OneShot, "One-Shot"),
			new KeyValuePair<ProfilePreferenceFlags, string>(ProfilePreferenceFlags.LongTerm, "Long-Term"),
			new KeyValuePair<ProfilePreferenceFlags, string>(ProfilePreferenceFlags.EventRoleplay, "Event RP"),
			new KeyValuePair<ProfilePreferenceFlags, string>(ProfilePreferenceFlags.SmallGroup, "Small Group"),
			new KeyValuePair<ProfilePreferenceFlags, string>(ProfilePreferenceFlags.LargeGroup, "Large Group")
		};

		public static readonly KeyValuePair<ProfileThemeFlags, string>[] ThemeOptions = new KeyValuePair<ProfileThemeFlags, string>[4]
		{
			new KeyValuePair<ProfileThemeFlags, string>(ProfileThemeFlags.Comedy, "Comedy"),
			new KeyValuePair<ProfileThemeFlags, string>(ProfileThemeFlags.Combat, "Combat"),
			new KeyValuePair<ProfileThemeFlags, string>(ProfileThemeFlags.Romance, "Romance"),
			new KeyValuePair<ProfileThemeFlags, string>(ProfileThemeFlags.SliceOfLife, "Slice of Life")
		};

		public static readonly KeyValuePair<ProfileStyleFlags, string>[] StyleOptions = new KeyValuePair<ProfileStyleFlags, string>[5]
		{
			new KeyValuePair<ProfileStyleFlags, string>(ProfileStyleFlags.WalkUpFriendly, "Walk-Up Friendly"),
			new KeyValuePair<ProfileStyleFlags, string>(ProfileStyleFlags.WhisperFirst, "Whisper First"),
			new KeyValuePair<ProfileStyleFlags, string>(ProfileStyleFlags.OpenToNewContacts, "Open to New Contacts"),
			new KeyValuePair<ProfileStyleFlags, string>(ProfileStyleFlags.LoreFriendly, "Lore Friendly"),
			new KeyValuePair<ProfileStyleFlags, string>(ProfileStyleFlags.FlexibleLore, "Flexible Lore")
		};

		public static string GetExperienceLabel(ProfileExperience experience)
		{
			return experience switch
			{
				ProfileExperience.New => "New", 
				ProfileExperience.Returning => "Returning", 
				ProfileExperience.Proficient => "Proficient", 
				ProfileExperience.Experienced => "Experienced", 
				_ => "Don't show", 
			};
		}

		public static ProfileExperience ParseExperience(string label)
		{
			string value = (label ?? string.Empty).Trim();
			if (string.Equals(value, "New", StringComparison.OrdinalIgnoreCase))
			{
				return ProfileExperience.New;
			}
			if (string.Equals(value, "Returning", StringComparison.OrdinalIgnoreCase))
			{
				return ProfileExperience.Returning;
			}
			if (string.Equals(value, "Proficient", StringComparison.OrdinalIgnoreCase))
			{
				return ProfileExperience.Proficient;
			}
			if (string.Equals(value, "Experienced", StringComparison.OrdinalIgnoreCase))
			{
				return ProfileExperience.Experienced;
			}
			return ProfileExperience.Hidden;
		}

		public static string StatusLabel(RPStatus status)
		{
			return status switch
			{
				RPStatus.Invisible => "Invisible", 
				RPStatus.Looking => "Looking for RP", 
				RPStatus.Busy => "Busy", 
				RPStatus.Offline => "Offline", 
				_ => "Online", 
			};
		}

		public static RPStatus ParseStatus(string label)
		{
			string value = (label ?? string.Empty).Trim();
			if (string.Equals(value, "Invisible", StringComparison.OrdinalIgnoreCase))
			{
				return RPStatus.Invisible;
			}
			if (string.Equals(value, "Looking for RP", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "Looking", StringComparison.OrdinalIgnoreCase))
			{
				return RPStatus.Looking;
			}
			if (string.Equals(value, "Busy", StringComparison.OrdinalIgnoreCase))
			{
				return RPStatus.Busy;
			}
			if (string.Equals(value, "Offline", StringComparison.OrdinalIgnoreCase))
			{
				return RPStatus.Offline;
			}
			return RPStatus.Online;
		}
	}
}
