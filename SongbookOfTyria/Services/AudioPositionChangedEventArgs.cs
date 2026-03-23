using System;

namespace SongbookOfTyria.Services
{
	public class AudioPositionChangedEventArgs : EventArgs
	{
		public TimeSpan CurrentPosition { get; }

		public TimeSpan TotalDuration { get; }

		public AudioPositionChangedEventArgs(TimeSpan currentPosition, TimeSpan totalDuration)
		{
			CurrentPosition = currentPosition;
			TotalDuration = totalDuration;
		}
	}
}
