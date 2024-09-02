using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;

namespace FarmingTracker
{
	public class Drf : IDisposable
	{
		private readonly SettingService _settingService;

		private readonly DrfWebSocketClient _drfWebSocketClient = new DrfWebSocketClient();

		private int _reconnectTriesCounter;

		private static readonly object _reconnectLock = new object();

		public DrfConnectionStatus DrfConnectionStatus { get; private set; }

		public int ReconnectDelaySeconds { get; private set; }

		public int ReconnectTriesCounter => _reconnectTriesCounter;

		public bool WindowsVersionIsTooLowToSupportWebSockets => _drfWebSocketClient.WindowsVersionIsTooLowToSupportWebSockets;

		public event EventHandler DrfConnectionStatusChanged;

		public Drf(SettingService settingService)
		{
			_settingService = settingService;
			InitializeEventHandlers();
			FireAndForgetConnectToDrf();
			settingService.DrfTokenSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnDrfTokenSettingChanged);
			settingService.IsFakeDrfServerUsedSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnIsFakeDrfServerUsedSettingChanged);
		}

		public void SetDrfConnectionStatus(DrfConnectionStatus status)
		{
			DrfConnectionStatus = status;
			this.DrfConnectionStatusChanged?.Invoke(this, EventArgs.Empty);
		}

		public void Dispose()
		{
			_settingService.DrfTokenSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnDrfTokenSettingChanged);
			_settingService.IsFakeDrfServerUsedSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnIsFakeDrfServerUsedSettingChanged);
			_drfWebSocketClient.Dispose();
		}

		public List<DrfMessage> GetDrfMessages()
		{
			return _drfWebSocketClient.GetDrfMessages();
		}

		private void InitializeEventHandlers()
		{
			_drfWebSocketClient.Connecting += delegate
			{
				Module.Logger.Debug("Connecting");
				SetDrfConnectionStatus(DrfConnectionStatus.Connecting);
			};
			_drfWebSocketClient.ConnectFailed += async delegate(object s, GenericEventArgs<Exception> e)
			{
				Module.Logger.Warn("ConnectFailed: " + ExceptionService.GetExceptionSummary(e.Data));
				await Reconnect();
			};
			_drfWebSocketClient.SendAuthenticationFailed += async delegate(object s, GenericEventArgs<Exception> e)
			{
				Module.Logger.Warn("SendAuthenticationFailed: " + ExceptionService.GetExceptionSummary(e.Data));
				await Reconnect();
			};
			_drfWebSocketClient.ConnectCrashed += delegate(object s, GenericEventArgs<Exception> e)
			{
				Module.Logger.Error($"ConnectCrashed: {e.Data}");
				SetDrfConnectionStatus(DrfConnectionStatus.ModuleError);
			};
			_drfWebSocketClient.ConnectedAndAuthenticationRequestSent += delegate
			{
				Module.Logger.Debug("ConnectedAndAuthenticationRequestSent");
				_reconnectTriesCounter = 0;
				SetDrfConnectionStatus(DrfConnectionStatus.Connected);
			};
			_drfWebSocketClient.UnexpectedNotOpenWhileReceiving += async delegate
			{
				Module.Logger.Warn("ReceivedUnexpectedNotOpen");
				await Reconnect();
			};
			_drfWebSocketClient.ReceiveFailed += async delegate(object s, GenericEventArgs<Exception> e)
			{
				Module.Logger.Warn("ReceiveFailed: " + ExceptionService.GetExceptionSummary(e.Data));
				await Reconnect();
			};
			_drfWebSocketClient.AuthenticationFailed += delegate
			{
				Module.Logger.Warn("AuthenticationFailed");
				SetDrfConnectionStatus(DrfConnectionStatus.AuthenticationFailed);
			};
			_drfWebSocketClient.ReceivedUnexpectedBinaryMessage += delegate
			{
				Module.Logger.Warn("ReceivedUnexpectedBinaryMessage");
			};
			_drfWebSocketClient.ReceiveCrashed += delegate(object s, GenericEventArgs<Exception> e)
			{
				Module.Logger.Error($"ReceiveCrashed: {e.Data}");
				SetDrfConnectionStatus(DrfConnectionStatus.ModuleError);
			};
		}

		private async Task Reconnect()
		{
			lock (_reconnectLock)
			{
				if (DrfConnectionStatus == DrfConnectionStatus.TryReconnect)
				{
					Module.Logger.Debug("Do not trigger a new reconnect because a reconnect is already in progress.");
					return;
				}
				DrfConnectionStatus = DrfConnectionStatus.TryReconnect;
			}
			int reconnectTriesCounter = Interlocked.Increment(ref _reconnectTriesCounter);
			int reconnectDelayMs = DetermineReconnectDelayMs(reconnectTriesCounter);
			int reconnectDelaySeconds = (ReconnectDelaySeconds = reconnectDelayMs / 1000);
			this.DrfConnectionStatusChanged?.Invoke(this, EventArgs.Empty);
			await Task.Delay(reconnectDelayMs);
			if (DrfConnectionStatus != DrfConnectionStatus.TryReconnect)
			{
				Module.Logger.Debug("Cancel reconnect because DrfConnectionStatus changed during reconnect delay.");
				return;
			}
			if (reconnectTriesCounter <= 6 || reconnectTriesCounter % 20 == 0)
			{
				Module.Logger.Warn($"{reconnectTriesCounter}. try to reconnect to DRF after {reconnectDelaySeconds}s delay. " + "This message will only appear for the first 6 tries and every 20 tries.");
			}
			FireAndForgetConnectToDrf();
		}

		private static int DetermineReconnectDelayMs(int reconnectTriesCounter)
		{
			return reconnectTriesCounter switch
			{
				1 => 0, 
				2 => 2000, 
				3 => 5000, 
				4 => 10000, 
				5 => 20000, 
				_ => 30000, 
			};
		}

		private async void OnDrfTokenSettingChanged(object sender = null, ValueChangedEventArgs<string> e = null)
		{
			string drfToken = _settingService.DrfTokenSetting.get_Value();
			if (DrfToken.HasValidFormat(drfToken))
			{
				await _drfWebSocketClient.Connect(drfToken);
			}
		}

		private async void FireAndForgetConnectToDrf()
		{
			_drfWebSocketClient.WebSocketUrl = (_settingService.IsFakeDrfServerUsedSetting.get_Value() ? "ws://localhost:8080" : "wss://drf.rs/ws");
			await _drfWebSocketClient.Connect(_settingService.DrfTokenSetting.get_Value());
		}

		private void OnIsFakeDrfServerUsedSettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			FireAndForgetConnectToDrf();
		}
	}
}
