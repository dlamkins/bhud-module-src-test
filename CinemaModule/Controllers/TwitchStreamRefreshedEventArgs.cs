using System;

namespace CinemaModule.Controllers
{
	public class TwitchStreamRefreshedEventArgs : EventArgs
	{
		public string ChannelName { get; }

		public string StreamUrl { get; }

		public TwitchStreamRefreshedEventArgs(string channelName, string streamUrl)
		{
			ChannelName = channelName;
			StreamUrl = streamUrl;
		}
	}
}
