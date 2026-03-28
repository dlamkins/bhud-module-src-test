using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Maestro.Models;
using Maestro.Services.Data;
using Maestro.UI.Main;
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

		private volatile bool _seekRequested;

		private int _seekTargetIndex;

		private int _seekTargetOctave;

		private int _lastWaitDuration;

		private bool _lastKeyUpWasNote;

		private Keys _lastKeyUpKey;

		private static bool Gw2HasFocus => GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus();

		private static bool IsGw2TextInputFocused => GameService.Gw2Mumble.get_UI().get_IsTextInputFocused();

		private static bool IsOverlayTextInputFocused => SongFilterBar.IsTextInputFocused;

		private static bool ShouldPauseForInput
		{
			get
			{
				if (Gw2HasFocus && !IsGw2TextInputFocused)
				{
					return IsOverlayTextInputFocused;
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
			_seekRequested = false;
			_cancellationTokenSource = new CancellationTokenSource();
			if (song.SeekData == null && song.Commands.Count > 0)
			{
				song.SeekData = NoteParser.ComputeSeekData(song.Commands);
			}
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

		public void SeekTo(float progress)
		{
			if (CurrentSong?.SeekData != null)
			{
				SeekData seekData = CurrentSong.SeekData;
				long targetMs = (long)(progress * (float)seekData.TotalDurationMs);
				int index = Array.BinarySearch(seekData.CumulativeTimeMs, targetMs);
				if (index < 0)
				{
					index = ~index;
				}
				index = Math.Min(index, CurrentSong.Commands.Count - 1);
				index = (_seekTargetIndex = Math.Max(index, 0));
				_seekTargetOctave = seekData.OctaveAtCommand[index];
				_seekRequested = true;
				Logger.Info($"Seek requested to {progress:P0} (command {index}, octave {_seekTargetOctave})");
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
			_seekRequested = false;
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
				CurrentCommandIndex = 0;
				while (CurrentCommandIndex < CurrentSong.Commands.Count && !cancellationToken.IsCancellationRequested)
				{
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
					if (_seekRequested)
					{
						_seekRequested = false;
						CurrentCommandIndex = _seekTargetIndex;
						_keyboardService.ReleaseAllKeys();
						ResetToTargetOctave(_seekTargetOctave);
						continue;
					}
					SongCommand command = CurrentSong.Commands[CurrentCommandIndex];
					DetectPhantomNoteRisk(command);
					ExecuteCommand(command);
					if (command.Type == CommandType.Wait && command.Duration > 0)
					{
						await Task.Delay((int)((float)command.Duration / _playbackSpeed), cancellationToken);
					}
					CurrentCommandIndex++;
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

		private void DetectPhantomNoteRisk(SongCommand command)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Invalid comparison between Unknown and I4
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Invalid comparison between Unknown and I4
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Invalid comparison between Unknown and I4
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Invalid comparison between Unknown and I4
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			if (command.Type == CommandType.Wait)
			{
				_lastWaitDuration = command.Duration;
				_lastKeyUpWasNote = false;
			}
			else if (command.Type == CommandType.KeyUp)
			{
				bool isOctaveKey = (int)command.Key == 105 || (int)command.Key == 96;
				_lastKeyUpWasNote = !isOctaveKey && (int)command.Key != 164;
				_lastKeyUpKey = command.Key;
			}
			else if (command.Type == CommandType.KeyDown && _lastKeyUpWasNote)
			{
				if (((int)command.Key == 105 || (int)command.Key == 96) && _lastWaitDuration >= 500)
				{
					Logger.Debug($"[PHANTOM-RISK] Long note ({_lastWaitDuration}ms) " + $"KeyUp({_lastKeyUpKey}) followed by octave change ({command.Key}) " + $"at command {CurrentCommandIndex}");
				}
				_lastKeyUpWasNote = false;
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
			IsAdjustingOctave = false;
			Logger logger = Logger;
			Song currentSong2 = CurrentSong;
			logger.Debug((currentSong2 != null && currentSong2.Instrument == InstrumentType.Bass) ? "Octave reset complete - Bass at Low octave" : "Octave reset complete - now at middle octave");
		}

		private void ResetToTargetOctave(int targetOctave)
		{
			IsAdjustingOctave = true;
			Logger.Debug($"Seek octave reset to {targetOctave}...");
			for (int j = 0; j < 5; j++)
			{
				_keyboardService.KeyDown((Keys)96);
				_keyboardService.KeyUp((Keys)96);
				Thread.Sleep(150);
			}
			Song currentSong = CurrentSong;
			int upsNeeded = ((currentSong != null && currentSong.Instrument == InstrumentType.Bass) ? targetOctave : (targetOctave + 1));
			upsNeeded = Math.Max(0, upsNeeded);
			for (int i = 0; i < upsNeeded; i++)
			{
				_keyboardService.KeyDown((Keys)105);
				_keyboardService.KeyUp((Keys)105);
				Thread.Sleep(150);
			}
			IsAdjustingOctave = false;
			Logger.Debug($"Seek octave reset complete - target octave {targetOctave}");
		}
	}
}
