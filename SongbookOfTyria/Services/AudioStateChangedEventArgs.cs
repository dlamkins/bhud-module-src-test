using System;

namespace SongbookOfTyria.Services
{
	public class AudioStateChangedEventArgs : EventArgs
	{
		public AudioPlaybackState State { get; }

		public AudioStateChangedEventArgs(AudioPlaybackState state)
		{
			State = state;
		}
	}
}
