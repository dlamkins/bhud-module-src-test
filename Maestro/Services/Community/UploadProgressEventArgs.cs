using System;

namespace Maestro.Services.Community
{
	public class UploadProgressEventArgs : EventArgs
	{
		public UploadState State { get; }

		public int Progress { get; }

		public string Message { get; }

		public UploadProgressEventArgs(UploadState state, int progress, string message)
		{
			State = state;
			Progress = progress;
			Message = message;
		}
	}
}
