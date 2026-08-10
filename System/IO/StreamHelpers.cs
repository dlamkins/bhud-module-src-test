namespace System.IO
{
	internal static class StreamHelpers
	{
		public static void ValidateCopyToArgs(Stream source, Stream destination, int bufferSize)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", bufferSize, _003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.ArgumentOutOfRange_NeedPosNum);
			}
			bool canRead = source.CanRead;
			if (!canRead && !source.CanWrite)
			{
				throw new ObjectDisposedException(null, _003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.ObjectDisposed_StreamClosed);
			}
			bool canWrite = destination.CanWrite;
			if (!canWrite && !destination.CanRead)
			{
				throw new ObjectDisposedException("destination", _003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.ObjectDisposed_StreamClosed);
			}
			if (!canRead)
			{
				throw new NotSupportedException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.NotSupported_UnreadableStream);
			}
			if (!canWrite)
			{
				throw new NotSupportedException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.NotSupported_UnwritableStream);
			}
		}
	}
}
