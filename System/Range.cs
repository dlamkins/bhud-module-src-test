using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System
{
	[ExcludeFromCodeCoverage]
	internal readonly struct Range : IEquatable<Range>
	{
		private static class HashHelpers
		{
			public static int Combine(int h1, int h2)
			{
				return ((int)((uint)(h1 << 5) | ((uint)h1 >> 27)) + h1) ^ h2;
			}
		}

		private static class ThrowHelper
		{
			[System.Diagnostics.CodeAnalysis.DoesNotReturn]
			public static void ThrowArgumentOutOfRangeException()
			{
				throw new ArgumentOutOfRangeException("length");
			}
		}

		public Index Start { get; }

		public Index End { get; }

		public static Range All => Index.Start..Index.End;

		public Range(Index start, Index end)
		{
			Start = start;
			End = end;
		}

		public override bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? value)
		{
			if (value is Range)
			{
				Range r = (Range)value;
				if (r.Start.Equals(Start))
				{
					return r.End.Equals(End);
				}
			}
			return false;
		}

		public bool Equals(Range other)
		{
			if (other.Start.Equals(Start))
			{
				return other.End.Equals(End);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashHelpers.Combine(Start.GetHashCode(), End.GetHashCode());
		}

		public override string ToString()
		{
			return Start.ToString() + ".." + End;
		}

		public static Range StartAt(Index start)
		{
			return start..Index.End;
		}

		public static Range EndAt(Index end)
		{
			return Index.Start..end;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public (int Offset, int Length) GetOffsetAndLength(int length)
		{
			Index startIndex = Start;
			int start = ((!startIndex.IsFromEnd) ? startIndex.Value : (length - startIndex.Value));
			Index endIndex = End;
			int end = ((!endIndex.IsFromEnd) ? endIndex.Value : (length - endIndex.Value));
			if ((uint)end > (uint)length || (uint)start > (uint)end)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			return (start, end - start);
		}
	}
}
