using System.Buffers.Binary;
using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics
{
	internal readonly struct ActivitySpanId : IEquatable<ActivitySpanId>
	{
		private readonly string _hexString;

		internal ActivitySpanId(string hexString)
		{
			_hexString = hexString;
		}

		public unsafe static ActivitySpanId CreateRandom()
		{
			ulong num = default(ulong);
			ActivityTraceId.SetToRandomBytes(new Span<byte>(&num, 8));
			return new ActivitySpanId(_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EHexConverter.ToString(new ReadOnlySpan<byte>(&num, 8), _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EHexConverter.Casing.Lower));
		}

		public static ActivitySpanId CreateFromBytes(ReadOnlySpan<byte> idData)
		{
			if (idData.Length != 8)
			{
				throw new ArgumentOutOfRangeException("idData");
			}
			return new ActivitySpanId(_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EHexConverter.ToString(idData, _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EHexConverter.Casing.Lower));
		}

		public static ActivitySpanId CreateFromUtf8String(ReadOnlySpan<byte> idData)
		{
			return new ActivitySpanId(idData);
		}

		public static ActivitySpanId CreateFromString(ReadOnlySpan<char> idData)
		{
			if (idData.Length != 16 || !ActivityTraceId.IsLowerCaseHexAndNotAllZeros(idData))
			{
				throw new ArgumentOutOfRangeException("idData");
			}
			return new ActivitySpanId(idData.ToString());
		}

		public string ToHexString()
		{
			return _hexString ?? "0000000000000000";
		}

		public override string ToString()
		{
			return ToHexString();
		}

		public static bool operator ==(ActivitySpanId spanId1, ActivitySpanId spandId2)
		{
			return spanId1._hexString == spandId2._hexString;
		}

		public static bool operator !=(ActivitySpanId spanId1, ActivitySpanId spandId2)
		{
			return spanId1._hexString != spandId2._hexString;
		}

		public bool Equals(ActivitySpanId spanId)
		{
			return _hexString == spanId._hexString;
		}

		public override bool Equals([_003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003ENotNullWhen(true)] object? obj)
		{
			if (obj is ActivitySpanId)
			{
				ActivitySpanId activitySpanId = (ActivitySpanId)obj;
				return _hexString == activitySpanId._hexString;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ToHexString().GetHashCode();
		}

		private unsafe ActivitySpanId(ReadOnlySpan<byte> idData)
		{
			if (idData.Length != 16)
			{
				throw new ArgumentOutOfRangeException("idData");
			}
			if (!Utf8Parser.TryParse(idData, out ulong value, out int _, 'x'))
			{
				_hexString = CreateRandom()._hexString;
				return;
			}
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			_hexString = _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EHexConverter.ToString(new ReadOnlySpan<byte>(&value, 8), _003Cd09c9507_002D1983_002D461f_002D8fce_002Df8aba78858d6_003EHexConverter.Casing.Lower);
		}

		public void CopyTo(Span<byte> destination)
		{
			ActivityTraceId.SetSpanFromHexChars(ToHexString().AsSpan(), destination);
		}
	}
}
