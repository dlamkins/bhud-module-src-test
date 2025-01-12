using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.Models;
using Ideka.NetCommon;

namespace Ideka.RacingMeter
{
	public class MumbleEngine : IDisposable
	{
		public static readonly TimeSpan PosUpdateRate = TimeSpan.FromMilliseconds(40.0);

		private static readonly TimeSpan MinStopTime = PosUpdateRate.Multiply(2.5);

		public readonly DropOutStack<int> TickHealth = new DropOutStack<int>(50);

		private readonly DisposableCollection _dc = new DisposableCollection();

		private int _lastTick;

		private TimeSpan _lastMovementTime = TimeSpan.Zero;

		private Coordinates3 _lastPosition;

		private readonly CancellationTokenSource _ct = new CancellationTokenSource();

		private TimeSpan _delay;

		public bool Disposed { get; private set; }

		public event Action<TimeSpan>? FrameTick;

		public event Action<TimeSpan, TimeSpan>? PositionTick;

		public MumbleEngine()
		{
			_dc.Add(RacingModule.Settings.MumblePollingRate.OnChangedAndNow(delegate(int value)
			{
				_delay = TimeSpan.FromSeconds(1.0 / (double)value);
			}));
			Loop(_ct.Token);
		}

		private Task Loop(CancellationToken ct)
		{
			return Task.Factory.StartNew((Func<Task>)async delegate
			{
				Stopwatch sw = new Stopwatch();
				sw.Start();
				while (true)
				{
					await Task.Delay(_delay);
					ct.ThrowIfCancellationRequested();
					try
					{
						GameService.Gw2Mumble.get_RawClient().Update();
					}
					catch (ObjectDisposedException)
					{
						continue;
					}
					ct.ThrowIfCancellationRequested();
					int tickDiff = GameService.Gw2Mumble.get_RawClient().get_Tick() - _lastTick;
					if (tickDiff > 0)
					{
						_lastTick = GameService.Gw2Mumble.get_Tick();
						TickHealth.Push(tickDiff - 1);
						TimeSpan time = sw.Elapsed;
						bool moved = GameService.Gw2Mumble.get_RawClient().get_AvatarPosition() != _lastPosition;
						if (_lastMovementTime == TimeSpan.Zero || moved)
						{
							_lastMovementTime = time;
							_lastPosition = GameService.Gw2Mumble.get_RawClient().get_AvatarPosition();
						}
						else if (!moved && time - _lastMovementTime < MinStopTime)
						{
							ct.ThrowIfCancellationRequested();
							this.FrameTick?.Invoke(time);
							continue;
						}
						ct.ThrowIfCancellationRequested();
						this.PositionTick?.Invoke(time, PosUpdateRate);
					}
				}
			}, ct, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		public void Dispose()
		{
			_dc.Dispose();
			_ct.Cancel();
		}
	}
}
