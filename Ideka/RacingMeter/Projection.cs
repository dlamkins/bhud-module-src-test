using System;
using Ideka.NetCommon;

namespace Ideka.RacingMeter
{
	public class Projection
	{
		public const int SoftMaxBP = 99;

		private Action<Projection>? _update;

		private ProjectionAnchors? _anchors;

		public double MinValue { get; set; }

		public double MaxValue { get; set; }

		public double SoftMax { get; private set; }

		public double FullExtent => MaxValue - MinValue;

		public double Portion(IMeter meter, double value, bool clamp = false)
		{
			return MathUtils.InverseLerp(MinValue, MaxValue, value, clamp) * meter.FullPortion;
		}

		public double Point(IMeter meter, double value, bool clamp = false)
		{
			return meter.MinProjected + Portion(meter, value, clamp);
		}

		public double GetAnchorLevel()
		{
			return _anchors?.GetLevel() ?? 0.0;
		}

		public int GetBP(double value)
		{
			return (int)Math.Round(value * (99.0 / SoftMax));
		}

		public bool OverSoftMax(double value)
		{
			return GetBP(value) > 99;
		}

		public Projection WithSoftMax(double softMax)
		{
			SoftMax = softMax;
			return this;
		}

		public Projection WithUpdate(Action<Projection> update)
		{
			_update = update;
			return this;
		}

		public Projection WithAnchors(ProjectionAnchors? anchors)
		{
			_anchors = anchors;
			return this;
		}

		public T AddAnchor<T>(T anchor) where T : IProjectionAnchor
		{
			_anchors?.AddAnchor(anchor);
			return anchor;
		}

		public void Update()
		{
			_anchors?.Update(this);
			_update?.Invoke(this);
		}

		public static Projection MinMax(double min, double max)
		{
			return new Projection
			{
				MinValue = min,
				MaxValue = max
			};
		}

		public static Projection ZeroTo(double max)
		{
			return MinMax(0.0, max);
		}

		public static Projection Symmetric(double amplitude)
		{
			return MinMax((0.0 - amplitude) / 2.0, amplitude / 2.0);
		}
	}
}
