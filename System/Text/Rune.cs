using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;

namespace System.Text
{
	internal readonly struct Rune : IEquatable<Rune>
	{
		private const int MaxUtf16CharsPerRune = 2;

		private const char HighSurrogateStart = '\ud800';

		private const char LowSurrogateStart = '\udc00';

		private const int HighSurrogateRange = 1023;

		private readonly uint _value;

		public bool IsAscii => UnicodeUtility.IsAsciiCodePoint(_value);

		public bool IsBmp => UnicodeUtility.IsBmpCodePoint(_value);

		public static Rune ReplacementChar => UnsafeCreate(65533u);

		public int Utf16SequenceLength => UnicodeUtility.GetUtf16SequenceLength(_value);

		public int Value => (int)_value;

		public Rune(uint value)
		{
			if (!UnicodeUtility.IsValidUnicodeScalar(value))
			{
				System.Text.Encodings.Web.ThrowHelper.ThrowArgumentOutOfRangeException(System.Text.Encodings.Web.ExceptionArgument.value);
			}
			_value = value;
		}

		public Rune(int value)
			: this((uint)value)
		{
		}

		private Rune(uint scalarValue, bool _)
		{
			_value = scalarValue;
		}

		public static bool operator ==(Rune left, Rune right)
		{
			return left._value == right._value;
		}

		public static bool operator !=(Rune left, Rune right)
		{
			return left._value != right._value;
		}

		public static bool IsControl(Rune value)
		{
			return ((value._value + 1) & 0xFFFFFF7Fu) <= 32;
		}

		public static OperationStatus DecodeFromUtf16(ReadOnlySpan<char> source, out Rune result, out int charsConsumed)
		{
			if (!source.IsEmpty)
			{
				char c = source[0];
				if (TryCreate(c, out result))
				{
					charsConsumed = 1;
					return OperationStatus.Done;
				}
				if (1u < (uint)source.Length)
				{
					char lowSurrogate = source[1];
					if (TryCreate(c, lowSurrogate, out result))
					{
						charsConsumed = 2;
						return OperationStatus.Done;
					}
				}
				else if (char.IsHighSurrogate(c))
				{
					goto IL_004c;
				}
				charsConsumed = 1;
				result = ReplacementChar;
				return OperationStatus.InvalidData;
			}
			goto IL_004c;
			IL_004c:
			charsConsumed = source.Length;
			result = ReplacementChar;
			return OperationStatus.NeedMoreData;
		}

		public static OperationStatus DecodeFromUtf8(ReadOnlySpan<byte> source, out Rune result, out int bytesConsumed)
		{
			int num = 0;
			uint num2;
			if ((uint)num < (uint)source.Length)
			{
				num2 = source[num];
				if (UnicodeUtility.IsAsciiCodePoint(num2))
				{
					goto IL_0021;
				}
				if (UnicodeUtility.IsInRangeInclusive(num2, 194u, 244u))
				{
					num2 = num2 - 194 << 6;
					num++;
					if ((uint)num >= (uint)source.Length)
					{
						goto IL_0163;
					}
					int num3 = (sbyte)source[num];
					if (num3 < -64)
					{
						num2 += (uint)num3;
						num2 += 128;
						num2 += 128;
						if (num2 < 2048)
						{
							goto IL_0021;
						}
						if (UnicodeUtility.IsInRangeInclusive(num2, 2080u, 3343u) && !UnicodeUtility.IsInRangeInclusive(num2, 2912u, 2943u) && !UnicodeUtility.IsInRangeInclusive(num2, 3072u, 3087u))
						{
							num++;
							if ((uint)num >= (uint)source.Length)
							{
								goto IL_0163;
							}
							num3 = (sbyte)source[num];
							if (num3 < -64)
							{
								num2 <<= 6;
								num2 += (uint)num3;
								num2 += 128;
								num2 -= 131072;
								if (num2 > 65535)
								{
									num++;
									if ((uint)num >= (uint)source.Length)
									{
										goto IL_0163;
									}
									num3 = (sbyte)source[num];
									if (num3 >= -64)
									{
										goto IL_0153;
									}
									num2 <<= 6;
									num2 += (uint)num3;
									num2 += 128;
									num2 -= 4194304;
								}
								goto IL_0021;
							}
						}
					}
				}
				else
				{
					num = 1;
				}
				goto IL_0153;
			}
			goto IL_0163;
			IL_0021:
			bytesConsumed = num + 1;
			result = UnsafeCreate(num2);
			return OperationStatus.Done;
			IL_0153:
			bytesConsumed = num;
			result = ReplacementChar;
			return OperationStatus.InvalidData;
			IL_0163:
			bytesConsumed = num;
			result = ReplacementChar;
			return OperationStatus.NeedMoreData;
		}

		public override bool Equals([_003C34553c85_002Ddaa9_002D492b_002D9f2f_002D061f10e4aa05_003ENotNullWhen(true)] object obj)
		{
			if (obj is Rune)
			{
				Rune other = (Rune)obj;
				return Equals(other);
			}
			return false;
		}

		public bool Equals(Rune other)
		{
			return this == other;
		}

		public override int GetHashCode()
		{
			return Value;
		}

		public static bool TryCreate(char ch, out Rune result)
		{
			if (!UnicodeUtility.IsSurrogateCodePoint(ch))
			{
				result = UnsafeCreate(ch);
				return true;
			}
			result = default(Rune);
			return false;
		}

		public static bool TryCreate(char highSurrogate, char lowSurrogate, out Rune result)
		{
			uint num = (uint)(highSurrogate - 55296);
			uint num2 = (uint)(lowSurrogate - 56320);
			if ((num | num2) <= 1023)
			{
				result = UnsafeCreate((uint)((int)(num << 10) + (lowSurrogate - 56320) + 65536));
				return true;
			}
			result = default(Rune);
			return false;
		}

		public bool TryEncodeToUtf16(Span<char> destination, out int charsWritten)
		{
			if (destination.Length >= 1)
			{
				if (IsBmp)
				{
					destination[0] = (char)_value;
					charsWritten = 1;
					return true;
				}
				if (destination.Length >= 2)
				{
					UnicodeUtility.GetUtf16SurrogatesFromSupplementaryPlaneScalar(_value, out destination[0], out destination[1]);
					charsWritten = 2;
					return true;
				}
			}
			charsWritten = 0;
			return false;
		}

		public bool TryEncodeToUtf8(Span<byte> destination, out int bytesWritten)
		{
			if (destination.Length >= 1)
			{
				if (IsAscii)
				{
					destination[0] = (byte)_value;
					bytesWritten = 1;
					return true;
				}
				if (destination.Length >= 2)
				{
					if (_value <= 2047)
					{
						destination[0] = (byte)(_value + 12288 >> 6);
						destination[1] = (byte)((_value & 0x3F) + 128);
						bytesWritten = 2;
						return true;
					}
					if (destination.Length >= 3)
					{
						if (_value <= 65535)
						{
							destination[0] = (byte)(_value + 917504 >> 12);
							destination[1] = (byte)(((_value & 0xFC0) >> 6) + 128);
							destination[2] = (byte)((_value & 0x3F) + 128);
							bytesWritten = 3;
							return true;
						}
						if (destination.Length >= 4)
						{
							destination[0] = (byte)(_value + 62914560 >> 18);
							destination[1] = (byte)(((_value & 0x3F000) >> 12) + 128);
							destination[2] = (byte)(((_value & 0xFC0) >> 6) + 128);
							destination[3] = (byte)((_value & 0x3F) + 128);
							bytesWritten = 4;
							return true;
						}
					}
				}
			}
			bytesWritten = 0;
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Rune UnsafeCreate(uint scalarValue)
		{
			return new Rune(scalarValue, _: false);
		}
	}
}
