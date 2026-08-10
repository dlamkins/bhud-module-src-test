using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
	internal sealed class PipeWriterStream : Stream
	{
		private readonly PipeWriter _pipeWriter;

		internal bool LeaveOpen { get; set; }

		public override bool CanRead => false;

		public override bool CanSeek => false;

		public override bool CanWrite => true;

		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public PipeWriterStream(PipeWriter pipeWriter, bool leaveOpen)
		{
			_pipeWriter = pipeWriter;
			LeaveOpen = leaveOpen;
		}

		protected override void Dispose(bool disposing)
		{
			if (!LeaveOpen)
			{
				_pipeWriter.Complete();
			}
		}

		public override void Flush()
		{
			FlushAsync().GetAwaiter().GetResult();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public sealed override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
		{
			return TaskToAsyncResult.Begin(WriteAsync(buffer, offset, count, default(CancellationToken)), callback, state);
		}

		public sealed override void EndWrite(IAsyncResult asyncResult)
		{
			TaskToAsyncResult.End(asyncResult);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			WriteAsync(buffer, offset, count).GetAwaiter().GetResult();
		}

		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (buffer == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.buffer);
			}
			ValueTask<FlushResult> valueTask = _pipeWriter.WriteAsync(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken);
			return GetFlushResultAsTask(valueTask);
		}

		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			ValueTask<FlushResult> valueTask = _pipeWriter.FlushAsync(cancellationToken);
			return GetFlushResultAsTask(valueTask);
		}

		private static Task GetFlushResultAsTask(ValueTask<FlushResult> valueTask)
		{
			if (valueTask.IsCompletedSuccessfully)
			{
				if (valueTask.Result.IsCanceled)
				{
					ThrowHelper.ThrowOperationCanceledException_FlushCanceled();
				}
				return Task.CompletedTask;
			}
			return AwaitTask(valueTask);
			static async Task AwaitTask(ValueTask<FlushResult> valueTask)
			{
				if ((await valueTask.ConfigureAwait(continueOnCapturedContext: false)).IsCanceled)
				{
					ThrowHelper.ThrowOperationCanceledException_FlushCanceled();
				}
			}
		}
	}
}
