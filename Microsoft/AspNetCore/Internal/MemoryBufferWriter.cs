using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Internal
{
	internal sealed class MemoryBufferWriter : Stream, IBufferWriter<byte>
	{
		internal readonly ref struct WrittenBuffers
		{
			public readonly List<CompletedBuffer> Segments;

			private readonly int _bytesWritten;

			public int ByteLength => _bytesWritten;

			public WrittenBuffers(List<CompletedBuffer> segments, int bytesWritten)
			{
				Segments = segments;
				_bytesWritten = bytesWritten;
			}

			public void Dispose()
			{
				for (int i = 0; i < Segments.Count; i++)
				{
					Segments[i].Return();
				}
				Segments.Clear();
			}
		}

		internal readonly struct CompletedBuffer
		{
			public byte[] Buffer { get; }

			public int Length { get; }

			public ReadOnlySpan<byte> Span => Buffer.AsSpan(0, Length);

			public CompletedBuffer(byte[] buffer, int length)
			{
				Buffer = buffer;
				Length = length;
			}

			public void Return()
			{
				ArrayPool<byte>.Shared.Return(Buffer);
			}
		}

		[ThreadStatic]
		private static MemoryBufferWriter _cachedInstance;

		private readonly int _minimumSegmentSize;

		private int _bytesWritten;

		private List<CompletedBuffer> _completedSegments;

		private byte[] _currentSegment;

		private int _position;

		public override long Length => _bytesWritten;

		public override bool CanRead => false;

		public override bool CanSeek => false;

		public override bool CanWrite => true;

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

		public MemoryBufferWriter(int minimumSegmentSize = 4096)
		{
			_minimumSegmentSize = minimumSegmentSize;
		}

		public static MemoryBufferWriter Get()
		{
			MemoryBufferWriter memoryBufferWriter = _cachedInstance;
			if (memoryBufferWriter == null)
			{
				memoryBufferWriter = new MemoryBufferWriter();
			}
			else
			{
				_cachedInstance = null;
			}
			return memoryBufferWriter;
		}

		public static void Return(MemoryBufferWriter writer)
		{
			_cachedInstance = writer;
			writer.Reset();
		}

		public void Reset()
		{
			if (_completedSegments != null)
			{
				for (int i = 0; i < _completedSegments.Count; i++)
				{
					_completedSegments[i].Return();
				}
				_completedSegments.Clear();
			}
			if (_currentSegment != null)
			{
				ArrayPool<byte>.Shared.Return(_currentSegment);
				_currentSegment = null;
			}
			_bytesWritten = 0;
			_position = 0;
		}

		public void Advance(int count)
		{
			_bytesWritten += count;
			_position += count;
		}

		public Memory<byte> GetMemory(int sizeHint = 0)
		{
			EnsureCapacity(sizeHint);
			return _currentSegment.AsMemory(_position, _currentSegment.Length - _position);
		}

		public Span<byte> GetSpan(int sizeHint = 0)
		{
			EnsureCapacity(sizeHint);
			return _currentSegment.AsSpan(_position, _currentSegment.Length - _position);
		}

		public void CopyTo(IBufferWriter<byte> destination)
		{
			if (_completedSegments != null)
			{
				int count = _completedSegments.Count;
				for (int i = 0; i < count; i++)
				{
					destination.Write(_completedSegments[i].Span);
				}
			}
			destination.Write(_currentSegment.AsSpan(0, _position));
		}

		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			if (_completedSegments == null && _currentSegment != null)
			{
				return destination.WriteAsync(_currentSegment, 0, _position, cancellationToken);
			}
			return CopyToSlowAsync(destination, cancellationToken);
		}

		[_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EMemberNotNull("_currentSegment")]
		private void EnsureCapacity(int sizeHint)
		{
			int valueOrDefault = (_currentSegment?.Length - _position).GetValueOrDefault();
			if ((sizeHint != 0 || valueOrDefault <= 0) && (sizeHint <= 0 || valueOrDefault < sizeHint))
			{
				AddSegment(sizeHint);
			}
		}

		[_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EMemberNotNull("_currentSegment")]
		private void AddSegment(int sizeHint = 0)
		{
			if (_currentSegment != null)
			{
				if (_completedSegments == null)
				{
					_completedSegments = new List<CompletedBuffer>();
				}
				_completedSegments.Add(new CompletedBuffer(_currentSegment, _position));
			}
			_currentSegment = ArrayPool<byte>.Shared.Rent(Math.Max(_minimumSegmentSize, sizeHint));
			_position = 0;
		}

		private async Task CopyToSlowAsync(Stream destination, CancellationToken cancellationToken)
		{
			if (_completedSegments != null)
			{
				int count = _completedSegments.Count;
				for (int i = 0; i < count; i++)
				{
					CompletedBuffer completedBuffer = _completedSegments[i];
					await destination.WriteAsync(completedBuffer.Buffer, 0, completedBuffer.Length, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			if (_currentSegment != null)
			{
				await destination.WriteAsync(_currentSegment, 0, _position, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public byte[] ToArray()
		{
			if (_currentSegment == null)
			{
				return Array.Empty<byte>();
			}
			byte[] array = new byte[_bytesWritten];
			int num = 0;
			if (_completedSegments != null)
			{
				int count = _completedSegments.Count;
				for (int i = 0; i < count; i++)
				{
					CompletedBuffer completedBuffer = _completedSegments[i];
					completedBuffer.Span.CopyTo(array.AsSpan(num));
					num += completedBuffer.Span.Length;
				}
			}
			_currentSegment.AsSpan(0, _position).CopyTo(array.AsSpan(num));
			return array;
		}

		public void CopyTo(Span<byte> span)
		{
			if (_currentSegment == null)
			{
				return;
			}
			int num = 0;
			if (_completedSegments != null)
			{
				int count = _completedSegments.Count;
				for (int i = 0; i < count; i++)
				{
					CompletedBuffer completedBuffer = _completedSegments[i];
					completedBuffer.Span.CopyTo(span.Slice(num));
					num += completedBuffer.Span.Length;
				}
			}
			_currentSegment.AsSpan(0, _position).CopyTo(span.Slice(num));
		}

		public override void Flush()
		{
		}

		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
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

		public override void WriteByte(byte value)
		{
			if (_currentSegment != null && (uint)_position < (uint)_currentSegment.Length)
			{
				_currentSegment[_position] = value;
			}
			else
			{
				AddSegment();
				_currentSegment[0] = value;
			}
			_position++;
			_bytesWritten++;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			int position = _position;
			if (_currentSegment != null && position < _currentSegment.Length - count)
			{
				Buffer.BlockCopy(buffer, offset, _currentSegment, position, count);
				_position = position + count;
				_bytesWritten += count;
			}
			else
			{
				this.Write(buffer.AsSpan(offset, count));
			}
		}

		public WrittenBuffers DetachAndReset()
		{
			if (_completedSegments == null)
			{
				_completedSegments = new List<CompletedBuffer>();
			}
			if (_currentSegment != null)
			{
				_completedSegments.Add(new CompletedBuffer(_currentSegment, _position));
			}
			WrittenBuffers result = new WrittenBuffers(_completedSegments, _bytesWritten);
			_currentSegment = null;
			_completedSegments = null;
			_bytesWritten = 0;
			_position = 0;
			return result;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Reset();
			}
		}
	}
}
