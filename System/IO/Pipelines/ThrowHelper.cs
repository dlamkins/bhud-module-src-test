using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
	internal static class ThrowHelper
	{
		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
		{
			throw CreateArgumentOutOfRangeException(argument);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(ExceptionArgument argument)
		{
			return new ArgumentOutOfRangeException(argument.ToString());
		}

		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		internal static void ThrowArgumentNullException(ExceptionArgument argument)
		{
			throw CreateArgumentNullException(argument);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static ArgumentNullException CreateArgumentNullException(ExceptionArgument argument)
		{
			return new ArgumentNullException(argument.ToString());
		}

		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_AlreadyReading()
		{
			throw CreateInvalidOperationException_AlreadyReading();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static InvalidOperationException CreateInvalidOperationException_AlreadyReading()
		{
			return new InvalidOperationException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.ReadingIsInProgress);
		}

		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_NoReadToComplete()
		{
			throw CreateInvalidOperationException_NoReadToComplete();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static InvalidOperationException CreateInvalidOperationException_NoReadToComplete()
		{
			return new InvalidOperationException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.NoReadingOperationToComplete);
		}

		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_NoConcurrentOperation()
		{
			throw CreateInvalidOperationException_NoConcurrentOperation();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static InvalidOperationException CreateInvalidOperationException_NoConcurrentOperation()
		{
			return new InvalidOperationException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.ConcurrentOperationsNotSupported);
		}

		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_GetResultNotCompleted()
		{
			throw CreateInvalidOperationException_GetResultNotCompleted();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static InvalidOperationException CreateInvalidOperationException_GetResultNotCompleted()
		{
			return new InvalidOperationException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.GetResultBeforeCompleted);
		}

		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_NoWritingAllowed()
		{
			throw CreateInvalidOperationException_NoWritingAllowed();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static InvalidOperationException CreateInvalidOperationException_NoWritingAllowed()
		{
			return new InvalidOperationException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.WritingAfterCompleted);
		}

		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_NoReadingAllowed()
		{
			throw CreateInvalidOperationException_NoReadingAllowed();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static InvalidOperationException CreateInvalidOperationException_NoReadingAllowed()
		{
			return new InvalidOperationException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.ReadingAfterCompleted);
		}

		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_InvalidExaminedPosition()
		{
			throw CreateInvalidOperationException_InvalidExaminedPosition();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static InvalidOperationException CreateInvalidOperationException_InvalidExaminedPosition()
		{
			return new InvalidOperationException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.InvalidExaminedPosition);
		}

		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_InvalidExaminedOrConsumedPosition()
		{
			throw CreateInvalidOperationException_InvalidExaminedOrConsumedPosition();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static InvalidOperationException CreateInvalidOperationException_InvalidExaminedOrConsumedPosition()
		{
			return new InvalidOperationException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.InvalidExaminedOrConsumedPosition);
		}

		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_AdvanceToInvalidCursor()
		{
			throw CreateInvalidOperationException_AdvanceToInvalidCursor();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static InvalidOperationException CreateInvalidOperationException_AdvanceToInvalidCursor()
		{
			return new InvalidOperationException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.AdvanceToInvalidCursor);
		}

		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_ResetIncompleteReaderWriter()
		{
			throw CreateInvalidOperationException_ResetIncompleteReaderWriter();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static InvalidOperationException CreateInvalidOperationException_ResetIncompleteReaderWriter()
		{
			return new InvalidOperationException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.ReaderAndWriterHasToBeCompleted);
		}

		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		public static void ThrowOperationCanceledException_ReadCanceled()
		{
			throw CreateOperationCanceledException_ReadCanceled();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static OperationCanceledException CreateOperationCanceledException_ReadCanceled()
		{
			return new OperationCanceledException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.ReadCanceledOnPipeReader);
		}

		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		public static void ThrowOperationCanceledException_FlushCanceled()
		{
			throw CreateOperationCanceledException_FlushCanceled();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static OperationCanceledException CreateOperationCanceledException_FlushCanceled()
		{
			return new OperationCanceledException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.FlushCanceledOnPipeWriter);
		}

		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		public static void ThrowInvalidOperationException_InvalidZeroByteRead()
		{
			throw CreateInvalidOperationException_InvalidZeroByteRead();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static InvalidOperationException CreateInvalidOperationException_InvalidZeroByteRead()
		{
			return new InvalidOperationException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.InvalidZeroByteRead);
		}

		[_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003EDoesNotReturn]
		public static void ThrowNotSupported_UnflushedBytes()
		{
			throw CreateNotSupportedException_UnflushedBytes();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static NotSupportedException CreateNotSupportedException_UnflushedBytes()
		{
			return new NotSupportedException(_003Cae155995_002D4f08_002D4885_002D94b9_002Daae0d6fa9756_003ESR.UnflushedBytesNotSupported);
		}
	}
}
