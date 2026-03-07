using System;

namespace CinemaModule.Services.Twitch
{
	public class TwitchChatConnectionEventArgs : EventArgs
	{
		public bool IsConnected { get; }

		public string Status { get; }

		public TwitchChatConnectionEventArgs(bool isConnected, string status)
		{
			IsConnected = isConnected;
			Status = status;
		}
	}
}
