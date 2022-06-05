using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Denrage.AchievementTrackerModule.UserInterface.Controls.FormattedLabel;

namespace Denrage.AchievementTrackerModule.Services.Factories.ItemDetails
{
	public class AchievementTableItemEntryFactory : AchievementTableEntryFactory<CollectionAchievementTable.CollectionAchievementTableItemEntry>
	{
		private readonly IExternalImageService externalImageService;

		public AchievementTableItemEntryFactory(IExternalImageService externalImageService)
		{
			this.externalImageService = externalImageService;
		}

		protected override Control CreateInternal(CollectionAchievementTable.CollectionAchievementTableItemEntry entry)
		{
			FormattedLabelBuilder formattedLabelBuilder = new FormattedLabelBuilder();
			FormattedLabelPartBuilder partBuilder = formattedLabelBuilder.CreatePart(entry.Name);
			if (!string.IsNullOrEmpty(entry.ImageUrl))
			{
				partBuilder.SetPrefixImage(externalImageService.GetImageFromIndirectLink(entry.ImageUrl));
			}
			return (Control)(object)formattedLabelBuilder.CreatePart(partBuilder).Build();
		}
	}
}
