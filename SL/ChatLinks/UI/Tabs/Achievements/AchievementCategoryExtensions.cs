using System.Linq;
using GuildWars2.Hero.Achievements.Categories;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Achievements
{
	internal static class AchievementCategoryExtensions
	{
		public static bool? IsParentOf(this AchievementCategory category, int achievementId)
		{
			ThrowHelper.ThrowIfNull(category, "category");
			if (category.Achievements.Any((AchievementRef achievement) => achievement.Id == achievementId))
			{
				return true;
			}
			if (category.Tomorrow == null)
			{
				return false;
			}
			if (category.Tomorrow.Any((AchievementRef achievement) => achievement.Id == achievementId))
			{
				return true;
			}
			return null;
		}
	}
}
