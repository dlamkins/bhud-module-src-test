using System;
using System.Collections.Generic;
using System.Linq;
using GuildWars2.Hero.Achievements;
using GuildWars2.Hero.Achievements.Categories;
using GuildWars2.Hero.Achievements.Groups;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Achievements
{
	internal static class AchievementExtensions
	{
		public static Uri? IconUrl(this Achievement achievement)
		{
			ThrowHelper.ThrowIfNull(achievement, "achievement");
			if (string.IsNullOrEmpty(achievement.IconHref))
			{
				return null;
			}
			return new Uri(achievement.IconHref);
		}

		public static Uri? IconUrl(this AchievementCategory category)
		{
			ThrowHelper.ThrowIfNull(category, "category");
			if (string.IsNullOrEmpty(category.IconHref))
			{
				return null;
			}
			return new Uri(category.IconHref);
		}

		public static bool IsLocked(this Achievement achievement, AchievementGroup? group, IReadOnlyList<AccountAchievement>? progression)
		{
			Achievement achievement2 = achievement;
			IReadOnlyList<AccountAchievement> progression2 = progression;
			if (progression2 != null && (object)group != null && group.IsPerCharacter())
			{
				return false;
			}
			if (achievement2.Flags.RequiresUnlock)
			{
				AccountAchievement obj = progression2?.SingleOrDefault((AccountAchievement progress) => progress.Id == achievement2.Id);
				if ((object)obj == null)
				{
					return true;
				}
				return !obj.Unlocked;
			}
			if (achievement2.Prerequisites.Count > 0)
			{
				List<AccountAchievement> list = new List<AccountAchievement>();
				list.AddRange(achievement2.Prerequisites.Select((int pre) => progression2?.SingleOrDefault((AccountAchievement progress) => progress.Id == pre)));
				return !list.All((AccountAchievement progress) => progress?.Done ?? false);
			}
			return false;
		}

		public static bool IsHidden(this Achievement achievement, IReadOnlyList<AccountAchievement>? progression)
		{
			Achievement achievement2 = achievement;
			if (achievement2.Flags.Hidden)
			{
				return !(progression?.Any((AccountAchievement progress) => progress.Id == achievement2.Id)).GetValueOrDefault();
			}
			return false;
		}
	}
}
