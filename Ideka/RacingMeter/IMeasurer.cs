using System;

namespace Ideka.RacingMeter
{
	public interface IMeasurer
	{
		PosSnapshot Pos { get; }

		SpeedTime Speed { get; }

		AccelTime Accel { get; }

		event Action<PosSnapshot>? NewPosition;

		event Action? Teleported;
	}
}
