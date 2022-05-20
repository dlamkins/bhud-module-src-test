using System.Collections.Generic;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Denrage.AchievementTrackerModule.UserInterface.Windows;

namespace Denrage.AchievementTrackerModule.Interfaces
{
	public interface IItemDetailWindowFactory
	{
		ItemDetailWindow Create(string name, string[] columns, List<CollectionAchievementTable.CollectionAchievementTableEntry> item, string achievementLink);
	}
}
