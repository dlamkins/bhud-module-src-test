namespace CinemaModule.Controllers
{
	public class TwitchStreamRefreshedEventArgs : StreamRefreshedEventArgs
	{
		public string ChannelName => base.Identifier;

		public TwitchStreamRefreshedEventArgs(string channelName, string streamUrl)
			: base(channelName, streamUrl)
		{
		}
	}
}
