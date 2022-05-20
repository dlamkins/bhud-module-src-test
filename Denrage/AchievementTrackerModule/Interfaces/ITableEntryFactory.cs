using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Libs.Achievement;

namespace Denrage.AchievementTrackerModule.Interfaces
{
	public interface ITableEntryFactory<TEntry> where TEntry : CollectionAchievementTable.CollectionAchievementTableEntry
	{
		Control Create(TEntry entry);
	}
}
