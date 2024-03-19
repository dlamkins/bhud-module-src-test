using System;
using Nekres.Music_Mixer.Core.Services.Audio.Source.DSP;

namespace Nekres.Music_Mixer.Core.Services.Audio.Source.Equalizer
{
	public class EqualizerChannelFilter : ICloneable
	{
		private readonly PeakFilter _biQuadFilter;

		public double GainDB
		{
			get
			{
				return _biQuadFilter.GainDB;
			}
			set
			{
				_biQuadFilter.GainDB = value;
			}
		}

		public double BandWidth
		{
			get
			{
				return _biQuadFilter.BandWidth;
			}
			set
			{
				_biQuadFilter.BandWidth = value;
			}
		}

		public double Frequency => _biQuadFilter.Frequency;

		public int SampleRate => _biQuadFilter.SampleRate;

		public EqualizerChannelFilter(int sampleRate, double centerFrequency, double bandWidth, double gain)
		{
			if (sampleRate <= 0)
			{
				throw new ArgumentOutOfRangeException("sampleRate");
			}
			if (centerFrequency <= 0.0)
			{
				throw new ArgumentOutOfRangeException("centerFrequency");
			}
			if (bandWidth <= 0.0)
			{
				throw new ArgumentOutOfRangeException("bandWidth");
			}
			_biQuadFilter = new PeakFilter(sampleRate, centerFrequency, bandWidth, gain);
		}

		public object Clone()
		{
			return new EqualizerChannelFilter(SampleRate, Frequency, BandWidth, GainDB);
		}

		public void Process(float[] input, int offset, int count, int channelIndex, int channelCount)
		{
			for (int i = channelIndex + offset; i < count + offset; i += channelCount)
			{
				input[i] = _biQuadFilter.Process(input[i]);
			}
		}
	}
}
