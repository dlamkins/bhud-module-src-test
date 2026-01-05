using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services;
using Microsoft.Xna.Framework;

namespace Maestro.UI.Components
{
	public class NowPlayingPanel : Panel
	{
		public static class Layout
		{
			public const int Height = 70;

			public const int ButtonY = 18;

			public const int ButtonWidth = 40;

			public const int PauseButtonX = 8;

			public const int StopButtonX = 52;

			public const int LabelX = 100;

			public const int LabelYPlaying = 8;

			public const int LabelYCentered = 21;

			public const int ProgressLabelY = 28;

			public const int LabelWidth = 300;
		}

		private readonly SongPlayer _songPlayer;

		private readonly StandardButton _pauseButton;

		private readonly StandardButton _stopButton;

		private readonly Label _nowPlayingLabel;

		private readonly Label _progressLabel;

		public NowPlayingPanel(SongPlayer songPlayer, int width)
			: this()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Expected O, but got Unknown
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Expected O, but got Unknown
			_songPlayer = songPlayer;
			((Control)this).set_Size(new Point(width, 70));
			((Control)this).set_BackgroundColor(MaestroColors.WithAlpha(MaestroColors.SlateGray, 150));
			((Panel)this).set_ShowBorder(true);
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("||");
			((Control)val).set_Location(new Point(8, 18));
			((Control)val).set_Width(40);
			((Control)val).set_Enabled(false);
			_pauseButton = val;
			((Control)_pauseButton).add_Click((EventHandler<MouseEventArgs>)OnPauseClicked);
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("X");
			((Control)val2).set_Location(new Point(52, 18));
			((Control)val2).set_Width(40);
			((Control)val2).set_Enabled(false);
			_stopButton = val2;
			((Control)_stopButton).add_Click((EventHandler<MouseEventArgs>)OnStopClicked);
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("No song playing");
			((Control)val3).set_Location(new Point(100, 21));
			((Control)val3).set_Width(300);
			val3.set_Font(GameService.Content.get_DefaultFont14());
			val3.set_TextColor(MaestroColors.MutedCream);
			_nowPlayingLabel = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("");
			((Control)val4).set_Location(new Point(100, 28));
			((Control)val4).set_Width(300);
			val4.set_Font(GameService.Content.get_DefaultFont12());
			val4.set_TextColor(MaestroColors.MutedCream);
			_progressLabel = val4;
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
			_songPlayer.TogglePause();
		}

		private void OnStopClicked(object sender, MouseEventArgs e)
		{
			_songPlayer.Stop();
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
				_nowPlayingLabel.set_Text(song2?.DisplayName ?? "Unknown");
				((Control)_nowPlayingLabel).set_Location(new Point(100, 8));
				if (_songPlayer.IsPaused)
				{
					_pauseButton.set_Text(">");
					_nowPlayingLabel.set_TextColor(MaestroColors.CreamWhite);
					_progressLabel.set_Text("Paused");
					_progressLabel.set_TextColor(MaestroColors.Paused);
				}
				else
				{
					_pauseButton.set_Text("||");
					_nowPlayingLabel.set_TextColor(MaestroColors.CreamWhite);
					if (_songPlayer.IsAdjustingOctave)
					{
						_progressLabel.set_Text("Adjusting...");
						_progressLabel.set_TextColor(MaestroColors.Paused);
					}
					else
					{
						if (song2 != null && _songPlayer.CurrentCommandIndex >= song2.Commands.Count)
						{
							_progressLabel.set_Text("Done!");
						}
						else
						{
							float progress = ((song2 != null && song2.Commands.Count > 0) ? ((float)_songPlayer.CurrentCommandIndex / (float)song2.Commands.Count * 100f) : 0f);
							_progressLabel.set_Text($"Playing... {progress:F0}%");
						}
						_progressLabel.set_TextColor(MaestroColors.MutedCream);
					}
				}
				((Control)_pauseButton).set_Enabled(true);
				((Control)_stopButton).set_Enabled(true);
			}
			else
			{
				Song song = _songPlayer.CurrentSong;
				if (song != null && _songPlayer.CurrentCommandIndex >= song.Commands.Count)
				{
					_nowPlayingLabel.set_Text(song.DisplayName);
					((Control)_nowPlayingLabel).set_Location(new Point(100, 8));
					_nowPlayingLabel.set_TextColor(MaestroColors.CreamWhite);
					_progressLabel.set_Text("Done!");
					_progressLabel.set_TextColor(MaestroColors.MutedCream);
				}
				else
				{
					_nowPlayingLabel.set_Text("No song playing");
					((Control)_nowPlayingLabel).set_Location(new Point(100, 21));
					_nowPlayingLabel.set_TextColor(MaestroColors.MutedCream);
					_progressLabel.set_Text("");
				}
				_pauseButton.set_Text("||");
				((Control)_pauseButton).set_Enabled(false);
				((Control)_stopButton).set_Enabled(false);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).UpdateContainer(gameTime);
			if (!_songPlayer.IsPlaying || _songPlayer.IsPaused)
			{
				return;
			}
			if (_songPlayer.IsAdjustingOctave)
			{
				_progressLabel.set_Text("Adjusting...");
				_progressLabel.set_TextColor(MaestroColors.Paused);
				return;
			}
			Song song = _songPlayer.CurrentSong;
			if (song != null && _songPlayer.CurrentCommandIndex >= song.Commands.Count)
			{
				_progressLabel.set_Text("Done!");
			}
			else
			{
				float progress = ((song != null && song.Commands.Count > 0) ? ((float)_songPlayer.CurrentCommandIndex / (float)song.Commands.Count * 100f) : 0f);
				_progressLabel.set_Text($"Playing... {progress:F0}%");
			}
			_progressLabel.set_TextColor(MaestroColors.MutedCream);
		}

		protected override void DisposeControl()
		{
			_songPlayer.OnStarted -= OnPlaybackStateChanged;
			_songPlayer.OnPaused -= OnPlaybackStateChanged;
			_songPlayer.OnResumed -= OnPlaybackStateChanged;
			_songPlayer.OnStopped -= OnPlaybackStateChanged;
			_songPlayer.OnCompleted -= OnPlaybackStateChanged;
			StandardButton pauseButton = _pauseButton;
			if (pauseButton != null)
			{
				((Control)pauseButton).Dispose();
			}
			StandardButton stopButton = _stopButton;
			if (stopButton != null)
			{
				((Control)stopButton).Dispose();
			}
			Label nowPlayingLabel = _nowPlayingLabel;
			if (nowPlayingLabel != null)
			{
				((Control)nowPlayingLabel).Dispose();
			}
			Label progressLabel = _progressLabel;
			if (progressLabel != null)
			{
				((Control)progressLabel).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
