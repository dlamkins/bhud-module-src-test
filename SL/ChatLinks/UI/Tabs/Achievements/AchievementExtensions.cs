using System.Collections.Generic;
using System.Linq;
using GuildWars2.Hero.Achievements;
using GuildWars2.Hero.Achievements.Groups;

namespace SL.ChatLinks.UI.Tabs.Achievements
{
	internal static class AchievementExtensions
	{
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
