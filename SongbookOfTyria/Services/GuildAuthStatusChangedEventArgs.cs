using System;

namespace SongbookOfTyria.Services
{
	public class GuildAuthStatusChangedEventArgs : EventArgs
	{
		public bool IsOpusMember { get; }

		public string AccountName { get; }

		public GuildAuthStatusChangedEventArgs(bool isOpusMember, string accountName)
		{
			IsOpusMember = isOpusMember;
			AccountName = accountName;
		}
	}
}
