using System;

namespace Nekres.Music_Mixer.Core.Services.Audio.Source.DSP
{
	public class PeakFilter : BiQuad
	{
		public double BandWidth
		{
			get
			{
				return base.Q;
			}
			set
			{
				if (value <= 0.0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				base.Q = value;
			}
		}

		public PeakFilter(int sampleRate, double frequency, double bandWidth, double peakGainDB)
			: base(sampleRate, frequency, bandWidth)
		{
			base.GainDB = peakGainDB;
		}

		protected override void CalculateBiQuadCoefficients()
		{
			double v = Math.Pow(10.0, Math.Abs(base.GainDB) / 20.0);
			double i = Math.Tan(Math.PI * base.Frequency / (double)base.SampleRate);
			double q = base.Q;
			if (base.GainDB >= 0.0)
			{
				double norm = 1.0 / (1.0 + 1.0 / q * i + i * i);
				A0 = (1.0 + v / q * i + i * i) * norm;
				A1 = 2.0 * (i * i - 1.0) * norm;
				A2 = (1.0 - v / q * i + i * i) * norm;
				B1 = A1;
				B2 = (1.0 - 1.0 / q * i + i * i) * norm;
			}
			else
			{
				double norm = 1.0 / (1.0 + v / q * i + i * i);
				A0 = (1.0 + 1.0 / q * i + i * i) * norm;
				A1 = 2.0 * (i * i - 1.0) * norm;
				A2 = (1.0 - 1.0 / q * i + i * i) * norm;
				B1 = A1;
				B2 = (1.0 - v / q * i + i * i) * norm;
			}
		}
	}
}
