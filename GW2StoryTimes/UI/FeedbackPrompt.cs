using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using GW2StoryTimes.Models;
using GW2StoryTimes.Services;
using Microsoft.Xna.Framework;

namespace GW2StoryTimes.UI
{
	public class FeedbackPrompt : Panel
	{
		private static readonly Logger Logger = Logger.GetLogger<FeedbackPrompt>();

		private readonly Mission _mission;

		private readonly TimeSpan _elapsed;

		private readonly TimeEstimate _estimate;

		private bool _isSubmitting;

		public FeedbackPrompt(Mission mission, TimeSpan elapsed, TimeEstimate estimate)
		{
			_mission = mission;
			_elapsed = elapsed;
			_estimate = estimate;
			base.Width = 420;
			base.Height = 180;
			base.ShowBorder = true;
			base.BackgroundColor = new Color(0, 0, 0, 200);
			base.Location = new Point((GameService.Graphics.SpriteScreen.Width - 420) / 2, (GameService.Graphics.SpriteScreen.Height - 180) / 2);
			BuildLayout();
		}

		private void BuildLayout()
		{
			string elapsedFormatted = FormatTimeSpan(_elapsed);
			new Label
			{
				Text = "Mission Complete!",
				Font = GameService.Content.DefaultFont18,
				TextColor = new Color(255, 200, 50),
				ShowShadow = true,
				AutoSizeHeight = true,
				AutoSizeWidth = true,
				Location = new Point(20, 12),
				Parent = this
			};
			new Label
			{
				Text = _mission.Name,
				Font = GameService.Content.DefaultFont16,
				TextColor = Color.White,
				ShowShadow = true,
				AutoSizeHeight = true,
				AutoSizeWidth = true,
				Location = new Point(20, 38),
				Parent = this
			};
			Color timeColor = Color.White;
			string comparisonText = "";
			if (_estimate?.AvgMins.HasValue ?? false)
			{
				double diff = _elapsed.TotalMinutes - _estimate.AvgMins.Value;
				if (diff < -2.0)
				{
					timeColor = new Color(100, 220, 100);
					comparisonText = $"  ({Math.Abs(diff):F0} min faster than estimate)";
				}
				else if (diff > 2.0)
				{
					timeColor = new Color(240, 80, 80);
					comparisonText = $"  ({diff:F0} min slower than estimate)";
				}
				else
				{
					timeColor = new Color(100, 220, 100);
					comparisonText = "  (right on target!)";
				}
			}
			new Label
			{
				Text = "Your time: " + elapsedFormatted + comparisonText,
				Font = GameService.Content.DefaultFont14,
				TextColor = timeColor,
				ShowShadow = true,
				AutoSizeHeight = true,
				AutoSizeWidth = true,
				Location = new Point(20, 65),
				Parent = this
			};
			StandardButton standardButton = new StandardButton();
			standardButton.Text = "Submit as Full Experience";
			standardButton.Width = 200;
			standardButton.Height = 30;
			standardButton.Location = new Point(20, 100);
			standardButton.Parent = this;
			standardButton.Click += delegate
			{
				Task.Run(() => Submit("full"));
			};
			StandardButton standardButton2 = new StandardButton();
			standardButton2.Text = "Submit as Speedrun";
			standardButton2.Width = 160;
			standardButton2.Height = 30;
			standardButton2.Location = new Point(230, 100);
			standardButton2.Parent = this;
			standardButton2.Click += delegate
			{
				Task.Run(() => Submit("speed"));
			};
			StandardButton standardButton3 = new StandardButton();
			standardButton3.Text = "Dismiss";
			standardButton3.Width = 100;
			standardButton3.Height = 28;
			standardButton3.Location = new Point(300, 140);
			standardButton3.Parent = this;
			standardButton3.Click += delegate
			{
				Dispose();
			};
		}

		private async Task Submit(string category)
		{
			if (_isSubmitting)
			{
				return;
			}
			_isSubmitting = true;
			double durationMins = _elapsed.TotalMinutes;
			StoryTimesApiClient apiClient = GW2StoryTimesModule.Instance?.ApiClient;
			if (apiClient != null)
			{
				if (await apiClient.SubmitTimeAsync(_mission.Id, category, durationMins))
				{
					ScreenNotification.ShowNotification("Story Times: Time submitted for " + _mission.Name + "!");
				}
				else
				{
					ScreenNotification.ShowNotification("Story Times: Submission failed. Try again later.", ScreenNotification.NotificationType.Warning);
				}
				Dispose();
			}
		}

		private static string FormatTimeSpan(TimeSpan ts)
		{
			if (!(ts.TotalHours >= 1.0))
			{
				return $"{ts.Minutes:D2}:{ts.Seconds:D2}";
			}
			return $"{(int)ts.TotalHours}:{ts.Minutes:D2}:{ts.Seconds:D2}";
		}
	}
}
