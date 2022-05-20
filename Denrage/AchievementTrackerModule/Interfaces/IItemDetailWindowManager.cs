using System;
using System.Collections.Generic;
using Denrage.AchievementTrackerModule.Libs.Achievement;

namespace Denrage.AchievementTrackerModule.Interfaces
{
	public interface IItemDetailWindowManager : IDisposable
	{
		void CreateAndShowWindow(string name, string[] columns, List<CollectionAchievementTable.CollectionAchievementTableEntry> item, string achievementLink, int achievementId, int itemIndex);

		bool ShowWindow(string name);

		void Update();
	}
}
