using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Microsoft.Xna.Framework;
using NAudio.Wave;

namespace SongbookOfTyria.Services
{
	public sealed class AudioService : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<AudioService>();

		private readonly HttpClient _httpClient;

		private readonly string _cacheDirectory;

		private WaveOutEvent _waveOut;

		private AudioFileReader _audioReader;

		private string _currentUrl;

		private bool _disposed;

		private CancellationTokenSource _loadCts;

		public AudioPlaybackState PlaybackState => GetPlaybackState();

		public TimeSpan CurrentPosition => _audioReader?.CurrentTime ?? TimeSpan.Zero;

		public TimeSpan TotalDuration => _audioReader?.TotalTime ?? TimeSpan.Zero;

		public bool IsLoaded => _audioReader != null;

		public event EventHandler<AudioStateChangedEventArgs> StateChanged;

		public event EventHandler<AudioPositionChangedEventArgs> PositionChanged;

		public AudioService(string cacheDirectory)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			_cacheDirectory = Path.Combine(cacheDirectory, "audio");
			HttpClient val = new HttpClient();
			val.set_Timeout(TimeSpan.FromMinutes(5.0));
			_httpClient = val;
			Directory.CreateDirectory(_cacheDirectory);
		}

		private AudioPlaybackState GetPlaybackState()
		{
			if (_waveOut == null)
			{
				return AudioPlaybackState.Stopped;
			}
			return _waveOut.PlaybackState switch
			{
				NAudio.Wave.PlaybackState.Playing => AudioPlaybackState.Playing, 
				NAudio.Wave.PlaybackState.Paused => AudioPlaybackState.Paused, 
				_ => AudioPlaybackState.Stopped, 
			};
		}

		public async Task LoadAsync(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				throw new ArgumentException("URL cannot be null or empty", "url");
			}
			if (_currentUrl == url && IsLoaded)
			{
				return;
			}
			Stop();
			DisposeCurrentAudio();
			_loadCts?.Cancel();
			_loadCts = new CancellationTokenSource();
			try
			{
				string cachedPath = await DownloadAndCacheAsync(url, _loadCts.Token).ConfigureAwait(continueOnCapturedContext: false);
				if (cachedPath == null)
				{
					throw new InvalidOperationException("Failed to download audio file");
				}
				_audioReader = new AudioFileReader(cachedPath);
				_waveOut = new WaveOutEvent();
				_waveOut.Init(_audioReader);
				_waveOut.PlaybackStopped += OnPlaybackStopped;
				_currentUrl = url;
				OnStateChanged(AudioPlaybackState.Stopped);
			}
			catch (OperationCanceledException)
			{
				Logger.Debug("Audio load was cancelled");
				throw;
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to load audio from URL: {Url}", new object[1] { url });
				throw;
			}
		}

		private async Task<string> DownloadAndCacheAsync(string url, CancellationToken cancellationToken)
		{
			_ = 2;
			try
			{
				string fileName = GetCacheFileName(url);
				string cachedPath = Path.Combine(_cacheDirectory, fileName);
				if (File.Exists(cachedPath))
				{
					return cachedPath;
				}
				HttpResponseMessage response = await _httpClient.GetAsync(url, (HttpCompletionOption)1, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					response.EnsureSuccessStatusCode();
					using Stream stream = await response.get_Content().ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
					using FileStream fileStream = new FileStream(cachedPath, FileMode.Create, FileAccess.Write, FileShare.None);
					await stream.CopyToAsync(fileStream, 81920, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				finally
				{
					((IDisposable)response)?.Dispose();
				}
				return cachedPath;
			}
			catch (Exception ex) when (!(ex is OperationCanceledException))
			{
				Logger.Warn(ex, "Failed to download audio file: {Url}", new object[1] { url });
				return null;
			}
		}

		private string GetCacheFileName(string url)
		{
			string hash = GetStableHashCode(url).ToString("X8");
			string extension = ".mp3";
			try
			{
				if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
				{
					string pathExtension = Path.GetExtension(uri.AbsolutePath);
					if (!string.IsNullOrEmpty(pathExtension))
					{
						extension = pathExtension;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Could not parse URL for extension, using default .mp3");
			}
			return "audio_" + hash + extension;
		}

		private static uint GetStableHashCode(string str)
		{
			uint hash = 2166136261u;
			foreach (char c in str)
			{
				hash = (hash ^ c) * 16777619;
			}
			return hash;
		}

		public void Play()
		{
			if (_waveOut != null && _audioReader != null)
			{
				try
				{
					_waveOut.Play();
					OnStateChanged(AudioPlaybackState.Playing);
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to play audio");
				}
			}
		}

		public void Pause()
		{
			if (_waveOut != null)
			{
				try
				{
					_waveOut.Pause();
					OnStateChanged(AudioPlaybackState.Paused);
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to pause audio");
				}
			}
		}

		public void Stop()
		{
			if (_waveOut == null)
			{
				return;
			}
			try
			{
				_waveOut.Stop();
				if (_audioReader != null)
				{
					_audioReader.Position = 0L;
				}
				OnStateChanged(AudioPlaybackState.Stopped);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to stop audio");
			}
		}

		public void Seek(TimeSpan position)
		{
			if (_audioReader != null)
			{
				try
				{
					long clampedTicks = Math.Max(0L, Math.Min(position.Ticks, _audioReader.TotalTime.Ticks));
					_audioReader.CurrentTime = TimeSpan.FromTicks(clampedTicks);
					OnPositionChanged();
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to seek audio");
				}
			}
		}

		public void SetVolume(float volume)
		{
			if (_waveOut != null)
			{
				_waveOut.Volume = MathHelper.Clamp(volume, 0f, 1f);
			}
		}

		public void UpdatePosition()
		{
			if (_audioReader != null)
			{
				WaveOutEvent waveOut = _waveOut;
				if (waveOut != null && waveOut.PlaybackState == NAudio.Wave.PlaybackState.Playing)
				{
					OnPositionChanged();
				}
			}
		}

		private void OnPlaybackStopped(object sender, StoppedEventArgs e)
		{
			if (e.Exception != null)
			{
				Logger.Warn(e.Exception, "Audio playback stopped with error");
			}
			if (_audioReader != null && _audioReader.CurrentTime >= _audioReader.TotalTime - TimeSpan.FromMilliseconds(100.0))
			{
				_audioReader.Position = 0L;
			}
			OnStateChanged(AudioPlaybackState.Stopped);
		}

		private void OnStateChanged(AudioPlaybackState state)
		{
			this.StateChanged?.Invoke(this, new AudioStateChangedEventArgs(state));
		}

		private void OnPositionChanged()
		{
			this.PositionChanged?.Invoke(this, new AudioPositionChangedEventArgs(CurrentPosition, TotalDuration));
		}

		private void DisposeCurrentAudio()
		{
			if (_waveOut != null)
			{
				_waveOut.PlaybackStopped -= OnPlaybackStopped;
				_waveOut.Dispose();
				_waveOut = null;
			}
			if (_audioReader != null)
			{
				_audioReader.Dispose();
				_audioReader = null;
			}
			_currentUrl = null;
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;
				_loadCts?.Cancel();
				_loadCts?.Dispose();
				DisposeCurrentAudio();
				HttpClient httpClient = _httpClient;
				if (httpClient != null)
				{
					((HttpMessageInvoker)httpClient).Dispose();
				}
			}
		}
	}
}
