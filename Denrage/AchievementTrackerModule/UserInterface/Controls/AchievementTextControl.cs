using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Achievement;
using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls
{
	public class AchievementTextControl : FlowPanel, IAchievementControl
	{
		private readonly AchievementTableEntry achievement;

		private readonly StringDescription description;

		private Label gameTextLabel;

		private Label gameHintLabel;

		public AchievementTextControl(AchievementTableEntry achievement, StringDescription description)
			: this()
		{
			this.achievement = achievement;
			this.description = description;
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)1);
		}

		public void BuildControl()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			if (!string.IsNullOrEmpty(description.GameText))
			{
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)this);
				val.set_Text(StringUtils.SanitizeHtml(description.GameText));
				val.set_AutoSizeHeight(true);
				((Control)val).set_Width(((Container)this).get_ContentRegion().Width);
				val.set_WrapText(true);
				gameTextLabel = val;
			}
			if (!string.IsNullOrEmpty(description.GameHint))
			{
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)this);
				((Control)val2).set_Width(((Container)this).get_ContentRegion().Width);
				val2.set_Text(StringUtils.SanitizeHtml(description.GameHint));
				val2.set_TextColor(Color.get_LightGray());
				val2.set_AutoSizeHeight(true);
				val2.set_WrapText(true);
				gameHintLabel = val2;
			}
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			if (gameTextLabel != null)
			{
				((Control)gameTextLabel).set_Width(((Container)this).get_ContentRegion().Width);
			}
			if (gameHintLabel != null)
			{
				((Control)gameHintLabel).set_Width(((Container)this).get_ContentRegion().Width);
			}
			((Container)this).OnResized(e);
		}

		Point IAchievementControl.get_Size()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).get_Size();
		}

		void IAchievementControl.set_Size(Point value)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(value);
		}
	}
}
