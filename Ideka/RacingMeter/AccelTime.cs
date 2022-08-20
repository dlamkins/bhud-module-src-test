using System;

namespace Ideka.RacingMeter
{
	public class AccelTime : ValueTime<SpeedTime>
	{
		public float Accel2D { get; }

		public float Accel3D { get; }

		public float UpAccel { get; }

		public float DownAccel { get; }

		public AccelTime()
		{
		}

		public AccelTime(SpeedTime first, SpeedTime second, TimeSpan dt)
			: base(first, second, dt, isDouble: false)
		{
			float t = ValueTime.TimeMagnitude(base.DeltaTime);
			Accel2D = (base.Second.Speed2D - base.First.Speed2D) / t;
			Accel3D = (base.Second.Speed3D - base.First.Speed3D) / t;
			UpAccel = (base.Second.UpSpeed - base.First.UpSpeed) / t;
			DownAccel = (base.Second.DownSpeed - base.First.DownSpeed) / t;
		}
	}
}
