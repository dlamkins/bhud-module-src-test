using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;

namespace Kenedia.Modules.Characters.Extensions
{
	public static class CustomEnumExtension
	{
		public static SettingsModel.SortBy GetSortType(this string s)
		{
			if (s == string.Format(strings.SortBy, strings.Name))
			{
				return SettingsModel.SortBy.Name;
			}
			if (s == string.Format(strings.SortBy, strings.Level))
			{
				return SettingsModel.SortBy.Level;
			}
			if (s == string.Format(strings.SortBy, strings.Race))
			{
				return SettingsModel.SortBy.Race;
			}
			if (s == string.Format(strings.SortBy, strings.Gender))
			{
				return SettingsModel.SortBy.Gender;
			}
			if (s == string.Format(strings.SortBy, strings.Profession))
			{
				return SettingsModel.SortBy.Profession;
			}
			if (s == string.Format(strings.SortBy, strings.Specialization))
			{
				return SettingsModel.SortBy.Specialization;
			}
			if (s == string.Format(strings.SortBy, strings.TimeSinceLogin))
			{
				return SettingsModel.SortBy.TimeSinceLogin;
			}
			if (s == string.Format(strings.SortBy, strings.Map))
			{
				return SettingsModel.SortBy.Map;
			}
			if (s == string.Format(strings.SortBy, strings.Tag))
			{
				return SettingsModel.SortBy.Tag;
			}
			return SettingsModel.SortBy.Custom;
		}

		public static string GetSortType(this SettingsModel.SortBy st)
		{
			return st switch
			{
				SettingsModel.SortBy.Name => string.Format(strings.SortBy, strings.Name), 
				SettingsModel.SortBy.Level => string.Format(strings.SortBy, strings.Level), 
				SettingsModel.SortBy.Race => string.Format(strings.SortBy, strings.Race), 
				SettingsModel.SortBy.Gender => string.Format(strings.SortBy, strings.Gender), 
				SettingsModel.SortBy.Profession => string.Format(strings.SortBy, strings.Profession), 
				SettingsModel.SortBy.Specialization => string.Format(strings.SortBy, strings.Specialization), 
				SettingsModel.SortBy.TimeSinceLogin => string.Format(strings.SortBy, strings.TimeSinceLogin), 
				SettingsModel.SortBy.Map => string.Format(strings.SortBy, strings.Map), 
				SettingsModel.SortBy.Tag => string.Format(strings.SortBy, strings.Tag), 
				SettingsModel.SortBy.Custom => strings.Custom, 
				_ => strings.Custom, 
			};
		}

		public static SettingsModel.SortDirection GetSortOrder(this string s)
		{
			if (!(s == strings.Descending))
			{
				return SettingsModel.SortDirection.Ascending;
			}
			return SettingsModel.SortDirection.Descending;
		}

		public static string GetSortOrder(this SettingsModel.SortDirection so)
		{
			return so switch
			{
				SettingsModel.SortDirection.Ascending => strings.Ascending, 
				SettingsModel.SortDirection.Descending => strings.Descending, 
				_ => strings.Ascending, 
			};
		}

		public static SettingsModel.FilterBehavior GetFilterBehavior(this string s)
		{
			if (!(s == strings.ExcludeMatches))
			{
				return SettingsModel.FilterBehavior.Include;
			}
			return SettingsModel.FilterBehavior.Exclude;
		}

		public static string GetFilterBehavior(this SettingsModel.FilterBehavior fb)
		{
			return fb switch
			{
				SettingsModel.FilterBehavior.Include => strings.IncludeMatches, 
				SettingsModel.FilterBehavior.Exclude => strings.ExcludeMatches, 
				_ => strings.IncludeMatches, 
			};
		}

		public static SettingsModel.MatchingBehavior GetMatchingBehavior(this string s)
		{
			if (!(s == strings.MatchAllFilter))
			{
				return SettingsModel.MatchingBehavior.MatchAny;
			}
			return SettingsModel.MatchingBehavior.MatchAll;
		}

		public static string GetMatchingBehavior(this SettingsModel.MatchingBehavior fb)
		{
			return fb switch
			{
				SettingsModel.MatchingBehavior.MatchAny => strings.MatchAnyFilter, 
				SettingsModel.MatchingBehavior.MatchAll => strings.MatchAllFilter, 
				_ => strings.MatchAnyFilter, 
			};
		}

		public static string GetPanelSize(this SettingsModel.PanelSizes s)
		{
			return s switch
			{
				SettingsModel.PanelSizes.Small => strings.Small, 
				SettingsModel.PanelSizes.Normal => strings.Normal, 
				SettingsModel.PanelSizes.Large => strings.Large, 
				SettingsModel.PanelSizes.Custom => strings.Custom, 
				_ => strings.Normal, 
			};
		}

		public static SettingsModel.PanelSizes GetPanelSize(this string s)
		{
			if (s == strings.Small)
			{
				return SettingsModel.PanelSizes.Small;
			}
			if (s == strings.Large)
			{
				return SettingsModel.PanelSizes.Large;
			}
			if (s == strings.Custom)
			{
				return SettingsModel.PanelSizes.Custom;
			}
			return SettingsModel.PanelSizes.Normal;
		}

		public static string GetPanelLayout(this SettingsModel.CharacterPanelLayout layout)
		{
			return layout switch
			{
				SettingsModel.CharacterPanelLayout.OnlyIcons => strings.OnlyIcons, 
				SettingsModel.CharacterPanelLayout.OnlyText => strings.OnlyText, 
				SettingsModel.CharacterPanelLayout.IconAndText => strings.TextAndIcon, 
				_ => strings.TextAndIcon, 
			};
		}

		public static SettingsModel.CharacterPanelLayout GetPanelLayout(this string layout)
		{
			if (layout == strings.OnlyIcons)
			{
				return SettingsModel.CharacterPanelLayout.OnlyIcons;
			}
			if (layout == strings.OnlyText)
			{
				return SettingsModel.CharacterPanelLayout.OnlyText;
			}
			return SettingsModel.CharacterPanelLayout.IconAndText;
		}
	}
}
