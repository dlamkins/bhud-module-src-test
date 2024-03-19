using System;
using System.Text;

namespace LiteDB
{
	internal static class BufferExtensions
	{
		public static int BinaryCompareTo(this byte[] lh, byte[] rh)
		{
			if (lh == null)
			{
				if (rh != null)
				{
					return -1;
				}
				return 0;
			}
			if (rh == null)
			{
				return 1;
			}
			int result = 0;
			int i = 0;
			int stop = Math.Min(lh.Length, rh.Length);
			while (result == 0 && i < stop)
			{
				result = lh[i].CompareTo(rh[i]);
				i++;
			}
			if (result != 0)
			{
				if (result >= 0)
				{
					return 1;
				}
				return -1;
			}
			if (i == lh.Length)
			{
				if (i != rh.Length)
				{
					return -1;
				}
				return 0;
			}
			return 1;
		}

		public unsafe static bool IsFullZero(this byte[] data)
		{
			fixed (byte* bytes = data)
			{
				int len = data.Length;
				int rem = len % 128;
				long* b = (long*)bytes;
				for (long* e = (long*)(bytes + len - rem); b < e; b += 16)
				{
					if ((*b | b[1] | b[2] | b[3] | b[4] | b[5] | b[6] | b[7] | b[8] | b[9] | b[10] | b[11] | b[12] | b[13] | b[14] | b[15]) != 0L)
					{
						return false;
					}
				}
				for (int i = 0; i < rem; i++)
				{
					if (data[len - 1 - i] != 0)
					{
						return false;
					}
				}
				return true;
			}
		}

		public static byte[] Fill(this byte[] array, byte value, int offset, int count)
		{
			for (int i = 0; i < count; i++)
			{
				array[i + offset] = value;
			}
			return array;
		}

		public static string ReadCString(this byte[] bytes, int startIndex, out int bytesCount)
		{
			int position = Array.IndexOf(bytes, (byte)0, startIndex);
			if (position > 0)
			{
				bytesCount = position - startIndex;
				return Encoding.UTF8.GetString(bytes, startIndex, bytesCount);
			}
			bytesCount = 0;
			return null;
		}

		public unsafe static void ToBytes(this short value, byte[] array, int startIndex)
		{
			fixed (byte* ptr = &array[startIndex])
			{
				*(short*)ptr = value;
			}
		}

		public unsafe static void ToBytes(this int value, byte[] array, int startIndex)
		{
			fixed (byte* ptr = &array[startIndex])
			{
				*(int*)ptr = value;
			}
		}

		public unsafe static void ToBytes(this long value, byte[] array, int startIndex)
		{
			fixed (byte* ptr = &array[startIndex])
			{
				*(long*)ptr = value;
			}
		}

		public unsafe static void ToBytes(this ushort value, byte[] array, int startIndex)
		{
			fixed (byte* ptr = &array[startIndex])
			{
				*(ushort*)ptr = value;
			}
		}

		public unsafe static void ToBytes(this uint value, byte[] array, int startIndex)
		{
			fixed (byte* ptr = &array[startIndex])
			{
				*(uint*)ptr = value;
			}
		}

		public unsafe static void ToBytes(this ulong value, byte[] array, int startIndex)
		{
			fixed (byte* ptr = &array[startIndex])
			{
				*(ulong*)ptr = value;
			}
		}

		public unsafe static void ToBytes(this float value, byte[] array, int startIndex)
		{
			(*(uint*)(&value)).ToBytes(array, startIndex);
		}

		public unsafe static void ToBytes(this double value, byte[] array, int startIndex)
		{
			(*(ulong*)(&value)).ToBytes(array, startIndex);
		}
	}
}
