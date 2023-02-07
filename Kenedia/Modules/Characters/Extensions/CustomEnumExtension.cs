using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;

namespace Kenedia.Modules.Characters.Extensions
{
	public static class CustomEnumExtension
	{
		public static SettingsModel.ESortType GetSortType(this string s)
		{
			if (s == string.Format(strings.SortBy, strings.Name))
			{
				return SettingsModel.ESortType.SortByName;
			}
			if (s == string.Format(strings.SortBy, strings.Tag))
			{
				return SettingsModel.ESortType.SortByTag;
			}
			if (s == string.Format(strings.SortBy, strings.Profession))
			{
				return SettingsModel.ESortType.SortByProfession;
			}
			if (s == string.Format(strings.SortBy, strings.LastLogin))
			{
				return SettingsModel.ESortType.SortByLastLogin;
			}
			if (s == string.Format(strings.SortBy, strings.Map))
			{
				return SettingsModel.ESortType.SortByMap;
			}
			return SettingsModel.ESortType.Custom;
		}

		public static string GetSortType(this SettingsModel.ESortType st)
		{
			return st switch
			{
				SettingsModel.ESortType.SortByName => string.Format(strings.SortBy, strings.Name), 
				SettingsModel.ESortType.SortByTag => string.Format(strings.SortBy, strings.Tag), 
				SettingsModel.ESortType.SortByProfession => string.Format(strings.SortBy, strings.Profession), 
				SettingsModel.ESortType.SortByLastLogin => string.Format(strings.SortBy, strings.LastLogin), 
				SettingsModel.ESortType.SortByMap => string.Format(strings.SortBy, strings.Map), 
				SettingsModel.ESortType.Custom => strings.Custom, 
				_ => strings.Custom, 
			};
		}

		public static SettingsModel.ESortOrder GetSortOrder(this string s)
		{
			if (!(s == strings.Descending))
			{
				return SettingsModel.ESortOrder.Ascending;
			}
			return SettingsModel.ESortOrder.Descending;
		}

		public static string GetSortOrder(this SettingsModel.ESortOrder so)
		{
			return so switch
			{
				SettingsModel.ESortOrder.Ascending => strings.Ascending, 
				SettingsModel.ESortOrder.Descending => strings.Descending, 
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
