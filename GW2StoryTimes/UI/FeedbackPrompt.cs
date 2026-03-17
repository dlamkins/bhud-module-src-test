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

		private readonly SubmissionCategory _preferredCategory;

		private bool _isSubmitting;

		public FeedbackPrompt(Mission mission, TimeSpan elapsed, TimeEstimate estimate, SubmissionCategory preferredCategory)
		{
			_mission = mission;
			_elapsed = elapsed;
			_estimate = estimate;
			_preferredCategory = preferredCategory;
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
			bool preferFull = _preferredCategory == SubmissionCategory.Full;
			StandardButton standardButton = new StandardButton();
			standardButton.Text = (preferFull ? "Submit as Full Experience" : "Submit as Speedrun");
			standardButton.Width = 220;
			standardButton.Height = 32;
			standardButton.Location = new Point(20, 98);
			standardButton.Parent = this;
			standardButton.Click += delegate
			{
				Task.Run(() => Submit(preferFull ? "full" : "speed"));
			};
			StandardButton standardButton2 = new StandardButton();
			standardButton2.Text = (preferFull ? "Submit as Speedrun" : "Submit as Full Experience");
			standardButton2.Width = 150;
			standardButton2.Height = 28;
			standardButton2.Location = new Point(248, 100);
			standardButton2.Parent = this;
			standardButton2.Click += delegate
			{
				Task.Run(() => Submit(preferFull ? "speed" : "full"));
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
				StoryTimesApiClient.SubmitResult result = await apiClient.SubmitTimeAsync(_mission.Id, category, durationMins);
				if (result.Success)
				{
					ScreenNotification.ShowNotification("Story Times: Time submitted for " + _mission.Name + "!");
				}
				else
				{
					ScreenNotification.ShowNotification("Story Times: " + result.Error, ScreenNotification.NotificationType.Warning);
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
