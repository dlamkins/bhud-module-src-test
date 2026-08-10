using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Internal;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.SignalR.Protocol
{
	internal sealed class JsonHubProtocol : IHubProtocol
	{
		private const string ResultPropertyName = "result";

		private static readonly JsonEncodedText ResultPropertyNameBytes = JsonEncodedText.Encode("result");

		private const string ItemPropertyName = "item";

		private static readonly JsonEncodedText ItemPropertyNameBytes = JsonEncodedText.Encode("item");

		private const string InvocationIdPropertyName = "invocationId";

		private static readonly JsonEncodedText InvocationIdPropertyNameBytes = JsonEncodedText.Encode("invocationId");

		private const string StreamIdsPropertyName = "streamIds";

		private static readonly JsonEncodedText StreamIdsPropertyNameBytes = JsonEncodedText.Encode("streamIds");

		private const string TypePropertyName = "type";

		private static readonly JsonEncodedText TypePropertyNameBytes = JsonEncodedText.Encode("type");

		private const string ErrorPropertyName = "error";

		private static readonly JsonEncodedText ErrorPropertyNameBytes = JsonEncodedText.Encode("error");

		private const string AllowReconnectPropertyName = "allowReconnect";

		private static readonly JsonEncodedText AllowReconnectPropertyNameBytes = JsonEncodedText.Encode("allowReconnect");

		private const string TargetPropertyName = "target";

		private static readonly JsonEncodedText TargetPropertyNameBytes = JsonEncodedText.Encode("target");

		private const string ArgumentsPropertyName = "arguments";

		private static readonly JsonEncodedText ArgumentsPropertyNameBytes = JsonEncodedText.Encode("arguments");

		private const string HeadersPropertyName = "headers";

		private static readonly JsonEncodedText HeadersPropertyNameBytes = JsonEncodedText.Encode("headers");

		private const string SequenceIdPropertyName = "sequenceId";

		private static readonly JsonEncodedText SequenceIdPropertyNameBytes = JsonEncodedText.Encode("sequenceId");

		private const string ProtocolName = "json";

		private const int ProtocolVersion = 2;

		private readonly JsonSerializerOptions _payloadSerializerOptions;

		public string Name => "json";

		public int Version => 2;

		public TransferFormat TransferFormat => TransferFormat.Text;

		public JsonHubProtocol()
			: this(Options.Create(new JsonHubProtocolOptions()))
		{
		}

		public JsonHubProtocol(IOptions<JsonHubProtocolOptions> options)
		{
			_payloadSerializerOptions = options.Value.PayloadSerializerOptions;
		}

		public bool IsVersionSupported(int version)
		{
			return version <= Version;
		}

		public bool TryParseMessage(ref ReadOnlySequence<byte> input, IInvocationBinder binder, [_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ENotNullWhen(true)] out HubMessage? message)
		{
			if (!TextMessageParser.TryParseMessage(ref input, out var payload))
			{
				message = null;
				return false;
			}
			message = ParseMessage(payload, binder);
			return message != null;
		}

		public void WriteMessage(HubMessage message, IBufferWriter<byte> output)
		{
			WriteMessageCore(message, output);
			TextMessageFormatter.WriteRecordSeparator(output);
		}

		public ReadOnlyMemory<byte> GetMessageBytes(HubMessage message)
		{
			return HubProtocolExtensions.GetMessageBytes(this, message);
		}

		private HubMessage ParseMessage(ReadOnlySequence<byte> input, IInvocationBinder binder)
		{
			try
			{
				int? num = null;
				string text = null;
				string text2 = null;
				string error = null;
				bool hasItem = false;
				object item = null;
				bool hasResult = false;
				object result = null;
				bool hasArguments = false;
				object[] arguments = null;
				string[] streamIds = null;
				bool flag = false;
				Utf8JsonReader reader = default(Utf8JsonReader);
				bool flag2 = false;
				Utf8JsonReader reader2 = default(Utf8JsonReader);
				bool flag3 = false;
				Utf8JsonReader reader3 = default(Utf8JsonReader);
				ExceptionDispatchInfo exceptionDispatchInfo = null;
				Dictionary<string, string> headers = null;
				bool flag4 = false;
				bool allowReconnect = false;
				long? sequenceId = null;
				Utf8JsonReader reader4 = new Utf8JsonReader(input, isFinalBlock: true, default(JsonReaderState));
				_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.CheckRead(ref reader4);
				_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.EnsureObjectStart(ref reader4);
				do
				{
					switch (reader4.TokenType)
					{
					case JsonTokenType.PropertyName:
						if (reader4.ValueTextEquals(TypePropertyNameBytes.EncodedUtf8Bytes))
						{
							num = _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.ReadAsInt32(ref reader4, "type");
							if (!num.HasValue)
							{
								throw new InvalidDataException(string.Format("Expected '{0}' to be of type {1}.", "type", JsonTokenType.Number));
							}
						}
						else if (reader4.ValueTextEquals(InvocationIdPropertyNameBytes.EncodedUtf8Bytes))
						{
							text = _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.ReadAsString(ref reader4, "invocationId");
						}
						else if (reader4.ValueTextEquals(StreamIdsPropertyNameBytes.EncodedUtf8Bytes))
						{
							_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.CheckRead(ref reader4);
							if (reader4.TokenType != JsonTokenType.StartArray)
							{
								throw new InvalidDataException("Expected 'streamIds' to be of type " + _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.GetTokenString(JsonTokenType.StartArray) + ".");
							}
							List<string> list = null;
							reader4.Read();
							while (reader4.TokenType != JsonTokenType.EndArray)
							{
								if (list == null)
								{
									list = new List<string>();
								}
								list.Add(reader4.GetString() ?? throw new InvalidDataException("Null value for 'streamIds' is not valid."));
								reader4.Read();
							}
							streamIds = list?.ToArray() ?? Array.Empty<string>();
						}
						else if (reader4.ValueTextEquals(TargetPropertyNameBytes.EncodedUtf8Bytes))
						{
							text2 = _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.ReadAsString(ref reader4, "target");
						}
						else if (reader4.ValueTextEquals(ErrorPropertyNameBytes.EncodedUtf8Bytes))
						{
							error = _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.ReadAsString(ref reader4, "error");
						}
						else if (reader4.ValueTextEquals(AllowReconnectPropertyNameBytes.EncodedUtf8Bytes))
						{
							allowReconnect = _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.ReadAsBoolean(ref reader4, "allowReconnect");
						}
						else if (reader4.ValueTextEquals(ResultPropertyNameBytes.EncodedUtf8Bytes))
						{
							hasResult = true;
							if (string.IsNullOrEmpty(text))
							{
								flag3 = true;
								reader3 = reader4;
								reader4.Skip();
								break;
							}
							Type type = ProtocolHelper.TryGetReturnType(binder, text);
							if ((object)type == null)
							{
								reader4.Skip();
								result = null;
								break;
							}
							try
							{
								result = BindType(ref reader4, input, type);
							}
							catch (Exception ex)
							{
								error = "Error trying to deserialize result to " + type.Name + ". " + ex.Message;
								hasResult = false;
							}
						}
						else if (reader4.ValueTextEquals(ItemPropertyNameBytes.EncodedUtf8Bytes))
						{
							_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.CheckRead(ref reader4);
							hasItem = true;
							string text3 = null;
							if (!string.IsNullOrEmpty(text))
							{
								text3 = text;
								try
								{
									Type streamItemType = binder.GetStreamItemType(text3);
									item = BindType(ref reader4, streamItemType);
								}
								catch (Exception source)
								{
									return new StreamBindingFailureMessage(text3, ExceptionDispatchInfo.Capture(source));
								}
							}
							else
							{
								flag2 = true;
								reader2 = reader4;
								reader4.Skip();
							}
						}
						else if (reader4.ValueTextEquals(ArgumentsPropertyNameBytes.EncodedUtf8Bytes))
						{
							_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.CheckRead(ref reader4);
							int currentDepth = reader4.CurrentDepth;
							if (reader4.TokenType != JsonTokenType.StartArray)
							{
								throw new InvalidDataException("Expected 'arguments' to be of type " + _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.GetTokenString(JsonTokenType.StartArray) + ".");
							}
							hasArguments = true;
							if (string.IsNullOrEmpty(text2))
							{
								flag = true;
								reader = reader4;
								reader4.Skip();
								break;
							}
							try
							{
								IReadOnlyList<Type> parameterTypes = binder.GetParameterTypes(text2);
								arguments = BindTypes(ref reader4, parameterTypes);
							}
							catch (Exception source2)
							{
								exceptionDispatchInfo = ExceptionDispatchInfo.Capture(source2);
								while ((reader4.CurrentDepth == currentDepth && reader4.TokenType == JsonTokenType.StartArray) || reader4.CurrentDepth > currentDepth)
								{
									_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.CheckRead(ref reader4);
								}
							}
						}
						else if (reader4.ValueTextEquals(HeadersPropertyNameBytes.EncodedUtf8Bytes))
						{
							_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.CheckRead(ref reader4);
							headers = ReadHeaders(ref reader4);
						}
						else if (reader4.ValueTextEquals(SequenceIdPropertyNameBytes.EncodedUtf8Bytes))
						{
							sequenceId = _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.ReadAsInt64(ref reader4, "sequenceId");
						}
						else
						{
							_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.CheckRead(ref reader4);
							reader4.Skip();
						}
						break;
					case JsonTokenType.EndObject:
						flag4 = true;
						break;
					}
				}
				while (!flag4 && _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.CheckRead(ref reader4));
				HubMessage message;
				switch (num)
				{
				case 1:
					if (text2 == null)
					{
						throw new InvalidDataException("Missing required property 'target'.");
					}
					if (flag)
					{
						try
						{
							IReadOnlyList<Type> parameterTypes2 = binder.GetParameterTypes(text2);
							arguments = BindTypes(ref reader, parameterTypes2);
						}
						catch (Exception source3)
						{
							exceptionDispatchInfo = ExceptionDispatchInfo.Capture(source3);
						}
					}
					message = ((exceptionDispatchInfo != null) ? new InvocationBindingFailureMessage(text, text2, exceptionDispatchInfo) : BindInvocationMessage(text, text2, arguments, hasArguments, streamIds));
					break;
				case 4:
					if (text2 == null)
					{
						throw new InvalidDataException("Missing required property 'target'.");
					}
					if (flag)
					{
						try
						{
							IReadOnlyList<Type> parameterTypes3 = binder.GetParameterTypes(text2);
							arguments = BindTypes(ref reader, parameterTypes3);
						}
						catch (Exception source4)
						{
							exceptionDispatchInfo = ExceptionDispatchInfo.Capture(source4);
						}
					}
					message = ((exceptionDispatchInfo != null) ? new InvocationBindingFailureMessage(text, text2, exceptionDispatchInfo) : BindStreamInvocationMessage(text, text2, arguments, hasArguments, streamIds));
					break;
				case 2:
					if (text == null)
					{
						throw new InvalidDataException("Missing required property 'invocationId'.");
					}
					if (flag2)
					{
						try
						{
							Type streamItemType2 = binder.GetStreamItemType(text);
							item = BindType(ref reader2, streamItemType2);
						}
						catch (JsonException source5)
						{
							message = new StreamBindingFailureMessage(text, ExceptionDispatchInfo.Capture(source5));
							break;
						}
					}
					message = BindStreamItemMessage(text, item, hasItem);
					break;
				case 3:
					if (text == null)
					{
						throw new InvalidDataException("Missing required property 'invocationId'.");
					}
					if (flag3)
					{
						Type type2 = ProtocolHelper.TryGetReturnType(binder, text);
						if ((object)type2 == null)
						{
							result = null;
						}
						else
						{
							try
							{
								result = BindType(ref reader3, input, type2);
							}
							catch (Exception ex2)
							{
								error = "Error trying to deserialize result to " + type2.Name + ". " + ex2.Message;
								hasResult = false;
							}
						}
					}
					message = BindCompletionMessage(text, error, result, hasResult);
					break;
				case 5:
					message = BindCancelInvocationMessage(text);
					break;
				case 6:
					return PingMessage.Instance;
				case 7:
					return BindCloseMessage(error, allowReconnect);
				case 8:
					return BindAckMessage(sequenceId);
				case 9:
					return BindSequenceMessage(sequenceId);
				case null:
					throw new InvalidDataException("Missing required property 'type'.");
				default:
					return null;
				}
				return ApplyHeaders(message, headers);
			}
			catch (JsonException innerException)
			{
				throw new InvalidDataException("Error reading JSON.", innerException);
			}
		}

		private static Dictionary<string, string> ReadHeaders(ref Utf8JsonReader reader)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.Ordinal);
			if (reader.TokenType != JsonTokenType.StartObject)
			{
				throw new InvalidDataException(string.Format("Expected '{0}' to be of type {1}.", "headers", JsonTokenType.StartObject));
			}
			while (reader.Read())
			{
				switch (reader.TokenType)
				{
				case JsonTokenType.PropertyName:
				{
					string @string = reader.GetString();
					_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.CheckRead(ref reader);
					if (reader.TokenType != JsonTokenType.String)
					{
						throw new InvalidDataException($"Expected header '{@string}' to be of type {JsonTokenType.String}.");
					}
					dictionary[@string] = reader.GetString();
					break;
				}
				case JsonTokenType.EndObject:
					return dictionary;
				}
			}
			throw new InvalidDataException("Unexpected end when reading message headers");
		}

		private void WriteMessageCore(HubMessage message, IBufferWriter<byte> stream)
		{
			_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter = _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter.Get(stream);
			try
			{
				Utf8JsonWriter jsonWriter = _003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter.GetJsonWriter();
				jsonWriter.WriteStartObject();
				InvocationMessage invocationMessage = message as InvocationMessage;
				if (invocationMessage == null)
				{
					StreamInvocationMessage streamInvocationMessage = message as StreamInvocationMessage;
					if (streamInvocationMessage == null)
					{
						StreamItemMessage streamItemMessage = message as StreamItemMessage;
						if (streamItemMessage == null)
						{
							CompletionMessage completionMessage = message as CompletionMessage;
							if (completionMessage == null)
							{
								CancelInvocationMessage cancelInvocationMessage = message as CancelInvocationMessage;
								if (cancelInvocationMessage == null)
								{
									if (!(message is PingMessage))
									{
										CloseMessage closeMessage = message as CloseMessage;
										if (closeMessage == null)
										{
											AckMessage ackMessage = message as AckMessage;
											if (ackMessage == null)
											{
												SequenceMessage sequenceMessage = message as SequenceMessage;
												if (sequenceMessage == null)
												{
													throw new InvalidOperationException("Unsupported message type: " + message.GetType().FullName);
												}
												WriteMessageType(jsonWriter, 9);
												WriteSequenceMessage(sequenceMessage, jsonWriter);
											}
											else
											{
												WriteMessageType(jsonWriter, 8);
												WriteAckMessage(ackMessage, jsonWriter);
											}
										}
										else
										{
											WriteMessageType(jsonWriter, 7);
											WriteCloseMessage(closeMessage, jsonWriter);
										}
									}
									else
									{
										WriteMessageType(jsonWriter, 6);
									}
								}
								else
								{
									WriteMessageType(jsonWriter, 5);
									WriteHeaders(jsonWriter, cancelInvocationMessage);
									WriteCancelInvocationMessage(cancelInvocationMessage, jsonWriter);
								}
							}
							else
							{
								WriteMessageType(jsonWriter, 3);
								WriteHeaders(jsonWriter, completionMessage);
								WriteCompletionMessage(completionMessage, jsonWriter);
							}
						}
						else
						{
							WriteMessageType(jsonWriter, 2);
							WriteHeaders(jsonWriter, streamItemMessage);
							WriteStreamItemMessage(streamItemMessage, jsonWriter);
						}
					}
					else
					{
						WriteMessageType(jsonWriter, 4);
						WriteHeaders(jsonWriter, streamInvocationMessage);
						WriteStreamInvocationMessage(streamInvocationMessage, jsonWriter);
					}
				}
				else
				{
					WriteMessageType(jsonWriter, 1);
					WriteHeaders(jsonWriter, invocationMessage);
					WriteInvocationMessage(invocationMessage, jsonWriter);
				}
				jsonWriter.WriteEndObject();
				jsonWriter.Flush();
			}
			finally
			{
				_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter.Return(_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003EReusableUtf8JsonWriter);
			}
		}

		private static void WriteHeaders(Utf8JsonWriter writer, HubInvocationMessage message)
		{
			if (message.Headers == null || message.Headers!.Count <= 0)
			{
				return;
			}
			writer.WriteStartObject(HeadersPropertyNameBytes);
			foreach (KeyValuePair<string, string> item in message.Headers!)
			{
				writer.WriteString(item.Key, item.Value);
			}
			writer.WriteEndObject();
		}

		private void WriteCompletionMessage(CompletionMessage message, Utf8JsonWriter writer)
		{
			WriteInvocationId(message, writer);
			if (!string.IsNullOrEmpty(message.Error))
			{
				writer.WriteString(ErrorPropertyNameBytes, message.Error);
			}
			else
			{
				if (!message.HasResult)
				{
					return;
				}
				writer.WritePropertyName(ResultPropertyNameBytes);
				if (message.Result == null)
				{
					writer.WriteNullValue();
					return;
				}
				RawResult rawResult = message.Result as RawResult;
				if (rawResult != null)
				{
					writer.WriteRawValue(rawResult.RawSerializedData, skipInputValidation: true);
				}
				else
				{
					JsonSerializer.Serialize(writer, message.Result, message.Result!.GetType(), _payloadSerializerOptions);
				}
			}
		}

		private static void WriteCancelInvocationMessage(CancelInvocationMessage message, Utf8JsonWriter writer)
		{
			WriteInvocationId(message, writer);
		}

		private void WriteStreamItemMessage(StreamItemMessage message, Utf8JsonWriter writer)
		{
			WriteInvocationId(message, writer);
			writer.WritePropertyName(ItemPropertyNameBytes);
			if (message.Item == null)
			{
				writer.WriteNullValue();
			}
			else
			{
				JsonSerializer.Serialize(writer, message.Item, message.Item!.GetType(), _payloadSerializerOptions);
			}
		}

		private void WriteInvocationMessage(InvocationMessage message, Utf8JsonWriter writer)
		{
			WriteInvocationId(message, writer);
			writer.WriteString(TargetPropertyNameBytes, message.Target);
			WriteArguments(message.Arguments, writer);
			WriteStreamIds(message.StreamIds, writer);
		}

		private void WriteStreamInvocationMessage(StreamInvocationMessage message, Utf8JsonWriter writer)
		{
			WriteInvocationId(message, writer);
			writer.WriteString(TargetPropertyNameBytes, message.Target);
			WriteArguments(message.Arguments, writer);
			WriteStreamIds(message.StreamIds, writer);
		}

		private static void WriteCloseMessage(CloseMessage message, Utf8JsonWriter writer)
		{
			if (message.Error != null)
			{
				writer.WriteString(ErrorPropertyNameBytes, message.Error);
			}
			if (message.AllowReconnect)
			{
				writer.WriteBoolean(AllowReconnectPropertyNameBytes, value: true);
			}
		}

		private static void WriteAckMessage(AckMessage message, Utf8JsonWriter writer)
		{
			writer.WriteNumber("sequenceId", message.SequenceId);
		}

		private static void WriteSequenceMessage(SequenceMessage message, Utf8JsonWriter writer)
		{
			writer.WriteNumber("sequenceId", message.SequenceId);
		}

		private void WriteArguments(object[] arguments, Utf8JsonWriter writer)
		{
			writer.WriteStartArray(ArgumentsPropertyNameBytes);
			foreach (object obj in arguments)
			{
				if (obj == null)
				{
					writer.WriteNullValue();
				}
				else
				{
					JsonSerializer.Serialize(writer, obj, obj.GetType(), _payloadSerializerOptions);
				}
			}
			writer.WriteEndArray();
		}

		private static void WriteStreamIds(string[] streamIds, Utf8JsonWriter writer)
		{
			if (streamIds != null)
			{
				writer.WriteStartArray(StreamIdsPropertyNameBytes);
				foreach (string value in streamIds)
				{
					writer.WriteStringValue(value);
				}
				writer.WriteEndArray();
			}
		}

		private static void WriteInvocationId(HubInvocationMessage message, Utf8JsonWriter writer)
		{
			if (!string.IsNullOrEmpty(message.InvocationId))
			{
				writer.WriteString(InvocationIdPropertyNameBytes, message.InvocationId);
			}
		}

		private static void WriteMessageType(Utf8JsonWriter writer, int type)
		{
			writer.WriteNumber(TypePropertyNameBytes, type);
		}

		private static HubMessage BindCancelInvocationMessage(string invocationId)
		{
			if (string.IsNullOrEmpty(invocationId))
			{
				throw new InvalidDataException("Missing required property 'invocationId'.");
			}
			return new CancelInvocationMessage(invocationId);
		}

		private static HubMessage BindCompletionMessage(string invocationId, string error, object result, bool hasResult)
		{
			if (string.IsNullOrEmpty(invocationId))
			{
				throw new InvalidDataException("Missing required property 'invocationId'.");
			}
			if (error != null && hasResult)
			{
				throw new InvalidDataException("The 'error' and 'result' properties are mutually exclusive.");
			}
			if (hasResult)
			{
				return new CompletionMessage(invocationId, error, result, hasResult: true);
			}
			return new CompletionMessage(invocationId, error, null, hasResult: false);
		}

		private static HubMessage BindStreamItemMessage(string invocationId, object item, bool hasItem)
		{
			if (string.IsNullOrEmpty(invocationId))
			{
				throw new InvalidDataException("Missing required property 'invocationId'.");
			}
			if (!hasItem)
			{
				throw new InvalidDataException("Missing required property 'item'.");
			}
			return new StreamItemMessage(invocationId, item);
		}

		private static HubMessage BindStreamInvocationMessage(string invocationId, string target, object[] arguments, bool hasArguments, string[] streamIds)
		{
			if (string.IsNullOrEmpty(invocationId))
			{
				throw new InvalidDataException("Missing required property 'invocationId'.");
			}
			if (!hasArguments)
			{
				throw new InvalidDataException("Missing required property 'arguments'.");
			}
			if (string.IsNullOrEmpty(target))
			{
				throw new InvalidDataException("Missing required property 'target'.");
			}
			return new StreamInvocationMessage(invocationId, target, arguments, streamIds);
		}

		private static HubMessage BindInvocationMessage(string invocationId, string target, object[] arguments, bool hasArguments, string[] streamIds)
		{
			if (string.IsNullOrEmpty(target))
			{
				throw new InvalidDataException("Missing required property 'target'.");
			}
			if (!hasArguments)
			{
				throw new InvalidDataException("Missing required property 'arguments'.");
			}
			return new InvocationMessage(invocationId, target, arguments, streamIds);
		}

		private object BindType(ref Utf8JsonReader reader, ReadOnlySequence<byte> input, Type type)
		{
			if (type == typeof(RawResult))
			{
				long bytesConsumed = reader.BytesConsumed;
				reader.Skip();
				long bytesConsumed2 = reader.BytesConsumed;
				return new RawResult(input.Slice(bytesConsumed, bytesConsumed2 - bytesConsumed));
			}
			return BindType(ref reader, type);
		}

		private object BindType(ref Utf8JsonReader reader, Type type)
		{
			return JsonSerializer.Deserialize(ref reader, type, _payloadSerializerOptions);
		}

		private object[] BindTypes(ref Utf8JsonReader reader, IReadOnlyList<Type> paramTypes)
		{
			object[] array = null;
			int num = 0;
			int count = paramTypes.Count;
			int currentDepth = reader.CurrentDepth;
			_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.CheckRead(ref reader);
			while (reader.TokenType != JsonTokenType.EndArray && reader.CurrentDepth > currentDepth)
			{
				if (num < count)
				{
					if (array == null)
					{
						array = new object[count];
					}
					try
					{
						array[num] = BindType(ref reader, paramTypes[num]);
					}
					catch (Exception innerException)
					{
						throw new InvalidDataException("Error binding arguments. Make sure that the types of the provided values match the types of the hub method being invoked.", innerException);
					}
				}
				else
				{
					reader.Skip();
				}
				_003Cb4c633b4_002D3a23_002D4085_002Db3de_002D9bc88c51e783_003ESystemTextJsonExtensions.CheckRead(ref reader);
				num++;
			}
			if (num != count)
			{
				throw new InvalidDataException($"Invocation provides {num} argument(s) but target expects {count}.");
			}
			return array ?? Array.Empty<object>();
		}

		private static CloseMessage BindCloseMessage(string error, bool allowReconnect)
		{
			if (error == null && !allowReconnect)
			{
				return CloseMessage.Empty;
			}
			return new CloseMessage(error, allowReconnect);
		}

		private static AckMessage BindAckMessage(long? sequenceId)
		{
			if (!sequenceId.HasValue)
			{
				throw new InvalidDataException("Missing required property 'sequenceId'.");
			}
			return new AckMessage(sequenceId.Value);
		}

		private static SequenceMessage BindSequenceMessage(long? sequenceId)
		{
			if (!sequenceId.HasValue)
			{
				throw new InvalidDataException("Missing required property 'sequenceId'.");
			}
			return new SequenceMessage(sequenceId.Value);
		}

		private static HubMessage ApplyHeaders(HubMessage message, Dictionary<string, string> headers)
		{
			if (headers != null)
			{
				HubInvocationMessage hubInvocationMessage = message as HubInvocationMessage;
				if (hubInvocationMessage != null)
				{
					hubInvocationMessage.Headers = headers;
				}
			}
			return message;
		}

		internal static JsonSerializerOptions CreateDefaultSerializerSettings()
		{
			return new JsonSerializerOptions
			{
				WriteIndented = false,
				ReadCommentHandling = JsonCommentHandling.Disallow,
				AllowTrailingCommas = false,
				DefaultIgnoreCondition = JsonIgnoreCondition.Never,
				IgnoreReadOnlyProperties = false,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				PropertyNameCaseInsensitive = true,
				MaxDepth = 64,
				DictionaryKeyPolicy = null,
				DefaultBufferSize = 16384,
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			};
		}
	}
}
