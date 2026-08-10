using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.AspNetCore.Internal
{
	internal sealed class _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EUtf8BufferTextWriter : TextWriter
	{
		private static readonly UTF8Encoding _utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

		private const int MaximumBytesPerUtf8Char = 4;

		[ThreadStatic]
		private static _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EUtf8BufferTextWriter _cachedInstance;

		private readonly Encoder _encoder;

		private IBufferWriter<byte> _bufferWriter;

		private Memory<byte> _memory;

		private int _memoryUsed;

		public override Encoding Encoding => _utf8NoBom;

		public _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EUtf8BufferTextWriter()
		{
			_encoder = _utf8NoBom.GetEncoder();
		}

		public static _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EUtf8BufferTextWriter Get(IBufferWriter<byte> bufferWriter)
		{
			_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EUtf8BufferTextWriter _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EUtf8BufferTextWriter2 = _cachedInstance;
			if (_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EUtf8BufferTextWriter2 == null)
			{
				_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EUtf8BufferTextWriter2 = new _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EUtf8BufferTextWriter();
			}
			_cachedInstance = null;
			_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EUtf8BufferTextWriter2.SetWriter(bufferWriter);
			return _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EUtf8BufferTextWriter2;
		}

		public static void Return(_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EUtf8BufferTextWriter writer)
		{
			_cachedInstance = writer;
			writer._encoder.Reset();
			writer._memory = Memory<byte>.Empty;
			writer._memoryUsed = 0;
			writer._bufferWriter = null;
		}

		public void SetWriter(IBufferWriter<byte> bufferWriter)
		{
			_bufferWriter = bufferWriter;
		}

		public override void Write(char[] buffer, int index, int count)
		{
			WriteInternal(buffer.AsSpan(index, count));
		}

		public override void Write(char[] buffer)
		{
			if (buffer != null)
			{
				WriteInternal(buffer);
			}
		}

		public override void Write(char value)
		{
			if (value <= '\u007f')
			{
				EnsureBuffer();
				_memory.Span[_memoryUsed] = (byte)value;
				_memoryUsed++;
			}
			else
			{
				WriteMultiByteChar(value);
			}
		}

		private unsafe void WriteMultiByteChar(char value)
		{
			Span<byte> buffer = GetBuffer();
			int bytesUsed = 0;
			int charsUsed = 0;
			fixed (byte* bytes = &MemoryMarshal.GetReference(buffer))
			{
				_encoder.Convert(&value, 1, bytes, buffer.Length, flush: false, out charsUsed, out bytesUsed, out var _);
			}
			_memoryUsed += bytesUsed;
		}

		public override void Write(string value)
		{
			if (value != null)
			{
				WriteInternal(value.AsSpan());
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Span<byte> GetBuffer()
		{
			EnsureBuffer();
			return _memory.Span.Slice(_memoryUsed, _memory.Length - _memoryUsed);
		}

		private void EnsureBuffer()
		{
			if (_memory.Length - _memoryUsed < 4)
			{
				if (_memoryUsed > 0)
				{
					_bufferWriter.Advance(_memoryUsed);
				}
				_memory = _bufferWriter.GetMemory(4);
				_memoryUsed = 0;
			}
		}

		private unsafe void WriteInternal(ReadOnlySpan<char> buffer)
		{
			while (buffer.Length > 0)
			{
				Span<byte> buffer2 = GetBuffer();
				int bytesUsed = 0;
				int charsUsed = 0;
				fixed (char* chars = &MemoryMarshal.GetReference(buffer))
				{
					fixed (byte* bytes = &MemoryMarshal.GetReference(buffer2))
					{
						_encoder.Convert(chars, buffer.Length, bytes, buffer2.Length, flush: false, out charsUsed, out bytesUsed, out var _);
					}
				}
				buffer = buffer.Slice(charsUsed);
				_memoryUsed += bytesUsed;
			}
		}

		public override void Flush()
		{
			if (_memoryUsed > 0)
			{
				_bufferWriter.Advance(_memoryUsed);
				_memory = _memory.Slice(_memoryUsed, _memory.Length - _memoryUsed);
				_memoryUsed = 0;
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				Flush();
			}
		}
	}
}
