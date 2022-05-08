using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Models.Achievement;

namespace Denrage.AchievementTrackerModule.Services.Factories.ItemDetails
{
	public class AchievementTableLinkEntryFactory : AchievementTableEntryFactory<CollectionAchievementTable.CollectionAchievementTableLinkEntry>
	{
		protected override Control CreateInternal(CollectionAchievementTable.CollectionAchievementTableLinkEntry entry)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text(entry?.Text ?? string.Empty);
			val.set_AutoSizeHeight(true);
			val.set_WrapText(true);
			return (Control)val;
		}
	}
}
