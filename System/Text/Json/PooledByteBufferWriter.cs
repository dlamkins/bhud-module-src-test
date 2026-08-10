using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Text.Json
{
	internal sealed class PooledByteBufferWriter : IBufferWriter<byte>, IDisposable
	{
		private byte[] _rentedBuffer;

		private int _index;

		private const int MinimumBufferSize = 256;

		public const int MaximumBufferSize = 2147483591;

		public ReadOnlyMemory<byte> WrittenMemory => _rentedBuffer.AsMemory(0, _index);

		public int WrittenCount => _index;

		public int Capacity => _rentedBuffer.Length;

		public int FreeCapacity => _rentedBuffer.Length - _index;

		private PooledByteBufferWriter()
		{
		}

		public PooledByteBufferWriter(int initialCapacity)
			: this()
		{
			_rentedBuffer = ArrayPool<byte>.Shared.Rent(initialCapacity);
			_index = 0;
		}

		public void Clear()
		{
			ClearHelper();
		}

		public void ClearAndReturnBuffers()
		{
			ClearHelper();
			byte[] rentedBuffer = _rentedBuffer;
			_rentedBuffer = null;
			ArrayPool<byte>.Shared.Return(rentedBuffer);
		}

		private void ClearHelper()
		{
			_rentedBuffer.AsSpan(0, _index).Clear();
			_index = 0;
		}

		public void Dispose()
		{
			if (_rentedBuffer != null)
			{
				ClearHelper();
				byte[] rentedBuffer = _rentedBuffer;
				_rentedBuffer = null;
				ArrayPool<byte>.Shared.Return(rentedBuffer);
			}
		}

		public void InitializeEmptyInstance(int initialCapacity)
		{
			_rentedBuffer = ArrayPool<byte>.Shared.Rent(initialCapacity);
			_index = 0;
		}

		public static PooledByteBufferWriter CreateEmptyInstanceForCaching()
		{
			return new PooledByteBufferWriter();
		}

		public void Advance(int count)
		{
			_index += count;
		}

		public Memory<byte> GetMemory(int sizeHint = 256)
		{
			CheckAndResizeBuffer(sizeHint);
			return _rentedBuffer.AsMemory(_index);
		}

		public Span<byte> GetSpan(int sizeHint = 256)
		{
			CheckAndResizeBuffer(sizeHint);
			return _rentedBuffer.AsSpan(_index);
		}

		internal Task WriteToStreamAsync(Stream destination, CancellationToken cancellationToken)
		{
			return destination.WriteAsync(_rentedBuffer, 0, _index, cancellationToken);
		}

		internal void WriteToStream(Stream destination)
		{
			destination.Write(_rentedBuffer, 0, _index);
		}

		private void CheckAndResizeBuffer(int sizeHint)
		{
			int num = _rentedBuffer.Length;
			int num2 = num - _index;
			if (_index >= 1073741795)
			{
				sizeHint = Math.Max(sizeHint, 2147483591 - num);
			}
			if (sizeHint <= num2)
			{
				return;
			}
			int num3 = Math.Max(sizeHint, num);
			int num4 = num + num3;
			if ((uint)num4 > 2147483591u)
			{
				num4 = num + sizeHint;
				if ((uint)num4 > 2147483591u)
				{
					ThrowHelper.ThrowOutOfMemoryException_BufferMaximumSizeExceeded((uint)num4);
				}
			}
			byte[] rentedBuffer = _rentedBuffer;
			_rentedBuffer = ArrayPool<byte>.Shared.Rent(num4);
			Span<byte> span = rentedBuffer.AsSpan(0, _index);
			span.CopyTo(_rentedBuffer);
			span.Clear();
			ArrayPool<byte>.Shared.Return(rentedBuffer);
		}
	}
}
