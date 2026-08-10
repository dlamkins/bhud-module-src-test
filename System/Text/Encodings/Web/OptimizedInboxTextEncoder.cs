using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text.Encodings.Web
{
	internal sealed class OptimizedInboxTextEncoder
	{
		[StructLayout(LayoutKind.Explicit)]
		private struct AllowedAsciiCodePoints
		{
			[FieldOffset(0)]
			private unsafe fixed byte AsBytes[16];

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe readonly bool IsAllowedAsciiCodePoint(uint codePoint)
			{
				if (codePoint > 127)
				{
					return false;
				}
				uint num = AsBytes[codePoint & 0xF];
				if ((num & (uint)(1 << (int)(codePoint >> 4))) == 0)
				{
					return false;
				}
				return true;
			}

			internal unsafe void PopulateAllowedCodePoints(in AllowedBmpCodePointsBitmap allowedBmpCodePoints)
			{
				this = default(AllowedAsciiCodePoints);
				for (int i = 32; i < 127; i++)
				{
					if (allowedBmpCodePoints.IsCharAllowed((char)i))
					{
						ref byte val = ref AsBytes[i & 0xF];
						val = (byte)(val | (byte)(1 << (i >> 4)));
					}
				}
			}
		}

		private struct AsciiPreescapedData
		{
			private unsafe fixed ulong Data[128];

			internal unsafe void PopulatePreescapedData(in AllowedBmpCodePointsBitmap allowedCodePointsBmp, ScalarEscaperBase innerEncoder)
			{
				this = default(AsciiPreescapedData);
				byte* intPtr = stackalloc byte[16];
				// IL initblk instruction
				Unsafe.InitBlock(intPtr, 0, 16);
				Span<char> span = new Span<char>(intPtr, 8);
				Span<char> span2 = span;
				for (int i = 0; i < 128; i++)
				{
					Rune value = new Rune(i);
					ulong num;
					int num2;
					if (!Rune.IsControl(value) && allowedCodePointsBmp.IsCharAllowed((char)i))
					{
						num = (uint)i;
						num2 = 1;
					}
					else
					{
						num2 = innerEncoder.EncodeUtf16(value, span2.Slice(0, 6));
						num = 0uL;
						span2.Slice(num2).Clear();
						for (int num3 = num2 - 1; num3 >= 0; num3--)
						{
							uint num4 = span2[num3];
							num = (num << 8) | num4;
						}
					}
					Data[i] = num | ((ulong)(uint)num2 << 56);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe readonly bool TryGetPreescapedData(uint codePoint, out ulong preescapedData)
			{
				if (codePoint <= 127)
				{
					preescapedData = Data[codePoint];
					return true;
				}
				preescapedData = 0uL;
				return false;
			}
		}

		private readonly AllowedAsciiCodePoints _allowedAsciiCodePoints;

		private readonly AsciiPreescapedData _asciiPreescapedData;

		private readonly AllowedBmpCodePointsBitmap _allowedBmpCodePoints;

		private readonly ScalarEscaperBase _scalarEscaper;

		internal OptimizedInboxTextEncoder(ScalarEscaperBase scalarEscaper, in AllowedBmpCodePointsBitmap allowedCodePointsBmp, bool forbidHtmlSensitiveCharacters = true, ReadOnlySpan<char> extraCharactersToEscape = default(ReadOnlySpan<char>))
		{
			_scalarEscaper = scalarEscaper;
			_allowedBmpCodePoints = allowedCodePointsBmp;
			_allowedBmpCodePoints.ForbidUndefinedCharacters();
			if (forbidHtmlSensitiveCharacters)
			{
				_allowedBmpCodePoints.ForbidHtmlCharacters();
			}
			ReadOnlySpan<char> readOnlySpan = extraCharactersToEscape;
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				char value = readOnlySpan[i];
				_allowedBmpCodePoints.ForbidChar(value);
			}
			_asciiPreescapedData.PopulatePreescapedData(in _allowedBmpCodePoints, scalarEscaper);
			_allowedAsciiCodePoints.PopulateAllowedCodePoints(in _allowedBmpCodePoints);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("FindFirstCharacterToEncode has been deprecated. It should only be used by the TextEncoder adapter.")]
		public unsafe int FindFirstCharacterToEncode(char* text, int textLength)
		{
			return GetIndexOfFirstCharToEncode(new ReadOnlySpan<char>(text, textLength));
		}

		[Obsolete("TryEncodeUnicodeScalar has been deprecated. It should only be used by the TextEncoder adapter.")]
		public unsafe bool TryEncodeUnicodeScalar(int unicodeScalar, char* buffer, int bufferLength, out int numberOfCharactersWritten)
		{
			Span<char> destination = new Span<char>(buffer, bufferLength);
			if (_allowedBmpCodePoints.IsCodePointAllowed((uint)unicodeScalar))
			{
				if (!destination.IsEmpty)
				{
					destination[0] = (char)unicodeScalar;
					numberOfCharactersWritten = 1;
					return true;
				}
			}
			else
			{
				int num = _scalarEscaper.EncodeUtf16(new Rune(unicodeScalar), destination);
				if (num >= 0)
				{
					numberOfCharactersWritten = num;
					return true;
				}
			}
			numberOfCharactersWritten = 0;
			return false;
		}

		public OperationStatus Encode(ReadOnlySpan<char> source, Span<char> destination, out int charsConsumed, out int charsWritten, bool isFinalBlock)
		{
			_AssertThisNotNull();
			int num = 0;
			int num2 = 0;
			OperationStatus result2;
			while (true)
			{
				int num3;
				Rune result;
				if (SpanUtility.IsValidIndex(source, num))
				{
					char c = source[num];
					if (_asciiPreescapedData.TryGetPreescapedData(c, out var preescapedData))
					{
						if (SpanUtility.IsValidIndex(destination, num2))
						{
							destination[num2] = (char)(byte)preescapedData;
							if (((int)preescapedData & 0xFF00) == 0)
							{
								num2++;
								num++;
								continue;
							}
							preescapedData >>= 8;
							num3 = num2 + 1;
							while (SpanUtility.IsValidIndex(destination, num3))
							{
								destination[num3++] = (char)(byte)preescapedData;
								if ((byte)(preescapedData >>= 8) != 0)
								{
									continue;
								}
								goto IL_0091;
							}
						}
						goto IL_0148;
					}
					if (Rune.TryCreate(c, out result))
					{
						goto IL_00e1;
					}
					int index = num + 1;
					if (SpanUtility.IsValidIndex(source, index))
					{
						if (Rune.TryCreate(c, source[index], out result))
						{
							goto IL_00e1;
						}
					}
					else if (!isFinalBlock && char.IsHighSurrogate(c))
					{
						result2 = OperationStatus.NeedMoreData;
						break;
					}
					result = Rune.ReplacementChar;
					goto IL_010d;
				}
				result2 = OperationStatus.Done;
				break;
				IL_0148:
				result2 = OperationStatus.DestinationTooSmall;
				break;
				IL_0091:
				num2 = num3;
				num++;
				continue;
				IL_010d:
				int num4 = _scalarEscaper.EncodeUtf16(result, destination.Slice(num2));
				if (num4 >= 0)
				{
					num2 += num4;
					num += result.Utf16SequenceLength;
					continue;
				}
				goto IL_0148;
				IL_00e1:
				if (!IsScalarValueAllowed(result))
				{
					goto IL_010d;
				}
				if (result.TryEncodeToUtf16(destination.Slice(num2), out var charsWritten2))
				{
					num2 += charsWritten2;
					num += charsWritten2;
					continue;
				}
				goto IL_0148;
			}
			charsConsumed = num;
			charsWritten = num2;
			return result2;
		}

		public OperationStatus EncodeUtf8(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, bool isFinalBlock)
		{
			_AssertThisNotNull();
			int num = 0;
			int num2 = 0;
			OperationStatus result2;
			while (true)
			{
				int num3;
				if (SpanUtility.IsValidIndex(source, num))
				{
					uint codePoint = source[num];
					if (_asciiPreescapedData.TryGetPreescapedData(codePoint, out var preescapedData))
					{
						if (SpanUtility.TryWriteUInt64LittleEndian(destination, num2, preescapedData))
						{
							num2 += (int)(preescapedData >> 56);
							num++;
							continue;
						}
						num3 = num2;
						while (SpanUtility.IsValidIndex(destination, num3))
						{
							destination[num3++] = (byte)preescapedData;
							if ((byte)(preescapedData >>= 8) != 0)
							{
								continue;
							}
							goto IL_0076;
						}
					}
					else
					{
						Rune result;
						int bytesConsumed2;
						OperationStatus operationStatus = Rune.DecodeFromUtf8(source.Slice(num), out result, out bytesConsumed2);
						if (operationStatus != 0)
						{
							if (!isFinalBlock && operationStatus == OperationStatus.NeedMoreData)
							{
								result2 = OperationStatus.NeedMoreData;
								break;
							}
						}
						else if (IsScalarValueAllowed(result))
						{
							if (result.TryEncodeToUtf8(destination.Slice(num2), out var bytesWritten2))
							{
								num2 += bytesWritten2;
								num += bytesWritten2;
								continue;
							}
							goto IL_0103;
						}
						int num4 = _scalarEscaper.EncodeUtf8(result, destination.Slice(num2));
						if (num4 >= 0)
						{
							num2 += num4;
							num += bytesConsumed2;
							continue;
						}
					}
					goto IL_0103;
				}
				result2 = OperationStatus.Done;
				break;
				IL_0076:
				num2 = num3;
				num++;
				continue;
				IL_0103:
				result2 = OperationStatus.DestinationTooSmall;
				break;
			}
			bytesConsumed = num;
			bytesWritten = num2;
			return result2;
		}

		public int GetIndexOfFirstByteToEncode(ReadOnlySpan<byte> data)
		{
			int length = data.Length;
			Rune result;
			int bytesConsumed;
			while (!data.IsEmpty && Rune.DecodeFromUtf8(data, out result, out bytesConsumed) == OperationStatus.Done && bytesConsumed < 4 && _allowedBmpCodePoints.IsCharAllowed((char)result.Value))
			{
				data = data.Slice(bytesConsumed);
			}
			if (!data.IsEmpty)
			{
				return length - data.Length;
			}
			return -1;
		}

		public unsafe int GetIndexOfFirstCharToEncode(ReadOnlySpan<char> data)
		{
			fixed (char* ptr = data)
			{
				nuint num = (uint)data.Length;
				nuint num2 = 0u;
				if (num2 < num)
				{
					_AssertThisNotNull();
					nint num3 = 0;
					while (true)
					{
						if (num - num2 >= 8)
						{
							num3 = -1;
							if (_allowedBmpCodePoints.IsCharAllowed(ptr[(nuint)((nint)num2 + ++num3)]) && _allowedBmpCodePoints.IsCharAllowed(ptr[(nuint)((nint)num2 + ++num3)]) && _allowedBmpCodePoints.IsCharAllowed(ptr[(nuint)((nint)num2 + ++num3)]) && _allowedBmpCodePoints.IsCharAllowed(ptr[(nuint)((nint)num2 + ++num3)]) && _allowedBmpCodePoints.IsCharAllowed(ptr[(nuint)((nint)num2 + ++num3)]) && _allowedBmpCodePoints.IsCharAllowed(ptr[(nuint)((nint)num2 + ++num3)]) && _allowedBmpCodePoints.IsCharAllowed(ptr[(nuint)((nint)num2 + ++num3)]) && _allowedBmpCodePoints.IsCharAllowed(ptr[(nuint)((nint)num2 + ++num3)]))
							{
								num2 += 8;
								continue;
							}
							num2 += (nuint)num3;
							break;
						}
						for (; num2 < num && _allowedBmpCodePoints.IsCharAllowed(ptr[num2]); num2++)
						{
						}
						break;
					}
				}
				int num4 = (int)num2;
				if (num4 == (int)num)
				{
					num4 = -1;
				}
				return num4;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsScalarValueAllowed(Rune value)
		{
			return _allowedBmpCodePoints.IsCodePointAllowed((uint)value.Value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void _AssertThisNotNull()
		{
			_ = GetType() == typeof(OptimizedInboxTextEncoder);
		}
	}
}
