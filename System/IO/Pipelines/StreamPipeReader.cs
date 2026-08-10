using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
	internal sealed class StreamPipeReader : PipeReader
	{
		internal const int InitialSegmentPoolSize = 4;

		internal const int MaxSegmentPoolSize = 256;

		private CancellationTokenSource _internalTokenSource;

		private bool _isReaderCompleted;

		private bool _isStreamCompleted;

		private BufferSegment _readHead;

		private int _readIndex;

		private BufferSegment _readTail;

		private long _bufferedBytes;

		private bool _examinedEverything;

		private readonly object _lock = new object();

		private BufferSegmentStack _bufferSegmentPool;

		private readonly StreamPipeReaderOptions _options;

		private bool LeaveOpen => _options.LeaveOpen;

		private bool UseZeroByteReads => _options.UseZeroByteReads;

		private int BufferSize => _options.BufferSize;

		private int MaxBufferSize => _options.MaxBufferSize;

		private int MinimumReadThreshold => _options.MinimumReadSize;

		private MemoryPool<byte> Pool => _options.Pool;

		public Stream InnerStream { get; }

		private CancellationTokenSource InternalTokenSource
		{
			get
			{
				lock (_lock)
				{
					return _internalTokenSource ?? (_internalTokenSource = new CancellationTokenSource());
				}
			}
		}

		public StreamPipeReader(Stream readingStream, StreamPipeReaderOptions options)
		{
			if (readingStream == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.readingStream);
			}
			if (options == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.options);
			}
			InnerStream = readingStream;
			_options = options;
			_bufferSegmentPool = new BufferSegmentStack(4);
		}

		public override void AdvanceTo(SequencePosition consumed)
		{
			AdvanceTo(consumed, consumed);
		}

		public override void AdvanceTo(SequencePosition consumed, SequencePosition examined)
		{
			ThrowIfCompleted();
			AdvanceTo((BufferSegment)consumed.GetObject(), consumed.GetInteger(), (BufferSegment)examined.GetObject(), examined.GetInteger());
		}

		private void AdvanceTo(BufferSegment consumedSegment, int consumedIndex, BufferSegment examinedSegment, int examinedIndex)
		{
			if (consumedSegment != null && examinedSegment != null)
			{
				if (_readHead == null)
				{
					ThrowHelper.ThrowInvalidOperationException_AdvanceToInvalidCursor();
				}
				BufferSegment bufferSegment = _readHead;
				BufferSegment bufferSegment2 = consumedSegment;
				long length = BufferSegment.GetLength(bufferSegment, _readIndex, consumedSegment, consumedIndex);
				_bufferedBytes -= length;
				_examinedEverything = false;
				if (examinedSegment == _readTail)
				{
					_examinedEverything = examinedIndex == _readTail.End;
				}
				if (_bufferedBytes == 0L)
				{
					bufferSegment2 = null;
					_readHead = null;
					_readTail = null;
					_readIndex = 0;
				}
				else if (consumedIndex == bufferSegment2.Length)
				{
					BufferSegment bufferSegment3 = (_readHead = bufferSegment2.NextSegment);
					_readIndex = 0;
					bufferSegment2 = bufferSegment3;
				}
				else
				{
					_readHead = consumedSegment;
					_readIndex = consumedIndex;
				}
				while (bufferSegment != bufferSegment2)
				{
					BufferSegment nextSegment = bufferSegment.NextSegment;
					ReturnSegmentUnsynchronized(bufferSegment);
					bufferSegment = nextSegment;
				}
			}
		}

		public override void CancelPendingRead()
		{
			InternalTokenSource.Cancel();
		}

		public override void Complete(Exception? exception = null)
		{
			if (CompleteAndGetNeedsDispose())
			{
				InnerStream.Dispose();
			}
		}

		private bool CompleteAndGetNeedsDispose()
		{
			if (_isReaderCompleted)
			{
				return false;
			}
			_isReaderCompleted = true;
			BufferSegment bufferSegment = _readHead;
			while (bufferSegment != null)
			{
				BufferSegment bufferSegment2 = bufferSegment;
				bufferSegment = bufferSegment.NextSegment;
				bufferSegment2.Reset();
			}
			return !LeaveOpen;
		}

		public override ValueTask<ReadResult> ReadAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			return ReadInternalAsync(null, cancellationToken);
		}

		protected override ValueTask<ReadResult> ReadAtLeastAsyncCore(int minimumSize, CancellationToken cancellationToken)
		{
			return ReadInternalAsync(minimumSize, cancellationToken);
		}

		private ValueTask<ReadResult> ReadInternalAsync(int? minimumSize, CancellationToken cancellationToken)
		{
			ThrowIfCompleted();
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTask<ReadResult>(Task.FromCanceled<ReadResult>(cancellationToken));
			}
			CancellationTokenSource internalTokenSource = InternalTokenSource;
			if (TryReadInternal(internalTokenSource, out var result) && (!minimumSize.HasValue || result.Buffer.Length >= minimumSize || result.IsCompleted || result.IsCanceled))
			{
				return new ValueTask<ReadResult>(result);
			}
			if (_isStreamCompleted)
			{
				ReadResult result2 = new ReadResult(default(ReadOnlySequence<byte>), isCanceled: false, isCompleted: true);
				return new ValueTask<ReadResult>(result2);
			}
			return Core(this, minimumSize, internalTokenSource, cancellationToken);
			static async ValueTask<ReadResult> Core(StreamPipeReader reader, int? minimumSize, CancellationTokenSource tokenSource, CancellationToken cancellationToken)
			{
				CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
				if (cancellationToken.CanBeCanceled)
				{
					cancellationTokenRegistration = cancellationToken.UnsafeRegister(delegate(object state)
					{
						((StreamPipeReader)state).Cancel();
					}, reader);
				}
				using (cancellationTokenRegistration)
				{
					bool isCanceled = false;
					try
					{
						if (reader.UseZeroByteReads && reader._bufferedBytes == 0L)
						{
							await reader.InnerStream.ReadAsync(Memory<byte>.Empty, tokenSource.Token).ConfigureAwait(continueOnCapturedContext: false);
						}
						do
						{
							reader.AllocateReadTail(minimumSize);
							Memory<byte> buffer = reader._readTail.AvailableMemory.Slice(reader._readTail.End);
							int num = await reader.InnerStream.ReadAsync(buffer, tokenSource.Token).ConfigureAwait(continueOnCapturedContext: false);
							reader._readTail.End += num;
							reader._bufferedBytes += num;
							if (num == 0)
							{
								reader._isStreamCompleted = true;
								break;
							}
						}
						while (minimumSize.HasValue && reader._bufferedBytes < minimumSize);
					}
					catch (OperationCanceledException ex)
					{
						reader.ClearCancellationToken();
						if (cancellationToken.IsCancellationRequested)
						{
							throw new OperationCanceledException(ex.Message, ex, cancellationToken);
						}
						if (!tokenSource.IsCancellationRequested)
						{
							throw;
						}
						isCanceled = true;
					}
					return new ReadResult(reader.GetCurrentReadOnlySequence(), isCanceled, reader._isStreamCompleted);
				}
			}
		}

		public override async Task CopyToAsync(PipeWriter destination, CancellationToken cancellationToken = default(CancellationToken))
		{
			ThrowIfCompleted();
			CancellationTokenSource tokenSource = InternalTokenSource;
			if (tokenSource.IsCancellationRequested)
			{
				ThrowHelper.ThrowOperationCanceledException_ReadCanceled();
			}
			CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			if (cancellationToken.CanBeCanceled)
			{
				cancellationTokenRegistration = cancellationToken.UnsafeRegister(delegate(object state)
				{
					((StreamPipeReader)state).Cancel();
				}, this);
			}
			using (cancellationTokenRegistration)
			{
				_ = 1;
				try
				{
					BufferSegment segment = _readHead;
					int start = _readIndex;
					try
					{
						while (segment != null)
						{
							FlushResult flushResult = await destination.WriteAsync(segment.Memory.Slice(start), tokenSource.Token).ConfigureAwait(continueOnCapturedContext: false);
							if (flushResult.IsCanceled)
							{
								ThrowHelper.ThrowOperationCanceledException_FlushCanceled();
							}
							segment = segment.NextSegment;
							start = 0;
							if (flushResult.IsCompleted)
							{
								return;
							}
						}
					}
					finally
					{
						if (segment != null)
						{
							AdvanceTo(segment, segment.End, segment, segment.End);
						}
					}
					if (_isStreamCompleted)
					{
						return;
					}
					await InnerStream.CopyToAsync(destination, tokenSource.Token).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (OperationCanceledException)
				{
					ClearCancellationToken();
					throw;
				}
			}
		}

		public override async Task CopyToAsync(Stream destination, CancellationToken cancellationToken = default(CancellationToken))
		{
			ThrowIfCompleted();
			CancellationTokenSource tokenSource = InternalTokenSource;
			if (tokenSource.IsCancellationRequested)
			{
				ThrowHelper.ThrowOperationCanceledException_ReadCanceled();
			}
			CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			if (cancellationToken.CanBeCanceled)
			{
				cancellationTokenRegistration = cancellationToken.UnsafeRegister(delegate(object state)
				{
					((StreamPipeReader)state).Cancel();
				}, this);
			}
			using (cancellationTokenRegistration)
			{
				_ = 1;
				try
				{
					BufferSegment segment = _readHead;
					int start = _readIndex;
					try
					{
						while (segment != null)
						{
							await destination.WriteAsync(segment.Memory.Slice(start), tokenSource.Token).ConfigureAwait(continueOnCapturedContext: false);
							segment = segment.NextSegment;
							start = 0;
						}
					}
					finally
					{
						if (segment != null)
						{
							AdvanceTo(segment, segment.End, segment, segment.End);
						}
					}
					if (_isStreamCompleted)
					{
						return;
					}
					await InnerStream.CopyToAsync(destination, tokenSource.Token).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (OperationCanceledException)
				{
					ClearCancellationToken();
					throw;
				}
			}
		}

		private void ClearCancellationToken()
		{
			lock (_lock)
			{
				_internalTokenSource = null;
			}
		}

		private void ThrowIfCompleted()
		{
			if (_isReaderCompleted)
			{
				ThrowHelper.ThrowInvalidOperationException_NoReadingAllowed();
			}
		}

		public override bool TryRead(out ReadResult result)
		{
			ThrowIfCompleted();
			return TryReadInternal(InternalTokenSource, out result);
		}

		private bool TryReadInternal(CancellationTokenSource source, out ReadResult result)
		{
			bool isCancellationRequested = source.IsCancellationRequested;
			if (isCancellationRequested || (_bufferedBytes > 0 && (!_examinedEverything || _isStreamCompleted)))
			{
				if (isCancellationRequested)
				{
					ClearCancellationToken();
				}
				ReadOnlySequence<byte> currentReadOnlySequence = GetCurrentReadOnlySequence();
				result = new ReadResult(currentReadOnlySequence, isCancellationRequested, _isStreamCompleted);
				return true;
			}
			result = default(ReadResult);
			return false;
		}

		private ReadOnlySequence<byte> GetCurrentReadOnlySequence()
		{
			if (_readHead != null)
			{
				return new ReadOnlySequence<byte>(_readHead, _readIndex, _readTail, _readTail.End);
			}
			return default(ReadOnlySequence<byte>);
		}

		private void AllocateReadTail(int? minimumSize = null)
		{
			if (_readHead == null)
			{
				_readHead = AllocateSegment(minimumSize);
				_readTail = _readHead;
			}
			else if (_readTail.WritableBytes < MinimumReadThreshold)
			{
				BufferSegment bufferSegment = AllocateSegment(minimumSize);
				_readTail.SetNext(bufferSegment);
				_readTail = bufferSegment;
			}
		}

		private BufferSegment AllocateSegment(int? minimumSize = null)
		{
			BufferSegment bufferSegment = CreateSegmentUnsynchronized();
			int num = minimumSize ?? BufferSize;
			int num2 = ((!_options.IsDefaultSharedMemoryPool) ? _options.Pool.MaxBufferSize : (-1));
			if (num <= num2)
			{
				int segmentSize = GetSegmentSize(num, num2);
				bufferSegment.SetOwnedMemory(_options.Pool.Rent(segmentSize));
			}
			else
			{
				int segmentSize2 = GetSegmentSize(num, MaxBufferSize);
				bufferSegment.SetOwnedMemory(ArrayPool<byte>.Shared.Rent(segmentSize2));
			}
			return bufferSegment;
		}

		private int GetSegmentSize(int sizeHint, int maxBufferSize)
		{
			sizeHint = Math.Max(BufferSize, sizeHint);
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

		private void Cancel()
		{
			InternalTokenSource.Cancel();
		}
	}
}
