using Blish_HUD;
using Microsoft.Xna.Framework;
using NAudio.Wave;

namespace Nekres.Music_Mixer.Core.Services.Audio.Source
{
	public class SubmergedVolumeProvider : ISampleProvider
	{
		private readonly ISampleProvider _source;

		public float Volume { get; set; }

		public bool Enabled { get; set; }

		public WaveFormat WaveFormat => _source.get_WaveFormat();

		public SubmergedVolumeProvider(ISampleProvider source)
		{
			_source = source;
			Volume = 1f;
		}

		public int Read(float[] buffer, int offset, int sampleCount)
		{
			float volume = GetDepthAdjustedVolume();
			int num = _source.Read(buffer, offset, sampleCount);
			if ((double)volume != 1.0)
			{
				for (int index = 0; index < sampleCount; index++)
				{
					buffer[offset + index] *= volume;
				}
			}
			return num;
		}

		private float GetDepthAdjustedVolume()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			float normalized = AudioUtil.GetNormalizedVolume(Volume);
			if (Enabled)
			{
				return MathHelper.Clamp(Map(GameService.Gw2Mumble.get_PlayerCamera().get_Position().Z, -130f, AudioUtil.GetNormalizedVolume(0.1f), 0f, normalized), 0f, 0.1f);
			}
			return normalized;
		}

		private static float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
		{
			return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
		}
	}
}
