using System;
using NAudio.Wave;

namespace Nekres.Music_Mixer.Core.Services.Audio.Source
{
	internal class EndOfStreamProvider : ISampleProvider
	{
		private MediaFoundationReader _mediaProvider;

		private ISampleProvider _sourceProvider;

		private bool _ended;

		public WaveFormat WaveFormat => _sourceProvider.get_WaveFormat();

		public bool IsBuffering { get; private set; }

		public event EventHandler<EventArgs> Ended;

		public EndOfStreamProvider(MediaFoundationReader mediaProvider)
		{
			_mediaProvider = mediaProvider;
			_sourceProvider = WaveExtensionMethods.ToSampleProvider((IWaveProvider)(object)mediaProvider);
		}

		public int Read(float[] buffer, int offset, int count)
		{
			int read = _sourceProvider.Read(buffer, offset, count);
			IsBuffering = read <= 0;
			if (((WaveStream)_mediaProvider).get_CurrentTime() < ((WaveStream)_mediaProvider).get_TotalTime() || _ended)
			{
				return read;
			}
			_ended = true;
			this.Ended?.Invoke(this, EventArgs.Empty);
			return read;
		}
	}
}
