using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;

namespace Denrage.AchievementTrackerModule.Services.Factories.ItemDetails
{
	public class AchievementTableLinkEntryFactory : AchievementTableEntryFactory<CollectionAchievementTable.CollectionAchievementTableLinkEntry>
	{
		private readonly IFormattedLabelHtmlService formattedLabelHtmlService;

		public AchievementTableLinkEntryFactory(IFormattedLabelHtmlService formattedLabelHtmlService)
		{
			this.formattedLabelHtmlService = formattedLabelHtmlService;
		}

		protected override Control CreateInternal(CollectionAchievementTable.CollectionAchievementTableLinkEntry entry)
		{
			return (Control)(object)formattedLabelHtmlService.CreateLabel(entry?.Text ?? string.Empty).AutoSizeHeight().Wrap()
				.Build();
		}
	}
}
