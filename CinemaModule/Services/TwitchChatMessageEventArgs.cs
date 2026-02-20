using System;
using CinemaModule.Models;

namespace CinemaModule.Services
{
	public class TwitchChatMessageEventArgs : EventArgs
	{
		public TwitchChatMessage Message { get; }

		public TwitchChatMessageEventArgs(TwitchChatMessage message)
		{
			Message = message;
		}
	}
}
