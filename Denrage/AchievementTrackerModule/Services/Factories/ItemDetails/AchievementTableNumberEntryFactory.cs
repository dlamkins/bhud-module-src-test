using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Libs.Achievement;

namespace Denrage.AchievementTrackerModule.Services.Factories.ItemDetails
{
	public class AchievementTableNumberEntryFactory : AchievementTableEntryFactory<CollectionAchievementTable.CollectionAchievementTableNumberEntry>
	{
		protected override Control CreateInternal(CollectionAchievementTable.CollectionAchievementTableNumberEntry entry)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text(entry?.Number.ToString() ?? string.Empty);
			val.set_AutoSizeHeight(true);
			val.set_WrapText(true);
			return (Control)val;
		}
	}
}
