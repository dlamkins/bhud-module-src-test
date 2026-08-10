using System;
using System.Buffers;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Microsoft.AspNetCore.Internal
{
	internal sealed class ReusableUtf8JsonWriter
	{
		[ThreadStatic]
		private static ReusableUtf8JsonWriter _cachedInstance;

		private readonly Utf8JsonWriter _writer;

		public ReusableUtf8JsonWriter(IBufferWriter<byte> stream)
		{
			_writer = new Utf8JsonWriter(stream, new JsonWriterOptions
			{
				SkipValidation = true,
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			});
		}

		public static ReusableUtf8JsonWriter Get(IBufferWriter<byte> stream)
		{
			ReusableUtf8JsonWriter reusableUtf8JsonWriter = _cachedInstance;
			if (reusableUtf8JsonWriter == null)
			{
				reusableUtf8JsonWriter = new ReusableUtf8JsonWriter(stream);
			}
			_cachedInstance = null;
			reusableUtf8JsonWriter._writer.Reset(stream);
			return reusableUtf8JsonWriter;
		}

		public static void Return(ReusableUtf8JsonWriter writer)
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
