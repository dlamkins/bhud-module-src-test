using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Achievement;
using Denrage.AchievementTrackerModule.UserInterface.Controls;

namespace Denrage.AchievementTrackerModule.Services.Factories.AchievementControl
{
	public class AchievementObjectiveControlFactory : AchievementControlFactory<AchievementObjectivesControl, ObjectivesDescription>
	{
		private readonly IAchievementService achievementService;

		private readonly IItemDetailWindowManager itemDetailWindowFactory;

		private readonly ContentsManager contentsManager;

		public AchievementObjectiveControlFactory(IAchievementService achievementService, IItemDetailWindowManager itemDetailWindowManager, ContentsManager contentsManager)
		{
			this.achievementService = achievementService;
			itemDetailWindowFactory = itemDetailWindowManager;
			this.contentsManager = contentsManager;
		}

		protected override AchievementObjectivesControl CreateInternal(AchievementTableEntry achievement, ObjectivesDescription description)
		{
			return new AchievementObjectivesControl(itemDetailWindowFactory, achievementService, contentsManager, achievement, description);
		}
	}
}
