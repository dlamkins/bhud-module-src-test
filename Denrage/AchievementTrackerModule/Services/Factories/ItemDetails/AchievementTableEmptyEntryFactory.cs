using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Models.Achievement;

namespace Denrage.AchievementTrackerModule.Services.Factories.ItemDetails
{
	public class AchievementTableEmptyEntryFactory : AchievementTableEntryFactory<CollectionAchievementTable.CollectionAchievementTableEmptyEntry>
	{
		protected override Control CreateInternal(CollectionAchievementTable.CollectionAchievementTableEmptyEntry entry)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text("EMPTY");
			val.set_AutoSizeHeight(true);
			val.set_WrapText(true);
			return (Control)val;
		}
	}
}
