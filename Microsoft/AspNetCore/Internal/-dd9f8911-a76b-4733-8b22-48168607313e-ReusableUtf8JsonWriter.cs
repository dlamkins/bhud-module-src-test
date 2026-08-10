using System;
using System.Buffers;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Microsoft.AspNetCore.Internal
{
	internal sealed class _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter
	{
		[ThreadStatic]
		private static _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter _cachedInstance;

		private readonly Utf8JsonWriter _writer;

		public _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter(IBufferWriter<byte> stream)
		{
			_writer = new Utf8JsonWriter(stream, new JsonWriterOptions
			{
				SkipValidation = true,
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			});
		}

		public static _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter Get(IBufferWriter<byte> stream)
		{
			_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter2 = _cachedInstance;
			if (_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter2 == null)
			{
				_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter2 = new _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter(stream);
			}
			_cachedInstance = null;
			_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter2._writer.Reset(stream);
			return _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter2;
		}

		public static void Return(_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter writer)
		{
			_cachedInstance = writer;
			writer._writer.Reset();
		}

		public Utf8JsonWriter GetJsonWriter()
		{
			return _writer;
		}
	}
}
