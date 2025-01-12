using System;
using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	public class MeasurerGhost : IMeasurer
	{
		public PosSnapshot Pos { get; private set; } = PosSnapshot.Empty;


		public SpeedTime Speed { get; private set; } = SpeedTime.Empty;


		public AccelTime Accel { get; private set; } = AccelTime.Empty;


		public event Action<PosSnapshot>? NewPosition;

		public event Action? Teleported;

		public void Update(Ghost ghost, TimeSpan time)
		{
			(GhostSnapshot a, GhostSnapshot b, GhostSnapshot c) tuple = ghost.TripleSnapshot(time);
			GhostSnapshot a2 = tuple.a;
			GhostSnapshot b2 = tuple.b;
			GhostSnapshot item = tuple.c;
			PosSnapshot posA = new PosSnapshot(a2);
			PosSnapshot posB = new PosSnapshot(b2);
			PosSnapshot posC = new PosSnapshot(item);
			SpeedTime speedA = new SpeedTime(posA, posB, getDt(posA.Time, posB.Time));
			SpeedTime speedB = new SpeedTime(posB, posC, getDt(posB.Time, posC.Time));
			if (SpeedTime.IsDoubling(speedB, speedA))
			{
				speedA = new SpeedTime(posA, posB, getDt(posA.Time, posB.Time, isDouble: true));
			}
			if (SpeedTime.IsDoubling(speedA, speedB))
			{
				speedB = new SpeedTime(posB, posC, getDt(posB.Time, posC.Time, isDouble: true));
			}
			Pos = posC;
			Speed = speedB;
			Accel = new AccelTime(speedA, speedB, getDt(speedA.Time, speedB.Time));
			static TimeSpan getDt(TimeSpan a, TimeSpan b, bool isDouble = false)
			{
				double fts = Math.Max(Math.Floor((b - a).TotalSeconds / MumbleEngine.PosUpdateRate.TotalSeconds), 1.0);
				if (isDouble)
				{
					fts *= 2.0;
				}
				return TimeSpan.FromSeconds(MumbleEngine.PosUpdateRate.TotalSeconds * fts);
			}
		}
	}
}
