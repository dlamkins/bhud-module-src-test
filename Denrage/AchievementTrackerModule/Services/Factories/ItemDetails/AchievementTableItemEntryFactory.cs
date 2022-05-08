using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Models.Achievement;

namespace Denrage.AchievementTrackerModule.Services.Factories.ItemDetails
{
	public class AchievementTableItemEntryFactory : AchievementTableEntryFactory<CollectionAchievementTable.CollectionAchievementTableItemEntry>
	{
		protected override Control CreateInternal(CollectionAchievementTable.CollectionAchievementTableItemEntry entry)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text(StringUtils.SanitizeHtml(entry?.Name ?? string.Empty));
			val.set_AutoSizeHeight(true);
			val.set_WrapText(true);
			return (Control)val;
		}
	}
}
