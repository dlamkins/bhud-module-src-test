using NAudio.Wave;
using Nekres.Music_Mixer.Core.Services.Audio.Source.DSP;

namespace Nekres.Music_Mixer.Core.Services.Audio.Source
{
	internal class BiQuadFilterSource : ISampleProvider
	{
		private readonly object _lockObject = new object();

		private BiQuad _biquad;

		private ISampleProvider _sourceProvider;

		public WaveFormat WaveFormat => _sourceProvider.get_WaveFormat();

		public bool Enabled { get; set; }

		public BiQuad Filter
		{
			get
			{
				return _biquad;
			}
			set
			{
				lock (_lockObject)
				{
					_biquad = value;
				}
			}
		}

		public BiQuadFilterSource(ISampleProvider source)
		{
			_sourceProvider = source;
		}

		public int Read(float[] buffer, int offset, int count)
		{
			int read = _sourceProvider.Read(buffer, offset, count);
			lock (_lockObject)
			{
				if (Filter != null)
				{
					if (Enabled)
					{
						for (int i = 0; i < read; i++)
						{
							buffer[i + offset] = Filter.Process(buffer[i + offset]);
						}
						return read;
					}
					return read;
				}
				return read;
			}
		}
	}
}
