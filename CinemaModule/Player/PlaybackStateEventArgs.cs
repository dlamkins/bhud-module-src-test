using System;

namespace CinemaModule.Player
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
