using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Achievement;
using Denrage.AchievementTrackerModule.Services.Factories.AchievementControl;

namespace Denrage.AchievementTrackerModule.UserInterface.Windows
{
	public class AchievementControlProvider : IAchievementControlProvider
	{
		private readonly Dictionary<Type, AchievementControlFactory> mapping = new Dictionary<Type, AchievementControlFactory>();

		public AchievementControlProvider(IAchievementService achievementService, IItemDetailWindowManager itemDetailWindowManager, ContentsManager contentsManager)
		{
			mapping.Add(typeof(StringDescription), new AchievementTextControlFactory());
			mapping.Add(typeof(CollectionDescription), new AchievementCollectionControlFactory(achievementService, itemDetailWindowManager, contentsManager));
			mapping.Add(typeof(ObjectivesDescription), new AchievementObjectiveControlFactory(achievementService, itemDetailWindowManager, contentsManager));
		}

		public Control GetAchievementControl(AchievementTableEntry achievement, AchievementTableEntryDescription description)
		{
			if (!mapping.TryGetValue(description.GetType(), out var factory))
			{
				return null;
			}
			return factory.Create(achievement, description);
		}
	}
}
