using System;

namespace Nekres.Music_Mixer.Core.Services.Audio.Source.DSP
{
	public abstract class BiQuad
	{
		protected double A0;

		protected double A1;

		protected double A2;

		protected double B1;

		protected double B2;

		private double _q;

		private double _gainDB;

		protected double Z1;

		protected double Z2;

		private double _frequency;

		public double Frequency
		{
			get
			{
				return _frequency;
			}
			set
			{
				if ((double)SampleRate < value * 2.0)
				{
					throw new ArgumentOutOfRangeException("value", "The samplerate has to be bigger than 2 * frequency.");
				}
				_frequency = value;
				CalculateBiQuadCoefficients();
			}
		}

		public int SampleRate { get; private set; }

		public double Q
		{
			get
			{
				return _q;
			}
			set
			{
				if (value <= 0.0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_q = value;
				CalculateBiQuadCoefficients();
			}
		}

		public double GainDB
		{
			get
			{
				return _gainDB;
			}
			set
			{
				_gainDB = value;
				CalculateBiQuadCoefficients();
			}
		}

		protected BiQuad(int sampleRate, double frequency)
			: this(sampleRate, frequency, 1.0 / Math.Sqrt(2.0))
		{
		}

		protected BiQuad(int sampleRate, double frequency, double q)
		{
			if (sampleRate <= 0)
			{
				throw new ArgumentOutOfRangeException("sampleRate");
			}
			if (frequency <= 0.0)
			{
				throw new ArgumentOutOfRangeException("frequency");
			}
			if (q <= 0.0)
			{
				throw new ArgumentOutOfRangeException("q");
			}
			SampleRate = sampleRate;
			Frequency = frequency;
			Q = q;
			GainDB = 6.0;
		}

		public float Process(float input)
		{
			double o = (double)input * A0 + Z1;
			Z1 = (double)input * A1 + Z2 - B1 * o;
			Z2 = (double)input * A2 - B2 * o;
			return (float)o;
		}

		public void Process(float[] input)
		{
			for (int i = 0; i < input.Length; i++)
			{
				input[i] = Process(input[i]);
			}
		}

		protected abstract void CalculateBiQuadCoefficients();
	}
}
