using System;

namespace Ideka.RacingMeter
{
	public class KalmanFilter
	{
		public static double Integrate(double data, double x, double k)
		{
			return x + (data - x) * k;
		}
	}
	public class KalmanFilter<T>
	{
		public double Q { get; private set; }

		public double R { get; private set; }

		public double P { get; private set; } = 1.0;


		public T X { get; private set; }

		public double K { get; private set; }

		public Func<T, T, double, T> Integrate { get; private set; }

		private KalmanFilter(double q, double r)
		{
			Q = q;
			R = r;
		}

		public KalmanFilter(Func<T, T, T> add, Func<T, double, T> multiply, double q, double r)
			: this(q, r)
		{
			Integrate = (T data, T x, double k) => add(x, multiply(add(data, multiply(x, -1.0)), k));
		}

		public KalmanFilter(Func<T, T, double, T> integrate, double q, double r)
			: this(q, r)
		{
			Integrate = integrate;
		}

		public void Reset()
		{
			P = 1.0;
			X = default(T);
			K = 0.0;
		}

		public T Update(T data, double? q = null, double? r = null)
		{
			Q = q ?? Q;
			R = r ?? R;
			if (X == null)
			{
				X = data;
				return X;
			}
			K = (P + Q) / (P + Q + R);
			P = R * (P + Q) / (R + P + Q);
			X = Integrate(data, X, K);
			return X;
		}
	}
}
