using System.Collections.Generic;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Denrage.AchievementTrackerModule.UserInterface.Windows;

namespace Denrage.AchievementTrackerModule.Services.Factories.ItemDetails
{
	public class ItemDetailWindowFactory : IItemDetailWindowFactory
	{
		private readonly ContentsManager contentsManager;

		private readonly IAchievementService achievementService;

		private readonly IAchievementTableEntryProvider achievementTableEntryProvider;

		private readonly ISubPageInformationWindowManager subPageInformationWindowManager;

		public ItemDetailWindowFactory(ContentsManager contentsManager, IAchievementService achievementService, IAchievementTableEntryProvider achievementTableEntryProvider, ISubPageInformationWindowManager subPageInformationWindowManager)
		{
			this.contentsManager = contentsManager;
			this.achievementService = achievementService;
			this.achievementTableEntryProvider = achievementTableEntryProvider;
			this.subPageInformationWindowManager = subPageInformationWindowManager;
		}

		public ItemDetailWindow Create(string name, string[] columns, List<CollectionAchievementTable.CollectionAchievementTableEntry> item, string achievementLink)
		{
			return new ItemDetailWindow(contentsManager, achievementService, achievementTableEntryProvider, subPageInformationWindowManager, achievementLink, name, columns, item);
		}
	}
}
