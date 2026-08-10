using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
	internal sealed class _003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EPipeWriterStream : Stream
	{
		private long _length;

		private readonly PipeWriter _pipeWriter;

		public override bool CanRead => false;

		public override bool CanSeek => false;

		public override bool CanWrite => true;

		public override long Length => _length;

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

		public _003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EPipeWriterStream(PipeWriter pipeWriter)
		{
			_pipeWriter = pipeWriter;
		}

		public override void Flush()
		{
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

		public override void Write(byte[] buffer, int offset, int count)
		{
			_pipeWriter.Write(new ReadOnlySpan<byte>(buffer, offset, count));
			_length += count;
		}

		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return WriteCoreAsync(buffer.AsMemory(offset, count), cancellationToken).AsTask();
		}

		private ValueTask WriteCoreAsync(ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTask(Task.FromCanceled(cancellationToken));
			}
			_length += source.Length;
			ValueTask<FlushResult> flushTask2 = _pipeWriter.WriteAsync(source, cancellationToken);
			if (flushTask2.IsCompletedSuccessfully)
			{
				if (flushTask2.Result.IsCanceled)
				{
					throw new OperationCanceledException();
				}
				return default(ValueTask);
			}
			return WriteSlowAsync(flushTask2);
			static async ValueTask WriteSlowAsync(ValueTask<FlushResult> flushTask)
			{
				if ((await flushTask.ConfigureAwait(continueOnCapturedContext: false)).IsCanceled)
				{
					throw new OperationCanceledException();
				}
			}
		}

		public void Reset()
		{
			_length = 0L;
		}
	}
}
