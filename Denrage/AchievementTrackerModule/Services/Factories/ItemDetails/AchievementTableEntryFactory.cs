using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;

namespace Denrage.AchievementTrackerModule.Services.Factories.ItemDetails
{
	public abstract class AchievementTableEntryFactory
	{
		public abstract Control Create(object entry);
	}
	public abstract class AchievementTableEntryFactory<TEntry> : AchievementTableEntryFactory, ITableEntryFactory<TEntry> where TEntry : CollectionAchievementTable.CollectionAchievementTableEntry
	{
		protected abstract Control CreateInternal(TEntry entry);

		public override Control Create(object entry)
		{
			return Create((TEntry)entry);
		}

		public Control Create(TEntry entry)
		{
			return CreateInternal(entry);
		}
	}
}
