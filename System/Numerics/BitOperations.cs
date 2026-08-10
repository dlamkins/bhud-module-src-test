using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics
{
	internal static class BitOperations
	{
		private static ReadOnlySpan<byte> Log2DeBruijn => new byte[32]
		{
			0, 9, 1, 10, 13, 21, 2, 29, 11, 14,
			16, 18, 22, 25, 3, 30, 8, 12, 20, 28,
			15, 17, 24, 7, 19, 27, 23, 6, 26, 5,
			4, 31
		};

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Log2(uint value)
		{
			return Log2SoftwareFallback(value | 1u);
		}

		private static int Log2SoftwareFallback(uint value)
		{
			value |= value >> 1;
			value |= value >> 2;
			value |= value >> 4;
			value |= value >> 8;
			value |= value >> 16;
			return Unsafe.AddByteOffset(ref MemoryMarshal.GetReference(Log2DeBruijn), (nint)(value * 130329821 >> 27));
		}
	}
}
