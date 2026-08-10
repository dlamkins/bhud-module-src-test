using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
	internal sealed class StreamPipeWriter : PipeWriter
	{
		internal const int InitialSegmentPoolSize = 4;

		internal const int MaxSegmentPoolSize = 256;

		private readonly int _minimumBufferSize;

		private BufferSegment _head;

		private BufferSegment _tail;

		private Memory<byte> _tailMemory;

		private int _tailBytesBuffered;

		private int _bytesBuffered;

		private readonly MemoryPool<byte> _pool;

		private readonly int _maxPooledBufferSize;

		private CancellationTokenSource _internalTokenSource;

		private bool _isCompleted;

		private readonly object _lockObject = new object();

		private BufferSegmentStack _bufferSegmentPool;

		private readonly bool _leaveOpen;

		private CancellationTokenSource InternalTokenSource
		{
			get
			{
				lock (_lockObject)
				{
					return _internalTokenSource ?? (_internalTokenSource = new CancellationTokenSource());
				}
			}
		}

		public Stream InnerStream { get; }

		public override bool CanGetUnflushedBytes => true;

		public override long UnflushedBytes => _bytesBuffered;

		public StreamPipeWriter(Stream writingStream, StreamPipeWriterOptions options)
		{
			if (writingStream == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.writingStream);
			}
			if (options == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.options);
			}
			InnerStream = writingStream;
			_minimumBufferSize = options.MinimumBufferSize;
			_pool = ((options.Pool == MemoryPool<byte>.Shared) ? null : options.Pool);
			_maxPooledBufferSize = _pool?.MaxBufferSize ?? (-1);
			_bufferSegmentPool = new BufferSegmentStack(4);
			_leaveOpen = options.LeaveOpen;
		}

		public override void Advance(int bytes)
		{
			if ((uint)bytes > (uint)_tailMemory.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.bytes);
			}
			_tailBytesBuffered += bytes;
			_bytesBuffered += bytes;
			_tailMemory = _tailMemory.Slice(bytes);
		}

		public override Memory<byte> GetMemory(int sizeHint = 0)
		{
			if (_isCompleted)
			{
				ThrowHelper.ThrowInvalidOperationException_NoWritingAllowed();
			}
			if (sizeHint < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.sizeHint);
			}
			AllocateMemory(sizeHint);
			return _tailMemory;
		}

		public override Span<byte> GetSpan(int sizeHint = 0)
		{
			if (_isCompleted)
			{
				ThrowHelper.ThrowInvalidOperationException_NoWritingAllowed();
			}
			if (sizeHint < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.sizeHint);
			}
			AllocateMemory(sizeHint);
			return _tailMemory.Span;
		}

		private void AllocateMemory(int sizeHint)
		{
			if (_head == null)
			{
				BufferSegment tail = AllocateSegment(sizeHint);
				_head = (_tail = tail);
				_tailBytesBuffered = 0;
				return;
			}
			int length = _tailMemory.Length;
			if (length == 0 || length < sizeHint)
			{
				if (_tailBytesBuffered > 0)
				{
					_tail.End += _tailBytesBuffered;
					_tailBytesBuffered = 0;
				}
				BufferSegment bufferSegment = AllocateSegment(sizeHint);
				_tail.SetNext(bufferSegment);
				_tail = bufferSegment;
			}
		}

		private BufferSegment AllocateSegment(int sizeHint)
		{
			BufferSegment bufferSegment = CreateSegmentUnsynchronized();
			int maxPooledBufferSize = _maxPooledBufferSize;
			if (sizeHint <= maxPooledBufferSize)
			{
				bufferSegment.SetOwnedMemory(_pool.Rent(GetSegmentSize(sizeHint, maxPooledBufferSize)));
			}
			else
			{
				int segmentSize = GetSegmentSize(sizeHint);
				bufferSegment.SetOwnedMemory(ArrayPool<byte>.Shared.Rent(segmentSize));
			}
			_tailMemory = bufferSegment.AvailableMemory;
			return bufferSegment;
		}

		private int GetSegmentSize(int sizeHint, int maxBufferSize = int.MaxValue)
		{
			sizeHint = Math.Max(_minimumBufferSize, sizeHint);
			return Math.Min(maxBufferSize, sizeHint);
		}

		private BufferSegment CreateSegmentUnsynchronized()
		{
			if (_bufferSegmentPool.TryPop(out var result))
			{
				return result;
			}
			return new BufferSegment();
		}

		private void ReturnSegmentUnsynchronized(BufferSegment segment)
		{
			segment.Reset();
			if (_bufferSegmentPool.Count < 256)
			{
				_bufferSegmentPool.Push(segment);
			}
		}

		public override void CancelPendingFlush()
		{
			Cancel();
		}

		public override void Complete(Exception? exception = null)
		{
			if (_isCompleted)
			{
				return;
			}
			_isCompleted = true;
			try
			{
				FlushInternal(exception == null);
			}
			finally
			{
				_internalTokenSource?.Dispose();
				if (!_leaveOpen)
				{
					InnerStream.Dispose();
				}
			}
		}

		public override async ValueTask CompleteAsync(Exception? exception = null)
		{
			if (_isCompleted)
			{
				return;
			}
			_isCompleted = true;
			try
			{
				await FlushAsyncInternal(exception == null, Memory<byte>.Empty).ConfigureAwait(continueOnCapturedContext: false);
			}
			finally
			{
				_internalTokenSource?.Dispose();
				if (!_leaveOpen)
				{
					InnerStream.Dispose();
				}
			}
		}

		public override ValueTask<FlushResult> FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (_bytesBuffered == 0)
			{
				return new ValueTask<FlushResult>(new FlushResult(isCanceled: false, isCompleted: false));
			}
			return FlushAsyncInternal(writeToStream: true, Memory<byte>.Empty, cancellationToken);
		}

		public override ValueTask<FlushResult> WriteAsync(ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return FlushAsyncInternal(writeToStream: true, source, cancellationToken);
		}

		private void Cancel()
		{
			InternalTokenSource.Cancel();
		}

		private async ValueTask<FlushResult> FlushAsyncInternal(bool writeToStream, ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default(CancellationToken))
		{
			CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			if (cancellationToken.CanBeCanceled)
			{
				cancellationTokenRegistration = cancellationToken.UnsafeRegister(delegate(object state)
				{
					((StreamPipeWriter)state).Cancel();
				}, this);
			}
			if (_tailBytesBuffered > 0)
			{
				_tail.End += _tailBytesBuffered;
				_tailBytesBuffered = 0;
			}
			using (cancellationTokenRegistration)
			{
				CancellationToken localToken = InternalTokenSource.Token;
				try
				{
					BufferSegment segment = _head;
					while (segment != null)
					{
						BufferSegment returnSegment = segment;
						segment = segment.NextSegment;
						if (returnSegment.Length > 0 && writeToStream)
						{
							await InnerStream.WriteAsync(returnSegment.Memory, localToken).ConfigureAwait(continueOnCapturedContext: false);
						}
						ReturnSegmentUnsynchronized(returnSegment);
						_head = segment;
					}
					if (writeToStream)
					{
						if (data.Length > 0)
						{
							await InnerStream.WriteAsync(data, localToken).ConfigureAwait(continueOnCapturedContext: false);
						}
						if (_bytesBuffered > 0 || data.Length > 0)
						{
							await InnerStream.FlushAsync(localToken).ConfigureAwait(continueOnCapturedContext: false);
						}
					}
					_head = null;
					_tail = null;
					_tailMemory = default(Memory<byte>);
					_bytesBuffered = 0;
					return new FlushResult(isCanceled: false, isCompleted: false);
				}
				catch (OperationCanceledException)
				{
					lock (_lockObject)
					{
						_internalTokenSource = null;
					}
					if (localToken.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
					{
						return new FlushResult(isCanceled: true, isCompleted: false);
					}
					throw;
				}
			}
		}

		private void FlushInternal(bool writeToStream)
		{
			if (_tailBytesBuffered > 0)
			{
				_tail.End += _tailBytesBuffered;
				_tailBytesBuffered = 0;
			}
			BufferSegment bufferSegment = _head;
			while (bufferSegment != null)
			{
				BufferSegment bufferSegment2 = bufferSegment;
				bufferSegment = bufferSegment.NextSegment;
				if (bufferSegment2.Length > 0 && writeToStream)
				{
					InnerStream.Write(bufferSegment2.Memory);
				}
				ReturnSegmentUnsynchronized(bufferSegment2);
				_head = bufferSegment;
			}
			if (_bytesBuffered > 0 && writeToStream)
			{
				InnerStream.Flush();
			}
			_head = null;
			_tail = null;
			_tailMemory = default(Memory<byte>);
			_bytesBuffered = 0;
		}
	}
}
