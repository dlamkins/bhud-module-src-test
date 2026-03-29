using Blish_HUD;
using Blish_HUD.Controls;
using GW2StoryTimes.Models;
using Microsoft.Xna.Framework;

namespace GW2StoryTimes.UI
{
	public class NextMissionPrompt : Panel
	{
		private readonly Mission _nextMission;

		public NextMissionPrompt(Mission nextMission)
		{
			_nextMission = nextMission;
			base.Width = 420;
			base.Height = 130;
			base.ShowBorder = true;
			base.BackgroundColor = new Color(0, 0, 0, 200);
			base.Location = new Point((GameService.Graphics.SpriteScreen.Width - 420) / 2, (GameService.Graphics.SpriteScreen.Height - 130) / 2);
			BuildLayout();
		}

		private void BuildLayout()
		{
			new Label
			{
				Text = "Continue to the next mission?",
				Font = GameService.Content.DefaultFont16,
				TextColor = new Color(255, 200, 50),
				ShowShadow = true,
				AutoSizeHeight = true,
				Width = 380,
				Location = new Point(20, 12),
				Parent = this
			};
			new Label
			{
				Text = (_nextMission.Breadcrumb ?? ""),
				Font = GameService.Content.DefaultFont12,
				TextColor = Color.LightGray,
				AutoSizeHeight = true,
				Width = 380,
				Location = new Point(20, 38),
				Parent = this
			};
			new Label
			{
				Text = _nextMission.Name,
				Font = GameService.Content.DefaultFont14,
				TextColor = Color.White,
				ShowShadow = true,
				AutoSizeHeight = true,
				Width = 380,
				Location = new Point(20, 56),
				Parent = this
			};
			StandardButton standardButton = new StandardButton();
			standardButton.Text = "Start Next Mission";
			standardButton.Width = 160;
			standardButton.Height = 30;
			standardButton.Location = new Point(20, 88);
			standardButton.Parent = this;
			standardButton.Click += delegate
			{
				GW2StoryTimesModule instance = GW2StoryTimesModule.Instance;
				if (instance != null)
				{
					instance.ActiveMission = _nextMission;
					instance.TimerService?.Reset();
				}
				Dispose();
			};
			StandardButton standardButton2 = new StandardButton();
			standardButton2.Text = "No Thanks";
			standardButton2.Width = 100;
			standardButton2.Height = 28;
			standardButton2.Location = new Point(300, 90);
			standardButton2.Parent = this;
			standardButton2.Click += delegate
			{
				Dispose();
			};
		}
	}
}
