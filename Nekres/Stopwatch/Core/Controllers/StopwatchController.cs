using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Nekres.Stopwatch.Core.Controls;
using Stopwatch;

namespace Nekres.Stopwatch.Core.Controllers
{
	internal class StopwatchController : IDisposable
	{
		private System.Diagnostics.Stopwatch _stopwatch;

		private TimeSpan _startTime;

		private StopwatchDisplay _display;

		private SoundEffect[] _rewindSfx;

		private SoundEffect _startSfx;

		private SoundEffect _beepSfx;

		private SoundEffect _longBeepSfx;

		private TimeSpan _prevBeep;

		private SoundEffect _tickSfx;

		private TimeSpan _prevTick;

		private Color _fontColor;

		private FontSize _fontSize;

		private Point _position;

		private Color _redShift;

		private bool _inInputPrompt;

		private SoundEffect RewindSfx => _rewindSfx[RandomUtil.GetRandom(0, _rewindSfx.Length - 1)];

		public Color FontColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _fontColor;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				_fontColor = value;
				if (_display != null)
				{
					_display.Color = value;
				}
			}
		}

		public FontSize FontSize
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _fontSize;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				_fontSize = value;
				if (_display != null)
				{
					_display.FontSize = value;
				}
			}
		}

		public Point Position
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _position;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				_position = value;
				if (_display != null)
				{
					((Control)_display).set_Location(value);
				}
			}
		}

		public float AudioVolume { get; set; }

		public StopwatchController()
		{
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			_rewindSfx = (SoundEffect[])(object)new SoundEffect[4]
			{
				StopwatchModule.ModuleInstance.ContentsManager.GetSound("audio\\stopwatch-rewind-1.wav"),
				StopwatchModule.ModuleInstance.ContentsManager.GetSound("audio\\stopwatch-rewind-2.wav"),
				StopwatchModule.ModuleInstance.ContentsManager.GetSound("audio\\stopwatch-rewind-3.wav"),
				StopwatchModule.ModuleInstance.ContentsManager.GetSound("audio\\stopwatch-rewind-4.wav")
			};
			_startSfx = StopwatchModule.ModuleInstance.ContentsManager.GetSound("audio\\stopwatch-start.wav");
			_tickSfx = StopwatchModule.ModuleInstance.ContentsManager.GetSound("audio\\stopwatch-tick.wav");
			_beepSfx = StopwatchModule.ModuleInstance.ContentsManager.GetSound("audio\\beep.wav");
			_longBeepSfx = StopwatchModule.ModuleInstance.ContentsManager.GetSound("audio\\long-beep.wav");
			_redShift = new Color(255, 57, 57);
			_stopwatch = new System.Diagnostics.Stopwatch();
		}

		public void StartAt()
		{
			TimeSpan prevValue = StopwatchModule.ModuleInstance.StartTime.get_Value();
			_inInputPrompt = true;
			TimeSpanInputPrompt.ShowPrompt(TimeSpanInputPromptCallback, "Enter a start time:", prevValue.Equals(TimeSpan.Zero) ? string.Empty : prevValue.ToString("hh\\:mm\\:ss\\.fff"));
		}

		public void Toggle()
		{
			if (!_inInputPrompt)
			{
				if (_stopwatch.IsRunning)
				{
					_stopwatch.Stop();
				}
				else
				{
					Start(StopwatchModule.ModuleInstance.StartTime.get_Value());
				}
			}
		}

		private void TimeSpanInputPromptCallback(bool confirmed, TimeSpan time)
		{
			_inInputPrompt = false;
			if (confirmed)
			{
				StopwatchModule.ModuleInstance.StartTime.set_Value(time);
				Reset();
			}
		}

		private void Start(TimeSpan? start = null)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			if (_display == null)
			{
				StopwatchDisplay stopwatchDisplay = new StopwatchDisplay();
				((Control)stopwatchDisplay).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((Control)stopwatchDisplay).set_Size(new Point(300, 250));
				((Control)stopwatchDisplay).set_Location(Position);
				stopwatchDisplay.Color = FontColor;
				stopwatchDisplay.FontSize = FontSize;
				_display = stopwatchDisplay;
			}
			if (start.HasValue)
			{
				_startTime = start.Value;
				_prevBeep = TimeSpan.Zero;
			}
			_startSfx.Play(AudioVolume, 0f, 0f);
			_stopwatch.Start();
			_prevTick = TimeSpan.Zero;
		}

		public void Stop()
		{
			StopwatchDisplay display = _display;
			if (display != null)
			{
				((Control)display).Dispose();
			}
			_display = null;
			_stopwatch.Stop();
		}

		public void Reset()
		{
			RewindSfx.Play(AudioVolume, 0f, 0f);
			StopwatchDisplay display = _display;
			if (display != null)
			{
				((Control)display).Dispose();
			}
			_display = null;
			_stopwatch.Reset();
		}

		public void Update()
		{
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			if (_display == null)
			{
				return;
			}
			if (!StopwatchModule.ModuleInstance.TickingSoundDisabledSetting.get_Value() && _stopwatch.Elapsed.Subtract(_prevTick).TotalMilliseconds > 500.0)
			{
				_tickSfx.Play(AudioVolume, 0f, 0f);
				_prevTick = _stopwatch.Elapsed;
			}
			if (_startTime.Equals(TimeSpan.Zero))
			{
				_display.Text = _stopwatch.Elapsed.ToString("hh\\:mm\\:ss\\.fff");
				return;
			}
			TimeSpan current = _startTime.Subtract(_stopwatch.Elapsed);
			_display.Text = ((current.Ticks < 0) ? "-" : "") + current.ToString("hh\\:mm\\:ss\\.fff");
			_display.Color = Color.Lerp(Color.get_White(), _redShift, (float)_stopwatch.ElapsedMilliseconds / (float)_startTime.TotalMilliseconds);
			if (StopwatchModule.ModuleInstance.BeepSoundDisabledSetting.get_Value())
			{
				return;
			}
			double totalSeconds = current.TotalSeconds;
			if (totalSeconds > -0.1 && totalSeconds < 3.0 && Math.Abs(current.TotalSeconds - _prevBeep.TotalSeconds) > 1.0)
			{
				_prevBeep = current;
				if (current.Ticks > 0)
				{
					_beepSfx.Play(AudioVolume, 0f, 0f);
				}
				else
				{
					_longBeepSfx.Play(AudioVolume, 0f, 0f);
				}
			}
		}

		public void Dispose()
		{
			StopwatchDisplay display = _display;
			if (display != null)
			{
				((Control)display).Dispose();
			}
			_stopwatch.Stop();
			SoundEffect[] rewindSfx = _rewindSfx;
			for (int i = 0; i < rewindSfx.Length; i++)
			{
				rewindSfx[i].Dispose();
			}
			_startSfx.Dispose();
			_tickSfx.Dispose();
			_beepSfx.Dispose();
			_longBeepSfx.Dispose();
		}
	}
}
