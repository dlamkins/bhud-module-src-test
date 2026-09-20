using Quarry.Models;
using Quarry.UserInterface.Controls;
using Quarry.WikiData.Achievement;

namespace Quarry.Interfaces
{
	public interface IAchievementCardFactory
	{
		AchievementCard Create(AchievementTableEntry achievement, string icon, GuidanceInfo guidance = null, IHereCardActions hereCardActions = null, int? rank = null);
	}
}
