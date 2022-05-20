using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Libs.Achievement;

namespace Denrage.AchievementTrackerModule.Services.Factories.ItemDetails
{
	public class AchievementTableCoinEntryFactory : AchievementTableEntryFactory<CollectionAchievementTable.CollectionAchievementTableCoinEntry>
	{
		protected override Control CreateInternal(CollectionAchievementTable.CollectionAchievementTableCoinEntry entry)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text((entry?.ItemId.ToString() ?? string.Empty) + ": " + (entry?.Type.ToString() ?? string.Empty));
			val.set_AutoSizeHeight(true);
			val.set_WrapText(true);
			return (Control)val;
		}
	}
}
