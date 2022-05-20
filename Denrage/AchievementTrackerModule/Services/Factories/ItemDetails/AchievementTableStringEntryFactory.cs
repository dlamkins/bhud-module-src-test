using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;

namespace Denrage.AchievementTrackerModule.Services.Factories.ItemDetails
{
	public class AchievementTableStringEntryFactory : AchievementTableEntryFactory<CollectionAchievementTable.CollectionAchievementTableStringEntry>
	{
		private readonly IFormattedLabelHtmlService formattedLabelHtmlService;

		public AchievementTableStringEntryFactory(IFormattedLabelHtmlService formattedLabelHtmlService)
		{
			this.formattedLabelHtmlService = formattedLabelHtmlService;
		}

		protected override Control CreateInternal(CollectionAchievementTable.CollectionAchievementTableStringEntry entry)
		{
			return (Control)(object)formattedLabelHtmlService.CreateLabel(entry?.Text ?? string.Empty).AutoSizeHeight().Wrap()
				.Build();
		}
	}
}
