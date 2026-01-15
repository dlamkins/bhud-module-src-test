using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services.Playback;
using Microsoft.Xna.Framework;

namespace Maestro.UI.Components
{
	public class NowPlayingPanel : Panel
	{
		public static class Layout
		{
			public const int Height = 95;

			public const int ButtonY = 18;

			public const int ButtonWidth = 40;

			public const int PauseButtonX = 8;

			public const int StopButtonX = 52;

			public const int LabelX = 100;

			public const int LabelYPlaying = 8;

			public const int LabelYCentered = 21;

			public const int ProgressLabelY = 28;

			public const int LabelWidth = 300;

			public const int SpeedLabelX = 8;

			public const int SpeedLabelY = 52;

			public const int SpeedSliderX = 60;

			public const int SpeedSliderY = 55;

			public const int SpeedSliderWidth = 180;

			public const int SpeedValueX = 248;
		}

		private readonly SongPlayer _songPlayer;

		private readonly StandardButton _pauseButton;

		private readonly StandardButton _stopButton;

		private readonly Label _nowPlayingLabel;

		private readonly Label _progressLabel;

		private readonly Label _speedLabel;

		private readonly TrackBar _speedSlider;

		private readonly Label _speedValueLabel;

		public NowPlayingPanel(SongPlayer songPlayer, int width)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			_songPlayer = songPlayer;
			base.Size = new Point(width, 95);
			base.BackgroundColor = MaestroTheme.WithAlpha(MaestroTheme.SlateGray, 150);
			base.ShowBorder = true;
			_pauseButton = new StandardButton
			{
				Parent = this,
				Text = "||",
				Location = new Point(8, 18),
				Width = 40,
				Enabled = false
			};
			_pauseButton.Click += OnPauseClicked;
			_stopButton = new StandardButton
			{
				Parent = this,
				Text = "X",
				Location = new Point(52, 18),
				Width = 40,
				Enabled = false
			};
			_stopButton.Click += OnStopClicked;
			_nowPlayingLabel = new Label
			{
				Parent = this,
				Text = "No song playing",
				Location = new Point(100, 21),
				Width = 300,
				Font = GameService.Content.DefaultFont14,
				TextColor = MaestroTheme.MutedCream
			};
			_progressLabel = new Label
			{
				Parent = this,
				Text = "",
				Location = new Point(100, 28),
				Width = 300,
				Font = GameService.Content.DefaultFont12,
				TextColor = MaestroTheme.MutedCream
			};
			_speedLabel = new Label
			{
				Parent = this,
				Text = "Speed:",
				Location = new Point(8, 52),
				AutoSizeWidth = true,
				Font = GameService.Content.DefaultFont12,
				TextColor = MaestroTheme.MutedCream
			};
			_speedSlider = new TrackBar
			{
				Parent = this,
				Location = new Point(60, 55),
				Width = 180,
				MinValue = 1f,
				MaxValue = 20f,
				Value = 10f,
				SmallStep = true
			};
			_speedSlider.ValueChanged += OnSpeedSliderChanged;
			_speedValueLabel = new Label
			{
				Parent = this,
				Text = "1.0x",
				Location = new Point(248, 52),
				AutoSizeWidth = true,
				Font = GameService.Content.DefaultFont12,
				TextColor = MaestroTheme.CreamWhite
			};
			SubscribeToEvents();
		}

		private void SubscribeToEvents()
		{
			_songPlayer.OnStarted += OnPlaybackStateChanged;
			_songPlayer.OnPaused += OnPlaybackStateChanged;
			_songPlayer.OnResumed += OnPlaybackStateChanged;
			_songPlayer.OnStopped += OnPlaybackStateChanged;
			_songPlayer.OnCompleted += OnPlaybackStateChanged;
		}

		private void OnPlaybackStateChanged(object sender, EventArgs e)
		{
			UpdatePlaybackState();
		}

		private void OnPauseClicked(object sender, MouseEventArgs e)
		{
			if (SongFilterBar.WasJustUnfocused)
			{
				SongFilterBar.WasJustUnfocused = false;
			}
			else
			{
				_songPlayer.TogglePause();
			}
		}

		private void OnStopClicked(object sender, MouseEventArgs e)
		{
			ClearTextInputFocus();
			_songPlayer.Stop();
		}

		private static void ClearTextInputFocus()
		{
			TextInputBase textInput = Control.FocusedControl as TextInputBase;
			if (textInput != null)
			{
				textInput.Focused = false;
			}
		}

		private void OnSpeedSliderChanged(object sender, ValueEventArgs<float> e)
		{
			float speed = e.Value / 10f;
			_songPlayer.PlaybackSpeed = speed;
			_speedValueLabel.Text = $"{speed:F1}x";
		}

		public void UpdatePlaybackState()
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			if (_songPlayer.IsPlaying)
			{
				Song song2 = _songPlayer.CurrentSong;
				_nowPlayingLabel.Text = song2?.DisplayName ?? "Unknown";
				_nowPlayingLabel.Location = new Point(100, 8);
				if (_songPlayer.IsPaused)
				{
					_pauseButton.Text = ">";
					_nowPlayingLabel.TextColor = MaestroTheme.CreamWhite;
					_progressLabel.Text = "Paused";
					_progressLabel.TextColor = MaestroTheme.Paused;
				}
				else
				{
					_pauseButton.Text = "||";
					_nowPlayingLabel.TextColor = MaestroTheme.CreamWhite;
					if (_songPlayer.IsAdjustingOctave)
					{
						_progressLabel.Text = "Adjusting...";
						_progressLabel.TextColor = MaestroTheme.Paused;
					}
					else
					{
						if (song2 != null && _songPlayer.CurrentCommandIndex >= song2.Commands.Count)
						{
							_progressLabel.Text = "Done!";
						}
						else
						{
							float progress = ((song2 != null && song2.Commands.Count > 0) ? ((float)_songPlayer.CurrentCommandIndex / (float)song2.Commands.Count * 100f) : 0f);
							_progressLabel.Text = $"Playing... {progress:F0}%";
						}
						_progressLabel.TextColor = MaestroTheme.MutedCream;
					}
				}
				_pauseButton.Enabled = true;
				_stopButton.Enabled = true;
			}
			else
			{
				Song song = _songPlayer.CurrentSong;
				if (song != null && _songPlayer.CurrentCommandIndex >= song.Commands.Count)
				{
					_nowPlayingLabel.Text = song.DisplayName;
					_nowPlayingLabel.Location = new Point(100, 8);
					_nowPlayingLabel.TextColor = MaestroTheme.CreamWhite;
					_progressLabel.Text = "Done!";
					_progressLabel.TextColor = MaestroTheme.MutedCream;
				}
				else
				{
					_nowPlayingLabel.Text = "No song playing";
					_nowPlayingLabel.Location = new Point(100, 21);
					_nowPlayingLabel.TextColor = MaestroTheme.MutedCream;
					_progressLabel.Text = "";
				}
				_pauseButton.Text = "||";
				_pauseButton.Enabled = false;
				_stopButton.Enabled = false;
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateContainer(gameTime);
			if (!_songPlayer.IsPlaying || _songPlayer.IsPaused)
			{
				return;
			}
			if (_songPlayer.IsWaitingForInput)
			{
				_pauseButton.Text = ">";
				_progressLabel.Text = "Paused";
				_progressLabel.TextColor = MaestroTheme.Paused;
				return;
			}
			if (_songPlayer.IsAdjustingOctave)
			{
				_progressLabel.Text = "Adjusting...";
				_progressLabel.TextColor = MaestroTheme.Paused;
				return;
			}
			_pauseButton.Text = "||";
			Song song = _songPlayer.CurrentSong;
			if (song != null && _songPlayer.CurrentCommandIndex >= song.Commands.Count)
			{
				_progressLabel.Text = "Done!";
			}
			else
			{
				float progress = ((song != null && song.Commands.Count > 0) ? ((float)_songPlayer.CurrentCommandIndex / (float)song.Commands.Count * 100f) : 0f);
				_progressLabel.Text = $"Playing... {progress:F0}%";
			}
			_progressLabel.TextColor = MaestroTheme.MutedCream;
		}

		protected override void DisposeControl()
		{
			_songPlayer.OnStarted -= OnPlaybackStateChanged;
			_songPlayer.OnPaused -= OnPlaybackStateChanged;
			_songPlayer.OnResumed -= OnPlaybackStateChanged;
			_songPlayer.OnStopped -= OnPlaybackStateChanged;
			_songPlayer.OnCompleted -= OnPlaybackStateChanged;
			_pauseButton?.Dispose();
			_stopButton?.Dispose();
			_nowPlayingLabel?.Dispose();
			_progressLabel?.Dispose();
			_speedLabel?.Dispose();
			_speedSlider?.Dispose();
			_speedValueLabel?.Dispose();
			base.DisposeControl();
		}
	}
}
