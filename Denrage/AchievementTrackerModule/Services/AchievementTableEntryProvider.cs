using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Achievement;
using Denrage.AchievementTrackerModule.Services.Factories.ItemDetails;

namespace Denrage.AchievementTrackerModule.Services
{
	public class AchievementTableEntryProvider : IAchievementTableEntryProvider
	{
		private readonly Dictionary<Type, AchievementTableEntryFactory> mapping = new Dictionary<Type, AchievementTableEntryFactory>();

		public AchievementTableEntryProvider(IAchievementService achievementService, Logger logger)
		{
			mapping.Add(typeof(CollectionAchievementTable.CollectionAchievementTableNumberEntry), new AchievementTableNumberEntryFactory());
			mapping.Add(typeof(CollectionAchievementTable.CollectionAchievementTableCoinEntry), new AchievementTableCoinEntryFactory());
			mapping.Add(typeof(CollectionAchievementTable.CollectionAchievementTableItemEntry), new AchievementTableItemEntryFactory());
			mapping.Add(typeof(CollectionAchievementTable.CollectionAchievementTableLinkEntry), new AchievementTableLinkEntryFactory());
			mapping.Add(typeof(CollectionAchievementTable.CollectionAchievementTableMapEntry), new AchievementTableMapEntryFactory(achievementService, logger));
			mapping.Add(typeof(CollectionAchievementTable.CollectionAchievementTableStringEntry), new AchievementTableStringEntryFactory());
			mapping.Add(typeof(CollectionAchievementTable.CollectionAchievementTableEmptyEntry), new AchievementTableEmptyEntryFactory());
		}

		public Control GetTableEntryControl(CollectionAchievementTable.CollectionAchievementTableEntry entry)
		{
			if (!mapping.TryGetValue(entry.GetType(), out var factory))
			{
				return null;
			}
			return factory.Create(entry);
		}
	}
}
