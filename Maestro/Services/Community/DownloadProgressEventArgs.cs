using System;

namespace Maestro.Services.Community
{
	public class DownloadProgressEventArgs : EventArgs
	{
		public string CommunityId { get; }

		public int Progress { get; }

		public DownloadState State { get; }

		public DownloadProgressEventArgs(string communityId, int progress, DownloadState state)
		{
			CommunityId = communityId;
			Progress = progress;
			State = state;
		}
	}
}
