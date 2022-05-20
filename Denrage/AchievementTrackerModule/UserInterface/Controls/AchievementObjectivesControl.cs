using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls
{
	public class AchievementObjectivesControl : AchievementListControl<ObjectivesDescription, TableDescriptionEntry>
	{
		public AchievementObjectivesControl(IItemDetailWindowManager itemDetailWindowManager, IAchievementService achievementService, IFormattedLabelHtmlService formattedLabelHtmlService, ContentsManager contentsManager, AchievementTableEntry achievement, ObjectivesDescription description)
			: base(itemDetailWindowManager, achievementService, formattedLabelHtmlService, contentsManager, achievement, description)
		{
		}

		protected override void ColorControl(Control control, bool achievementBitFinished)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			control.set_BackgroundColor(achievementBitFinished ? Color.FromNonPremultiplied(144, 238, 144, 50) : Color.get_Transparent());
		}

		protected override Control CreateEntryControl(int index, TableDescriptionEntry entry, Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent(parent);
			((Control)val).set_Width(32);
			((Control)val).set_Height(32);
			val.set_Text((index + 1).ToString());
			val.set_Font(Control.get_Content().get_DefaultFont18());
			val.set_VerticalAlignment((VerticalAlignment)1);
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val).set_ZIndex(1);
			return (Control)val;
		}

		protected override string GetDisplayName(TableDescriptionEntry entry)
		{
			return entry.DisplayName;
		}

		protected override IEnumerable<TableDescriptionEntry> GetEntries(ObjectivesDescription description)
		{
			return description.EntryList;
		}
	}
}
