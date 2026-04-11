using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Glide;
using Microsoft.Xna.Framework;

namespace CinemaModule.UI.Controls
{
	public class BaseVideoControls
	{
		public const int IconSize = 32;

		public const int TrackBarWidth = 100;

		public const int TrackBarHeight = 16;

		public const int SeekBarHeight = 16;

		public const int ControlSpacing = 8;

		public const int QualityDropdownWidth = 140;

		public const float FadeDuration = 0.2f;

		protected int LastVolume = 100;

		protected Tween FadeAnimation;

		private bool _wasSeekBarDragging;

		private float _currentPosition;

		private long _duration;

		private float _pendingSeekPosition = -1f;

		private const float SeekPositionTolerance = 0.02f;

		private bool _isWatchPartyViewer;

		private int _volume = 100;

		private float _opacity;

		private bool _suppressQualityEvent;

		public VideoControlsRenderer Renderer { get; }

		public TrackBar VolumeTrackBar { get; }

		public TrackBar SeekBar { get; }

		public Dropdown QualityDropdown { get; }

		public bool IsPaused { get; set; }

		public bool IsWatchPartyViewer
		{
			get
			{
				return _isWatchPartyViewer;
			}
			set
			{
				_isWatchPartyViewer = value;
			}
		}

		public int Volume
		{
			get
			{
				return _volume;
			}
			set
			{
				if (_volume != value)
				{
					_volume = value;
					OnVolumePropertyChanged(value);
				}
			}
		}

		public virtual float Opacity
		{
			get
			{
				return _opacity;
			}
			set
			{
				_opacity = value;
			}
		}

		public bool IsSeekBarDragging
		{
			get
			{
				TrackBar seekBar = SeekBar;
				if (seekBar == null)
				{
					return false;
				}
				return seekBar.get_Dragging();
			}
		}

		private bool IsSeekPending => _pendingSeekPosition >= 0f;

		private bool JustStoppedDragging
		{
			get
			{
				if (_wasSeekBarDragging)
				{
					return !IsSeekBarDragging;
				}
				return false;
			}
		}

		public float CurrentPosition
		{
			get
			{
				return _currentPosition;
			}
			set
			{
				if (SeekBar == null || IsSeekBarDragging || JustStoppedDragging)
				{
					return;
				}
				if (IsSeekPending)
				{
					if (!(Math.Abs(value - _pendingSeekPosition) < 0.02f))
					{
						return;
					}
					_pendingSeekPosition = -1f;
				}
				_currentPosition = value;
				SeekBar.set_Value(value * 100f);
			}
		}

		public long Duration
		{
			get
			{
				return _duration;
			}
			set
			{
				_duration = value;
			}
		}

		public event EventHandler PlayPauseClicked;

		public event EventHandler<int> VolumeChanged;

		public event EventHandler SettingsClicked;

		public event EventHandler<int> QualityChanged;

		public event EventHandler<float> SeekRequested;

		public BaseVideoControls(Container parent, int trackBarWidth, int trackBarHeight, int dropdownWidth, bool createSeekBar = false)
		{
			Renderer = new VideoControlsRenderer(CinemaModule.Instance.TextureService);
			VolumeTrackBar = CreateVolumeTrackBar(parent, trackBarWidth, trackBarHeight);
			VolumeTrackBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)OnVolumeTrackBarChanged);
			QualityDropdown = CreateQualityDropdown(parent, dropdownWidth);
			QualityDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnQualityDropdownChanged);
			if (createSeekBar)
			{
				SeekBar = CreateSeekBar(parent);
				SeekBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)OnSeekBarValueChanged);
			}
		}

		private TrackBar CreateVolumeTrackBar(Container parent, int width, int height)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			TrackBar val = new TrackBar();
			((Control)val).set_Parent(parent);
			val.set_MinValue(0f);
			val.set_MaxValue(100f);
			val.set_Value(100f);
			val.set_SmallStep(true);
			((Control)val).set_Size(new Point(width, height));
			return val;
		}

		private Dropdown CreateQualityDropdown(Container parent, int width)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			Dropdown val = new Dropdown();
			((Control)val).set_Parent(parent);
			((Control)val).set_Size(new Point(width, 27));
			((Control)val).set_Visible(false);
			return val;
		}

		private TrackBar CreateSeekBar(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			TrackBar val = new TrackBar();
			((Control)val).set_Parent(parent);
			val.set_MinValue(0f);
			val.set_MaxValue(100f);
			val.set_Value(0f);
			val.set_SmallStep(false);
			((Control)val).set_Size(new Point(200, 16));
			((Control)val).set_Visible(false);
			return val;
		}

		private void OnSeekBarValueChanged(object sender, ValueEventArgs<float> e)
		{
			if (SeekBar.get_Dragging())
			{
				if (_isWatchPartyViewer)
				{
					SeekBar.set_Value(_currentPosition * 100f);
				}
				else
				{
					_currentPosition = e.get_Value() / 100f;
				}
			}
		}

		public void UpdateSeekBarDragState()
		{
			if (SeekBar != null)
			{
				if (_wasSeekBarDragging && !SeekBar.get_Dragging())
				{
					float seekPosition = (_currentPosition = (_pendingSeekPosition = SeekBar.get_Value() / 100f));
					this.SeekRequested?.Invoke(this, seekPosition);
				}
				_wasSeekBarDragging = SeekBar.get_Dragging();
			}
		}

		public string FormatTimeDisplay()
		{
			TimeSpan ts = TimeSpan.FromMilliseconds((float)_duration * _currentPosition);
			return string.Concat(str2: FormatTime(TimeSpan.FromMilliseconds(_duration)), str0: FormatTime(ts), str1: " / ");
		}

		private static string FormatTime(TimeSpan ts)
		{
			if (ts.Hours <= 0)
			{
				return $"{ts.Minutes}:{ts.Seconds:D2}";
			}
			return $"{ts.Hours}:{ts.Minutes:D2}:{ts.Seconds:D2}";
		}

		protected virtual void OnVolumePropertyChanged(int newValue)
		{
			if (!VolumeTrackBar.get_Dragging())
			{
				VolumeTrackBar.set_Value((float)newValue);
			}
		}

		private void OnVolumeTrackBarChanged(object sender, ValueEventArgs<float> e)
		{
			int newVolume = (int)e.get_Value();
			if (newVolume != _volume)
			{
				_volume = newVolume;
				HandleVolumeTrackBarChange(newVolume);
			}
		}

		protected virtual void HandleVolumeTrackBarChange(int newVolume)
		{
			RaiseVolumeChanged(newVolume);
		}

		public void ToggleMuteAndNotify()
		{
			if (_volume > 0)
			{
				LastVolume = _volume;
				_volume = 0;
			}
			else
			{
				_volume = ((LastVolume > 0) ? LastVolume : 50);
			}
			VolumeTrackBar.set_Value((float)_volume);
			RaiseVolumeChanged(_volume);
		}

		private void OnQualityDropdownChanged(object sender, ValueChangedEventArgs e)
		{
			if (!_suppressQualityEvent)
			{
				int selectedIndex = QualityDropdown.get_Items().IndexOf(QualityDropdown.get_SelectedItem());
				if (selectedIndex >= 0)
				{
					RaiseQualityChanged(selectedIndex);
				}
			}
		}

		public void UpdateAvailableQualities(IReadOnlyList<string> qualityNames, int selectedIndex)
		{
			_suppressQualityEvent = true;
			QualityDropdown.get_Items().Clear();
			if (qualityNames == null || qualityNames.Count == 0)
			{
				((Control)QualityDropdown).set_Visible(false);
				_suppressQualityEvent = false;
				return;
			}
			foreach (string name in qualityNames)
			{
				QualityDropdown.get_Items().Add(name);
			}
			if (selectedIndex >= 0 && selectedIndex < QualityDropdown.get_Items().Count)
			{
				QualityDropdown.set_SelectedItem(QualityDropdown.get_Items()[selectedIndex]);
			}
			_suppressQualityEvent = false;
		}

		protected void StartFadeIn()
		{
			Tween fadeAnimation = FadeAnimation;
			if (fadeAnimation != null)
			{
				fadeAnimation.Cancel();
			}
			FadeAnimation = ((TweenerImpl)GameService.Animation.get_Tweener()).Tween<BaseVideoControls>(this, (object)new
			{
				Opacity = 1f
			}, 0.2f, 0f, true).Ease((Func<float, float>)Ease.QuadOut);
		}

		protected void StartFadeOut(Action onComplete = null)
		{
			Tween fadeAnimation = FadeAnimation;
			if (fadeAnimation != null)
			{
				fadeAnimation.Cancel();
			}
			Tween tween = ((TweenerImpl)GameService.Animation.get_Tweener()).Tween<BaseVideoControls>(this, (object)new
			{
				Opacity = 0f
			}, 0.2f, 0f, true).Ease((Func<float, float>)Ease.QuadIn);
			if (onComplete != null)
			{
				tween.OnComplete(onComplete);
			}
			FadeAnimation = tween;
		}

		public void RaisePlayPauseClicked()
		{
			this.PlayPauseClicked?.Invoke(this, EventArgs.Empty);
		}

		public void RaiseVolumeChanged(int volume)
		{
			this.VolumeChanged?.Invoke(this, volume);
		}

		public void RaiseSettingsClicked()
		{
			this.SettingsClicked?.Invoke(this, EventArgs.Empty);
		}

		public void RaiseQualityChanged(int index)
		{
			this.QualityChanged?.Invoke(this, index);
		}

		public virtual void Dispose()
		{
			CleanupEventHandlers();
			((Control)VolumeTrackBar).Dispose();
			TrackBar seekBar = SeekBar;
			if (seekBar != null)
			{
				((Control)seekBar).Dispose();
			}
			((Control)QualityDropdown).Dispose();
		}

		protected virtual void CleanupEventHandlers()
		{
			VolumeTrackBar.remove_ValueChanged((EventHandler<ValueEventArgs<float>>)OnVolumeTrackBarChanged);
			QualityDropdown.remove_ValueChanged((EventHandler<ValueChangedEventArgs>)OnQualityDropdownChanged);
			if (SeekBar != null)
			{
				SeekBar.remove_ValueChanged((EventHandler<ValueEventArgs<float>>)OnSeekBarValueChanged);
			}
		}
	}
}
