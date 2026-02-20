using System;

namespace CinemaModule.VideoPlayer
{
	public class PlaybackStateEventArgs : EventArgs
	{
		public PlaybackState State { get; }

		public PlaybackStateEventArgs(PlaybackState state)
		{
			State = state;
		}
	}
}
