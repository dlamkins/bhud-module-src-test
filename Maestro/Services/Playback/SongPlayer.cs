using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Maestro.Models;
using Microsoft.Xna.Framework.Input;

namespace Maestro.Services.Playback
{
	public class SongPlayer
	{
		private static readonly Logger Logger = Logger.GetLogger<SongPlayer>();

		private readonly KeyboardService _keyboardService;

		private CancellationTokenSource _cancellationTokenSource;

		private Task _playbackTask;

		private readonly object _pauseLock = new object();

		private float _playbackSpeed = 1f;

		private static bool Gw2HasFocus => GameService.GameIntegration.Gw2Instance.Gw2HasFocus;

		private static bool IsTextInputFocused => GameService.Gw2Mumble.UI.IsTextInputFocused;

		private static bool ShouldPauseForInput
		{
			get
			{
				if (Gw2HasFocus)
				{
					return IsTextInputFocused;
				}
				return true;
			}
		}

		public Song CurrentSong { get; private set; }

		public float PlaybackSpeed
		{
			get
			{
				return _playbackSpeed;
			}
			set
			{
				_playbackSpeed = Math.Max(0.1f, Math.Min(2f, value));
			}
		}

		public int CurrentCommandIndex { get; private set; }

		public bool IsPlaying
		{
			get
			{
				if (_playbackTask != null)
				{
					return !_playbackTask.IsCompleted;
				}
				return false;
			}
		}

		public bool IsPaused { get; private set; }

		public bool IsWaitingForInput
		{
			get
			{
				if (IsPlaying && !IsPaused)
				{
					return ShouldPauseForInput;
				}
				return false;
			}
		}

		public bool IsAdjustingOctave { get; private set; }

		public event EventHandler OnStarted;

		public event EventHandler OnPaused;

		public event EventHandler OnResumed;

		public event EventHandler OnStopped;

		public event EventHandler OnCompleted;

		public SongPlayer(KeyboardService keyboardService)
		{
			_keyboardService = keyboardService;
		}

		public void Play(Song song)
		{
			Stop();
			CurrentSong = song;
			CurrentCommandIndex = 0;
			IsPaused = false;
			_cancellationTokenSource = new CancellationTokenSource();
			_keyboardService.StartDebugLog(song.DisplayName);
			_playbackTask = Task.Run(() => PlaybackLoop(_cancellationTokenSource.Token));
			this.OnStarted?.Invoke(this, EventArgs.Empty);
			Logger.Info($"Started playing: {song.DisplayName} ({song.Commands.Count} commands)");
		}

		public void Pause()
		{
			if (IsPlaying && !IsPaused)
			{
				lock (_pauseLock)
				{
					IsPaused = true;
				}
				_keyboardService.ReleaseAllKeys();
				this.OnPaused?.Invoke(this, EventArgs.Empty);
				Logger.Info("Playback paused");
			}
		}

		public void Resume()
		{
			if (IsPlaying && IsPaused)
			{
				lock (_pauseLock)
				{
					IsPaused = false;
					Monitor.Pulse(_pauseLock);
				}
				this.OnResumed?.Invoke(this, EventArgs.Empty);
				Logger.Info("Playback resumed");
			}
		}

		public void TogglePause()
		{
			if (IsPaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}

		public void Stop()
		{
			if (_cancellationTokenSource != null)
			{
				_cancellationTokenSource.Cancel();
				lock (_pauseLock)
				{
					IsPaused = false;
					Monitor.Pulse(_pauseLock);
				}
				try
				{
					_playbackTask?.Wait(1000);
				}
				catch (AggregateException)
				{
				}
				_cancellationTokenSource.Dispose();
				_cancellationTokenSource = null;
			}
			_keyboardService.ReleaseAllKeys();
			_keyboardService.StopDebugLog();
			CurrentSong = null;
			CurrentCommandIndex = 0;
			this.OnStopped?.Invoke(this, EventArgs.Empty);
			Logger.Info("Playback stopped");
		}

		private async Task PlaybackLoop(CancellationToken cancellationToken)
		{
			_ = 1;
			try
			{
				if (!CurrentSong.SkipOctaveReset)
				{
					ResetToMiddleOctave();
				}
				await Task.Delay(300, cancellationToken);
				for (int i = 0; i < CurrentSong.Commands.Count; i++)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						break;
					}
					lock (_pauseLock)
					{
						while ((IsPaused || ShouldPauseForInput) && !cancellationToken.IsCancellationRequested)
						{
							Monitor.Wait(_pauseLock, 100);
						}
					}
					if (cancellationToken.IsCancellationRequested)
					{
						break;
					}
					CurrentCommandIndex = i;
					SongCommand command = CurrentSong.Commands[i];
					ExecuteCommand(command);
					if (command.Type == CommandType.Wait && command.Duration > 0)
					{
						await Task.Delay((int)((float)command.Duration / _playbackSpeed), cancellationToken);
					}
				}
				if (!cancellationToken.IsCancellationRequested)
				{
					CurrentCommandIndex = CurrentSong.Commands.Count;
					this.OnCompleted?.Invoke(this, EventArgs.Empty);
					Logger.Info("Completed playing: " + CurrentSong?.DisplayName);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Error during playback - song stopped");
			}
		}

		private void ExecuteCommand(SongCommand command)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			switch (command.Type)
			{
			case CommandType.KeyDown:
				_keyboardService.KeyDown(command.Key);
				break;
			case CommandType.KeyUp:
				_keyboardService.KeyUp(command.Key);
				break;
			case CommandType.Wait:
				break;
			}
		}

		private void ResetToMiddleOctave()
		{
			IsAdjustingOctave = true;
			Logger.Debug("Resetting octave...");
			for (int i = 0; i < 5; i++)
			{
				_keyboardService.KeyDown((Keys)96);
				_keyboardService.KeyUp((Keys)96);
				Thread.Sleep(150);
			}
			Song currentSong = CurrentSong;
			if (currentSong == null || currentSong.Instrument != InstrumentType.Bass)
			{
				_keyboardService.KeyDown((Keys)105);
				_keyboardService.KeyUp((Keys)105);
				Thread.Sleep(150);
			}
			Thread.Sleep(200);
			IsAdjustingOctave = false;
			Logger logger = Logger;
			Song currentSong2 = CurrentSong;
			logger.Debug((currentSong2 != null && currentSong2.Instrument == InstrumentType.Bass) ? "Octave reset complete - Bass at Low octave" : "Octave reset complete - now at middle octave");
		}
	}
}
