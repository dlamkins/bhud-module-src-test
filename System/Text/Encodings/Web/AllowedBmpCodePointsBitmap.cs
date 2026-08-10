using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text.Unicode;

namespace System.Text.Encodings.Web
{
	internal struct AllowedBmpCodePointsBitmap
	{
		private const int BitmapLengthInDWords = 2048;

		private unsafe fixed uint Bitmap[2048];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void AllowChar(char value)
		{
			_GetIndexAndOffset(value, out var index, out var offset);
			Bitmap[(ulong)index] |= (uint)(1 << offset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void ForbidChar(char value)
		{
			_GetIndexAndOffset(value, out var index, out var offset);
			Bitmap[(ulong)index] &= (uint)(~(1 << offset));
		}

		public void ForbidHtmlCharacters()
		{
			ForbidChar('<');
			ForbidChar('>');
			ForbidChar('&');
			ForbidChar('\'');
			ForbidChar('"');
			ForbidChar('+');
		}

		public unsafe void ForbidUndefinedCharacters()
		{
			fixed (uint* pointer = Bitmap)
			{
				ReadOnlySpan<byte> definedBmpCodePointsBitmapLittleEndian = UnicodeHelpers.GetDefinedBmpCodePointsBitmapLittleEndian();
				Span<uint> span = new Span<uint>(pointer, 2048);
				for (int i = 0; i < span.Length; i++)
				{
					span[i] &= BinaryPrimitives.ReadUInt32LittleEndian(definedBmpCodePointsBitmapLittleEndian.Slice(i * 4));
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe readonly bool IsCharAllowed(char value)
		{
			_GetIndexAndOffset(value, out var index, out var offset);
			if ((Bitmap[(ulong)index] & (uint)(1 << offset)) != 0)
			{
				return true;
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe readonly bool IsCodePointAllowed(uint value)
		{
			if (!UnicodeUtility.IsBmpCodePoint(value))
			{
				return false;
			}
			_GetIndexAndOffset(value, out var index, out var offset);
			if ((Bitmap[(ulong)index] & (uint)(1 << offset)) != 0)
			{
				return true;
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void _GetIndexAndOffset(uint value, out nuint index, out int offset)
		{
			index = value >> 5;
			offset = (int)(value & 0x1F);
		}
	}
}
