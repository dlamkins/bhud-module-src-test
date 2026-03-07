namespace CinemaModule.Controllers.WatchParty
{
	public readonly struct VideoLoadResult
	{
		public bool IsSuccess { get; }

		public bool IsCancelled { get; }

		public string StreamUrl { get; }

		public bool IsLiveStream { get; }

		private VideoLoadResult(bool isSuccess, bool isCancelled, string streamUrl, bool isLiveStream)
		{
			IsSuccess = isSuccess;
			IsCancelled = isCancelled;
			StreamUrl = streamUrl;
			IsLiveStream = isLiveStream;
		}

		public static VideoLoadResult Success(string streamUrl, bool isLiveStream)
		{
			return new VideoLoadResult(isSuccess: true, isCancelled: false, streamUrl, isLiveStream);
		}

		public static VideoLoadResult Failed()
		{
			return new VideoLoadResult(isSuccess: false, isCancelled: false, null, isLiveStream: false);
		}

		public static VideoLoadResult Cancelled()
		{
			return new VideoLoadResult(isSuccess: false, isCancelled: true, null, isLiveStream: false);
		}
	}
}
