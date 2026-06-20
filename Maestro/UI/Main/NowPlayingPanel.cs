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
			public const int Height = 144;

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

			public const int SideMargin = 8;

			public const int SpeedLabelY = 52;

			public const int SpeedSliderY = 78;

			public const int SeekLabelY = 100;

			public const int SeekSliderY = 126;

			public const int QueueButtonWidth = 30;

			public const int QueueButtonRightPadding = 8;

			public const int QueueButtonY = 8;
		}

		private readonly SongPlayer _songPlayer;

		private readonly IconButton _pauseButton;

		private readonly StandardButton _stopButton;

		private readonly MarqueeLabel _nowPlayingLabel;

		private readonly Label _progressLabel;

		private readonly Label _instrumentLabel;

		private readonly Label _speedLabel;

		private readonly TrackBar _speedSlider;

		private readonly Label _speedValueLabel;

		private readonly Label _elapsedLabel;

		private readonly TrackBar _seekSlider;

		private readonly Label _totalLabel;

		private readonly IconButton _queueButton;

		private bool _isPlayingFromQueue;

		private Song _pendingSong;

		private InstrumentType? _currentInstrument;

		private bool _wasPlayingBeforeSeek;

		private int RowWidth => ((Control)this).get_Width() - 16;

		public event EventHandler StopRequested;

		public event EventHandler<Song> PlayPendingRequested;

		public event EventHandler QueueToggleClicked;

		public NowPlayingPanel(SongPlayer songPlayer, int width)
			: this()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_songPlayer = songPlayer;
			((Control)this).set_Size(new Point(width, 144));
			((Control)this).set_BackgroundColor(Color.get_Transparent());
			((Panel)this).set_ShowBorder(false);
			_pauseButton = CreatePauseButton();
			_stopButton = CreateStopButton();
			_nowPlayingLabel = CreateNowPlayingLabel();
			_progressLabel = CreateProgressLabel();
			_instrumentLabel = CreateInstrumentLabel();
			_speedLabel = CreateSpeedLabel();
			_speedSlider = CreateSpeedSlider();
			_speedValueLabel = CreateSpeedValueLabel();
			_elapsedLabel = CreateElapsedLabel();
			_seekSlider = CreateSeekSlider();
			_totalLabel = CreateTotalLabel();
			_queueButton = CreateQueueButton(width);
			SubscribeToEvents();
		}

		public void SetQueuePlaybackMode(bool isPlaying)
		{
			_isPlayingFromQueue = isPlaying;
			UpdatePlaybackState();
		}

		public void SetQueueActive(bool active)
		{
			_queueButton.Selected = active;
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
			if (_songPlayer.IsPlaying && !_seekSlider.get_Dragging())
			{
				UpdateSeekSlider();
			}
		}

		protected override void DisposeControl()
		{
			UnsubscribeFromEvents();
			DisposeControls();
			((Panel)this).DisposeControl();
		}

		private IconButton CreatePauseButton()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			IconButton iconButton = new IconButton(MaestroIcons.Pause, MaestroTheme.IconGlyph);
			((Control)iconButton).set_Parent((Container)(object)this);
			((Control)iconButton).set_BasicTooltipText("Pause");
			((Control)iconButton).set_Location(new Point(8, 18));
			((Control)iconButton).set_Width(40);
			((Control)iconButton).set_Enabled(false);
			((Control)iconButton).add_Click((EventHandler<MouseEventArgs>)OnPauseClicked);
			return iconButton;
		}

		private StandardButton CreateStopButton()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			IconButton iconButton = new IconButton(MaestroIcons.Stop, MaestroTheme.IconGlyph);
			((Control)iconButton).set_Parent((Container)(object)this);
			((Control)iconButton).set_BasicTooltipText("Stop");
			((Control)iconButton).set_Location(new Point(52, 18));
			((Control)iconButton).set_Width(40);
			((Control)iconButton).set_Enabled(false);
			((Control)iconButton).add_Click((EventHandler<MouseEventArgs>)OnStopClicked);
			return (StandardButton)(object)iconButton;
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
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Speed:");
			((Control)val).set_Location(new Point(8, 52));
			((Control)val).set_Width(RowWidth);
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroTheme.MutedCream);
			return val;
		}

		private TrackBar CreateSpeedSlider()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			TrackBar val = new TrackBar();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(8, 78));
			((Control)val).set_Width(RowWidth);
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
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("1.0x");
			((Control)val).set_Location(new Point(8, 52));
			((Control)val).set_Width(RowWidth);
			val.set_HorizontalAlignment((HorizontalAlignment)2);
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroTheme.CreamWhite);
			return val;
		}

		private Label CreateElapsedLabel()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("0:00");
			((Control)val).set_Location(new Point(8, 100));
			((Control)val).set_Width(RowWidth);
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroTheme.MutedCream);
			return val;
		}

		private TrackBar CreateSeekSlider()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			TrackBar val = new TrackBar();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(8, 126));
			((Control)val).set_Width(RowWidth);
			val.set_MinValue(0f);
			val.set_MaxValue(1000f);
			val.set_Value(0f);
			val.set_SmallStep(true);
			((Control)val).set_Enabled(false);
			val.add_IsDraggingChanged((EventHandler<ValueEventArgs<bool>>)OnSeekDraggingChanged);
			val.add_ValueChanged((EventHandler<ValueEventArgs<float>>)OnSeekValueChanged);
			return val;
		}

		private Label CreateTotalLabel()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("0:00");
			((Control)val).set_Location(new Point(8, 100));
			((Control)val).set_Width(RowWidth);
			val.set_HorizontalAlignment((HorizontalAlignment)2);
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroTheme.MutedCream);
			return val;
		}

		private IconButton CreateQueueButton(int panelWidth)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			IconButton iconButton = new IconButton(MaestroIcons.Queue, MaestroTheme.IconGlyph);
			((Control)iconButton).set_Parent((Container)(object)this);
			((Control)iconButton).set_Location(new Point(panelWidth - 30 - 8, 8));
			((Control)iconButton).set_Size(new Point(30, 26));
			((Control)iconButton).set_BasicTooltipText("Toggle Queue");
			((Control)iconButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.QueueToggleClicked?.Invoke(this, EventArgs.Empty);
			});
			return iconButton;
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

		private void OnSeekDraggingChanged(object sender, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				_wasPlayingBeforeSeek = _songPlayer.IsPlaying && !_songPlayer.IsPaused;
				if (_wasPlayingBeforeSeek)
				{
					_songPlayer.Pause();
				}
			}
			else
			{
				_songPlayer.SeekTo(_seekSlider.get_Value() / 1000f);
				if (_wasPlayingBeforeSeek)
				{
					_songPlayer.Resume();
				}
			}
		}

		private void OnSeekValueChanged(object sender, ValueEventArgs<float> e)
		{
			if (_seekSlider.get_Dragging())
			{
				Song song = _songPlayer.CurrentSong;
				if (song?.SeekData != null)
				{
					long targetMs = (long)(e.get_Value() / 1000f * (float)song.SeekData.TotalDurationMs);
					_elapsedLabel.set_Text(FormatTime(targetMs));
				}
			}
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
			((Control)_seekSlider).set_Enabled(true);
			if (song?.SeekData != null)
			{
				_totalLabel.set_Text(FormatTime(song.SeekData.TotalDurationMs));
			}
		}

		private void UpdatePausedState()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			_pauseButton.IconTexture = MaestroIcons.Play;
			_nowPlayingLabel.TextColor = MaestroTheme.CreamWhite;
			_progressLabel.set_Text("Paused" + GetQueueSuffix());
			_progressLabel.set_TextColor(MaestroTheme.Paused);
		}

		private void UpdateActivePlayingState(Song song)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			_pauseButton.IconTexture = MaestroIcons.Pause;
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
			_pauseButton.IconTexture = MaestroIcons.Pause;
			((Control)_pauseButton).set_Enabled(false);
			((Control)_stopButton).set_Enabled(false);
			((Control)_seekSlider).set_Enabled(false);
			_seekSlider.set_Value(0f);
			_elapsedLabel.set_Text("0:00");
			_totalLabel.set_Text("0:00");
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
			_pauseButton.IconTexture = MaestroIcons.Play;
			((Control)_pauseButton).set_Enabled(true);
			((Control)_stopButton).set_Enabled(false);
			((Control)_seekSlider).set_Enabled(false);
		}

		private void UpdateLiveProgress()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			if (_songPlayer.IsWaitingForInput)
			{
				_pauseButton.IconTexture = MaestroIcons.Play;
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
				_pauseButton.IconTexture = MaestroIcons.Pause;
				Song song = _songPlayer.CurrentSong;
				UpdateProgressText(song);
				_progressLabel.set_TextColor(MaestroTheme.MutedCream);
			}
		}

		private void UpdateSeekSlider()
		{
			Song song = _songPlayer.CurrentSong;
			if (song?.SeekData != null)
			{
				SeekData seekData = song.SeekData;
				int index = _songPlayer.CurrentCommandIndex;
				if (index >= seekData.CumulativeTimeMs.Length)
				{
					index = seekData.CumulativeTimeMs.Length - 1;
				}
				if (index >= 0)
				{
					long elapsedMs = seekData.CumulativeTimeMs[index];
					float progress = ((seekData.TotalDurationMs > 0) ? ((float)elapsedMs / (float)seekData.TotalDurationMs * 1000f) : 0f);
					_seekSlider.set_Value(progress);
					_elapsedLabel.set_Text(FormatTime(elapsedMs));
				}
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

		private static string FormatTime(long ms)
		{
			TimeSpan span = TimeSpan.FromMilliseconds(Math.Max(0L, ms));
			if (!(span.TotalHours >= 1.0))
			{
				return span.ToString("m\\:ss");
			}
			return span.ToString("h\\:mm\\:ss");
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
			IconButton pauseButton = _pauseButton;
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
			Label elapsedLabel = _elapsedLabel;
			if (elapsedLabel != null)
			{
				((Control)elapsedLabel).Dispose();
			}
			TrackBar seekSlider = _seekSlider;
			if (seekSlider != null)
			{
				((Control)seekSlider).Dispose();
			}
			Label totalLabel = _totalLabel;
			if (totalLabel != null)
			{
				((Control)totalLabel).Dispose();
			}
			IconButton queueButton = _queueButton;
			if (queueButton != null)
			{
				((Control)queueButton).Dispose();
			}
		}
	}
}
