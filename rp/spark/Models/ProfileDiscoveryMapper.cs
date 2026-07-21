using System;
using System.Collections.Generic;
using System.Linq;

namespace rp.spark.Models
{
	internal static class ProfileDiscoveryMapper
	{
		public static ProfileDiscoveryTags FromProfile(CharacterProfile profile)
		{
			if (profile != null)
			{
				return FromSelections(profile.Preferences, profile.Themes, profile.Styles, profile.DiscoveryTags);
			}
			return new ProfileDiscoveryTags();
		}

		public static ProfileDiscoveryTags FromSelections(ProfilePreferenceFlags preferences, ProfileThemeFlags themes, ProfileStyleFlags styles, ProfileDiscoveryTags discoveryTags = null)
		{
			ProfileDiscoveryTags profileDiscoveryTags = Normalize(discoveryTags);
			profileDiscoveryTags.Preferences.AddRange(from option in ProfileLabels.PreferenceOptions
				where (preferences & option.Key) == option.Key
				select option.Key.ToString());
			profileDiscoveryTags.Themes.AddRange(from option in ProfileLabels.ThemeOptions
				where (themes & option.Key) == option.Key
				select option.Key.ToString());
			profileDiscoveryTags.Styles.AddRange(from option in ProfileLabels.StyleOptions
				where (styles & option.Key) == option.Key
				select option.Key.ToString());
			return Normalize(profileDiscoveryTags);
		}

		public static ProfileDiscoveryTags Normalize(ProfileDiscoveryTags tags)
		{
			return new ProfileDiscoveryTags
			{
				Preferences = NormalizeValues(tags?.Preferences),
				Themes = NormalizeValues(tags?.Themes),
				Styles = NormalizeValues(tags?.Styles)
			};
		}

		public static ProfileDiscoveryTags Merge(params ProfileDiscoveryTags[] values)
		{
			ProfileDiscoveryTags[] available = values ?? Array.Empty<ProfileDiscoveryTags>();
			return new ProfileDiscoveryTags
			{
				Preferences = NormalizeValues(available.SelectMany(delegate(ProfileDiscoveryTags value)
				{
					IEnumerable<string> enumerable3 = value?.Preferences;
					return enumerable3 ?? Enumerable.Empty<string>();
				})),
				Themes = NormalizeValues(available.SelectMany(delegate(ProfileDiscoveryTags value)
				{
					IEnumerable<string> enumerable2 = value?.Themes;
					return enumerable2 ?? Enumerable.Empty<string>();
				})),
				Styles = NormalizeValues(available.SelectMany(delegate(ProfileDiscoveryTags value)
				{
					IEnumerable<string> enumerable = value?.Styles;
					return enumerable ?? Enumerable.Empty<string>();
				}))
			};
		}

		public static bool AreEqual(ProfileDiscoveryTags left, ProfileDiscoveryTags right)
		{
			ProfileDiscoveryTags normalizedLeft = Normalize(left);
			ProfileDiscoveryTags normalizedRight = Normalize(right);
			if (new HashSet<string>(normalizedLeft.Preferences, StringComparer.OrdinalIgnoreCase).SetEquals(normalizedRight.Preferences) && new HashSet<string>(normalizedLeft.Themes, StringComparer.OrdinalIgnoreCase).SetEquals(normalizedRight.Themes))
			{
				return new HashSet<string>(normalizedLeft.Styles, StringComparer.OrdinalIgnoreCase).SetEquals(normalizedRight.Styles);
			}
			return false;
		}

		private static List<string> NormalizeValues(IEnumerable<string> values)
		{
			return (from value in values ?? Enumerable.Empty<string>()
				select value?.Trim().ToLowerInvariant() into value
				where !string.IsNullOrWhiteSpace(value)
				select value).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy((string value) => value, StringComparer.OrdinalIgnoreCase).ToList();
		}
	}
}
