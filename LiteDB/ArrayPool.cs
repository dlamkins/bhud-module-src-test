using System;
using System.Runtime.CompilerServices;

namespace LiteDB
{
	internal class ArrayPool<T>
	{
		private sealed class SlotBuff
		{
			private readonly T[][] _buff = new T[8][];

			private int _size;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool TryPush(T[] item)
			{
				if (_size >= 8)
				{
					return false;
				}
				_buff[_size++] = item;
				return true;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public T[] TryPop()
			{
				if (_size <= 0)
				{
					return null;
				}
				T[] result = _buff[--_size];
				_buff[_size] = null;
				return result;
			}
		}

		private static readonly T[] Emptry = new T[0];

		private const int SLOT_COUNT = 8;

		private readonly SlotBuff[] _buckets;

		public ArrayPool()
		{
			_buckets = new SlotBuff[17];
			for (int i = 0; i < 17; i++)
			{
				int maxSlotSize = BucketHelper.GetMaxSizeForBucket(i);
				SlotBuff slotBuff = new SlotBuff();
				_buckets[i] = slotBuff;
				for (int j = 0; j < 8; j++)
				{
					if (!slotBuff.TryPush(new T[maxSlotSize]))
					{
						throw new InvalidOperationException();
					}
				}
			}
		}

		public T[] Rent(int minSize)
		{
			if (minSize < 0)
			{
				throw new ArgumentOutOfRangeException("minSize");
			}
			if (minSize == 0)
			{
				return Emptry;
			}
			int bucketIdx = BucketHelper.GetBucketIndex(minSize);
			if (bucketIdx < 0)
			{
				return new T[minSize];
			}
			T[] returnBuff = _buckets[bucketIdx].TryPop();
			if (returnBuff != null)
			{
				return returnBuff;
			}
			return new T[minSize];
		}

		public void Return(T[] buff)
		{
			if (buff == null)
			{
				throw new ArgumentNullException("buff");
			}
			if (buff.Length != 0)
			{
				int bucketIndex = BucketHelper.GetBucketIndex(buff.Length);
				if (bucketIndex >= 0)
				{
					_buckets[bucketIndex].TryPush(buff);
				}
			}
		}
	}
}
