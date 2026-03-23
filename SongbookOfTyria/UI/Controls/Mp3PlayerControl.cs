using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using SongbookOfTyria.Services;

namespace SongbookOfTyria.UI.Controls
{
	public class Mp3PlayerControl : Panel
	{
		private const int IconSize = 32;

		private const int VolumeIconSize = 24;

		private const int TimeLabelWidth = 90;

		private const int VolumeTrackBarWidth = 80;

		private const int TrackBarMaxValue = 1000;

		private const int ControlHeight = 32;

		private const int SeekBarHeight = 16;

		private const float DefaultVolume = 0.5f;

		private readonly AudioService _audioService;

		private readonly TextureService _textureService;

		private readonly string _mp3Url;

		private readonly AsyncTexture2D _playTexture;

		private readonly AsyncTexture2D _pauseTexture;

		private readonly AsyncTexture2D _volumeTexture;

		private readonly AsyncTexture2D _volumeMutedTexture;

		private GlowButton _playPauseButton;

		private TrackBar _seekBar;

		private Label _timeLabel;

		private Label _loadingLabel;

		private GlowButton _volumeButton;

		private TrackBar _volumeTrackBar;

		private bool _isLoading;

		private bool _isLoaded;

		private bool _disposed;

		private bool _isSeeking;

		private float _lastVolume = 0.5f;

		private float _currentVolume = 0.5f;

		public Mp3PlayerControl(AudioService audioService, TextureService textureService, string mp3Url)
			: this()
		{
			_audioService = audioService ?? throw new ArgumentNullException("audioService");
			_textureService = textureService ?? throw new ArgumentNullException("textureService");
			_mp3Url = mp3Url;
			_playTexture = _textureService.GetPlayIcon();
			_pauseTexture = _textureService.GetPauseIcon();
			_volumeTexture = _textureService.GetVolumeIcon();
			_volumeMutedTexture = _textureService.GetVolumeMutedIcon();
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Control)this).set_Height(32);
			BuildControls();
			SubscribeToEvents();
			if (!string.IsNullOrEmpty(_mp3Url))
			{
				LoadAudioAsync();
			}
		}

		private void BuildControls()
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Expected O, but got Unknown
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Expected O, but got Unknown
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Expected O, but got Unknown
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Expected O, but got Unknown
			int xOffset = 0;
			GlowButton val = new GlowButton();
			val.set_Icon(_playTexture);
			val.set_ActiveIcon(_playTexture);
			((Control)val).set_Size(new Point(32, 32));
			((Control)val).set_Location(new Point(xOffset, 0));
			((Control)val).set_BasicTooltipText("Play");
			((Control)val).set_Parent((Container)(object)this);
			_playPauseButton = val;
			((Control)_playPauseButton).add_Click((EventHandler<MouseEventArgs>)OnPlayPauseClick);
			xOffset += 40;
			Label val2 = new Label();
			val2.set_Text("Loading audio...");
			val2.set_Font(GameService.Content.get_DefaultFont12());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Location(new Point(xOffset, 9));
			((Control)val2).set_Visible(false);
			((Control)val2).set_Parent((Container)(object)this);
			_loadingLabel = val2;
			TrackBar val3 = new TrackBar();
			val3.set_MinValue(0f);
			val3.set_MaxValue(1000f);
			val3.set_Value(0f);
			val3.set_SmallStep(true);
			((Control)val3).set_Location(new Point(xOffset, 8));
			((Control)val3).set_Enabled(false);
			((Control)val3).set_Parent((Container)(object)this);
			_seekBar = val3;
			_seekBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)OnSeekBarValueChanged);
			((Control)_seekBar).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnSeekBarMousePressed);
			((Control)_seekBar).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnSeekBarMouseReleased);
			Label val4 = new Label();
			val4.set_Text("0:00 / 0:00");
			val4.set_Font(GameService.Content.get_DefaultFont12());
			val4.set_TextColor(Color.get_LightGray());
			((Control)val4).set_Width(90);
			val4.set_AutoSizeHeight(true);
			val4.set_HorizontalAlignment((HorizontalAlignment)0);
			((Control)val4).set_Parent((Container)(object)this);
			_timeLabel = val4;
			GlowButton val5 = new GlowButton();
			val5.set_Icon(_volumeTexture);
			val5.set_ActiveIcon(_volumeTexture);
			((Control)val5).set_Size(new Point(24, 24));
			((Control)val5).set_BasicTooltipText("Mute");
			((Control)val5).set_Parent((Container)(object)this);
			_volumeButton = val5;
			((Control)_volumeButton).add_Click((EventHandler<MouseEventArgs>)OnVolumeButtonClick);
			TrackBar val6 = new TrackBar();
			val6.set_MinValue(0f);
			val6.set_MaxValue(100f);
			val6.set_Value(_currentVolume * 100f);
			val6.set_SmallStep(true);
			((Control)val6).set_Width(80);
			((Control)val6).set_Parent((Container)(object)this);
			_volumeTrackBar = val6;
			_volumeTrackBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)OnVolumeTrackBarChanged);
			_audioService.SetVolume(_currentVolume);
			UpdateLayout();
		}

		private void SubscribeToEvents()
		{
			_audioService.StateChanged += OnAudioStateChanged;
			_audioService.PositionChanged += OnAudioPositionChanged;
		}

		private void UnsubscribeFromEvents()
		{
			_audioService.StateChanged -= OnAudioStateChanged;
			_audioService.PositionChanged -= OnAudioPositionChanged;
		}

		private async void LoadAudioAsync()
		{
			if (string.IsNullOrEmpty(_mp3Url))
			{
				return;
			}
			_isLoading = true;
			((Control)_loadingLabel).set_Visible(true);
			((Control)_seekBar).set_Visible(false);
			try
			{
				await _audioService.LoadAsync(_mp3Url).ConfigureAwait(continueOnCapturedContext: false);
				_isLoaded = _audioService.IsLoaded;
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					if (!_disposed)
					{
						_isLoading = false;
						((Control)_loadingLabel).set_Visible(false);
						((Control)_seekBar).set_Visible(true);
						UpdateIconStates();
						UpdateTimeLabels();
					}
				});
			}
			catch (Exception ex)
			{
				Logger.GetLogger<Mp3PlayerControl>().Warn(ex, "Failed to load audio");
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					//IL_0026: Unknown result type (might be due to invalid IL or missing references)
					if (!_disposed)
					{
						_isLoading = false;
						_loadingLabel.set_Text("Failed to load");
						_loadingLabel.set_TextColor(Color.get_Red());
					}
				});
			}
		}

		private void OnPlayPauseClick(object sender, MouseEventArgs e)
		{
			if (_isLoaded)
			{
				if (_audioService.PlaybackState == AudioPlaybackState.Playing)
				{
					_audioService.Pause();
				}
				else
				{
					_audioService.Play();
				}
			}
		}

		private void OnAudioStateChanged(object sender, AudioStateChangedEventArgs e)
		{
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				if (!_disposed)
				{
					UpdateIconStates();
				}
			});
		}

		private void OnAudioPositionChanged(object sender, AudioPositionChangedEventArgs e)
		{
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				if (!_disposed)
				{
					UpdateTimeLabels();
					UpdateSeekBar();
				}
			});
		}

		private void OnSeekBarValueChanged(object sender, ValueEventArgs<float> e)
		{
			if (_isLoaded && _isSeeking)
			{
				double totalSeconds = _audioService.TotalDuration.TotalSeconds;
				if (!(totalSeconds <= 0.0))
				{
					TimeSpan seekPosition = TimeSpan.FromSeconds((double)(e.get_Value() / 1000f) * totalSeconds);
					_audioService.Seek(seekPosition);
					UpdateTimeLabels();
				}
			}
		}

		private void OnSeekBarMousePressed(object sender, MouseEventArgs e)
		{
			_isSeeking = true;
		}

		private void OnSeekBarMouseReleased(object sender, MouseEventArgs e)
		{
			_isSeeking = false;
		}

		private void OnVolumeButtonClick(object sender, MouseEventArgs e)
		{
			if (_currentVolume > 0f)
			{
				_lastVolume = _currentVolume;
				_currentVolume = 0f;
			}
			else
			{
				_currentVolume = ((_lastVolume > 0f) ? _lastVolume : 0.5f);
			}
			_volumeTrackBar.set_Value(_currentVolume * 100f);
			_audioService.SetVolume(_currentVolume);
			UpdateVolumeIcon();
		}

		private void OnVolumeTrackBarChanged(object sender, ValueEventArgs<float> e)
		{
			_currentVolume = e.get_Value() / 100f;
			_audioService.SetVolume(_currentVolume);
			UpdateVolumeIcon();
		}

		private void UpdateVolumeIcon()
		{
			if (_currentVolume <= 0f)
			{
				_volumeButton.set_Icon(_volumeMutedTexture);
				_volumeButton.set_ActiveIcon(_volumeMutedTexture);
				((Control)_volumeButton).set_BasicTooltipText("Unmute");
			}
			else
			{
				_volumeButton.set_Icon(_volumeTexture);
				_volumeButton.set_ActiveIcon(_volumeTexture);
				((Control)_volumeButton).set_BasicTooltipText("Mute");
			}
		}

		private void UpdateIconStates()
		{
			bool enabled = _isLoaded && !_isLoading;
			((Control)_playPauseButton).set_Enabled(enabled);
			((Control)_seekBar).set_Enabled(enabled);
			if (_audioService.PlaybackState == AudioPlaybackState.Playing)
			{
				_playPauseButton.set_Icon(_pauseTexture);
				_playPauseButton.set_ActiveIcon(_pauseTexture);
				((Control)_playPauseButton).set_BasicTooltipText("Pause");
			}
			else
			{
				_playPauseButton.set_Icon(_playTexture);
				_playPauseButton.set_ActiveIcon(_playTexture);
				((Control)_playPauseButton).set_BasicTooltipText("Play");
			}
		}

		private void UpdateTimeLabels()
		{
			string currentTime = FormatTime(_audioService.CurrentPosition);
			string totalTime = FormatTime(_audioService.TotalDuration);
			_timeLabel.set_Text(currentTime + " / " + totalTime);
		}

		private void UpdateLayout()
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			if (_seekBar == null || _timeLabel == null || _volumeButton == null || _volumeTrackBar == null)
			{
				return;
			}
			int panelWidth = ((Control)this).get_Width();
			if (panelWidth > 0)
			{
				int volumeControlsWidth = 108;
				int volumeGap = 15;
				int seekBarStart = 40;
				int seekBarWidth = panelWidth - seekBarStart - 90 - volumeControlsWidth - volumeGap - 15;
				if (seekBarWidth < 50)
				{
					seekBarWidth = 50;
				}
				((Control)_seekBar).set_Location(new Point(seekBarStart, 8));
				((Control)_seekBar).set_Width(seekBarWidth);
				int timeLabelX = seekBarStart + seekBarWidth + 10;
				((Control)_timeLabel).set_Location(new Point(timeLabelX, 9));
				int volumeButtonX = timeLabelX + 90 + volumeGap - 15;
				((Control)_volumeButton).set_Location(new Point(volumeButtonX, 4));
				int volumeTrackBarX = volumeButtonX + 24 + 4;
				((Control)_volumeTrackBar).set_Location(new Point(volumeTrackBarX, 8));
			}
		}

		private void UpdateSeekBar()
		{
			if (!_isSeeking && !(_audioService.TotalDuration.TotalSeconds <= 0.0))
			{
				double progress = _audioService.CurrentPosition.TotalSeconds / _audioService.TotalDuration.TotalSeconds;
				_seekBar.set_Value((float)(progress * 1000.0));
			}
		}

		private static string FormatTime(TimeSpan time)
		{
			if (time.TotalHours >= 1.0)
			{
				return $"{(int)time.TotalHours}:{time.Minutes:D2}:{time.Seconds:D2}";
			}
			return $"{(int)time.TotalMinutes}:{time.Seconds:D2}";
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Container)this).UpdateContainer(gameTime);
			if (_isLoaded && _audioService.PlaybackState == AudioPlaybackState.Playing)
			{
				_audioService.UpdatePosition();
			}
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		protected override void DisposeControl()
		{
			_disposed = true;
			UnsubscribeFromEvents();
			if (_playPauseButton != null)
			{
				((Control)_playPauseButton).remove_Click((EventHandler<MouseEventArgs>)OnPlayPauseClick);
			}
			if (_seekBar != null)
			{
				_seekBar.remove_ValueChanged((EventHandler<ValueEventArgs<float>>)OnSeekBarValueChanged);
				((Control)_seekBar).remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnSeekBarMousePressed);
				((Control)_seekBar).remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnSeekBarMouseReleased);
			}
			if (_volumeButton != null)
			{
				((Control)_volumeButton).remove_Click((EventHandler<MouseEventArgs>)OnVolumeButtonClick);
			}
			if (_volumeTrackBar != null)
			{
				_volumeTrackBar.remove_ValueChanged((EventHandler<ValueEventArgs<float>>)OnVolumeTrackBarChanged);
			}
			((Panel)this).DisposeControl();
		}
	}
}
