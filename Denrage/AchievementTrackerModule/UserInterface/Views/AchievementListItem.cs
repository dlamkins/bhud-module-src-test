using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Libs.Achievement;
using Microsoft.Xna.Framework;

namespace Denrage.AchievementTrackerModule.UserInterface.Views
{
	public class AchievementListItem : View
	{
		private readonly IAchievementTrackerService achievementTrackerService;

		private readonly IAchievementService achievementService;

		private readonly ContentService contentService;

		private readonly string icon;

		private readonly AchievementTableEntry achievement;

		private DetailsButton button;

		public AchievementListItem(AchievementTableEntry achievement, IAchievementTrackerService achievementTrackerService, IAchievementService achievementService, ContentService contentService, string icon)
			: this()
		{
			this.achievement = achievement;
			this.achievementTrackerService = achievementTrackerService;
			this.achievementService = achievementService;
			this.contentService = contentService;
			this.icon = icon;
			this.achievementService.PlayerAchievementsLoaded += delegate
			{
				ColorAchievement();
			};
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			((Control)buildPanel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				achievementTrackerService.TrackAchievement(achievement.Id);
			});
			DetailsButton val = new DetailsButton();
			val.set_Text(achievement.Name);
			((Control)val).set_Parent(buildPanel);
			val.set_ShowToggleButton(true);
			val.set_Icon(contentService.GetRenderServiceTexture(icon));
			button = val;
			ColorAchievement();
		}

		public void ColorAchievement()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			if (button != null && achievementService.HasFinishedAchievement(achievement.Id))
			{
				((Control)button).set_BackgroundColor(Color.FromNonPremultiplied(144, 238, 144, 50));
			}
		}
	}
}
