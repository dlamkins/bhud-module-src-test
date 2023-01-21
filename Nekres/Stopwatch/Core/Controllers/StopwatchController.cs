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

		private float _backgroundOpacity;

		private Color _redShift;

		private bool _inInputPrompt;

		private Vector3 PlayerPosition;

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

		public float BackgroundOpacity
		{
			get
			{
				return _backgroundOpacity;
			}
			set
			{
				_backgroundOpacity = value;
				if (_display != null)
				{
					_display.BackgroundOpacity = value;
				}
			}
		}

		public bool IsRunning => _stopwatch.IsRunning;

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

		private void TimeSpanInputPromptCallback(bool confirmed, TimeSpan time)
		{
			_inInputPrompt = false;
			if (confirmed)
			{
				StopwatchModule.ModuleInstance.StartTime.set_Value(time);
				Reset();
			}
		}

		public void Start(TimeSpan? start = null)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			if (_inInputPrompt || !((Vector3)(ref PlayerPosition)).Equals(Vector3.get_Zero()))
			{
				return;
			}
			_startSfx.Play(AudioVolume, 0f, 0f);
			if (_display != null && !_stopwatch.IsRunning)
			{
				Start();
				return;
			}
			StopwatchDisplay display = _display;
			if (display != null)
			{
				((Control)display).Dispose();
			}
			StopwatchDisplay stopwatchDisplay = new StopwatchDisplay();
			((Control)stopwatchDisplay).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)stopwatchDisplay).set_Size(new Point(400, 100));
			((Control)stopwatchDisplay).set_Location(Position);
			stopwatchDisplay.Color = FontColor;
			stopwatchDisplay.FontSize = FontSize;
			stopwatchDisplay.BackgroundOpacity = BackgroundOpacity;
			_display = stopwatchDisplay;
			if (start.HasValue)
			{
				_startTime = start.Value;
				_prevBeep = TimeSpan.Zero;
			}
			Start();
		}

		private void Start()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			_prevTick = TimeSpan.Zero;
			if (StopwatchModule.ModuleInstance.StartOnMovementEnabled.get_Value())
			{
				PlayerPosition = (GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode() ? GameService.Gw2Mumble.get_PlayerCamera().get_Position() : GameService.Gw2Mumble.get_PlayerCharacter().get_Position());
				_display.Text = $"Awaiting movement...\nX:{PlayerPosition.X:F} Y:{PlayerPosition.Y:F} Z:{PlayerPosition.Z:F}";
			}
			else
			{
				_stopwatch.Start();
			}
		}

		public void Stop()
		{
			if (!_inInputPrompt)
			{
				_startSfx.Play(AudioVolume, 0f, 0f);
				_stopwatch.Stop();
			}
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
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			if (!((Vector3)(ref PlayerPosition)).Equals(Vector3.get_Zero()) && !((Vector3)(ref PlayerPosition)).Equals(GameService.Gw2Mumble.get_PlayerCharacter().get_Position()))
			{
				PlayerPosition = Vector3.get_Zero();
				_stopwatch.Start();
			}
			if (_display == null || !_stopwatch.IsRunning)
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
			_stopwatch.Stop();
			StopwatchDisplay display = _display;
			if (display != null)
			{
				((Control)display).Dispose();
			}
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
