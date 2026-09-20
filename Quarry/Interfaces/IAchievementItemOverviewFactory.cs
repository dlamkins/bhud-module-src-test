using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;
using Quarry.UserInterface.Views;
using Quarry.WikiData.Achievement;

namespace Quarry.Interfaces
{
	public interface IAchievementItemOverviewFactory
	{
		AchievementItemOverview Create(IEnumerable<(AchievementCategory, AchievementTableEntry)> achievements, string title);
	}
}
