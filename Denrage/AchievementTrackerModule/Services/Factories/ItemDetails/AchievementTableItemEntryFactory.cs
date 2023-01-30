using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;

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
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			FormattedLabelBuilder val = new FormattedLabelBuilder();
			FormattedLabelPartBuilder partBuilder = val.CreatePart(entry.Name);
			if (!string.IsNullOrEmpty(entry.ImageUrl))
			{
				partBuilder.SetPrefixImage(externalImageService.GetImageFromIndirectLink(entry.ImageUrl));
			}
			return (Control)(object)val.CreatePart(partBuilder).Build();
		}
	}
}
