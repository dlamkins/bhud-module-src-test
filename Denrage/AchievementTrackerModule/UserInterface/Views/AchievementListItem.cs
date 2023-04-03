using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Denrage.AchievementTrackerModule.UserInterface.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Denrage.AchievementTrackerModule.UserInterface.Views
{
	public class AchievementListItem : View
	{
		private readonly IAchievementTrackerService achievementTrackerService;

		private readonly IAchievementService achievementService;

		private readonly ContentService contentService;

		private readonly ITextureService textureService;

		private readonly string icon;

		private readonly AchievementTableEntry achievement;

		private AchievementButton button;

		private GlowButton trackButton;

		public AchievementListItem(AchievementTableEntry achievement, IAchievementTrackerService achievementTrackerService, IAchievementService achievementService, ContentService contentService, ITextureService textureService, string icon)
			: this()
		{
			this.achievement = achievement;
			this.achievementTrackerService = achievementTrackerService;
			this.achievementService = achievementService;
			this.contentService = contentService;
			this.textureService = textureService;
			this.icon = icon;
		}

		protected override void Build(Container buildPanel)
		{
			achievementTrackerService.AchievementUntracked += Tracker_AchievementUntracked;
			((Control)buildPanel).set_Height(112);
			((Control)buildPanel).set_Width(((Control)buildPanel).get_Width() - 10);
			AchievementButton achievementButton = new AchievementButton();
			((DetailsButton)achievementButton).set_Text(achievement.Name.Trim());
			((Control)achievementButton).set_Parent(buildPanel);
			((DetailsButton)achievementButton).set_Icon(textureService.GetTexture(icon));
			((Container)achievementButton).set_HeightSizingMode((SizingMode)2);
			((Container)achievementButton).set_WidthSizingMode((SizingMode)2);
			button = achievementButton;
			if (achievementService.HasFinishedAchievement(achievement.Id))
			{
				BuildCompleteButton(button);
				return;
			}
			((Control)buildPanel).add_Click((EventHandler<MouseEventArgs>)BuildPanel_Click);
			BuildInCompleteButton(button);
		}

		public void BuildCompleteButton(AchievementButton button)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			if (button != null)
			{
				button.CompleteText = achievement.Name.Trim();
				((DetailsButton)button).set_Text((string)null);
				button.Description = "Complete";
				((DetailsButton)button).set_BottomSectionHeight(0);
				((Control)button).set_BackgroundColor(Color.FromNonPremultiplied(144, 238, 144, 50));
			}
		}

		public void BuildInCompleteButton(AchievementButton button)
		{
			if (button != null)
			{
				((DetailsButton)button).set_ShowToggleButton(true);
				trackButton = BuildTrackingButton(button);
			}
		}

		public GlowButton BuildTrackingButton(AchievementButton parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			GlowButton val = new GlowButton();
			val.set_ActiveIcon(textureService.GetRefTexture("track_enabled.png"));
			val.set_Icon(textureService.GetRefTexture("track_disabled.png"));
			((Control)val).set_BasicTooltipText("Track achievement");
			((Control)val).set_Parent((Container)(object)parent);
			val.set_ToggleGlow(true);
			val.set_Checked(achievementTrackerService.IsBeingTracked(achievement.Id));
			return val;
		}

		private void Tracker_AchievementUntracked(int achievementId)
		{
			if (achievement.Id == achievementId)
			{
				trackButton.set_Checked(false);
			}
		}

		private void BuildPanel_Click(object sender, MouseEventArgs e)
		{
			if (achievementTrackerService.IsBeingTracked(achievement.Id))
			{
				achievementTrackerService.RemoveAchievement(achievement.Id);
				trackButton.set_Checked(false);
			}
			else if (achievementTrackerService.TrackAchievement(achievement.Id))
			{
				trackButton.set_Checked(true);
			}
			else
			{
				trackButton.set_Checked(false);
				ScreenNotification.ShowNotification("You can have a maximum of 15 achievements tracked concurrently.\n Untrack one to add a new one.", (NotificationType)0, (Texture2D)null, 4);
			}
		}
	}
}
