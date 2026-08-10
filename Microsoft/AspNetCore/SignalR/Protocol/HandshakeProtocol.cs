using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Internal;

namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal static class HandshakeProtocol
	{
		private const string ProtocolPropertyName = "protocol";

		private static readonly JsonEncodedText ProtocolPropertyNameBytes = JsonEncodedText.Encode("protocol");

		private const string ProtocolVersionPropertyName = "version";

		private static readonly JsonEncodedText ProtocolVersionPropertyNameBytes = JsonEncodedText.Encode("version");

		private const string ErrorPropertyName = "error";

		private static readonly JsonEncodedText ErrorPropertyNameBytes = JsonEncodedText.Encode("error");

		private const string TypePropertyName = "type";

		private static readonly JsonEncodedText TypePropertyNameBytes = JsonEncodedText.Encode("type");

		private static readonly ReadOnlyMemory<byte> _successHandshakeData = GetSuccessHandshakeData();

		private static ReadOnlyMemory<byte> GetSuccessHandshakeData()
		{
			MemoryBufferWriter memoryBufferWriter = MemoryBufferWriter.Get();
			try
			{
				WriteResponseMessage(HandshakeResponseMessage.Empty, memoryBufferWriter);
				return memoryBufferWriter.ToArray();
			}
			finally
			{
				MemoryBufferWriter.Return(memoryBufferWriter);
			}
		}

		public static ReadOnlySpan<byte> GetSuccessfulHandshake(IHubProtocol protocol)
		{
			return _successHandshakeData.Span;
		}

		public static void WriteRequestMessage(HandshakeRequestMessage requestMessage, IBufferWriter<byte> output)
		{
			_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter = _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter.Get(output);
			try
			{
				Utf8JsonWriter jsonWriter = _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter.GetJsonWriter();
				jsonWriter.WriteStartObject();
				jsonWriter.WriteString(ProtocolPropertyNameBytes, requestMessage.Protocol);
				jsonWriter.WriteNumber(ProtocolVersionPropertyNameBytes, requestMessage.Version);
				jsonWriter.WriteEndObject();
				jsonWriter.Flush();
			}
			finally
			{
				_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter.Return(_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter);
			}
			_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ETextMessageFormatter.WriteRecordSeparator(output);
		}

		public static void WriteResponseMessage(HandshakeResponseMessage responseMessage, IBufferWriter<byte> output)
		{
			_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter = _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter.Get(output);
			try
			{
				Utf8JsonWriter jsonWriter = _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter.GetJsonWriter();
				jsonWriter.WriteStartObject();
				if (!string.IsNullOrEmpty(responseMessage.Error))
				{
					jsonWriter.WriteString(ErrorPropertyNameBytes, responseMessage.Error);
				}
				jsonWriter.WriteEndObject();
				jsonWriter.Flush();
			}
			finally
			{
				_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter.Return(_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003EReusableUtf8JsonWriter);
			}
			_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ETextMessageFormatter.WriteRecordSeparator(output);
		}

		public static bool TryParseResponseMessage(ref ReadOnlySequence<byte> buffer, [_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ENotNullWhen(true)] out HandshakeResponseMessage? responseMessage)
		{
			if (!_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ETextMessageParser.TryParseMessage(ref buffer, out var payload))
			{
				responseMessage = null;
				return false;
			}
			Utf8JsonReader reader = new Utf8JsonReader(payload, isFinalBlock: true, default(JsonReaderState));
			_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ESystemTextJsonExtensions.CheckRead(ref reader);
			_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ESystemTextJsonExtensions.EnsureObjectStart(ref reader);
			string error = null;
			while (_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ESystemTextJsonExtensions.CheckRead(ref reader))
			{
				if (reader.TokenType == JsonTokenType.PropertyName)
				{
					if (reader.ValueTextEquals(TypePropertyNameBytes.EncodedUtf8Bytes))
					{
						throw new InvalidDataException("Expected a handshake response from the server.");
					}
					if (reader.ValueTextEquals(ErrorPropertyNameBytes.EncodedUtf8Bytes))
					{
						error = _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ESystemTextJsonExtensions.ReadAsString(ref reader, "error");
					}
					else
					{
						reader.Skip();
					}
					continue;
				}
				if (reader.TokenType == JsonTokenType.EndObject)
				{
					break;
				}
				throw new InvalidDataException($"Unexpected token '{reader.TokenType}' when reading handshake response JSON.");
			}
			responseMessage = new HandshakeResponseMessage(error);
			return true;
		}

		public static bool TryParseRequestMessage(ref ReadOnlySequence<byte> buffer, [_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ENotNullWhen(true)] out HandshakeRequestMessage? requestMessage)
		{
			if (!_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ETextMessageParser.TryParseMessage(ref buffer, out var payload))
			{
				requestMessage = null;
				return false;
			}
			Utf8JsonReader reader = new Utf8JsonReader(payload, isFinalBlock: true, default(JsonReaderState));
			_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ESystemTextJsonExtensions.CheckRead(ref reader);
			_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ESystemTextJsonExtensions.EnsureObjectStart(ref reader);
			string text = null;
			int? num = null;
			while (_003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ESystemTextJsonExtensions.CheckRead(ref reader))
			{
				if (reader.TokenType == JsonTokenType.PropertyName)
				{
					if (reader.ValueTextEquals(ProtocolPropertyNameBytes.EncodedUtf8Bytes))
					{
						text = _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ESystemTextJsonExtensions.ReadAsString(ref reader, "protocol");
					}
					else if (reader.ValueTextEquals(ProtocolVersionPropertyNameBytes.EncodedUtf8Bytes))
					{
						num = _003Cdd9f8911_002Da76b_002D4733_002D8b22_002D48168607313e_003ESystemTextJsonExtensions.ReadAsInt32(ref reader, "version");
					}
					else
					{
						reader.Skip();
					}
					continue;
				}
				if (reader.TokenType == JsonTokenType.EndObject)
				{
					break;
				}
				throw new InvalidDataException($"Unexpected token '{reader.TokenType}' when reading handshake request JSON. Message content: {GetPayloadAsString()}");
			}
			if (text == null)
			{
				throw new InvalidDataException("Missing required property 'protocol'. Message content: " + GetPayloadAsString());
			}
			if (!num.HasValue)
			{
				throw new InvalidDataException("Missing required property 'version'. Message content: " + GetPayloadAsString());
			}
			requestMessage = new HandshakeRequestMessage(text, num.Value);
			return true;
			string GetPayloadAsString()
			{
				return Encoding.UTF8.GetString(BuffersExtensions.ToArray(in payload));
			}
		}
	}
}
