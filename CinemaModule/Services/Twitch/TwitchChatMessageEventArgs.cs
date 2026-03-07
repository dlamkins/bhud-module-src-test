using System;
using CinemaModule.Models.Twitch;

namespace CinemaModule.Services.Twitch
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
