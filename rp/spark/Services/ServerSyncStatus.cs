using System;

namespace rp.spark.Services
{
	public class ServerSyncStatus
	{
		public ServerSyncState State { get; }

		public string Message { get; }

		public DateTime LastAttempt { get; }

		public DateTime LastSuccess { get; }

		public string DisplayName => State switch
		{
			ServerSyncState.Info => string.Empty, 
			ServerSyncState.Connected => "Connected", 
			ServerSyncState.ApiUnavailable => "SPARK webserver unavailable", 
			ServerSyncState.BlockedByWindows => "Blocked by Windows", 
			ServerSyncState.ServerError => "SPARK webserver error", 
			_ => "Disconnected", 
		};

		public ServerSyncStatus(ServerSyncState state, string message, DateTime lastAttempt = default(DateTime), DateTime lastSuccess = default(DateTime))
		{
			State = state;
			Message = message ?? string.Empty;
			LastAttempt = lastAttempt;
			LastSuccess = lastSuccess;
		}

		public static ServerSyncStatus Disconnected(string message)
		{
			return new ServerSyncStatus(ServerSyncState.Disconnected, message);
		}
	}
}
