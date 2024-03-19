using System;
using System.Text;

namespace LiteDB
{
	internal class BufferSlice
	{
		public int Offset { get; }

		public int Count { get; }

		public byte[] Array { get; }

		public byte this[int index]
		{
			get
			{
				return Array[Offset + index];
			}
			set
			{
				Array[Offset + index] = value;
			}
		}

		public BufferSlice(byte[] array, int offset, int count)
		{
			Array = array;
			Offset = offset;
			Count = count;
		}

		public void Clear()
		{
			System.Array.Clear(Array, Offset, Count);
		}

		public void Clear(int offset, int count)
		{
			Constants.ENSURE(offset + count <= Count, "must fit in this page");
			System.Array.Clear(Array, Offset + offset, count);
		}

		public void Fill(byte value)
		{
			for (int i = 0; i < Count; i++)
			{
				Array[Offset + i] = value;
			}
		}

		public bool All(byte value)
		{
			for (int i = 0; i < Count; i++)
			{
				if (Array[Offset + i] != value)
				{
					return false;
				}
			}
			return true;
		}

		public string ToHex()
		{
			StringBuilder output = new StringBuilder();
			long position = 0L;
			while (position < Count)
			{
				for (int i = 0; i < 32; i++)
				{
					if (position >= Count)
					{
						break;
					}
					output.Append(Array[Offset + position].ToString("X2") + " ");
					position++;
				}
				output.AppendLine();
			}
			return output.ToString();
		}

		public BufferSlice Slice(int offset, int count)
		{
			return new BufferSlice(Array, Offset + offset, count);
		}

		public byte[] ToArray()
		{
			byte[] buffer = new byte[Count];
			Buffer.BlockCopy(Array, Offset, buffer, 0, Count);
			return buffer;
		}

		public override string ToString()
		{
			return $"Offset: {Offset} - Count: {Count}";
		}
	}
}
