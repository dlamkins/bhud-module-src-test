using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Nekres.Music_Mixer.Core.Services.Audio.Source.Equalizer
{
	[DebuggerDisplay("{AverageFrequency}Hz")]
	public sealed class EqualizerFilter : IEnumerable<KeyValuePair<int, EqualizerChannelFilter>>, IEnumerable, IComparable<EqualizerFilter>
	{
		public Dictionary<int, EqualizerChannelFilter> Filters { get; private set; }

		public double AverageFrequency
		{
			get
			{
				if (Filters == null || Filters.Count <= 0)
				{
					return 0.0;
				}
				return Filters.Average((KeyValuePair<int, EqualizerChannelFilter> x) => x.Value.Frequency);
			}
		}

		public double AverageGainDB
		{
			get
			{
				return Filters.Average((KeyValuePair<int, EqualizerChannelFilter> x) => x.Value.GainDB);
			}
			set
			{
				SetGain(value);
			}
		}

		public EqualizerFilter()
		{
			Filters = new Dictionary<int, EqualizerChannelFilter>();
		}

		public EqualizerFilter(int channels, EqualizerChannelFilter filter)
			: this()
		{
			Filters.Add(0, filter);
			for (int c = 1; c < channels; c++)
			{
				Filters.Add(c, (EqualizerChannelFilter)filter.Clone());
			}
		}

		int IComparable<EqualizerFilter>.CompareTo(EqualizerFilter other)
		{
			if (other == null)
			{
				return 1;
			}
			return AverageFrequency.CompareTo(other.AverageFrequency);
		}

		public IEnumerator<KeyValuePair<int, EqualizerChannelFilter>> GetEnumerator()
		{
			return Filters.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public static EqualizerFilter CreateFilter(int channels, int sampleRate, double frequency, int bandWidth, float gain)
		{
			EqualizerFilter result = new EqualizerFilter();
			for (int c = 0; c < channels; c++)
			{
				result.Filters.Add(c, new EqualizerChannelFilter(sampleRate, frequency, bandWidth, gain));
			}
			return result;
		}

		private void SetGain(double gainDB)
		{
			foreach (KeyValuePair<int, EqualizerChannelFilter> filter in Filters)
			{
				filter.Value.GainDB = gainDB;
			}
		}
	}
}
