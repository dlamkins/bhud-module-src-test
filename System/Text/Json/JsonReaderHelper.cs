using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text.Json
{
	internal static class JsonReaderHelper
	{
		private const string SpecialCharacters = ". '/\"[]()\t\n\r\f\b\\\u0085\u2028\u2029";

		public static readonly UTF8Encoding s_utf8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

		private const ulong XorPowerOfTwoToHighByte = 283686952306184uL;

		public static bool ContainsSpecialCharacters(this ReadOnlySpan<char> text)
		{
			return text.IndexOfAny(". '/\"[]()\t\n\r\f\b\\\u0085\u2028\u2029".AsSpan()) >= 0;
		}

		public static (int, int) CountNewLines(ReadOnlySpan<byte> data)
		{
			int num = data.LastIndexOf<byte>(10);
			int num2 = 0;
			if (num >= 0)
			{
				num2 = 1;
				data = data.Slice(0, num);
				int num3;
				while ((num3 = data.IndexOf<byte>(10)) >= 0)
				{
					num2++;
					data = data.Slice(num3 + 1);
				}
			}
			return (num2, num);
		}

		internal static JsonValueKind ToValueKind(this JsonTokenType tokenType)
		{
			switch (tokenType)
			{
			case JsonTokenType.None:
				return JsonValueKind.Undefined;
			case JsonTokenType.StartArray:
				return JsonValueKind.Array;
			case JsonTokenType.StartObject:
				return JsonValueKind.Object;
			case JsonTokenType.String:
			case JsonTokenType.Number:
			case JsonTokenType.True:
			case JsonTokenType.False:
			case JsonTokenType.Null:
				return (JsonValueKind)(tokenType - 4);
			default:
				return JsonValueKind.Undefined;
			}
		}

		public static bool IsTokenTypePrimitive(JsonTokenType tokenType)
		{
			return (int)(tokenType - 7) <= 4;
		}

		public static bool IsHexDigit(byte nextByte)
		{
			return HexConverter.IsHexChar(nextByte);
		}

		public static bool TryGetEscapedDateTime(ReadOnlySpan<byte> source, out DateTime value)
		{
			Span<byte> span = stackalloc byte[252];
			Unescape(source, span, out var written);
			span = span.Slice(0, written);
			if (JsonHelpers.IsValidUnescapedDateTimeOffsetParseLength(span.Length) && JsonHelpers.TryParseAsISO((ReadOnlySpan<byte>)span, out DateTime value2))
			{
				value = value2;
				return true;
			}
			value = default(DateTime);
			return false;
		}

		public static bool TryGetEscapedDateTimeOffset(ReadOnlySpan<byte> source, out DateTimeOffset value)
		{
			Span<byte> span = stackalloc byte[252];
			Unescape(source, span, out var written);
			span = span.Slice(0, written);
			if (JsonHelpers.IsValidUnescapedDateTimeOffsetParseLength(span.Length) && JsonHelpers.TryParseAsISO((ReadOnlySpan<byte>)span, out DateTimeOffset value2))
			{
				value = value2;
				return true;
			}
			value = default(DateTimeOffset);
			return false;
		}

		public static bool TryGetEscapedGuid(ReadOnlySpan<byte> source, out Guid value)
		{
			Span<byte> span = stackalloc byte[216];
			Unescape(source, span, out var written);
			span = span.Slice(0, written);
			if (span.Length == 36 && Utf8Parser.TryParse((ReadOnlySpan<byte>)span, out Guid value2, out int _, 'D'))
			{
				value = value2;
				return true;
			}
			value = default(Guid);
			return false;
		}

		public static bool TryGetFloatingPointConstant(ReadOnlySpan<byte> span, out float value)
		{
			if (span.Length == 3)
			{
				if (span.SequenceEqual(JsonConstants.NaNValue))
				{
					value = float.NaN;
					return true;
				}
			}
			else if (span.Length == 8)
			{
				if (span.SequenceEqual(JsonConstants.PositiveInfinityValue))
				{
					value = float.PositiveInfinity;
					return true;
				}
			}
			else if (span.Length == 9 && span.SequenceEqual(JsonConstants.NegativeInfinityValue))
			{
				value = float.NegativeInfinity;
				return true;
			}
			value = 0f;
			return false;
		}

		public static bool TryGetFloatingPointConstant(ReadOnlySpan<byte> span, out double value)
		{
			if (span.Length == 3)
			{
				if (span.SequenceEqual(JsonConstants.NaNValue))
				{
					value = double.NaN;
					return true;
				}
			}
			else if (span.Length == 8)
			{
				if (span.SequenceEqual(JsonConstants.PositiveInfinityValue))
				{
					value = double.PositiveInfinity;
					return true;
				}
			}
			else if (span.Length == 9 && span.SequenceEqual(JsonConstants.NegativeInfinityValue))
			{
				value = double.NegativeInfinity;
				return true;
			}
			value = 0.0;
			return false;
		}

		public static bool TryGetUnescapedBase64Bytes(ReadOnlySpan<byte> utf8Source, [_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ENotNullWhen(true)] out byte[] bytes)
		{
			byte[] array = null;
			Span<byte> span = ((utf8Source.Length > 256) ? ((Span<byte>)(array = ArrayPool<byte>.Shared.Rent(utf8Source.Length))) : stackalloc byte[256]);
			Span<byte> span2 = span;
			Unescape(utf8Source, span2, out var written);
			span2 = span2.Slice(0, written);
			bool result = TryDecodeBase64InPlace(span2, out bytes);
			if (array != null)
			{
				span2.Clear();
				ArrayPool<byte>.Shared.Return(array);
			}
			return result;
		}

		public static string GetUnescapedString(ReadOnlySpan<byte> utf8Source)
		{
			int length = utf8Source.Length;
			byte[] array = null;
			Span<byte> span = ((length > 256) ? ((Span<byte>)(array = ArrayPool<byte>.Shared.Rent(length))) : stackalloc byte[256]);
			Span<byte> span2 = span;
			Unescape(utf8Source, span2, out var written);
			span2 = span2.Slice(0, written);
			string result = TranscodeHelper(span2);
			if (array != null)
			{
				span2.Clear();
				ArrayPool<byte>.Shared.Return(array);
			}
			return result;
		}

		public static ReadOnlySpan<byte> GetUnescapedSpan(ReadOnlySpan<byte> utf8Source)
		{
			int length = utf8Source.Length;
			byte[] array = null;
			Span<byte> span = ((length > 256) ? ((Span<byte>)(array = ArrayPool<byte>.Shared.Rent(length))) : stackalloc byte[256]);
			Span<byte> destination = span;
			Unescape(utf8Source, destination, out var written);
			ReadOnlySpan<byte> result = destination.Slice(0, written).ToArray();
			if (array != null)
			{
				new Span<byte>(array, 0, written).Clear();
				ArrayPool<byte>.Shared.Return(array);
			}
			return result;
		}

		public static bool UnescapeAndCompare(ReadOnlySpan<byte> utf8Source, ReadOnlySpan<byte> other)
		{
			byte[] array = null;
			Span<byte> span = ((utf8Source.Length > 256) ? ((Span<byte>)(array = ArrayPool<byte>.Shared.Rent(utf8Source.Length))) : stackalloc byte[256]);
			Span<byte> span2 = span;
			Unescape(utf8Source, span2, 0, out var written);
			span2 = span2.Slice(0, written);
			bool result = other.SequenceEqual(span2);
			if (array != null)
			{
				span2.Clear();
				ArrayPool<byte>.Shared.Return(array);
			}
			return result;
		}

		public static bool UnescapeAndCompare(ReadOnlySequence<byte> utf8Source, ReadOnlySpan<byte> other)
		{
			byte[] array = null;
			byte[] array2 = null;
			int num = checked((int)utf8Source.Length);
			Span<byte> span = ((num > 256) ? ((Span<byte>)(array2 = ArrayPool<byte>.Shared.Rent(num))) : stackalloc byte[256]);
			Span<byte> span2 = span;
			Span<byte> span3 = ((num > 256) ? ((Span<byte>)(array = ArrayPool<byte>.Shared.Rent(num))) : stackalloc byte[256]);
			Span<byte> span4 = span3;
			utf8Source.CopyTo(span4);
			span4 = span4.Slice(0, num);
			Unescape(span4, span2, 0, out var written);
			span2 = span2.Slice(0, written);
			bool result = other.SequenceEqual(span2);
			if (array2 != null)
			{
				span2.Clear();
				ArrayPool<byte>.Shared.Return(array2);
				span4.Clear();
				ArrayPool<byte>.Shared.Return(array);
			}
			return result;
		}

		public static bool TryDecodeBase64InPlace(Span<byte> utf8Unescaped, [_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ENotNullWhen(true)] out byte[] bytes)
		{
			if (Base64.DecodeFromUtf8InPlace(utf8Unescaped, out var bytesWritten) != 0)
			{
				bytes = null;
				return false;
			}
			bytes = utf8Unescaped.Slice(0, bytesWritten).ToArray();
			return true;
		}

		public static bool TryDecodeBase64(ReadOnlySpan<byte> utf8Unescaped, [_003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ENotNullWhen(true)] out byte[] bytes)
		{
			byte[] array = null;
			Span<byte> span = ((utf8Unescaped.Length > 256) ? ((Span<byte>)(array = ArrayPool<byte>.Shared.Rent(utf8Unescaped.Length))) : stackalloc byte[256]);
			Span<byte> bytes2 = span;
			if (Base64.DecodeFromUtf8(utf8Unescaped, bytes2, out var _, out var bytesWritten) != 0)
			{
				bytes = null;
				if (array != null)
				{
					bytes2.Clear();
					ArrayPool<byte>.Shared.Return(array);
				}
				return false;
			}
			bytes = bytes2.Slice(0, bytesWritten).ToArray();
			if (array != null)
			{
				bytes2.Clear();
				ArrayPool<byte>.Shared.Return(array);
			}
			return true;
		}

		public unsafe static string TranscodeHelper(ReadOnlySpan<byte> utf8Unescaped)
		{
			try
			{
				if (utf8Unescaped.IsEmpty)
				{
					return string.Empty;
				}
				fixed (byte* bytes = utf8Unescaped)
				{
					return s_utf8Encoding.GetString(bytes, utf8Unescaped.Length);
				}
			}
			catch (DecoderFallbackException innerException)
			{
				throw ThrowHelper.GetInvalidOperationException_ReadInvalidUTF8(innerException);
			}
		}

		public unsafe static int TranscodeHelper(ReadOnlySpan<byte> utf8Unescaped, Span<char> destination)
		{
			try
			{
				if (utf8Unescaped.IsEmpty)
				{
					return 0;
				}
				fixed (byte* bytes = utf8Unescaped)
				{
					fixed (char* chars = destination)
					{
						return s_utf8Encoding.GetChars(bytes, utf8Unescaped.Length, chars, destination.Length);
					}
				}
			}
			catch (DecoderFallbackException innerException)
			{
				throw ThrowHelper.GetInvalidOperationException_ReadInvalidUTF8(innerException);
			}
			catch (ArgumentException)
			{
				destination.Clear();
				throw;
			}
		}

		public unsafe static void ValidateUtf8(ReadOnlySpan<byte> utf8Buffer)
		{
			try
			{
				if (utf8Buffer.IsEmpty)
				{
					return;
				}
				try
				{
					fixed (byte* bytes = utf8Buffer)
					{
						s_utf8Encoding.GetCharCount(bytes, utf8Buffer.Length);
					}
				}
				finally
				{
				}
			}
			catch (DecoderFallbackException innerException)
			{
				throw ThrowHelper.GetInvalidOperationException_ReadInvalidUTF8(innerException);
			}
		}

		internal unsafe static int GetUtf8ByteCount(ReadOnlySpan<char> text)
		{
			try
			{
				if (text.IsEmpty)
				{
					return 0;
				}
				fixed (char* chars = text)
				{
					return s_utf8Encoding.GetByteCount(chars, text.Length);
				}
			}
			catch (EncoderFallbackException innerException)
			{
				throw ThrowHelper.GetArgumentException_ReadInvalidUTF16(innerException);
			}
		}

		internal unsafe static int GetUtf8FromText(ReadOnlySpan<char> text, Span<byte> dest)
		{
			try
			{
				if (text.IsEmpty)
				{
					return 0;
				}
				fixed (char* chars = text)
				{
					fixed (byte* bytes = dest)
					{
						return s_utf8Encoding.GetBytes(chars, text.Length, bytes, dest.Length);
					}
				}
			}
			catch (EncoderFallbackException innerException)
			{
				throw ThrowHelper.GetArgumentException_ReadInvalidUTF16(innerException);
			}
		}

		internal unsafe static string GetTextFromUtf8(ReadOnlySpan<byte> utf8Text)
		{
			if (utf8Text.IsEmpty)
			{
				return string.Empty;
			}
			fixed (byte* bytes = utf8Text)
			{
				return s_utf8Encoding.GetString(bytes, utf8Text.Length);
			}
		}

		internal static void Unescape(ReadOnlySpan<byte> source, Span<byte> destination, out int written)
		{
			int idx = source.IndexOf<byte>(92);
			bool flag = TryUnescape(source, destination, idx, out written);
		}

		internal static void Unescape(ReadOnlySpan<byte> source, Span<byte> destination, int idx, out int written)
		{
			bool flag = TryUnescape(source, destination, idx, out written);
		}

		internal static bool TryUnescape(ReadOnlySpan<byte> source, Span<byte> destination, out int written)
		{
			int idx = source.IndexOf<byte>(92);
			return TryUnescape(source, destination, idx, out written);
		}

		private static bool TryUnescape(ReadOnlySpan<byte> source, Span<byte> destination, int idx, out int written)
		{
			if (!source.Slice(0, idx).TryCopyTo(destination))
			{
				written = 0;
			}
			else
			{
				written = idx;
				while (written != destination.Length)
				{
					byte b = source[++idx];
					if ((uint)b <= 98u)
					{
						if ((uint)b <= 47u)
						{
							if (b != 34)
							{
								if (b != 47)
								{
									goto IL_0179;
								}
								destination[written++] = 47;
							}
							else
							{
								destination[written++] = 34;
							}
						}
						else if (b != 92)
						{
							if (b != 98)
							{
								goto IL_0179;
							}
							destination[written++] = 8;
						}
						else
						{
							destination[written++] = 92;
						}
					}
					else if ((uint)b <= 110u)
					{
						if (b != 102)
						{
							if (b != 110)
							{
								goto IL_0179;
							}
							destination[written++] = 10;
						}
						else
						{
							destination[written++] = 12;
						}
					}
					else if (b != 114)
					{
						if (b != 116)
						{
							goto IL_0179;
						}
						destination[written++] = 9;
					}
					else
					{
						destination[written++] = 13;
					}
					goto IL_025b;
					IL_025b:
					if (++idx != source.Length)
					{
						if (source[idx] == 92)
						{
							continue;
						}
						ReadOnlySpan<byte> span = source.Slice(idx);
						int num = span.IndexOf<byte>(92);
						if (num < 0)
						{
							num = span.Length;
						}
						if ((uint)(written + num) >= (uint)destination.Length)
						{
							break;
						}
						switch (num)
						{
						case 1:
							destination[written++] = source[idx++];
							break;
						case 2:
							destination[written++] = source[idx++];
							destination[written++] = source[idx++];
							break;
						case 3:
							destination[written++] = source[idx++];
							destination[written++] = source[idx++];
							destination[written++] = source[idx++];
							break;
						default:
							span.Slice(0, num).CopyTo(destination.Slice(written));
							written += num;
							idx += num;
							break;
						}
						if (idx != source.Length)
						{
							continue;
						}
					}
					return true;
					IL_0179:
					bool flag = Utf8Parser.TryParse(source.Slice(idx + 1, 4), out int value, out int bytesConsumed, 'x');
					idx += 4;
					if (JsonHelpers.IsInRangeInclusive((uint)value, 55296u, 57343u))
					{
						if (value >= 56320)
						{
							ThrowHelper.ThrowInvalidOperationException_ReadInvalidUTF16(value);
						}
						if (source.Length < idx + 7 || source[idx + 1] != 92 || source[idx + 2] != 117)
						{
							ThrowHelper.ThrowInvalidOperationException_ReadIncompleteUTF16();
						}
						flag = Utf8Parser.TryParse(source.Slice(idx + 3, 4), out int value2, out bytesConsumed, 'x');
						idx += 6;
						if (!JsonHelpers.IsInRangeInclusive((uint)value2, 56320u, 57343u))
						{
							ThrowHelper.ThrowInvalidOperationException_ReadInvalidUTF16(value2);
						}
						value = 1024 * (value - 55296) + (value2 - 56320) + 65536;
					}
					if (!TryEncodeToUtf8Bytes((uint)value, destination.Slice(written), out var bytesWritten))
					{
						break;
					}
					written += bytesWritten;
					goto IL_025b;
				}
			}
			return false;
		}

		private static bool TryEncodeToUtf8Bytes(uint scalar, Span<byte> utf8Destination, out int bytesWritten)
		{
			if (scalar < 128)
			{
				if ((uint)utf8Destination.Length < 1u)
				{
					bytesWritten = 0;
					return false;
				}
				utf8Destination[0] = (byte)scalar;
				bytesWritten = 1;
			}
			else if (scalar < 2048)
			{
				if ((uint)utf8Destination.Length < 2u)
				{
					bytesWritten = 0;
					return false;
				}
				utf8Destination[0] = (byte)(0xC0u | (scalar >> 6));
				utf8Destination[1] = (byte)(0x80u | (scalar & 0x3Fu));
				bytesWritten = 2;
			}
			else if (scalar < 65536)
			{
				if ((uint)utf8Destination.Length < 3u)
				{
					bytesWritten = 0;
					return false;
				}
				utf8Destination[0] = (byte)(0xE0u | (scalar >> 12));
				utf8Destination[1] = (byte)(0x80u | ((scalar >> 6) & 0x3Fu));
				utf8Destination[2] = (byte)(0x80u | (scalar & 0x3Fu));
				bytesWritten = 3;
			}
			else
			{
				if ((uint)utf8Destination.Length < 4u)
				{
					bytesWritten = 0;
					return false;
				}
				utf8Destination[0] = (byte)(0xF0u | (scalar >> 18));
				utf8Destination[1] = (byte)(0x80u | ((scalar >> 12) & 0x3Fu));
				utf8Destination[2] = (byte)(0x80u | ((scalar >> 6) & 0x3Fu));
				utf8Destination[3] = (byte)(0x80u | (scalar & 0x3Fu));
				bytesWritten = 4;
			}
			return true;
		}

		public unsafe static int IndexOfQuoteOrAnyControlOrBackSlash(this ReadOnlySpan<byte> span)
		{
			ref byte reference = ref MemoryMarshal.GetReference(span);
			int length = span.Length;
			IntPtr intPtr = (IntPtr)0;
			IntPtr intPtr2 = (IntPtr)length;
			if (System.Numerics.Vector.IsHardwareAccelerated && length >= Vector<byte>.Count * 2)
			{
				int num = (int)Unsafe.AsPointer(ref reference) & (Vector<byte>.Count - 1);
				intPtr2 = (IntPtr)((Vector<byte>.Count - num) & (Vector<byte>.Count - 1));
			}
			while (true)
			{
				if ((nuint)(void*)intPtr2 >= (nuint)8u)
				{
					intPtr2 -= 8;
					uint num2 = Unsafe.AddByteOffset(ref reference, intPtr);
					if (34 == num2 || 92 == num2 || 32 > num2)
					{
						goto IL_03b2;
					}
					num2 = Unsafe.AddByteOffset(ref reference, intPtr + 1);
					if (34 == num2 || 92 == num2 || 32 > num2)
					{
						goto IL_03ba;
					}
					num2 = Unsafe.AddByteOffset(ref reference, intPtr + 2);
					if (34 == num2 || 92 == num2 || 32 > num2)
					{
						goto IL_03c8;
					}
					num2 = Unsafe.AddByteOffset(ref reference, intPtr + 3);
					if (34 != num2 && 92 != num2 && 32 <= num2)
					{
						num2 = Unsafe.AddByteOffset(ref reference, intPtr + 4);
						if (34 != num2 && 92 != num2 && 32 <= num2)
						{
							num2 = Unsafe.AddByteOffset(ref reference, intPtr + 5);
							if (34 != num2 && 92 != num2 && 32 <= num2)
							{
								num2 = Unsafe.AddByteOffset(ref reference, intPtr + 6);
								if (34 != num2 && 92 != num2 && 32 <= num2)
								{
									num2 = Unsafe.AddByteOffset(ref reference, intPtr + 7);
									if (34 == num2 || 92 == num2 || 32 > num2)
									{
										break;
									}
									intPtr += 8;
									continue;
								}
								return (int)(void*)(intPtr + 6);
							}
							return (int)(void*)(intPtr + 5);
						}
						return (int)(void*)(intPtr + 4);
					}
					goto IL_03d6;
				}
				if ((nuint)(void*)intPtr2 >= (nuint)4u)
				{
					intPtr2 -= 4;
					uint num2 = Unsafe.AddByteOffset(ref reference, intPtr);
					if (34 == num2 || 92 == num2 || 32 > num2)
					{
						goto IL_03b2;
					}
					num2 = Unsafe.AddByteOffset(ref reference, intPtr + 1);
					if (34 == num2 || 92 == num2 || 32 > num2)
					{
						goto IL_03ba;
					}
					num2 = Unsafe.AddByteOffset(ref reference, intPtr + 2);
					if (34 == num2 || 92 == num2 || 32 > num2)
					{
						goto IL_03c8;
					}
					num2 = Unsafe.AddByteOffset(ref reference, intPtr + 3);
					if (34 == num2 || 92 == num2 || 32 > num2)
					{
						goto IL_03d6;
					}
					intPtr += 4;
				}
				while ((void*)intPtr2 != null)
				{
					intPtr2 -= 1;
					uint num2 = Unsafe.AddByteOffset(ref reference, intPtr);
					if (34 != num2 && 92 != num2 && 32 <= num2)
					{
						intPtr += 1;
						continue;
					}
					goto IL_03b2;
				}
				if (System.Numerics.Vector.IsHardwareAccelerated && (int)(void*)intPtr < length)
				{
					intPtr2 = (IntPtr)((length - (int)(void*)intPtr) & ~(Vector<byte>.Count - 1));
					Vector<byte> right = new Vector<byte>(34);
					Vector<byte> right2 = new Vector<byte>(92);
					Vector<byte> right3 = new Vector<byte>(32);
					for (; (void*)intPtr2 > (void*)intPtr; intPtr += Vector<byte>.Count)
					{
						Vector<byte> left = Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.AddByteOffset(ref reference, intPtr));
						Vector<byte> vector = System.Numerics.Vector.BitwiseOr(System.Numerics.Vector.BitwiseOr(System.Numerics.Vector.Equals(left, right), System.Numerics.Vector.Equals(left, right2)), System.Numerics.Vector.LessThan(left, right3));
						if (!Vector<byte>.Zero.Equals(vector))
						{
							return (int)(void*)intPtr + LocateFirstFoundByte(vector);
						}
					}
					if ((int)(void*)intPtr < length)
					{
						intPtr2 = (IntPtr)(length - (int)(void*)intPtr);
						continue;
					}
				}
				return -1;
				IL_03b2:
				return (int)(void*)intPtr;
				IL_03ba:
				return (int)(void*)(intPtr + 1);
				IL_03d6:
				return (int)(void*)(intPtr + 3);
				IL_03c8:
				return (int)(void*)(intPtr + 2);
			}
			return (int)(void*)(intPtr + 7);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int LocateFirstFoundByte(Vector<byte> match)
		{
			Vector<ulong> vector = System.Numerics.Vector.AsVectorUInt64(match);
			ulong num = 0uL;
			int i;
			for (i = 0; i < Vector<ulong>.Count; i++)
			{
				num = vector[i];
				if (num != 0L)
				{
					break;
				}
			}
			return i * 8 + LocateFirstFoundByte(num);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int LocateFirstFoundByte(ulong match)
		{
			ulong num = match ^ (match - 1);
			return (int)(num * 283686952306184L >> 57);
		}
	}
}
