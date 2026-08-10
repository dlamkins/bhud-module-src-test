using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.AspNetCore.SignalR.Internal
{
	internal sealed class _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EUtf8BufferTextReader : TextReader
	{
		private readonly Decoder _decoder;

		private ReadOnlySequence<byte> _utf8Buffer;

		[ThreadStatic]
		private static _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EUtf8BufferTextReader _cachedInstance;

		public _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EUtf8BufferTextReader()
		{
			_decoder = Encoding.UTF8.GetDecoder();
		}

		public static _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EUtf8BufferTextReader Get(in ReadOnlySequence<byte> utf8Buffer)
		{
			_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EUtf8BufferTextReader _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EUtf8BufferTextReader2 = _cachedInstance;
			if (_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EUtf8BufferTextReader2 == null)
			{
				_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EUtf8BufferTextReader2 = new _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EUtf8BufferTextReader();
			}
			_cachedInstance = null;
			_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EUtf8BufferTextReader2.SetBuffer(in utf8Buffer);
			return _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EUtf8BufferTextReader2;
		}

		public static void Return(_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EUtf8BufferTextReader reader)
		{
			_cachedInstance = reader;
		}

		public void SetBuffer(in ReadOnlySequence<byte> utf8Buffer)
		{
			_utf8Buffer = utf8Buffer;
			_decoder.Reset();
		}

		public unsafe override int Read(char[] buffer, int index, int count)
		{
			if (_utf8Buffer.IsEmpty)
			{
				return 0;
			}
			ReadOnlySpan<byte> span = _utf8Buffer.First.Span;
			int bytesUsed = 0;
			int charsUsed = 0;
			fixed (char* chars = &buffer[index])
			{
				fixed (byte* bytes = &MemoryMarshal.GetReference(span))
				{
					_decoder.Convert(bytes, span.Length, chars, count, flush: false, out bytesUsed, out charsUsed, out var _);
				}
			}
			_utf8Buffer = _utf8Buffer.Slice(bytesUsed);
			return charsUsed;
		}
	}
}
