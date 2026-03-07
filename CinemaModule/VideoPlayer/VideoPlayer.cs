using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using LibVLCSharp.Shared;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.VideoPlayer
{
	public class VideoPlayer : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<VideoPlayer>();

		private readonly GraphicsDevice _graphicsDevice;

		private readonly LibVLC _libVLC;

		private readonly MediaPlayer _mediaPlayer;

		private readonly VideoBuffer _buffer;

		private readonly VideoFormatHandler _formatHandler;

		private readonly VideoCallbackHandler _callbackHandler;

		private Texture2D _videoTexture;

		private bool _isDisposed;

		private string _currentUrl;

		private List<VideoQuality> _availableQualities = new List<VideoQuality>();

		private int _selectedQualityIndex = -1;

		private volatile bool _qualitiesNeedRefresh;

		private PlaybackState _lastReportedState;

		public Texture2D VideoTexture => _videoTexture;

		public bool IsPlaying => _mediaPlayer?.IsPlaying ?? false;

		public bool IsPaused
		{
			get
			{
				MediaPlayer mediaPlayer = _mediaPlayer;
				if (mediaPlayer == null)
				{
					return false;
				}
				return mediaPlayer.State == VLCState.Paused;
			}
		}

		public bool IsEnded
		{
			get
			{
				MediaPlayer mediaPlayer = _mediaPlayer;
				if (mediaPlayer == null)
				{
					return false;
				}
				return mediaPlayer.State == VLCState.Ended;
			}
		}

		public bool IsBuffering
		{
			get
			{
				VLCState? state = _mediaPlayer?.State;
				if (state.GetValueOrDefault() != VLCState.Buffering)
				{
					return state.GetValueOrDefault() == VLCState.Opening;
				}
				return true;
			}
		}

		public int Volume
		{
			get
			{
				return _mediaPlayer?.Volume ?? 0;
			}
			set
			{
				if (_mediaPlayer != null)
				{
					_mediaPlayer.Volume = Math.Max(0, Math.Min(100, value));
				}
			}
		}

		public float Position
		{
			get
			{
				return _mediaPlayer?.Position ?? 0f;
			}
			set
			{
				if (_mediaPlayer != null)
				{
					_mediaPlayer.Position = Math.Max(0f, Math.Min(1f, value));
				}
			}
		}

		public long Length => _mediaPlayer?.Length ?? 0;

		public bool IsSeekable => _mediaPlayer?.IsSeekable ?? false;

		public int VideoWidth => (int)_formatHandler.Width;

		public int VideoHeight => (int)_formatHandler.Height;

		public string CurrentUrl => _currentUrl;

		public IReadOnlyList<VideoQuality> AvailableQualities => _availableQualities;

		public int SelectedQualityIndex => _selectedQualityIndex;

		public VideoQuality SelectedQuality
		{
			get
			{
				if (_selectedQualityIndex < 0 || _selectedQualityIndex >= _availableQualities.Count)
				{
					return null;
				}
				return _availableQualities[_selectedQualityIndex];
			}
		}

		public event EventHandler FrameReady;

		public event EventHandler<PlaybackStateEventArgs> PlaybackStateChanged;

		public event EventHandler QualitiesChanged;

		public VideoPlayer(GraphicsDevice device, VideoPlayerOptions options)
		{
			_graphicsDevice = device;
			string[] libVlcOptions = BuildLibVlcOptions(options);
			_libVLC = new LibVLC(libVlcOptions);
			_mediaPlayer = new MediaPlayer(_libVLC);
			_buffer = new VideoBuffer();
			_formatHandler = new VideoFormatHandler(_buffer);
			_callbackHandler = new VideoCallbackHandler(_buffer);
			SetupCallbacks();
			SetupEventHandlers();
		}

		private string[] BuildLibVlcOptions(VideoPlayerOptions options)
		{
			List<string> optionsList = new List<string>
			{
				"--no-osd",
				$"--network-caching={options.NetworkCachingMs}",
				"--adaptive-logic=highest",
				$"--adaptive-maxwidth={options.MaxWidth}",
				$"--adaptive-maxheight={options.MaxHeight}",
				$"--preferred-resolution={options.PreferredResolution}"
			};
			if (options.EnableHardwareAcceleration)
			{
				optionsList.Add("--avcodec-hw=any");
			}
			if (options.AdditionalLibVlcOptions != null)
			{
				optionsList.AddRange(options.AdditionalLibVlcOptions);
			}
			return optionsList.ToArray();
		}

		private void SetupCallbacks()
		{
			_mediaPlayer.SetVideoFormatCallbacks(new MediaPlayer.LibVLCVideoFormatCb(_formatHandler.HandleFormatCallback), null);
			_mediaPlayer.SetVideoCallbacks(new MediaPlayer.LibVLCVideoLockCb(_callbackHandler.LockCallback), null, new MediaPlayer.LibVLCVideoDisplayCb(_callbackHandler.DisplayCallback));
		}

		private void SetupEventHandlers()
		{
			_mediaPlayer.Playing += delegate
			{
				_qualitiesNeedRefresh = true;
			};
			_mediaPlayer.ESAdded += delegate(object s, MediaPlayerESAddedEventArgs e)
			{
				if (e.Type == TrackType.Video)
				{
					_qualitiesNeedRefresh = true;
				}
			};
		}

		public void Play(string url, string audioSlaveUrl = null)
		{
			if (string.IsNullOrEmpty(url))
			{
				return;
			}
			if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
			{
				OnPlaybackStateChanged(PlaybackState.Error);
				return;
			}
			Stop();
			_currentUrl = url;
			using Media media = new Media(_libVLC, uri);
			if (!string.IsNullOrEmpty(audioSlaveUrl))
			{
				media.AddOption(":input-slave=" + audioSlaveUrl);
			}
			_mediaPlayer.Play(media);
		}

		public void Pause()
		{
			if (IsPlaying)
			{
				_mediaPlayer.SetPause(pause: true);
			}
		}

		public void Resume()
		{
			if (!IsPlaying && !IsBuffering)
			{
				if (IsPaused)
				{
					_mediaPlayer.SetPause(pause: false);
				}
				else if (IsEnded && !string.IsNullOrEmpty(_currentUrl))
				{
					string url = _currentUrl;
					Play(url);
				}
			}
		}

		public void TogglePause()
		{
			if (IsPlaying)
			{
				Pause();
			}
			else
			{
				Resume();
			}
		}

		public void Stop()
		{
			_mediaPlayer.Stop();
			_formatHandler.Reset();
			Texture2D videoTexture = _videoTexture;
			if (videoTexture != null)
			{
				((GraphicsResource)videoTexture).Dispose();
			}
			_videoTexture = null;
			_currentUrl = null;
			_availableQualities.Clear();
			_selectedQualityIndex = -1;
		}

		public void RefreshAvailableQualities()
		{
			try
			{
				Media media = _mediaPlayer.Media;
				if (media == null)
				{
					_availableQualities.Clear();
					_selectedQualityIndex = -1;
					return;
				}
				MediaTrack[] tracks = media.Tracks;
				if (tracks == null || tracks.Length == 0)
				{
					return;
				}
				List<VideoQuality> newQualities = new List<VideoQuality>();
				int currentTrackId = _mediaPlayer.VideoTrack;
				MediaTrack[] array = tracks;
				for (int i = 0; i < array.Length; i++)
				{
					MediaTrack track = array[i];
					if (track.TrackType == TrackType.Video)
					{
						VideoQuality quality = new VideoQuality
						{
							TrackId = track.Id,
							Width = (int)track.Data.Video.Width,
							Height = (int)track.Data.Video.Height,
							Name = BuildQualityName(track),
							IsSelected = (track.Id == currentTrackId)
						};
						newQualities.Add(quality);
					}
				}
				newQualities = (from q in newQualities
					orderby q.Height descending, q.Width descending
					select q).ToList();
				HashSet<string> seenNames = new HashSet<string>();
				newQualities = newQualities.Where(delegate(VideoQuality q)
				{
					if (seenNames.Contains(q.Name))
					{
						return false;
					}
					seenNames.Add(q.Name);
					return true;
				}).ToList();
				int selectedIdx = newQualities.FindIndex((VideoQuality q) => q.IsSelected);
				bool num = !AreQualitiesEqual(_availableQualities, newQualities);
				_availableQualities = newQualities;
				_selectedQualityIndex = selectedIdx;
				if (num)
				{
					this.QualitiesChanged?.Invoke(this, EventArgs.Empty);
				}
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to refresh available qualities - will retry");
			}
		}

		private string BuildQualityName(MediaTrack track)
		{
			return VideoQuality.GetQualityName((int)track.Data.Video.Width, (int)track.Data.Video.Height);
		}

		private bool AreQualitiesEqual(List<VideoQuality> a, List<VideoQuality> b)
		{
			if (a.Count != b.Count)
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (a[i].TrackId != b[i].TrackId || a[i].Height != b[i].Height)
				{
					return false;
				}
			}
			return true;
		}

		public void SetQuality(int index)
		{
			if (index >= 0 && index < _availableQualities.Count)
			{
				VideoQuality quality = _availableQualities[index];
				SetQualityByTrackId(quality.TrackId);
			}
		}

		public void SetQualityByTrackId(int trackId)
		{
			try
			{
				_mediaPlayer.SetVideoTrack(trackId);
				for (int i = 0; i < _availableQualities.Count; i++)
				{
					_availableQualities[i].IsSelected = _availableQualities[i].TrackId == trackId;
					if (_availableQualities[i].IsSelected)
					{
						_selectedQualityIndex = i;
					}
				}
				this.QualitiesChanged?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, $"Failed to set quality to track ID {trackId}");
			}
		}

		public void Update()
		{
			if (_isDisposed)
			{
				return;
			}
			SyncPlaybackState();
			if (_qualitiesNeedRefresh)
			{
				_qualitiesNeedRefresh = false;
				RefreshAvailableQualities();
			}
			if (_formatHandler.IsInitialized)
			{
				bool needsNewTexture = _videoTexture == null;
				if (_videoTexture != null && (_videoTexture.get_Width() != (int)_formatHandler.Width || _videoTexture.get_Height() != (int)_formatHandler.Height))
				{
					((GraphicsResource)_videoTexture).Dispose();
					_videoTexture = null;
					needsNewTexture = true;
				}
				if (needsNewTexture)
				{
					CreateTexture();
				}
			}
			if (_callbackHandler.IsFrameDirty && _videoTexture != null)
			{
				_buffer.CopyToTextureWithPitch(_videoTexture, (int)_formatHandler.Width, (int)_formatHandler.Height, _formatHandler.Pitch);
				_callbackHandler.ClearFrameDirty();
				this.FrameReady?.Invoke(this, EventArgs.Empty);
			}
		}

		private void CreateTexture()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			int width = (int)_formatHandler.Width;
			int height = (int)_formatHandler.Height;
			if (width > 0 && height > 0)
			{
				_videoTexture = new Texture2D(_graphicsDevice, width, height, false, (SurfaceFormat)0);
			}
		}

		private void SyncPlaybackState()
		{
			PlaybackState actualState = GetActualPlaybackState();
			if (actualState != _lastReportedState)
			{
				OnPlaybackStateChanged(actualState);
			}
		}

		private PlaybackState GetActualPlaybackState()
		{
			if (_mediaPlayer == null)
			{
				return PlaybackState.Stopped;
			}
			switch (_mediaPlayer.State)
			{
			case VLCState.Playing:
				return PlaybackState.Playing;
			case VLCState.Paused:
				return PlaybackState.Paused;
			case VLCState.Stopped:
				return PlaybackState.Stopped;
			case VLCState.Ended:
				return PlaybackState.Ended;
			case VLCState.Error:
				return PlaybackState.Error;
			case VLCState.Opening:
			case VLCState.Buffering:
				return PlaybackState.Buffering;
			default:
				return _lastReportedState;
			}
		}

		private void OnPlaybackStateChanged(PlaybackState state)
		{
			_lastReportedState = state;
			this.PlaybackStateChanged?.Invoke(this, new PlaybackStateEventArgs(state));
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_mediaPlayer?.Stop();
				_mediaPlayer?.Dispose();
				_libVLC?.Dispose();
				_buffer?.Dispose();
				Texture2D videoTexture = _videoTexture;
				if (videoTexture != null)
				{
					((GraphicsResource)videoTexture).Dispose();
				}
			}
		}
	}
}
