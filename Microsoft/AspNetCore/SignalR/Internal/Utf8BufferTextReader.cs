using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.AspNetCore.SignalR.Internal
{
	internal sealed class Utf8BufferTextReader : TextReader
	{
		private readonly Decoder _decoder;

		private ReadOnlySequence<byte> _utf8Buffer;

		[ThreadStatic]
		private static Utf8BufferTextReader _cachedInstance;

		public Utf8BufferTextReader()
		{
			_decoder = Encoding.UTF8.GetDecoder();
		}

		public static Utf8BufferTextReader Get(in ReadOnlySequence<byte> utf8Buffer)
		{
			Utf8BufferTextReader utf8BufferTextReader = _cachedInstance;
			if (utf8BufferTextReader == null)
			{
				utf8BufferTextReader = new Utf8BufferTextReader();
			}
			_cachedInstance = null;
			utf8BufferTextReader.SetBuffer(in utf8Buffer);
			return utf8BufferTextReader;
		}

		public static void Return(Utf8BufferTextReader reader)
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
