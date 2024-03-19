namespace LiteDB.Engine
{
	internal class BufferPool
	{
		private static readonly object _lock;

		private static readonly ArrayPool<byte> _bytePool;

		static BufferPool()
		{
			_lock = new object();
			_bytePool = new ArrayPool<byte>();
		}

		public static byte[] Rent(int count)
		{
			lock (_lock)
			{
				return _bytePool.Rent(count);
			}
		}

		public static void Return(byte[] buffer)
		{
			lock (_lock)
			{
				_bytePool.Return(buffer);
			}
		}
	}
}
