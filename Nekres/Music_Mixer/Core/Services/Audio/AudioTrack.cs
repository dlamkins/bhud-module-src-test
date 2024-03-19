using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Blish_HUD;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Nekres.Music_Mixer.Core.Services.Audio.Source;
using Nekres.Music_Mixer.Core.Services.Audio.Source.DSP;
using Nekres.Music_Mixer.Core.Services.Audio.Source.Equalizer;
using Nekres.Music_Mixer.Core.Services.Data;

namespace Nekres.Music_Mixer.Core.Services.Audio
{
	internal class AudioTrack : IDisposable
	{
		public static AudioTrack Empty = new AudioTrack();

		public readonly bool IsEmpty;

		private bool _muted;

		public readonly AudioSource Source;

		private readonly WasapiOut _outputDevice;

		private readonly MediaFoundationReader _mediaProvider;

		private readonly EndOfStreamProvider _endOfStream;

		private readonly SubmergedVolumeProvider _volumeProvider;

		private readonly FadeInOutSampleProvider _fadeInOut;

		private readonly BiQuadFilterSource _lowPassFilter;

		private readonly Equalizer _equalizer;

		private bool _initialized;

		public bool Muted
		{
			get
			{
				return _muted;
			}
			set
			{
				_muted = value;
				if (_volumeProvider != null)
				{
					_volumeProvider.Volume = (value ? 0f : Source.Volume);
				}
			}
		}

		public TimeSpan CurrentTime => ((WaveStream)_mediaProvider).get_CurrentTime();

		public TimeSpan TotalTime => ((WaveStream)_mediaProvider).get_TotalTime();

		public bool IsBuffering => _endOfStream.IsBuffering;

		public event EventHandler<EventArgs> Finished;

		private AudioTrack()
		{
			IsEmpty = true;
		}

		private AudioTrack(AudioSource source)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			Source = source;
			_outputDevice = new WasapiOut(GameService.GameIntegration.get_Audio().get_AudioDevice(), (AudioClientShareMode)0, false, 100);
			_mediaProvider = new MediaFoundationReader(Source.AudioUrl);
			_endOfStream = new EndOfStreamProvider(_mediaProvider);
			_endOfStream.Ended += OnEndOfStreamReached;
			_volumeProvider = new SubmergedVolumeProvider((ISampleProvider)(object)_endOfStream)
			{
				Volume = Source.Volume
			};
			Source.VolumeChanged += OnVolumeChanged;
			_fadeInOut = new FadeInOutSampleProvider((ISampleProvider)(object)_volumeProvider, false);
			_lowPassFilter = new BiQuadFilterSource((ISampleProvider)(object)_fadeInOut)
			{
				Filter = new LowPassFilter(_fadeInOut.get_WaveFormat().get_SampleRate(), 400.0)
			};
			_equalizer = Equalizer.Create10BandEqualizer((ISampleProvider)(object)_lowPassFilter);
		}

		public static async Task<AudioTrack> TryGetStream(AudioSource source, int retries = 3, int delayMs = 500, Logger logger = null)
		{
			if (string.IsNullOrWhiteSpace(source.AudioUrl))
			{
				return Empty;
			}
			if (logger == null)
			{
				logger = Logger.GetLogger(typeof(AudioTrack));
			}
			try
			{
				return new AudioTrack(source);
			}
			catch (Exception e)
			{
				if (retries > 0)
				{
					logger.Info(e, $"Failed to create audio output stream. Remaining retries: {retries}.");
					await Task.Delay(delayMs);
					return await TryGetStream(source, retries - 1, delayMs, logger);
				}
				Exception ex = e;
				if (!(ex is InvalidCastException))
				{
					if (!(ex is UnauthorizedAccessException))
					{
						if (ex is COMException)
						{
							if (e.HResult == -2004287480)
							{
								logger.Warn(e, "Output device does not support shared mode.");
							}
							else if (e.HResult == -2147221164)
							{
								logger.Warn(e, "Output device unsupported. Component class not registered.");
							}
							else if (e.HResult == -2147023728)
							{
								logger.Warn(e, "Output device is not supported or was not found.");
							}
							else if (e.HResult == -2147024891)
							{
								logger.Warn(e, "Output device unavailable. Access denied.");
							}
							else if (e.HResult == -2004287478)
							{
								logger.Warn(e, "Output device unavailable. Device is being used in exclusive mode.");
							}
							else
							{
								logger.Warn(e, $"Output device unavailable. HRESULT: {e.HResult}");
							}
						}
						else
						{
							logger.Warn(e, e.Message);
						}
					}
					else
					{
						logger.Info(e, "Output device unavailable. Access denied.");
					}
				}
				else
				{
					logger.Warn(e, e.Message);
				}
			}
			return Empty;
		}

		public async Task Play(int fadeInDuration = 500, int retries = 3, int delayMs = 500, Logger logger = null)
		{
			if (logger == null)
			{
				logger = Logger.GetLogger(typeof(AudioTrack));
			}
			try
			{
				if (!_initialized)
				{
					_initialized = true;
					WaveExtensionMethods.Init((IWavePlayer)(object)_outputDevice, (ISampleProvider)(object)_equalizer, false);
				}
				_outputDevice.Play();
				_fadeInOut.BeginFadeIn((double)fadeInDuration);
				ToggleSubmergedFx(MusicMixer.Instance.Gw2State.IsSubmerged);
			}
			catch (Exception e)
			{
				if (retries > 0)
				{
					await Task.Delay(delayMs);
					await Play(fadeInDuration, retries - 1, delayMs, logger);
				}
				else if (!(e is InvalidCastException))
				{
					if (!(e is UnauthorizedAccessException))
					{
						if (e is COMException)
						{
							logger.Warn(e, $"Output device unavailable. HRESULT: {e.HResult}");
						}
						else
						{
							logger.Warn(e, e.Message);
						}
					}
					else
					{
						logger.Info(e, "Access to output device denied.");
					}
				}
				else
				{
					logger.Warn(e, e.Message);
				}
			}
		}

		public void Seek(float seconds)
		{
			if (!IsEmpty)
			{
				((WaveStream)(object)_mediaProvider)?.SetPosition(seconds);
			}
		}

		public void Seek(TimeSpan timespan)
		{
			if (!IsEmpty)
			{
				((WaveStream)(object)_mediaProvider)?.SetPosition(timespan);
			}
		}

		public void Pause()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Invalid comparison between Unknown and I4
			if (!IsEmpty && _outputDevice != null && (int)_outputDevice.get_PlaybackState() != 2)
			{
				_outputDevice.Pause();
			}
		}

		public void Resume()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Invalid comparison between Unknown and I4
			if (!IsEmpty && _outputDevice != null && (int)_outputDevice.get_PlaybackState() == 2)
			{
				_outputDevice.Play();
			}
		}

		public void ToggleSubmergedFx(bool enable)
		{
			if (!IsEmpty && _equalizer != null)
			{
				_lowPassFilter.Enabled = enable;
				_volumeProvider.Enabled = enable;
				_equalizer.SampleFilters[1].AverageGainDB = (enable ? 19.5f : 0f);
				_equalizer.SampleFilters[9].AverageGainDB = (enable ? 13.4f : 0f);
			}
		}

		public void Dispose()
		{
			if (!IsEmpty)
			{
				_endOfStream.Ended -= OnEndOfStreamReached;
				DisposeMediaInterfaces();
			}
		}

		public async Task DisposeAsync()
		{
			if (!IsEmpty)
			{
				_endOfStream.Ended -= OnEndOfStreamReached;
				_fadeInOut.BeginFadeOut(2000.0);
				await Task.Delay(2005).ContinueWith(delegate
				{
					DisposeMediaInterfaces();
				});
			}
		}

		private void DisposeMediaInterfaces()
		{
			Source.VolumeChanged -= OnVolumeChanged;
			try
			{
				WasapiOut outputDevice = _outputDevice;
				if (outputDevice != null)
				{
					outputDevice.Dispose();
				}
				((Stream)(object)_mediaProvider)?.Dispose();
			}
			catch (Exception ex) when (((ex is NullReferenceException || ex is ObjectDisposedException) ? 1 : 0) != 0)
			{
			}
		}

		private void OnVolumeChanged(object sender, ValueEventArgs<float> e)
		{
			_volumeProvider.Volume = e.get_Value();
		}

		private void OnEndOfStreamReached(object o, EventArgs e)
		{
			_endOfStream.Ended -= OnEndOfStreamReached;
			this.Finished?.Invoke(this, EventArgs.Empty);
		}
	}
}
