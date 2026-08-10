using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.Primitives
{
	[DebuggerDisplay("{Value}")]
	internal readonly struct StringSegment : IEquatable<StringSegment>, IEquatable<string?>
	{
		public static readonly StringSegment Empty = string.Empty;

		public string? Buffer { get; }

		public int Offset { get; }

		public int Length { get; }

		public string? Value
		{
			get
			{
				if (!HasValue)
				{
					return null;
				}
				return Buffer!.Substring(Offset, Length);
			}
		}

		[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EMemberNotNullWhen(true, "Buffer")]
		[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EMemberNotNullWhen(true, "Value")]
		public bool HasValue
		{
			[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EMemberNotNullWhen(true, "Buffer")]
			[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EMemberNotNullWhen(true, "Value")]
			get
			{
				return Buffer != null;
			}
		}

		public char this[int index]
		{
			get
			{
				if ((uint)index >= (uint)Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
				}
				return Buffer![Offset + index];
			}
		}

		public StringSegment(string? buffer)
		{
			Buffer = buffer;
			Offset = 0;
			Length = buffer?.Length ?? 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public StringSegment(string buffer, int offset, int length)
		{
			if (buffer == null || (uint)offset > (uint)buffer.Length || (uint)length > (uint)(buffer.Length - offset))
			{
				ThrowInvalidArguments(buffer, offset, length);
			}
			Buffer = buffer;
			Offset = offset;
			Length = length;
		}

		public ReadOnlySpan<char> AsSpan()
		{
			return Buffer.AsSpan(Offset, Length);
		}

		public ReadOnlySpan<char> AsSpan(int start)
		{
			if (!HasValue || start < 0)
			{
				ThrowInvalidArguments(start, Length - start, ExceptionArgument.start);
			}
			return Buffer.AsSpan(Offset + start, Length - start);
		}

		public ReadOnlySpan<char> AsSpan(int start, int length)
		{
			if (!HasValue || start < 0 || length < 0 || (uint)(start + length) > (uint)Length)
			{
				ThrowInvalidArguments(start, length, ExceptionArgument.start);
			}
			return Buffer.AsSpan(Offset + start, length);
		}

		public ReadOnlyMemory<char> AsMemory()
		{
			return Buffer.AsMemory(Offset, Length);
		}

		public static int Compare(StringSegment a, StringSegment b, StringComparison comparisonType)
		{
			if (a.HasValue && b.HasValue)
			{
				return a.AsSpan().CompareTo(b.AsSpan(), comparisonType);
			}
			CheckStringComparison(comparisonType);
			if (a.HasValue)
			{
				return 1;
			}
			if (!b.HasValue)
			{
				return 0;
			}
			return -1;
		}

		public override bool Equals([_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003ENotNullWhen(true)] object? obj)
		{
			if (obj is StringSegment)
			{
				StringSegment other = (StringSegment)obj;
				return Equals(other);
			}
			return false;
		}

		public bool Equals(StringSegment other)
		{
			return Equals(other, StringComparison.Ordinal);
		}

		public bool Equals(StringSegment other, StringComparison comparisonType)
		{
			if (HasValue && other.HasValue)
			{
				return MemoryExtensions.Equals(AsSpan(), other.AsSpan(), comparisonType);
			}
			CheckStringComparison(comparisonType);
			if (!HasValue)
			{
				return !other.HasValue;
			}
			return false;
		}

		public static bool Equals(StringSegment a, StringSegment b, StringComparison comparisonType)
		{
			return a.Equals(b, comparisonType);
		}

		public bool Equals(string? text)
		{
			return Equals(text, StringComparison.Ordinal);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(string? text, StringComparison comparisonType)
		{
			if (!HasValue || text == null)
			{
				CheckStringComparison(comparisonType);
				return text == Buffer;
			}
			return MemoryExtensions.Equals(AsSpan(), text.AsSpan(), comparisonType);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return Value?.GetHashCode() ?? 0;
		}

		public static bool operator ==(StringSegment left, StringSegment right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(StringSegment left, StringSegment right)
		{
			return !left.Equals(right);
		}

		public static implicit operator StringSegment(string? value)
		{
			return new StringSegment(value);
		}

		public static implicit operator ReadOnlySpan<char>(StringSegment segment)
		{
			return segment.AsSpan();
		}

		public static implicit operator ReadOnlyMemory<char>(StringSegment segment)
		{
			return segment.AsMemory();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool StartsWith(string text, StringComparison comparisonType)
		{
			if (text == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.text);
			}
			if (!HasValue)
			{
				CheckStringComparison(comparisonType);
				return false;
			}
			return AsSpan().StartsWith(text.AsSpan(), comparisonType);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool EndsWith(string text, StringComparison comparisonType)
		{
			if (text == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.text);
			}
			if (!HasValue)
			{
				CheckStringComparison(comparisonType);
				return false;
			}
			return AsSpan().EndsWith(text.AsSpan(), comparisonType);
		}

		public string Substring(int offset)
		{
			return Substring(offset, Length - offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string Substring(int offset, int length)
		{
			if (!HasValue || offset < 0 || length < 0 || (uint)(offset + length) > (uint)Length)
			{
				ThrowInvalidArguments(offset, length, ExceptionArgument.offset);
			}
			return Buffer!.Substring(Offset + offset, length);
		}

		public StringSegment Subsegment(int offset)
		{
			return Subsegment(offset, Length - offset);
		}

		public StringSegment Subsegment(int offset, int length)
		{
			if (!HasValue || offset < 0 || length < 0 || (uint)(offset + length) > (uint)Length)
			{
				ThrowInvalidArguments(offset, length, ExceptionArgument.offset);
			}
			return new StringSegment(Buffer, Offset + offset, length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int IndexOf(char c, int start, int count)
		{
			int num = -1;
			if (HasValue)
			{
				if ((uint)start > (uint)Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
				}
				if ((uint)count > (uint)(Length - start))
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count);
				}
				num = AsSpan(start, count).IndexOf(c);
				if (num >= 0)
				{
					num += start;
				}
			}
			return num;
		}

		public int IndexOf(char c, int start)
		{
			return IndexOf(c, start, Length - start);
		}

		public int IndexOf(char c)
		{
			return IndexOf(c, 0, Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int IndexOfAny(char[] anyOf, int startIndex, int count)
		{
			int num = -1;
			if (HasValue)
			{
				if ((uint)startIndex > (uint)Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
				}
				if ((uint)count > (uint)(Length - startIndex))
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count);
				}
				num = Buffer!.IndexOfAny(anyOf, Offset + startIndex, count);
				if (num != -1)
				{
					num -= Offset;
				}
			}
			return num;
		}

		public int IndexOfAny(char[] anyOf, int startIndex)
		{
			return IndexOfAny(anyOf, startIndex, Length - startIndex);
		}

		public int IndexOfAny(char[] anyOf)
		{
			return IndexOfAny(anyOf, 0, Length);
		}

		public int LastIndexOf(char value)
		{
			return AsSpan().LastIndexOf(value);
		}

		public StringSegment Trim()
		{
			return TrimStart().TrimEnd();
		}

		public StringSegment TrimStart()
		{
			ReadOnlySpan<char> readOnlySpan = AsSpan();
			int i;
			for (i = 0; i < readOnlySpan.Length && char.IsWhiteSpace(readOnlySpan[i]); i++)
			{
			}
			return Subsegment(i);
		}

		public StringSegment TrimEnd()
		{
			ReadOnlySpan<char> readOnlySpan = AsSpan();
			int num = readOnlySpan.Length - 1;
			while (num >= 0 && char.IsWhiteSpace(readOnlySpan[num]))
			{
				num--;
			}
			return Subsegment(0, num + 1);
		}

		public StringTokenizer Split(char[] chars)
		{
			return new StringTokenizer(this, chars);
		}

		public static bool IsNullOrEmpty(StringSegment value)
		{
			bool result = false;
			if (!value.HasValue || value.Length == 0)
			{
				result = true;
			}
			return result;
		}

		public override string ToString()
		{
			return Value ?? string.Empty;
		}

		private static void CheckStringComparison(StringComparison comparisonType)
		{
			if ((uint)comparisonType > 5u)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.comparisonType);
			}
		}

		[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EDoesNotReturn]
		private static void ThrowInvalidArguments(string buffer, int offset, int length)
		{
			throw GetInvalidArgumentsException();
			Exception GetInvalidArgumentsException()
			{
				if (buffer == null)
				{
					return ThrowHelper.GetArgumentNullException(ExceptionArgument.buffer);
				}
				if (offset < 0)
				{
					return ThrowHelper.GetArgumentOutOfRangeException(ExceptionArgument.offset);
				}
				if (length < 0)
				{
					return ThrowHelper.GetArgumentOutOfRangeException(ExceptionArgument.length);
				}
				return ThrowHelper.GetArgumentException(ExceptionResource.Argument_InvalidOffsetLength);
			}
		}

		[_003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EDoesNotReturn]
		private void ThrowInvalidArguments(int offset, int length, ExceptionArgument offsetOrStart)
		{
			throw GetInvalidArgumentsException(HasValue);
			Exception GetInvalidArgumentsException(bool hasValue)
			{
				if (!hasValue)
				{
					return ThrowHelper.GetArgumentOutOfRangeException(offsetOrStart);
				}
				if (offset < 0)
				{
					return ThrowHelper.GetArgumentOutOfRangeException(offsetOrStart);
				}
				if (length < 0)
				{
					return ThrowHelper.GetArgumentOutOfRangeException(ExceptionArgument.length);
				}
				return ThrowHelper.GetArgumentException(ExceptionResource.Argument_InvalidOffsetLengthStringSegment);
			}
		}
	}
}
