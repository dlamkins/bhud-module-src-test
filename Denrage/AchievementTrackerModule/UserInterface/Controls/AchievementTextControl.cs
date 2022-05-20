using Blish_HUD.Controls;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Denrage.AchievementTrackerModule.UserInterface.Controls.FormattedLabel;
using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls
{
	public class AchievementTextControl : FlowPanel, IAchievementControl
	{
		private readonly IFormattedLabelHtmlService formattedLabelHtmlService;

		private readonly AchievementTableEntry achievement;

		private readonly StringDescription description;

		private Denrage.AchievementTrackerModule.UserInterface.Controls.FormattedLabel.FormattedLabel gameTextLabel;

		private Denrage.AchievementTrackerModule.UserInterface.Controls.FormattedLabel.FormattedLabel gameHintLabel;

		public AchievementTextControl(IFormattedLabelHtmlService formattedLabelHtmlService, AchievementTableEntry achievement, StringDescription description)
			: this()
		{
			this.formattedLabelHtmlService = formattedLabelHtmlService;
			this.achievement = achievement;
			this.description = description;
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)1);
		}

		public void BuildControl()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			if (!string.IsNullOrEmpty(description.GameText))
			{
				FormattedLabelBuilder labelBuilder2 = formattedLabelHtmlService.CreateLabel(description.GameText).AutoSizeHeight().SetWidth(((Container)this).get_ContentRegion().Width)
					.Wrap();
				gameTextLabel = labelBuilder2.Build();
				((Control)gameTextLabel).set_Parent((Container)(object)this);
			}
			if (!string.IsNullOrEmpty(description.GameHint))
			{
				FormattedLabelBuilder labelBuilder = formattedLabelHtmlService.CreateLabel(description.GameHint).AutoSizeHeight().SetWidth(((Container)this).get_ContentRegion().Width)
					.Wrap();
				gameHintLabel = labelBuilder.Build();
				((Control)gameHintLabel).set_Parent((Container)(object)this);
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
