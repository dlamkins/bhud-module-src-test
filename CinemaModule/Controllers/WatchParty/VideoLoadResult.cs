namespace CinemaModule.Controllers.WatchParty
{
	public readonly struct VideoLoadResult
	{
		public bool IsSuccess { get; }

		public bool IsCancelled { get; }

		public string StreamUrl { get; }

		public string AudioUrl { get; }

		public bool IsLiveStream { get; }

		private VideoLoadResult(bool isSuccess, bool isCancelled, string streamUrl, string audioUrl, bool isLiveStream)
		{
			IsSuccess = isSuccess;
			IsCancelled = isCancelled;
			StreamUrl = streamUrl;
			AudioUrl = audioUrl;
			IsLiveStream = isLiveStream;
		}

		public static VideoLoadResult Success(string streamUrl, string audioUrl, bool isLiveStream)
		{
			return new VideoLoadResult(isSuccess: true, isCancelled: false, streamUrl, audioUrl, isLiveStream);
		}

		public static VideoLoadResult Failed()
		{
			return new VideoLoadResult(isSuccess: false, isCancelled: false, null, null, isLiveStream: false);
		}

		public static VideoLoadResult Cancelled()
		{
			return new VideoLoadResult(isSuccess: false, isCancelled: true, null, null, isLiveStream: false);
		}
	}
}
