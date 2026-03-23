using System;

namespace SongbookOfTyria.Settings
{
	public class StatusChangedEventArgs : EventArgs
	{
		public string Message { get; }

		public StatusType Type { get; }

		public StatusChangedEventArgs(string message, StatusType type)
		{
			Message = message;
			Type = type;
		}
	}
}
