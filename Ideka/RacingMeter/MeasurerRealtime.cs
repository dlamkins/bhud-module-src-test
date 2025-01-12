using System;
using System.Linq;
using Ideka.NetCommon;

namespace Ideka.RacingMeter
{
	public class MeasurerRealtime : IMeasurer, IDisposable
	{
		private const int TeleportThreshold = 100000;

		public readonly DropOutStack<bool> DoublingStack = new DropOutStack<bool>(50);

		private PosSnapshot? _pos;

		private SpeedTime? _speed;

		private AccelTime? _accel;

		private readonly MumbleEngine _engine;

		private bool _clearing;

		public PosSnapshot Pos => _pos ?? PosSnapshot.Empty;

		public SpeedTime Speed => _speed ?? SpeedTime.Empty;

		public AccelTime Accel => _accel ?? AccelTime.Empty;

		public int MissedTicks { get; private set; }

		public int Doublings { get; private set; }

		public float DoubledDiff { get; private set; }

		public event Action<PosSnapshot>? NewPosition;

		public event Action? Teleported;

		public MeasurerRealtime()
		{
			_engine = new MumbleEngine();
			_engine.FrameTick += new Action<TimeSpan>(FrameTick);
			_engine.PositionTick += new Action<TimeSpan, TimeSpan>(PositionTick);
		}

		public void Reset()
		{
			_clearing = true;
			_pos = null;
			_speed = null;
			_accel = null;
			MissedTicks = 0;
			_clearing = false;
		}

		private void FrameTick(TimeSpan time)
		{
			_pos?.Update(time);
			_speed?.Update();
		}

		private void PositionTick(TimeSpan time, TimeSpan deltaTime)
		{
			if (_clearing)
			{
				return;
			}
			PosSnapshot prevPos = _pos;
			PosSnapshot newPos = (_pos = new PosSnapshot(time));
			if (prevPos != null)
			{
				SpeedTime prevSpeed = _speed;
				SpeedTime newSpeed = (_speed = new SpeedTime(prevPos, newPos, deltaTime));
				bool doubled = false;
				if (prevSpeed != null)
				{
					DoubledDiff = SpeedTime.GetDoubledDiff(prevSpeed, newSpeed);
					if (SpeedTime.IsDoubling(prevSpeed, newSpeed))
					{
						newSpeed = (_speed = new SpeedTime(prevPos, newPos, deltaTime.Multiply(2.0)));
						doubled = true;
					}
				}
				DoublingStack.Push(doubled);
				if (prevSpeed != null)
				{
					AccelTime prevAccel = _accel;
					AccelTime newAccel = (_accel = new AccelTime(prevSpeed, newSpeed, deltaTime));
					if (prevAccel != null && prevAccel.Accel3D > 100000f && newAccel.Accel3D < (0f - prevAccel.Accel3D) / 2f)
					{
						this.Teleported?.Invoke();
					}
				}
			}
			MissedTicks = _engine.TickHealth.Sum();
			Doublings = DoublingStack.Sum((bool b) => b ? 1 : 0);
			this.NewPosition?.Invoke(new PosSnapshot(newPos));
		}

		public void Dispose()
		{
			_engine?.Dispose();
		}
	}
}
