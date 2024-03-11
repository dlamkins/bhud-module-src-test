using System;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.ArcDps;
using Blish_HUD.ArcDps.Models;

namespace Ideka.CustomCombatText.Bridge
{
	public class BridgeService : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<BridgeService>();

		private TimeSpan _lastMessage = TimeSpan.Zero;

		private CancellationTokenSource? _cts;

		private BridgeSocket? _bridge;

		private readonly object _lock = new object();

		public bool HudIsActive { get; private set; }

		public int Loops { get; private set; }

		public bool IsActive => GameService.Overlay.get_CurrentGameTime().get_TotalGameTime() - _lastMessage <= TimeSpan.FromSeconds(1.0);

		public event EventHandler<RawCombatEventArgs>? RawCombatEvent;

		public BridgeService()
		{
			GameService.GameIntegration.get_Gw2Instance().add_Gw2Started((EventHandler<EventArgs>)Gw2Started);
			GameService.GameIntegration.get_Gw2Instance().add_Gw2Closed((EventHandler<EventArgs>)Gw2Closed);
			if (GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning())
			{
				Start();
			}
		}

		private void Start()
		{
			Stop();
			CancellationTokenSource cts;
			BridgeSocket bridge;
			lock (_lock)
			{
				cts = (_cts = new CancellationTokenSource());
				bridge = (_bridge = new BridgeSocket());
				bridge.RawMessage += new EventHandler<byte[]>(RawMessage);
			}
			((Func<Task>)async delegate
			{
				Loops = 0;
				while (!bridge.Disposed)
				{
					Logger.Info("Starting socket...");
					await bridge.Start(GetPort());
					await Task.Delay(TimeSpan.FromSeconds(1.0), cts.Token);
					int loops = Loops;
					Loops = loops + 1;
				}
			})();
		}

		private void RawMessage(object sender, byte[] messageData)
		{
			_lastMessage = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime();
			try
			{
				switch (messageData[0])
				{
				case 1:
					HudIsActive = messageData[1] != 0;
					break;
				case 2:
					triggerCombatEvent(messageData, (CombatEventType)0);
					break;
				case 3:
					triggerCombatEvent(messageData, (CombatEventType)1);
					break;
				}
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Failed processing received message.");
			}
			void triggerCombatEvent(byte[] data, CombatEventType eventType)
			{
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Expected O, but got Unknown
				CombatEvent message = CombatParser.ProcessCombat(data);
				this.RawCombatEvent?.Invoke(this, new RawCombatEventArgs(message, eventType));
			}
		}

		private void Stop()
		{
			lock (_lock)
			{
				Logger.Info("Stopping socket...");
				_cts?.Cancel();
				_cts = null;
				if (_bridge != null)
				{
					_bridge!.RawMessage -= new EventHandler<byte[]>(RawMessage);
					_bridge!.Dispose();
					_bridge = null;
				}
			}
		}

		private void Gw2Started(object sender, EventArgs e)
		{
			Start();
		}

		private void Gw2Closed(object sender, EventArgs e)
		{
			Stop();
		}

		public void Dispose()
		{
			GameService.GameIntegration.get_Gw2Instance().remove_Gw2Started((EventHandler<EventArgs>)Gw2Started);
			GameService.GameIntegration.get_Gw2Instance().remove_Gw2Closed((EventHandler<EventArgs>)Gw2Closed);
			Stop();
		}

		private static int GetPort()
		{
			return GetPort((uint)GameService.GameIntegration.get_Gw2Instance().get_Gw2Process().Id);
		}

		private static int GetPort(uint processId)
		{
			return (ushort)processId | 0xC000;
		}
	}
}
