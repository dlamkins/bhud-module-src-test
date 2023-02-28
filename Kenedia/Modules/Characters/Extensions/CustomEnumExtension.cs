using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;

namespace Kenedia.Modules.Characters.Extensions
{
	public static class CustomEnumExtension
	{
		public static Settings.SortBy GetSortType(this string s)
		{
			if (s == string.Format(strings.SortBy, strings.Name))
			{
				return Settings.SortBy.Name;
			}
			if (s == string.Format(strings.SortBy, strings.Level))
			{
				return Settings.SortBy.Level;
			}
			if (s == string.Format(strings.SortBy, strings.Race))
			{
				return Settings.SortBy.Race;
			}
			if (s == string.Format(strings.SortBy, strings.Gender))
			{
				return Settings.SortBy.Gender;
			}
			if (s == string.Format(strings.SortBy, strings.Profession))
			{
				return Settings.SortBy.Profession;
			}
			if (s == string.Format(strings.SortBy, strings.Specialization))
			{
				return Settings.SortBy.Specialization;
			}
			if (s == string.Format(strings.SortBy, strings.TimeSinceLogin))
			{
				return Settings.SortBy.TimeSinceLogin;
			}
			if (s == string.Format(strings.SortBy, strings.Map))
			{
				return Settings.SortBy.Map;
			}
			if (s == string.Format(strings.SortBy, strings.Tag))
			{
				return Settings.SortBy.Tag;
			}
			return Settings.SortBy.Custom;
		}

		public static string GetSortType(this Settings.SortBy st)
		{
			return st switch
			{
				Settings.SortBy.Name => string.Format(strings.SortBy, strings.Name), 
				Settings.SortBy.Level => string.Format(strings.SortBy, strings.Level), 
				Settings.SortBy.Race => string.Format(strings.SortBy, strings.Race), 
				Settings.SortBy.Gender => string.Format(strings.SortBy, strings.Gender), 
				Settings.SortBy.Profession => string.Format(strings.SortBy, strings.Profession), 
				Settings.SortBy.Specialization => string.Format(strings.SortBy, strings.Specialization), 
				Settings.SortBy.TimeSinceLogin => string.Format(strings.SortBy, strings.TimeSinceLogin), 
				Settings.SortBy.Map => string.Format(strings.SortBy, strings.Map), 
				Settings.SortBy.Tag => string.Format(strings.SortBy, strings.Tag), 
				Settings.SortBy.Custom => strings.Custom, 
				_ => strings.Custom, 
			};
		}

		public static Settings.SortDirection GetSortOrder(this string s)
		{
			if (!(s == strings.Descending))
			{
				return Settings.SortDirection.Ascending;
			}
			return Settings.SortDirection.Descending;
		}

		public static string GetSortOrder(this Settings.SortDirection so)
		{
			return so switch
			{
				Settings.SortDirection.Ascending => strings.Ascending, 
				Settings.SortDirection.Descending => strings.Descending, 
				_ => strings.Ascending, 
			};
		}

		public static Settings.FilterBehavior GetFilterBehavior(this string s)
		{
			if (!(s == strings.ExcludeMatches))
			{
				return Settings.FilterBehavior.Include;
			}
			return Settings.FilterBehavior.Exclude;
		}

		public static string GetFilterBehavior(this Settings.FilterBehavior fb)
		{
			return fb switch
			{
				Settings.FilterBehavior.Include => strings.IncludeMatches, 
				Settings.FilterBehavior.Exclude => strings.ExcludeMatches, 
				_ => strings.IncludeMatches, 
			};
		}

		public static Settings.MatchingBehavior GetMatchingBehavior(this string s)
		{
			if (!(s == strings.MatchAllFilter))
			{
				return Settings.MatchingBehavior.MatchAny;
			}
			return Settings.MatchingBehavior.MatchAll;
		}

		public static string GetMatchingBehavior(this Settings.MatchingBehavior fb)
		{
			return fb switch
			{
				Settings.MatchingBehavior.MatchAny => strings.MatchAnyFilter, 
				Settings.MatchingBehavior.MatchAll => strings.MatchAllFilter, 
				_ => strings.MatchAnyFilter, 
			};
		}

		public static string GetPanelSize(this Settings.PanelSizes s)
		{
			return s switch
			{
				Settings.PanelSizes.Small => strings.Small, 
				Settings.PanelSizes.Normal => strings.Normal, 
				Settings.PanelSizes.Large => strings.Large, 
				Settings.PanelSizes.Custom => strings.Custom, 
				_ => strings.Normal, 
			};
		}

		public static Settings.PanelSizes GetPanelSize(this string s)
		{
			if (s == strings.Small)
			{
				return Settings.PanelSizes.Small;
			}
			if (s == strings.Large)
			{
				return Settings.PanelSizes.Large;
			}
			if (s == strings.Custom)
			{
				return Settings.PanelSizes.Custom;
			}
			return Settings.PanelSizes.Normal;
		}

		public static string GetPanelLayout(this Settings.CharacterPanelLayout layout)
		{
			return layout switch
			{
				Settings.CharacterPanelLayout.OnlyIcons => strings.OnlyIcons, 
				Settings.CharacterPanelLayout.OnlyText => strings.OnlyText, 
				Settings.CharacterPanelLayout.IconAndText => strings.TextAndIcon, 
				_ => strings.TextAndIcon, 
			};
		}

		public static Settings.CharacterPanelLayout GetPanelLayout(this string layout)
		{
			if (layout == strings.OnlyIcons)
			{
				return Settings.CharacterPanelLayout.OnlyIcons;
			}
			if (layout == strings.OnlyText)
			{
				return Settings.CharacterPanelLayout.OnlyText;
			}
			return Settings.CharacterPanelLayout.IconAndText;
		}
	}
}
