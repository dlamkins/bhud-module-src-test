using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule.Interfaces
{
	public interface IAchievementControl
	{
		Point Size { get; set; }

		void BuildControl();
	}
}
