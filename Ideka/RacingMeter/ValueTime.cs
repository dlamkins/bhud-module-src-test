using System;

namespace Ideka.RacingMeter
{
	public abstract class ValueTime : ITimedMeasure
	{
		public abstract TimeSpan Time { get; }

		public static float TimeMagnitude(TimeSpan timeSpan)
		{
			return (float)timeSpan.TotalSeconds;
		}
	}
	public abstract class ValueTime<TData> : ValueTime where TData : ITimedMeasure
	{
		public TData First { get; }

		public TData Second { get; }

		public override TimeSpan Time => Second.Time;

		public TimeSpan MeasuredDeltaTime => Second.Time - First.Time;

		public TimeSpan DeltaTime { get; }

		public bool IsDouble { get; }

		protected ValueTime()
		{
		}

		public ValueTime(TData first, TData second, TimeSpan deltaTime, bool isDouble = false)
		{
			First = first;
			Second = second;
			IsDouble = isDouble;
			DeltaTime = (isDouble ? (deltaTime + deltaTime) : deltaTime);
		}
	}
}