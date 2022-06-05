using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Denrage.AchievementTrackerModule.Services.Factories.ItemDetails;

namespace Denrage.AchievementTrackerModule.Services
{
	public class AchievementTableEntryProvider : IAchievementTableEntryProvider
	{
		private readonly Dictionary<Type, AchievementTableEntryFactory> mapping = new Dictionary<Type, AchievementTableEntryFactory>();

		public AchievementTableEntryProvider(IFormattedLabelHtmlService formattedLabelHtmlService, IExternalImageService externalImageService, Logger logger, Gw2ApiManager gw2ApiManager, ContentsManager contentsManager)
		{
			mapping.Add(typeof(CollectionAchievementTable.CollectionAchievementTableNumberEntry), new AchievementTableNumberEntryFactory());
			mapping.Add(typeof(CollectionAchievementTable.CollectionAchievementTableCoinEntry), new AchievementTableCoinEntryFactory(gw2ApiManager, contentsManager));
			mapping.Add(typeof(CollectionAchievementTable.CollectionAchievementTableItemEntry), new AchievementTableItemEntryFactory(externalImageService));
			mapping.Add(typeof(CollectionAchievementTable.CollectionAchievementTableLinkEntry), new AchievementTableLinkEntryFactory(formattedLabelHtmlService));
			mapping.Add(typeof(CollectionAchievementTable.CollectionAchievementTableMapEntry), new AchievementTableMapEntryFactory(externalImageService, logger));
			mapping.Add(typeof(CollectionAchievementTable.CollectionAchievementTableStringEntry), new AchievementTableStringEntryFactory(formattedLabelHtmlService));
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
