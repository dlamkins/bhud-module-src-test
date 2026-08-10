using System;
using System.Buffers;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Microsoft.AspNetCore.Internal
{
	internal sealed class _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter
	{
		[ThreadStatic]
		private static _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter _cachedInstance;

		private readonly Utf8JsonWriter _writer;

		public _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter(IBufferWriter<byte> stream)
		{
			_writer = new Utf8JsonWriter(stream, new JsonWriterOptions
			{
				SkipValidation = true,
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			});
		}

		public static _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter Get(IBufferWriter<byte> stream)
		{
			_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter2 = _cachedInstance;
			if (_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter2 == null)
			{
				_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter2 = new _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter(stream);
			}
			_cachedInstance = null;
			_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter2._writer.Reset(stream);
			return _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter2;
		}

		public static void Return(_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter writer)
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
