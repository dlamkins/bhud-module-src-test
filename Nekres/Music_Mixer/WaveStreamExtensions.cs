using System;
using System.IO;
using NAudio.Wave;

namespace Nekres.Music_Mixer
{
	public static class WaveStreamExtensions
	{
		public static void SetPosition(this WaveStream strm, long position)
		{
			long adj = position % strm.get_WaveFormat().get_BlockAlign();
			long newPos = (((Stream)(object)strm).Position = Math.Max(0L, Math.Min(((Stream)(object)strm).Length, position - adj)));
		}

		public static void SetPosition(this WaveStream strm, double seconds)
		{
			strm.SetPosition((long)(seconds * (double)strm.get_WaveFormat().get_AverageBytesPerSecond()));
		}

		public static void SetPosition(this WaveStream strm, TimeSpan time)
		{
			strm.SetPosition(time.TotalSeconds);
		}

		public static void Seek(this WaveStream strm, double offset)
		{
			strm.SetPosition(((Stream)(object)strm).Position + (long)(offset * (double)strm.get_WaveFormat().get_AverageBytesPerSecond()));
		}
	}
}
