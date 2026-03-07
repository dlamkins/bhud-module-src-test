using System;

namespace CinemaModule.Controllers
{
	public abstract class StreamRefreshedEventArgs : EventArgs
	{
		public string Identifier { get; }

		public string StreamUrl { get; }

		protected StreamRefreshedEventArgs(string identifier, string streamUrl)
		{
			Identifier = identifier;
			StreamUrl = streamUrl;
		}
	}
}
