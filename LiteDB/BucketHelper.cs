using System.Runtime.CompilerServices;

namespace LiteDB
{
	internal static class BucketHelper
	{
		public const int BucketCount = 17;

		private static readonly int[] _bucketSize;

		static BucketHelper()
		{
			_bucketSize = new int[17];
			for (int i = 0; i < 17; i++)
			{
				_bucketSize[i] = GetMaxSizeForBucket(i);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int GetBucketIndex(int bufferSize)
		{
			for (int i = 0; i < _bucketSize.Length; i++)
			{
				if (_bucketSize[i] >= bufferSize)
				{
					return i;
				}
			}
			return -1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int GetMaxSizeForBucket(int binIndex)
		{
			return 16 << binIndex;
		}
	}
}
