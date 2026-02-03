using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services.Playback;
using Maestro.UI.Controls;
using Microsoft.Xna.Framework;

namespace Maestro.UI.Main
{
	public class NowPlayingPanel : Panel
	{
		public static class Layout
		{
			public const int Height = 105;

			public const int ButtonY = 18;

			public const int ButtonWidth = 40;

			public const int PauseButtonX = 8;

			public const int StopButtonX = 52;

			public const int LabelX = 100;

			public const int LabelYPlaying = 8;

			public const int LabelYCentered = 21;

			public const int ProgressLabelY = 28;

			public const int InstrumentLabelY = 45;

			public const int LabelWidth = 300;

			public const int SpeedLabelX = 8;

			public const int SpeedLabelY = 62;

			public const int SpeedSliderX = 60;

			public const int SpeedSliderY = 65;

			public const int SpeedSliderWidth = 180;

			public const int SpeedValueX = 248;

			public const int QueueButtonWidth = 30;

			public const int QueueButtonRightPadding = 8;

			public const int QueueButtonY = 8;
		}

		private readonly SongPlayer _songPlayer;

		private readonly StandardButton _pauseButton;

		private readonly StandardButton _stopButton;

		private readonly MarqueeLabel _nowPlayingLabel;

		private readonly Label _progressLabel;

		private readonly Label _instrumentLabel;

		private readonly Label _speedLabel;

		private readonly TrackBar _speedSlider;

		private readonly Label _speedValueLabel;

		private readonly StandardButton _queueButton;

		private bool _isPlayingFromQueue;

		private Song _pendingSong;

		private InstrumentType? _currentInstrument;

		public event EventHandler StopRequested;

		public event EventHandler<Song> PlayPendingRequested;

		public event EventHandler QueueToggleClicked;

		public NowPlayingPanel(SongPlayer songPlayer, int width)
			: this()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			_songPlayer = songPlayer;
			((Control)this).set_Size(new Point(width, 105));
			((Control)this).set_BackgroundColor(MaestroTheme.WithAlpha(MaestroTheme.SlateGray, 150));
			((Panel)this).set_ShowBorder(true);
			_pauseButton = CreatePauseButton();
			_stopButton = CreateStopButton();
			_nowPlayingLabel = CreateNowPlayingLabel();
			_progressLabel = CreateProgressLabel();
			_instrumentLabel = CreateInstrumentLabel();
			_speedLabel = CreateSpeedLabel();
			_speedSlider = CreateSpeedSlider();
			_speedValueLabel = CreateSpeedValueLabel();
			_queueButton = CreateQueueButton(width);
			SubscribeToEvents();
		}

		public void SetQueuePlaybackMode(bool isPlaying)
		{
			_isPlayingFromQueue = isPlaying;
			UpdatePlaybackState();
		}

		public void SetPendingSong(Song song)
		{
			_pendingSong = song;
			ShowPendingState();
		}

		public void ClearPendingSong()
		{
			_pendingSong = null;
			UpdatePlaybackState();
		}

		public void SetCurrentInstrument(InstrumentType? instrument)
		{
			_currentInstrument = instrument;
			_instrumentLabel.set_Text(instrument?.ToString() ?? "");
		}

		public void UpdatePlaybackState()
		{
			if (_songPlayer.IsPlaying)
			{
				UpdatePlayingState();
			}
			else
			{
				UpdateStoppedState();
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Container)this).UpdateContainer(gameTime);
			if (_songPlayer.IsPlaying && !_songPlayer.IsPaused)
			{
				UpdateLiveProgress();
			}
		}

		protected override void DisposeControl()
		{
			UnsubscribeFromEvents();
			DisposeControls();
			((Panel)this).DisposeControl();
		}

		private StandardButton CreatePauseButton()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("||");
			((Control)val).set_Location(new Point(8, 18));
			((Control)val).set_Width(40);
			((Control)val).set_Enabled(false);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)OnPauseClicked);
			return val;
		}

		private StandardButton CreateStopButton()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("X");
			((Control)val).set_Location(new Point(52, 18));
			((Control)val).set_Width(40);
			((Control)val).set_Enabled(false);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)OnStopClicked);
			return val;
		}

		private MarqueeLabel CreateNowPlayingLabel()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			int availableWidth = ((Control)this).get_Width() - 100 - 30 - 8 - 5;
			MarqueeLabel marqueeLabel = new MarqueeLabel();
			((Control)marqueeLabel).set_Parent((Container)(object)this);
			marqueeLabel.Text = "No song playing";
			((Control)marqueeLabel).set_Location(new Point(100, 21));
			((Control)marqueeLabel).set_Width(availableWidth);
			marqueeLabel.Font = GameService.Content.get_DefaultFont14();
			marqueeLabel.TextColor = MaestroTheme.MutedCream;
			return marqueeLabel;
		}

		private Label CreateProgressLabel()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("");
			((Control)val).set_Location(new Point(100, 28));
			((Control)val).set_Width(300);
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroTheme.MutedCream);
			return val;
		}

		private Label CreateInstrumentLabel()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("");
			((Control)val).set_Location(new Point(100, 45));
			((Control)val).set_Width(300);
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroTheme.MutedCream);
			return val;
		}

		private Label CreateSpeedLabel()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Speed:");
			((Control)val).set_Location(new Point(8, 62));
			val.set_AutoSizeWidth(true);
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroTheme.MutedCream);
			return val;
		}

		private TrackBar CreateSpeedSlider()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			TrackBar val = new TrackBar();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(60, 65));
			((Control)val).set_Width(180);
			val.set_MinValue(1f);
			val.set_MaxValue(20f);
			val.set_Value(10f);
			val.set_SmallStep(true);
			val.add_ValueChanged((EventHandler<ValueEventArgs<float>>)OnSpeedSliderChanged);
			return val;
		}

		private Label CreateSpeedValueLabel()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("1.0x");
			((Control)val).set_Location(new Point(248, 62));
			val.set_AutoSizeWidth(true);
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroTheme.CreamWhite);
			return val;
		}

		private StandardButton CreateQueueButton(int panelWidth)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(">>");
			((Control)val).set_Location(new Point(panelWidth - 30 - 8, 8));
			((Control)val).set_Size(new Point(30, 26));
			((Control)val).set_BasicTooltipText("Toggle Queue");
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.QueueToggleClicked?.Invoke(this, EventArgs.Empty);
			});
			return val;
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
			if (_pendingSong != null)
			{
				Song song = _pendingSong;
				_pendingSong = null;
				this.PlayPendingRequested?.Invoke(this, song);
			}
			else if (SongFilterBar.WasJustUnfocused)
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
			this.StopRequested?.Invoke(this, EventArgs.Empty);
		}

		private void OnSpeedSliderChanged(object sender, ValueEventArgs<float> e)
		{
			float speed = e.get_Value() / 10f;
			_songPlayer.PlaybackSpeed = speed;
			_speedValueLabel.set_Text($"{speed:F1}x");
		}

		private void UpdatePlayingState()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			Song song = _songPlayer.CurrentSong;
			_nowPlayingLabel.Text = song?.DisplayName ?? "Unknown";
			((Control)_nowPlayingLabel).set_Location(new Point(100, 8));
			_instrumentLabel.set_Text(song?.Instrument.ToString() ?? "");
			if (_songPlayer.IsPaused)
			{
				UpdatePausedState();
			}
			else
			{
				UpdateActivePlayingState(song);
			}
			((Control)_pauseButton).set_Enabled(true);
			((Control)_stopButton).set_Enabled(true);
		}

		private void UpdatePausedState()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			_pauseButton.set_Text(">");
			_nowPlayingLabel.TextColor = MaestroTheme.CreamWhite;
			_progressLabel.set_Text("Paused" + GetQueueSuffix());
			_progressLabel.set_TextColor(MaestroTheme.Paused);
		}

		private void UpdateActivePlayingState(Song song)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			_pauseButton.set_Text("||");
			_nowPlayingLabel.TextColor = MaestroTheme.CreamWhite;
			if (_songPlayer.IsAdjustingOctave)
			{
				_progressLabel.set_Text("Adjusting..." + GetQueueSuffix());
				_progressLabel.set_TextColor(MaestroTheme.Paused);
			}
			else
			{
				UpdateProgressText(song);
				_progressLabel.set_TextColor(MaestroTheme.MutedCream);
			}
		}

		private void UpdateProgressText(Song song)
		{
			if (song != null && _songPlayer.CurrentCommandIndex >= song.Commands.Count)
			{
				_progressLabel.set_Text("Done!" + GetQueueSuffix());
				return;
			}
			float progress = CalculateProgress(song);
			_progressLabel.set_Text($"Playing... {progress:F0}%" + GetQueueSuffix());
		}

		private void UpdateStoppedState()
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			Song song = _songPlayer.CurrentSong;
			if (song != null && _songPlayer.CurrentCommandIndex >= song.Commands.Count)
			{
				_nowPlayingLabel.Text = song.DisplayName;
				((Control)_nowPlayingLabel).set_Location(new Point(100, 8));
				_nowPlayingLabel.TextColor = MaestroTheme.CreamWhite;
				_progressLabel.set_Text("Done!");
				_progressLabel.set_TextColor(MaestroTheme.MutedCream);
			}
			else
			{
				_nowPlayingLabel.Text = "No song playing";
				((Control)_nowPlayingLabel).set_Location(new Point(100, 21));
				_nowPlayingLabel.TextColor = MaestroTheme.MutedCream;
				_progressLabel.set_Text("");
			}
			_pauseButton.set_Text("||");
			((Control)_pauseButton).set_Enabled(false);
			((Control)_stopButton).set_Enabled(false);
		}

		private void ShowPendingState()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			_nowPlayingLabel.Text = _pendingSong?.DisplayName ?? "Unknown";
			((Control)_nowPlayingLabel).set_Location(new Point(100, 8));
			_nowPlayingLabel.TextColor = MaestroTheme.CreamWhite;
			_instrumentLabel.set_Text(_pendingSong?.Instrument.ToString() ?? "");
			_progressLabel.set_Text("Ready" + GetQueueSuffix());
			_progressLabel.set_TextColor(MaestroTheme.Paused);
			_pauseButton.set_Text(">");
			((Control)_pauseButton).set_Enabled(true);
			((Control)_stopButton).set_Enabled(false);
		}

		private void UpdateLiveProgress()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			if (_songPlayer.IsWaitingForInput)
			{
				_pauseButton.set_Text(">");
				_progressLabel.set_Text("Paused" + GetQueueSuffix());
				_progressLabel.set_TextColor(MaestroTheme.Paused);
			}
			else if (_songPlayer.IsAdjustingOctave)
			{
				_progressLabel.set_Text("Adjusting..." + GetQueueSuffix());
				_progressLabel.set_TextColor(MaestroTheme.Paused);
			}
			else
			{
				_pauseButton.set_Text("||");
				Song song = _songPlayer.CurrentSong;
				UpdateProgressText(song);
				_progressLabel.set_TextColor(MaestroTheme.MutedCream);
			}
		}

		private string GetQueueSuffix()
		{
			if (!_isPlayingFromQueue)
			{
				return "";
			}
			return " - Queue";
		}

		private float CalculateProgress(Song song)
		{
			if (song == null || song.Commands.Count <= 0)
			{
				return 0f;
			}
			return (float)_songPlayer.CurrentCommandIndex / (float)song.Commands.Count * 100f;
		}

		private static void ClearTextInputFocus()
		{
			Control focusedControl = Control.get_FocusedControl();
			TextInputBase textInput = (TextInputBase)(object)((focusedControl is TextInputBase) ? focusedControl : null);
			if (textInput != null)
			{
				textInput.set_Focused(false);
			}
		}

		private void UnsubscribeFromEvents()
		{
			_songPlayer.OnStarted -= OnPlaybackStateChanged;
			_songPlayer.OnPaused -= OnPlaybackStateChanged;
			_songPlayer.OnResumed -= OnPlaybackStateChanged;
			_songPlayer.OnStopped -= OnPlaybackStateChanged;
			_songPlayer.OnCompleted -= OnPlaybackStateChanged;
		}

		private void DisposeControls()
		{
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
			MarqueeLabel nowPlayingLabel = _nowPlayingLabel;
			if (nowPlayingLabel != null)
			{
				((Control)nowPlayingLabel).Dispose();
			}
			Label progressLabel = _progressLabel;
			if (progressLabel != null)
			{
				((Control)progressLabel).Dispose();
			}
			Label instrumentLabel = _instrumentLabel;
			if (instrumentLabel != null)
			{
				((Control)instrumentLabel).Dispose();
			}
			Label speedLabel = _speedLabel;
			if (speedLabel != null)
			{
				((Control)speedLabel).Dispose();
			}
			TrackBar speedSlider = _speedSlider;
			if (speedSlider != null)
			{
				((Control)speedSlider).Dispose();
			}
			Label speedValueLabel = _speedValueLabel;
			if (speedValueLabel != null)
			{
				((Control)speedValueLabel).Dispose();
			}
			StandardButton queueButton = _queueButton;
			if (queueButton != null)
			{
				((Control)queueButton).Dispose();
			}
		}
	}
}
