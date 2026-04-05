using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using MeltySynth;
using NAudio.Wave;
using SongbookOfTyria.Models;
using SongbookOfTyria.Utilities;

namespace SongbookOfTyria.Services
{
	public sealed class MidiPlaybackService : IDisposable
	{
		private class ScheduledNoteOff
		{
			public int Channel;

			public int NoteNumber;

			public double EndTime;
		}

		private class SynthesizerWaveProvider : IWaveProvider
		{
			private const float VolumeBoost = 12f;

			private readonly Synthesizer _synth;

			private readonly object _lock;

			private readonly float[] _leftBuffer;

			private readonly float[] _rightBuffer;

			public WaveFormat WaveFormat { get; }

			public float MasterVolume { get; set; } = 1f;


			public SynthesizerWaveProvider(Synthesizer synth, object lockObj)
			{
				_synth = synth;
				_lock = lockObj;
				WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
				_leftBuffer = new float[4096];
				_rightBuffer = new float[4096];
			}

			public int Read(byte[] buffer, int offset, int count)
			{
				int sampleCount = count / 8;
				if (sampleCount > _leftBuffer.Length)
				{
					sampleCount = _leftBuffer.Length;
				}
				lock (_lock)
				{
					_synth.Render(_leftBuffer.AsSpan(0, sampleCount), _rightBuffer.AsSpan(0, sampleCount));
				}
				float gain = MasterVolume * 12f;
				int byteIndex = offset;
				for (int i = 0; i < sampleCount; i++)
				{
					float left = Math.Max(-1f, Math.Min(1f, _leftBuffer[i] * gain));
					float right = Math.Max(-1f, Math.Min(1f, _rightBuffer[i] * gain));
					WriteFloatToBuffer(buffer, byteIndex, left);
					byteIndex += 4;
					WriteFloatToBuffer(buffer, byteIndex, right);
					byteIndex += 4;
				}
				return sampleCount * 8;
			}

			private static void WriteFloatToBuffer(byte[] buffer, int index, float value)
			{
				Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, index, 4);
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<MidiPlaybackService>();

		private const int SampleRate = 44100;

		private const int BufferSize = 4096;

		private const int SchedulerIntervalMs = 10;

		private const int MaxMidiChannels = 16;

		private const int PercussionChannel = 9;

		private const int MidiControlChange = 176;

		private const int MidiProgramChange = 192;

		private const int MidiVolumeController = 7;

		private readonly string _cacheDirectory;

		private readonly HttpClient _httpClient;

		private readonly bool _ownsHttpClient;

		private MidiData _midiData;

		private SoundFont _soundFont;

		private Synthesizer _synthesizer;

		private WaveOutEvent _waveOut;

		private SynthesizerWaveProvider _waveProvider;

		private Stopwatch _playbackStopwatch;

		private Timer _schedulerTimer;

		private double _startTimeOffset;

		private volatile bool _isPlaying;

		private volatile bool _disposed;

		private volatile bool _soundFontLoaded;

		private readonly Dictionary<int, float> _trackVolumes = new Dictionary<int, float>();

		private readonly Dictionary<int, bool> _trackMuted = new Dictionary<int, bool>();

		private readonly Dictionary<int, int> _trackChannelMap = new Dictionary<int, int>();

		private readonly HashSet<long> _scheduledNoteKeys = new HashSet<long>();

		private readonly List<ScheduledNoteOff> _pendingNoteOffs = new List<ScheduledNoteOff>();

		private readonly List<ActiveNoteInfo> _activeNotes = new List<ActiveNoteInfo>();

		private readonly object _synthLock = new object();

		private readonly object _stateLock = new object();

		private float _masterVolume = 1f;

		private float _playbackSpeed = 1f;

		public bool IsPlaying => _isPlaying;

		public double CurrentPosition
		{
			get
			{
				if (!_isPlaying)
				{
					return _startTimeOffset;
				}
				return _startTimeOffset + (_playbackStopwatch?.Elapsed.TotalSeconds ?? 0.0) * (double)_playbackSpeed;
			}
		}

		public double Duration => _midiData?.Duration ?? 0.0;

		public bool HasMidiData
		{
			get
			{
				MidiData midiData = _midiData;
				if (midiData == null)
				{
					return false;
				}
				return midiData.Tracks?.Count > 0;
			}
		}

		public bool IsSoundFontLoaded => _soundFontLoaded;

		public float PlaybackSpeed => _playbackSpeed;

		public event EventHandler<double> PositionChanged;

		public event EventHandler<ActiveNoteEventArgs> ActiveNotesChanged;

		public event EventHandler PlaybackStarted;

		public event EventHandler PlaybackStopped;

		public event EventHandler PlaybackFinished;

		public event EventHandler SoundFontLoaded;

		public MidiPlaybackService(string cacheDirectory, HttpClient httpClient = null)
		{
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			_cacheDirectory = Path.Combine(cacheDirectory, "soundfonts");
			_ownsHttpClient = httpClient == null;
			object obj = httpClient;
			if (obj == null)
			{
				HttpClient val = new HttpClient();
				obj = (object)val;
				val.set_Timeout(TimeSpan.FromMinutes(5.0));
			}
			_httpClient = (HttpClient)obj;
			_playbackStopwatch = new Stopwatch();
			Directory.CreateDirectory(_cacheDirectory);
		}

		public async Task LoadSoundFontAsync(string soundFontUrl)
		{
			if (_soundFontLoaded)
			{
				return;
			}
			try
			{
				string localPath = await DownloadSoundFontAsync(soundFontUrl).ConfigureAwait(continueOnCapturedContext: false);
				if (localPath != null)
				{
					InitializeSynthesizer(localPath);
					Logger.Debug("SoundFont loaded successfully from {Path}", new object[1] { localPath });
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load SoundFont from {Url}", new object[1] { soundFontUrl });
			}
		}

		public void LoadSoundFontFromFile(string filePath)
		{
			if (!_soundFontLoaded)
			{
				try
				{
					InitializeSynthesizer(filePath);
					Logger.Debug("SoundFont loaded successfully from {Path}", new object[1] { filePath });
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to load SoundFont from {Path}", new object[1] { filePath });
				}
			}
		}

		private void InitializeSynthesizer(string soundFontPath)
		{
			_soundFont = new SoundFont(soundFontPath);
			SynthesizerSettings settings = new SynthesizerSettings(44100)
			{
				BlockSize = 64,
				MaximumPolyphony = 64,
				EnableReverbAndChorus = true
			};
			_synthesizer = new Synthesizer(_soundFont, settings);
			_soundFontLoaded = true;
			this.SoundFontLoaded?.Invoke(this, EventArgs.Empty);
		}

		private async Task<string> DownloadSoundFontAsync(string url)
		{
			string fileName = GenerateCacheFileName(url);
			string cachedPath = Path.Combine(_cacheDirectory, fileName);
			if (File.Exists(cachedPath))
			{
				Logger.Debug("Using cached SoundFont: {Path}", new object[1] { cachedPath });
				return cachedPath;
			}
			Logger.Debug("Downloading SoundFont from {Url}", new object[1] { url });
			HttpResponseMessage response = await _httpClient.GetAsync(url, (HttpCompletionOption)1).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				response.EnsureSuccessStatusCode();
				using Stream stream = await response.get_Content().ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
				using FileStream fileStream = new FileStream(cachedPath, FileMode.Create, FileAccess.Write, FileShare.None);
				await stream.CopyToAsync(fileStream).ConfigureAwait(continueOnCapturedContext: false);
			}
			finally
			{
				((IDisposable)response)?.Dispose();
			}
			return cachedPath;
		}

		private static string GenerateCacheFileName(string url)
		{
			string hash = HashUtility.GetStableHashCode(url).ToString("X8");
			string originalName = Path.GetFileNameWithoutExtension(new Uri(url).LocalPath);
			if (string.IsNullOrEmpty(originalName))
			{
				originalName = "soundfont";
			}
			return originalName + "_" + hash + ".sf2";
		}

		public void UnloadMidiData()
		{
			Stop();
			_midiData = null;
			_trackVolumes.Clear();
			_trackMuted.Clear();
			_trackChannelMap.Clear();
		}

		public void LoadMidiData(MidiData midiData)
		{
			UnloadMidiData();
			_midiData = midiData;
			if (_midiData?.Tracks == null)
			{
				return;
			}
			int nextChannel = 0;
			foreach (MidiTrack track in _midiData.Tracks)
			{
				_trackVolumes[track.Index] = 1f;
				_trackMuted[track.Index] = false;
				if (nextChannel == 9)
				{
					nextChannel++;
				}
				if (nextChannel >= 16)
				{
					nextChannel = 0;
				}
				_trackChannelMap[track.Index] = nextChannel;
				nextChannel++;
			}
		}

		public void Play()
		{
			if (_midiData == null || _isPlaying || !_soundFontLoaded)
			{
				return;
			}
			try
			{
				lock (_synthLock)
				{
					_synthesizer.Reset();
					SetupTrackInstruments();
					ApplyTrackVolumes();
				}
				if (_waveOut == null)
				{
					_waveProvider = new SynthesizerWaveProvider(_synthesizer, _synthLock);
					_waveProvider.MasterVolume = _masterVolume;
					_waveOut = new WaveOutEvent
					{
						DesiredLatency = 100,
						NumberOfBuffers = 3
					};
					_waveOut.Init(_waveProvider);
				}
				_waveOut.Play();
				_playbackStopwatch.Restart();
				_isPlaying = true;
				_scheduledNoteKeys.Clear();
				PreScheduleNoteKeys();
				_schedulerTimer = new Timer(OnSchedulerTick, null, 0, 10);
				this.PlaybackStarted?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to start MIDI playback");
				_isPlaying = false;
			}
		}

		public void Pause()
		{
			if (_isPlaying)
			{
				_startTimeOffset = CurrentPosition;
				StopPlayback(sendStopEvent: true);
			}
		}

		public void Stop()
		{
			_startTimeOffset = 0.0;
			StopPlayback(sendStopEvent: true);
		}

		public void Seek(double timeInSeconds)
		{
			bool isPlaying = _isPlaying;
			if (isPlaying)
			{
				StopPlayback(sendStopEvent: false);
			}
			_startTimeOffset = Math.Max(0.0, Math.Min(timeInSeconds, Duration));
			if (isPlaying)
			{
				Play();
			}
			else
			{
				this.PositionChanged?.Invoke(this, _startTimeOffset);
			}
		}

		public void SetTrackVolume(int trackIndex, float volume)
		{
			_trackVolumes[trackIndex] = Math.Max(0f, Math.Min(1f, volume));
			if (_isPlaying && _synthesizer != null && _trackChannelMap.TryGetValue(trackIndex, out var ch))
			{
				lock (_synthLock)
				{
					int midiVol = (int)(volume * 127f);
					_synthesizer.ProcessMidiMessage(ch, 176, 7, midiVol);
				}
			}
		}

		public void SetTrackMuted(int trackIndex, bool muted)
		{
			_trackMuted[trackIndex] = muted;
			if (!_isPlaying || _synthesizer == null || !_trackChannelMap.TryGetValue(trackIndex, out var ch))
			{
				return;
			}
			lock (_synthLock)
			{
				if (muted)
				{
					_synthesizer.ProcessMidiMessage(ch, 176, 7, 0);
					return;
				}
				float v;
				int midiVol = (int)((_trackVolumes.TryGetValue(trackIndex, out v) ? v : 1f) * 127f);
				_synthesizer.ProcessMidiMessage(ch, 176, 7, midiVol);
			}
		}

		public float GetTrackVolume(int trackIndex)
		{
			if (!_trackVolumes.TryGetValue(trackIndex, out var vol))
			{
				return 1f;
			}
			return vol;
		}

		public bool IsTrackMuted(int trackIndex)
		{
			bool muted;
			return _trackMuted.TryGetValue(trackIndex, out muted) && muted;
		}

		public void SetMasterVolume(float volume)
		{
			_masterVolume = Math.Max(0f, Math.Min(2f, volume));
			if (_waveProvider != null)
			{
				_waveProvider.MasterVolume = _masterVolume;
			}
		}

		public float GetMasterVolume()
		{
			return _masterVolume;
		}

		public void SetPlaybackSpeed(float speed)
		{
			float newSpeed = Math.Max(0.5f, Math.Min(1f, speed));
			if (!(Math.Abs(newSpeed - _playbackSpeed) < 0.001f))
			{
				if (_isPlaying)
				{
					_startTimeOffset = CurrentPosition;
					_playbackStopwatch.Restart();
				}
				_playbackSpeed = newSpeed;
			}
		}

		private void SetupTrackInstruments()
		{
			if (_midiData?.Tracks == null || _synthesizer == null)
			{
				return;
			}
			foreach (MidiTrack track in _midiData.Tracks)
			{
				if (_trackChannelMap.TryGetValue(track.Index, out var ch))
				{
					int program = track.Instrument?.Number ?? 0;
					_synthesizer.ProcessMidiMessage(ch, 192, program, 0);
				}
			}
		}

		private void ApplyTrackVolumes()
		{
			if (_midiData?.Tracks == null || _synthesizer == null)
			{
				return;
			}
			foreach (MidiTrack track in _midiData.Tracks)
			{
				if (_trackChannelMap.TryGetValue(track.Index, out var ch))
				{
					bool i;
					bool num = _trackMuted.TryGetValue(track.Index, out i) && i;
					float v;
					float vol = (_trackVolumes.TryGetValue(track.Index, out v) ? v : 1f);
					int midiVol = ((!num) ? ((int)(vol * 127f)) : 0);
					_synthesizer.ProcessMidiMessage(ch, 176, 7, midiVol);
				}
			}
		}

		private void PreScheduleNoteKeys()
		{
			_scheduledNoteKeys.Clear();
			if (_midiData?.Tracks == null)
			{
				return;
			}
			foreach (MidiTrack track in _midiData.Tracks)
			{
				if (track.Notes == null)
				{
					continue;
				}
				foreach (MidiNote note in track.Notes)
				{
					if (note.Time < _startTimeOffset)
					{
						_scheduledNoteKeys.Add(MakeNoteKey(track.Index, note));
					}
				}
			}
		}

		private void OnSchedulerTick(object state)
		{
			if (_disposed || !_isPlaying || _synthesizer == null)
			{
				return;
			}
			try
			{
				double currentPos = CurrentPosition;
				lock (_synthLock)
				{
					ProcessNoteOffs(currentPos);
					ScheduleNewNotes(currentPos);
				}
				UpdateActiveNotes(currentPos);
				this.PositionChanged?.Invoke(this, currentPos);
				if (currentPos >= Duration)
				{
					_startTimeOffset = 0.0;
					StopPlayback(sendStopEvent: false);
					this.PlaybackFinished?.Invoke(this, EventArgs.Empty);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Error during MIDI scheduler tick");
			}
		}

		private void ScheduleNewNotes(double currentPos)
		{
			if (_midiData?.Tracks == null)
			{
				return;
			}
			foreach (MidiTrack track in _midiData.Tracks)
			{
				if (track.Notes == null || !_trackChannelMap.TryGetValue(track.Index, out var ch))
				{
					continue;
				}
				foreach (MidiNote note in track.Notes)
				{
					if (note.Time > currentPos)
					{
						break;
					}
					if (!(note.Time < currentPos - 0.05))
					{
						long key = MakeNoteKey(track.Index, note);
						if (!_scheduledNoteKeys.Contains(key))
						{
							_scheduledNoteKeys.Add(key);
							int velocity = Math.Max(1, Math.Min(127, note.Velocity));
							_synthesizer.NoteOn(ch, note.Midi, velocity);
							_pendingNoteOffs.Add(new ScheduledNoteOff
							{
								Channel = ch,
								NoteNumber = note.Midi,
								EndTime = note.Time + note.Duration
							});
						}
					}
				}
			}
		}

		private void ProcessNoteOffs(double currentPos)
		{
			for (int i = _pendingNoteOffs.Count - 1; i >= 0; i--)
			{
				ScheduledNoteOff noteOff = _pendingNoteOffs[i];
				if (currentPos >= noteOff.EndTime)
				{
					_synthesizer?.NoteOff(noteOff.Channel, noteOff.NoteNumber);
					_pendingNoteOffs.RemoveAt(i);
				}
			}
		}

		private void UpdateActiveNotes(double currentPos)
		{
			if (_midiData?.Tracks == null)
			{
				return;
			}
			lock (_stateLock)
			{
				_activeNotes.Clear();
				foreach (MidiTrack track in _midiData.Tracks)
				{
					if (track.Notes == null)
					{
						continue;
					}
					for (int i = FindFirstNoteAtOrAfter(track.Notes, currentPos - GetMaxNoteDuration(track)); i < track.Notes.Count; i++)
					{
						MidiNote note = track.Notes[i];
						if (note.Time > currentPos)
						{
							break;
						}
						if (note.Time + note.Duration > currentPos)
						{
							_activeNotes.Add(new ActiveNoteInfo
							{
								TrackIndex = track.Index,
								NoteIndex = i,
								MidiNote = note.Midi,
								Time = note.Time
							});
						}
					}
				}
			}
			this.ActiveNotesChanged?.Invoke(this, new ActiveNoteEventArgs(_activeNotes));
		}

		private static int FindFirstNoteAtOrAfter(List<MidiNote> notes, double time)
		{
			if (time <= 0.0)
			{
				return 0;
			}
			int low = 0;
			int high = notes.Count - 1;
			int result = notes.Count;
			while (low <= high)
			{
				int mid = low + (high - low) / 2;
				if (notes[mid].Time >= time)
				{
					result = mid;
					high = mid - 1;
				}
				else
				{
					low = mid + 1;
				}
			}
			return Math.Max(0, result - 1);
		}

		private static double GetMaxNoteDuration(MidiTrack track)
		{
			if (track.Notes == null || track.Notes.Count == 0)
			{
				return 1.0;
			}
			double maxDuration = 0.0;
			foreach (MidiNote note in track.Notes)
			{
				if (note.Duration > maxDuration)
				{
					maxDuration = note.Duration;
				}
			}
			return Math.Max(1.0, maxDuration);
		}

		private void AllNotesOff()
		{
			if (_synthesizer != null)
			{
				lock (_synthLock)
				{
					_synthesizer.NoteOffAll(immediate: false);
				}
				_pendingNoteOffs.Clear();
				lock (_stateLock)
				{
					_activeNotes.Clear();
				}
			}
		}

		private void StopPlayback(bool sendStopEvent)
		{
			_isPlaying = false;
			_playbackStopwatch.Stop();
			_schedulerTimer?.Dispose();
			_schedulerTimer = null;
			_waveOut?.Stop();
			AllNotesOff();
			_scheduledNoteKeys.Clear();
			if (sendStopEvent)
			{
				this.PlaybackStopped?.Invoke(this, EventArgs.Empty);
			}
		}

		private static long MakeNoteKey(int trackIndex, MidiNote note)
		{
			long timeTicks = (long)(note.Time * 10000.0);
			return ((long)trackIndex << 48) | ((long)note.Midi << 32) | timeTicks;
		}

		public void Dispose()
		{
			if (_disposed)
			{
				return;
			}
			_disposed = true;
			StopPlayback(sendStopEvent: false);
			if (_waveOut != null)
			{
				try
				{
					_waveOut.Dispose();
				}
				catch
				{
				}
				_waveOut = null;
			}
			if (_ownsHttpClient)
			{
				try
				{
					((HttpMessageInvoker)_httpClient).Dispose();
				}
				catch
				{
				}
			}
			_waveProvider = null;
			_synthesizer = null;
			_soundFont = null;
		}
	}
}
