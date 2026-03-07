namespace CinemaModule.Controllers
{
	public class YouTubeStreamRefreshedEventArgs : StreamRefreshedEventArgs
	{
		public string VideoId => base.Identifier;

		public YouTubeStreamRefreshedEventArgs(string videoId, string streamUrl)
			: base(videoId, streamUrl)
		{
		}
	}
}
