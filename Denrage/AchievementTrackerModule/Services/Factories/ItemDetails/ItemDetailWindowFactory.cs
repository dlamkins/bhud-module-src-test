using System.Collections.Generic;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Achievement;
using Denrage.AchievementTrackerModule.UserInterface.Windows;

namespace Denrage.AchievementTrackerModule.Services.Factories.ItemDetails
{
	public class ItemDetailWindowFactory : IItemDetailWindowFactory
	{
		private readonly ContentsManager contentsManager;

		private readonly IAchievementService achievementService;

		private readonly IAchievementTableEntryProvider achievementTableEntryProvider;

		public ItemDetailWindowFactory(ContentsManager contentsManager, IAchievementService achievementService, IAchievementTableEntryProvider achievementTableEntryProvider)
		{
			this.contentsManager = contentsManager;
			this.achievementService = achievementService;
			this.achievementTableEntryProvider = achievementTableEntryProvider;
		}

		public ItemDetailWindow Create(string name, string[] columns, List<CollectionAchievementTable.CollectionAchievementTableEntry> item, string achievementLink)
		{
			return new ItemDetailWindow(contentsManager, achievementService, achievementTableEntryProvider, achievementLink, name, columns, item);
		}
	}
}
