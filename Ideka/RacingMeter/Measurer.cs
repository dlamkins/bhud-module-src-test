using System;
using System.Linq;

namespace Ideka.RacingMeter
{
	public class Measurer : IDisposable
	{
		private const int TeleportThreshold = 100000;

		private const float DoubleP = 0.1f;

		private readonly DropOutStack<PosSnapshot> PosStack = new DropOutStack<PosSnapshot>(2);

		private readonly DropOutStack<SpeedTime> SpeedStack = new DropOutStack<SpeedTime>(2);

		private readonly DropOutStack<AccelTime> AccelStack = new DropOutStack<AccelTime>(2);

		public readonly DropOutStack<bool> DoublingStack = new DropOutStack<bool>(50);

		private PosSnapshot? _pos;

		private SpeedTime? _speed;

		private AccelTime? _accel;

		private readonly MumbleEngine _engine;

		private readonly KalmanFilter<PosSnapshot> _posFilter = new KalmanFilter<PosSnapshot>(PosSnapshot.Integrate, 1.0, 0.0);

		private bool _clearing;

		public PosSnapshot Pos => _pos ?? new PosSnapshot(TimeSpan.Zero);

		public SpeedTime Speed => _speed ?? new SpeedTime();

		public AccelTime Accel => _accel ?? new AccelTime();

		public int MissedTicks { get; private set; }

		public int Doublings { get; private set; }

		public float DoubledDiff { get; private set; }

		public event Action<PosSnapshot>? NewPosition;

		public event Action? Teleported;

		public Measurer()
		{
			_engine = new MumbleEngine();
			_engine.FrameTick += FrameTick;
			_engine.PositionTick += PositionTick;
		}

		public void Reset()
		{
			_clearing = true;
			PosStack.Clear();
			SpeedStack.Clear();
			AccelStack.Clear();
			_pos = null;
			_speed = null;
			_accel = null;
			MissedTicks = 0;
			_posFilter.Reset();
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
			PosSnapshot newPos = (_pos = _posFilter.Update(new PosSnapshot(time)));
			PosStack.Push(newPos);
			if (prevPos != null)
			{
				SpeedTime prevSpeed = _speed;
				SpeedTime newSpeed = (_speed = new SpeedTime(prevPos, newPos, deltaTime));
				SpeedStack.Push(newSpeed);
				bool doubled = false;
				if (prevSpeed != null && !prevSpeed.IsDouble)
				{
					float? num = prevSpeed?.Speed3D;
					if (num.HasValue)
					{
						float ps3 = num.GetValueOrDefault();
						DoubledDiff = Math.Abs(newSpeed.Speed3D - ps3 * 2f) / (ps3 * 2f);
						if (DoubledDiff < 0.1f)
						{
							newSpeed = (_speed = new SpeedTime(prevPos, newPos, deltaTime, isDouble: true));
							doubled = true;
						}
					}
				}
				DoublingStack.Push(doubled);
				if (prevSpeed != null)
				{
					AccelTime prevAccel = _accel;
					AccelTime newAccel = (_accel = new AccelTime(prevSpeed, newSpeed, deltaTime));
					AccelStack.Push(newAccel);
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
