using System;

namespace Nekres.Music_Mixer.Core.Services.Audio.Source.DSP
{
	public class LowPassFilter : BiQuad
	{
		public LowPassFilter(int sampleRate, double frequency)
			: base(sampleRate, frequency)
		{
		}

		protected override void CalculateBiQuadCoefficients()
		{
			double i = Math.Tan(Math.PI * base.Frequency / (double)base.SampleRate);
			double norm = 1.0 / (1.0 + i / base.Q + i * i);
			A0 = i * i * norm;
			A1 = 2.0 * A0;
			A2 = A0;
			B1 = 2.0 * (i * i - 1.0) * norm;
			B2 = (1.0 - i / base.Q + i * i) * norm;
		}
	}
}
