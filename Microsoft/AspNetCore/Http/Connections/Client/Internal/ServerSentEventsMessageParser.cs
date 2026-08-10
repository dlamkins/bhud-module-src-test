using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Microsoft.AspNetCore.Http.Connections.Client.Internal
{
	internal sealed class ServerSentEventsMessageParser
	{
		public enum ParseResult
		{
			Completed,
			Incomplete
		}

		private enum InternalParseState
		{
			ReadMessagePayload,
			ReadEndOfMessage,
			Error
		}

		private const byte ByteCR = 13;

		private const byte ByteLF = 10;

		private const byte ByteColon = 58;

		private static readonly byte[] _newLine = Encoding.UTF8.GetBytes(Environment.NewLine);

		private InternalParseState _internalParserState;

		private readonly List<byte[]> _data = new List<byte[]>();

		private static ReadOnlySpan<byte> DataPrefix => new byte[6] { 100, 97, 116, 97, 58, 32 };

		private static ReadOnlySpan<byte> SseLineEnding => new byte[2] { 13, 10 };

		public ParseResult ParseMessage(ReadOnlySequence<byte> buffer, out SequencePosition consumed, out SequencePosition examined, out byte[]? message)
		{
			consumed = buffer.Start;
			examined = buffer.End;
			message = null;
			SequencePosition start = consumed;
			while (buffer.Length > 0)
			{
				SequencePosition? sequencePosition = buffer.PositionOf<byte>(10);
				ReadOnlySequence<byte> buffer2;
				if (sequencePosition.HasValue)
				{
					SequencePosition valueOrDefault = sequencePosition.GetValueOrDefault();
					valueOrDefault = buffer.GetPosition(1L, valueOrDefault);
					buffer2 = buffer.Slice(start, valueOrDefault);
					ReadOnlySpan<byte> line = ConvertBufferToSpan(in buffer2);
					buffer = buffer.Slice(line.Length);
					if (line.Length <= 1)
					{
						throw new FormatException("There was an error in the frame format");
					}
					if (line[0] == 58)
					{
						start = valueOrDefault;
						consumed = valueOrDefault;
						continue;
					}
					if (IsMessageEnd(line))
					{
						_internalParserState = InternalParseState.ReadEndOfMessage;
					}
					else
					{
						if (line[line.Length - SseLineEnding.Length] != 13)
						{
							throw new FormatException("Unexpected '\\n' in message. A '\\n' character can only be used as part of the newline sequence '\\r\\n'");
						}
						EnsureStartsWithDataPrefix(line);
					}
					byte[] array = Array.Empty<byte>();
					switch (_internalParserState)
					{
					case InternalParseState.ReadMessagePayload:
					{
						EnsureStartsWithDataPrefix(line);
						int length = line.Length - (DataPrefix.Length + SseLineEnding.Length);
						byte[] item = line.Slice(DataPrefix.Length, length).ToArray();
						_data.Add(item);
						start = valueOrDefault;
						consumed = valueOrDefault;
						break;
					}
					case InternalParseState.ReadEndOfMessage:
						if (_data.Count == 1)
						{
							array = _data[0];
						}
						else if (_data.Count > 1)
						{
							int num = 0;
							foreach (byte[] datum in _data)
							{
								num += datum.Length;
							}
							num += _newLine.Length * _data.Count;
							array = new byte[num - _newLine.Length];
							int num2 = 0;
							foreach (byte[] datum2 in _data)
							{
								datum2.CopyTo(array, num2);
								num2 += datum2.Length;
								if (num2 < array.Length)
								{
									_newLine.CopyTo(array, num2);
									num2 += _newLine.Length;
								}
							}
						}
						message = array;
						consumed = valueOrDefault;
						examined = consumed;
						return ParseResult.Completed;
					}
					if (buffer.Length > 0 && buffer.First.Span[0] == 13)
					{
						_internalParserState = InternalParseState.ReadEndOfMessage;
					}
					continue;
				}
				if (_internalParserState == InternalParseState.ReadEndOfMessage)
				{
					buffer2 = buffer.Slice(start, buffer.End);
					if (ConvertBufferToSpan(in buffer2).Length > 1)
					{
						throw new FormatException("Expected a \\r\\n frame ending");
					}
				}
				return ParseResult.Incomplete;
			}
			return ParseResult.Incomplete;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ReadOnlySpan<byte> ConvertBufferToSpan(in ReadOnlySequence<byte> buffer)
		{
			if (buffer.IsSingleSegment)
			{
				return buffer.First.Span;
			}
			return BuffersExtensions.ToArray(in buffer);
		}

		public void Reset()
		{
			_internalParserState = InternalParseState.ReadMessagePayload;
			_data.Clear();
		}

		private static void EnsureStartsWithDataPrefix(ReadOnlySpan<byte> line)
		{
			if (!line.StartsWith(DataPrefix))
			{
				throw new FormatException("Expected the message prefix 'data: '");
			}
		}

		private static bool IsMessageEnd(ReadOnlySpan<byte> line)
		{
			if (line.Length == SseLineEnding.Length)
			{
				return line.SequenceEqual(SseLineEnding);
			}
			return false;
		}
	}
}
