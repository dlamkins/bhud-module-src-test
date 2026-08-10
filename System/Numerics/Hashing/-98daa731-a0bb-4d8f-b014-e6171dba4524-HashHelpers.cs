namespace System.Numerics.Hashing
{
	internal static class _003C98daa731_002Da0bb_002D4d8f_002Db014_002De6171dba4524_003EHashHelpers
	{
		public static int Combine(int h1, int h2)
		{
			uint num = (uint)(h1 << 5) | ((uint)h1 >> 27);
			return ((int)num + h1) ^ h2;
		}
	}
}
