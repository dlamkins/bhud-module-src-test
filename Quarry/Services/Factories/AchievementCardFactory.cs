using Quarry.Interfaces;
using Quarry.Models;
using Quarry.UserInterface.Controls;
using Quarry.WikiData.Achievement;

namespace Quarry.Services.Factories
{
	public class AchievementCardFactory : IAchievementCardFactory
	{
		private readonly IAchievementTrackerService achievementTrackerService;

		private readonly IAchievementService achievementService;

		private readonly ITextureService textureService;

		private readonly IHuntService huntService;

		private readonly INearestObjectiveService nearestObjectiveService;

		private readonly ICurrentMapService currentMapService;

		public AchievementCardFactory(IAchievementTrackerService achievementTrackerService, IAchievementService achievementService, ITextureService textureService, IHuntService huntService, INearestObjectiveService nearestObjectiveService, ICurrentMapService currentMapService)
		{
			this.achievementTrackerService = achievementTrackerService;
			this.achievementService = achievementService;
			this.textureService = textureService;
			this.huntService = huntService;
			this.nearestObjectiveService = nearestObjectiveService;
			this.currentMapService = currentMapService;
		}

		public AchievementCard Create(AchievementTableEntry achievement, string icon, GuidanceInfo guidance = null, IHereCardActions hereCardActions = null, int? rank = null)
		{
			return new AchievementCard(achievement, achievementTrackerService, achievementService, textureService, huntService, nearestObjectiveService, currentMapService, icon, guidance, hereCardActions, rank);
		}
	}
}
