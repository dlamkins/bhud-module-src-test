using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using GW2StoryTimes.Models;
using GW2StoryTimes.Services;
using Microsoft.Xna.Framework;

namespace GW2StoryTimes.UI
{
	public class StoryTimesWidget : Panel
	{
		private static readonly Color ColorOnPace = new Color(100, 220, 100);

		private static readonly Color ColorApproaching = new Color(255, 200, 50);

		private static readonly Color ColorOvertime = new Color(240, 80, 80);

		private Label _breadcrumbLabel;

		private Label _missionNameLabel;

		private Label _estimateLabel;

		private Label _timerLabel;

		private Label _statusLabel;

		private StandardButton _toggleButton;

		private StandardButton _resetButton;

		private StandardButton _submitButton;

		private StandardButton _menuButton;

		private StandardButton _clearButton;

		private Mission _displayedMission;

		private bool _dragging;

		private Point _dragOffset;

		private bool _positioned;

		public StoryTimesWidget()
		{
			base.Width = 360;
			base.Height = 135;
			base.BackgroundColor = new Color(0, 0, 0, 180);
			base.ShowBorder = true;
			BuildLayout();
		}

		private void BuildLayout()
		{
			_menuButton = new StandardButton
			{
				Text = "...",
				Width = 30,
				Height = 22,
				Location = new Point(4, 4),
				BasicTooltipText = "Browse Missions",
				Parent = this
			};
			_menuButton.Click += delegate
			{
				GW2StoryTimesModule.Instance?.OpenMissionSelector();
			};
			_clearButton = new StandardButton
			{
				Text = "X",
				Width = 26,
				Height = 22,
				Location = new Point(326, 4),
				BasicTooltipText = "Clear Mission",
				Visible = false,
				Parent = this
			};
			_clearButton.Click += delegate
			{
				ClearMission();
			};
			_breadcrumbLabel = new Label
			{
				Text = "No mission selected",
				Font = GameService.Content.DefaultFont12,
				TextColor = Color.LightGray,
				AutoSizeHeight = true,
				Width = 310,
				Location = new Point(40, 6),
				Parent = this
			};
			_breadcrumbLabel.Click += OnMissionAreaClicked;
			_missionNameLabel = new Label
			{
				Text = "(click to browse)",
				Font = GameService.Content.DefaultFont16,
				TextColor = Color.White,
				ShowShadow = true,
				AutoSizeHeight = true,
				Width = 230,
				Location = new Point(40, 24),
				Parent = this
			};
			_missionNameLabel.Click += OnMissionAreaClicked;
			_estimateLabel = new Label
			{
				Text = "",
				Font = GameService.Content.DefaultFont14,
				TextColor = ColorApproaching,
				ShowShadow = true,
				AutoSizeHeight = true,
				AutoSizeWidth = true,
				HorizontalAlignment = HorizontalAlignment.Right,
				Location = new Point(270, 26),
				Parent = this
			};
			_timerLabel = new Label
			{
				Text = "00:00",
				Font = GameService.Content.DefaultFont32,
				TextColor = Color.White,
				ShowShadow = true,
				AutoSizeHeight = true,
				AutoSizeWidth = true,
				Location = new Point(8, 52),
				Parent = this
			};
			_toggleButton = new StandardButton
			{
				Text = "Start",
				Width = 60,
				Height = 26,
				Location = new Point(155, 58),
				Parent = this
			};
			_toggleButton.Click += delegate
			{
				GW2StoryTimesModule.Instance?.TimerService?.Toggle();
			};
			_resetButton = new StandardButton
			{
				Text = "Reset",
				Width = 60,
				Height = 26,
				Location = new Point(220, 58),
				Parent = this
			};
			_resetButton.Click += delegate
			{
				GW2StoryTimesModule.Instance?.TimerService?.Reset();
			};
			_submitButton = new StandardButton
			{
				Text = "Submit",
				Width = 65,
				Height = 26,
				Location = new Point(285, 58),
				Visible = false,
				Parent = this
			};
			_submitButton.Click += delegate
			{
				OnSubmitClicked();
			};
			_statusLabel = new Label
			{
				Text = "",
				Font = GameService.Content.DefaultFont12,
				TextColor = Color.DarkGray,
				AutoSizeHeight = true,
				Width = 340,
				Location = new Point(8, 88),
				Parent = this
			};
		}

		public void UpdateMission(Mission mission)
		{
			_displayedMission = mission;
			_clearButton.Visible = mission != null;
			if (mission == null)
			{
				_breadcrumbLabel.Text = "No mission selected";
				_missionNameLabel.Text = "(click to browse)";
				_estimateLabel.Text = "";
			}
			else
			{
				_breadcrumbLabel.Text = mission.Breadcrumb ?? "";
				_missionNameLabel.Text = mission.Name;
				TimeEstimate estimate = (((GW2StoryTimesModule.Instance?.SettingPreferredCategory?.Value).GetValueOrDefault() != SubmissionCategory.Speed) ? mission.Times?.Full : mission.Times?.Speed);
				_estimateLabel.Text = ((estimate != null && estimate.FormattedEstimate != null) ? ("~" + estimate.FormattedEstimate) : "");
				_estimateLabel.BasicTooltipText = ((estimate != null && estimate.FormattedRange != null) ? ("Range: " + estimate.FormattedRange) : null);
			}
		}

		private void ClearMission()
		{
			GW2StoryTimesModule module = GW2StoryTimesModule.Instance;
			if (module != null)
			{
				module.ActiveMission = null;
				module.TimerService?.Reset();
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (!_positioned)
			{
				ApplyInitialPosition();
			}
			if (_dragging)
			{
				Point mousePos = GameService.Input.Mouse.Position;
				base.Location = new Point(mousePos.X - _dragOffset.X, mousePos.Y - _dragOffset.Y);
			}
			GW2StoryTimesModule module = GW2StoryTimesModule.Instance;
			TimerService timer = module?.TimerService;
			if (timer != null)
			{
				if (_displayedMission != module.ActiveMission)
				{
					UpdateMission(module.ActiveMission);
				}
				_timerLabel.Text = timer.FormattedElapsed;
				_toggleButton.Text = (timer.IsRunning ? "Pause" : "Start");
				bool hasTime = !timer.IsRunning && timer.Elapsed.TotalSeconds >= 30.0;
				bool hasMission = module.ActiveMission != null;
				_submitButton.Visible = hasTime && hasMission;
				UpdateTimerColor(timer, module);
			}
		}

		private void ApplyInitialPosition()
		{
			Screen screen = GameService.Graphics.SpriteScreen;
			if (screen.Width >= 100 && screen.Height >= 100)
			{
				_positioned = true;
				GW2StoryTimesModule instance = GW2StoryTimesModule.Instance;
				int sx = (instance?.SettingWidgetX?.Value).GetValueOrDefault(-1);
				int sy = (instance?.SettingWidgetY?.Value).GetValueOrDefault(-1);
				if (sx >= 0 && sy >= 0 && sx < screen.Width && sy < screen.Height)
				{
					base.Location = new Point(sx, sy);
				}
				else
				{
					base.Location = new Point((screen.Width - base.Width) / 2, (screen.Height - base.Height) / 2);
				}
			}
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			base.OnLeftMouseButtonPressed(e);
			Point relPos = GameService.Input.Mouse.Position - base.Location;
			if (relPos.Y < 50)
			{
				_dragging = true;
				_dragOffset = relPos;
			}
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			base.OnLeftMouseButtonReleased(e);
			if (_dragging)
			{
				_dragging = false;
				SavePosition();
			}
		}

		private void SavePosition()
		{
			GW2StoryTimesModule module = GW2StoryTimesModule.Instance;
			if (module != null)
			{
				module.SettingWidgetX.Value = base.Location.X;
				module.SettingWidgetY.Value = base.Location.Y;
			}
		}

		private void UpdateTimerColor(TimerService timer, GW2StoryTimesModule module)
		{
			Mission mission = module.ActiveMission;
			if (mission == null)
			{
				_timerLabel.TextColor = Color.White;
				_statusLabel.Text = (timer.IsRunning ? "Timer running" : "");
				return;
			}
			if (!timer.IsRunning && timer.Elapsed.TotalSeconds < 1.0)
			{
				_timerLabel.TextColor = Color.White;
				_statusLabel.Text = "";
				return;
			}
			TimeEstimate estimate = (((module.SettingPreferredCategory?.Value ?? SubmissionCategory.Full) != SubmissionCategory.Speed) ? mission.Times?.Full : mission.Times?.Speed);
			if (estimate == null || !estimate.AvgMins.HasValue)
			{
				_timerLabel.TextColor = Color.White;
				_statusLabel.Text = (timer.IsRunning ? "No estimate to compare" : "");
				return;
			}
			double estimateMins = estimate.AvgMins.Value;
			double elapsedMins = timer.Elapsed.TotalMinutes;
			double ratio = elapsedMins / estimateMins;
			if (ratio >= 0.9 && ratio <= 1.1)
			{
				_timerLabel.TextColor = ColorOnPace;
				_statusLabel.Text = "On target (est. ~" + estimate.FormattedEstimate + ")";
			}
			else if (ratio >= 0.75 && ratio <= 1.25)
			{
				_timerLabel.TextColor = ColorApproaching;
				string direction2 = ((ratio < 1.0) ? "ahead of" : "behind");
				_statusLabel.Text = "Slightly " + direction2 + " estimate (~" + estimate.FormattedEstimate + ")";
			}
			else
			{
				_timerLabel.TextColor = ColorOvertime;
				string direction = ((ratio < 1.0) ? $"{estimateMins - elapsedMins:F0} min ahead" : $"{elapsedMins - estimateMins:F0} min behind");
				_statusLabel.Text = "Well outside estimate (" + direction + ")";
			}
		}

		private void OnMissionAreaClicked(object sender, MouseEventArgs e)
		{
			if (_displayedMission == null)
			{
				GW2StoryTimesModule.Instance?.OpenMissionSelector();
			}
		}

		private void OnSubmitClicked()
		{
			GW2StoryTimesModule module = GW2StoryTimesModule.Instance;
			Mission mission = module?.ActiveMission;
			TimerService timer = module?.TimerService;
			if (mission != null && timer != null)
			{
				_submitButton.Enabled = false;
				module.ShowFeedbackPrompt(mission, timer.Elapsed);
			}
		}

		internal void ReenableSubmit()
		{
			_submitButton.Enabled = true;
		}
	}
}
