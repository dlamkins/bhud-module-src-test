using System.Collections.Generic;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Achievement;
using Denrage.AchievementTrackerModule.UserInterface.Views;
using Gw2Sharp.WebApi.V2.Models;

namespace Denrage.AchievementTrackerModule.Services.Factories
{
	public class AchievementItemOverviewFactory : IAchievementItemOverviewFactory
	{
		private readonly IAchievementListItemFactory achievementListItemFactory;

		private readonly IAchievementService achievementService;

		public AchievementItemOverviewFactory(IAchievementListItemFactory achievementListItemFactory, IAchievementService achievementService)
		{
			this.achievementListItemFactory = achievementListItemFactory;
			this.achievementService = achievementService;
		}

		public AchievementItemOverview Create(IEnumerable<(AchievementCategory, AchievementTableEntry)> achievements, string title)
		{
			return new AchievementItemOverview(achievements, title, achievementService, achievementListItemFactory);
		}
	}
}
