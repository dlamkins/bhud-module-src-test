using System;
using System.Collections;
using System.Collections.Generic;
using NAudio.Wave;

namespace Nekres.Music_Mixer.Core.Services.Audio.Source.Equalizer
{
	public class Equalizer : ISampleProvider
	{
		private class EqualizerFilterCollection : IList<EqualizerFilter>, ICollection<EqualizerFilter>, IEnumerable<EqualizerFilter>, IEnumerable
		{
			private readonly List<EqualizerFilter> _list = new List<EqualizerFilter>();

			public int Count => _list.Count;

			public bool IsReadOnly => false;

			public EqualizerFilter this[int index]
			{
				get
				{
					return _list[index];
				}
				set
				{
					_list[index] = value;
					_list.Sort();
				}
			}

			public IEnumerator<EqualizerFilter> GetEnumerator()
			{
				return _list.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public void Add(EqualizerFilter item)
			{
				_list.Add(item);
				_list.Sort();
			}

			public void Clear()
			{
				_list.Clear();
			}

			public bool Contains(EqualizerFilter item)
			{
				return _list.Contains(item);
			}

			public void CopyTo(EqualizerFilter[] array, int arrayIndex)
			{
				_list.CopyTo(array, arrayIndex);
			}

			public bool Remove(EqualizerFilter item)
			{
				return _list.Remove(item);
			}

			public int IndexOf(EqualizerFilter item)
			{
				return _list.IndexOf(item);
			}

			public void Insert(int index, EqualizerFilter item)
			{
				_list.Insert(index, item);
				_list.Sort();
			}

			public void RemoveAt(int index)
			{
				_list.RemoveAt(index);
			}
		}

		private readonly EqualizerFilterCollection _equalizerFilters = new EqualizerFilterCollection();

		private ISampleProvider _sampleProvider;

		public WaveFormat WaveFormat => _sampleProvider.get_WaveFormat();

		public IList<EqualizerFilter> SampleFilters => _equalizerFilters;

		public Equalizer(ISampleProvider source)
		{
			_sampleProvider = source;
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
		}

		public static Equalizer Create10BandEqualizer(ISampleProvider source)
		{
			return Create10BandEqualizer(source, 18, 0);
		}

		public static Equalizer Create10BandEqualizer(ISampleProvider source, int bandWidth, int defaultGain)
		{
			int sampleRate = source.get_WaveFormat().get_SampleRate();
			int channels = source.get_WaveFormat().get_Channels();
			if (sampleRate < 32000)
			{
				throw new ArgumentException("The sample rate of the source must not be less than 32kHz since the 10 band eq includes a 16kHz filter.", "source");
			}
			EqualizerChannelFilter[] obj = new EqualizerChannelFilter[10]
			{
				new EqualizerChannelFilter(sampleRate, 31.0, bandWidth, defaultGain),
				new EqualizerChannelFilter(sampleRate, 62.0, bandWidth, defaultGain),
				new EqualizerChannelFilter(sampleRate, 125.0, bandWidth, defaultGain),
				new EqualizerChannelFilter(sampleRate, 250.0, bandWidth, defaultGain),
				new EqualizerChannelFilter(sampleRate, 500.0, bandWidth, defaultGain),
				new EqualizerChannelFilter(sampleRate, 1000.0, bandWidth, defaultGain),
				new EqualizerChannelFilter(sampleRate, 2000.0, bandWidth, defaultGain),
				new EqualizerChannelFilter(sampleRate, 4000.0, bandWidth, defaultGain),
				new EqualizerChannelFilter(sampleRate, 8000.0, bandWidth, defaultGain),
				new EqualizerChannelFilter(sampleRate, 16000.0, bandWidth, defaultGain)
			};
			Equalizer equalizer = new Equalizer(source);
			EqualizerChannelFilter[] array = obj;
			foreach (EqualizerChannelFilter equalizerChannelFilter in array)
			{
				equalizer.SampleFilters.Add(new EqualizerFilter(channels, equalizerChannelFilter));
			}
			return equalizer;
		}

		public int Read(float[] buffer, int offset, int count)
		{
			int read = _sampleProvider.Read(buffer, offset, count);
			for (int c = 0; c < WaveFormat.get_Channels(); c++)
			{
				int i = _equalizerFilters.Count;
				while (i-- > 0)
				{
					_equalizerFilters[i].Filters[c].Process(buffer, offset, read, c, WaveFormat.get_Channels());
				}
			}
			for (int j = offset; j < count; j++)
			{
				buffer[j] = Math.Max(-1f, Math.Min(buffer[j], 1f));
			}
			return read;
		}
	}
}
