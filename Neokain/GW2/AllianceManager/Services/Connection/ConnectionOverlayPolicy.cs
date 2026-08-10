using System;

namespace Neokain.GW2.AllianceManager.Services.Connection
{
	public static class ConnectionOverlayPolicy
	{
		public static bool ShouldShowOverlay(ModuleConnectionState state, bool isOnConnectionTab)
		{
			if (state != ModuleConnectionState.Connected)
			{
				return !isOnConnectionTab;
			}
			return false;
		}

		public static bool CanReconnect(ModuleConnectionState state)
		{
			if (state != 0)
			{
				return state != ModuleConnectionState.BadKeyFormat;
			}
			return false;
		}

		public static string OverlayText(ConnectionStatus status)
		{
			if (status == null)
			{
				return "Connection status unavailable.";
			}
			switch (status.State)
			{
			case ModuleConnectionState.NoKey:
				return "Not connected — set up your API key on the Connection tab.";
			case ModuleConnectionState.BadKeyFormat:
				return "API key format looks wrong — fix it on the Connection tab.";
			case ModuleConnectionState.Connecting:
				if (!string.IsNullOrWhiteSpace(status.Endpoint))
				{
					return "Connecting to " + status.Endpoint + "…";
				}
				return "Connecting…";
			case ModuleConnectionState.FailedTransient:
				if (!string.IsNullOrWhiteSpace(status.Reason))
				{
					return status.Reason;
				}
				return "Connection lost — retrying…";
			case ModuleConnectionState.FailedTerminal:
				if (!string.IsNullOrWhiteSpace(status.Reason))
				{
					return status.Reason;
				}
				return "Connection failed — see the Connection tab.";
			case ModuleConnectionState.Connected:
				return "Connected.";
			default:
				return "Unknown connection state.";
			}
		}

		public static string CountdownText(ConnectionStatus status, DateTime nowUtc)
		{
			if (status == null || status.State != ModuleConnectionState.FailedTransient || !status.NextRetryUtc.HasValue)
			{
				return string.Empty;
			}
			TimeSpan remaining = status.NextRetryUtc.Value - nowUtc;
			if (remaining <= TimeSpan.Zero)
			{
				return string.Empty;
			}
			int secs = (int)Math.Ceiling(remaining.TotalSeconds);
			return $"Next attempt in {secs}s";
		}
	}
}
