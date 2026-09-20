using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;
using Quarry.Interfaces;
using Quarry.UserInterface.Views;
using Quarry.WikiData.Achievement;

namespace Quarry.Services.Factories
{
	public class AchievementItemOverviewFactory : IAchievementItemOverviewFactory
	{
		private readonly IAchievementCardFactory achievementCardFactory;

		private readonly IAchievementService achievementService;

		private readonly IBitAlignmentService bitAlignmentService;

		public AchievementItemOverviewFactory(IAchievementCardFactory achievementCardFactory, IAchievementService achievementService, IBitAlignmentService bitAlignmentService)
		{
			this.achievementCardFactory = achievementCardFactory;
			this.achievementService = achievementService;
			this.bitAlignmentService = bitAlignmentService;
		}

		public AchievementItemOverview Create(IEnumerable<(AchievementCategory, AchievementTableEntry)> achievements, string title)
		{
			return new AchievementItemOverview(achievements, title, achievementService, achievementCardFactory, bitAlignmentService);
		}
	}
}
