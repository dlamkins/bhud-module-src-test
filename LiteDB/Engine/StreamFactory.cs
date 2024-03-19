using System.IO;

namespace LiteDB.Engine
{
	internal class StreamFactory : IStreamFactory
	{
		private readonly Stream _stream;

		private readonly string _password;

		public string Name
		{
			get
			{
				if (!(_stream is MemoryStream))
				{
					if (!(_stream is TempStream))
					{
						return ":stream:";
					}
					return ":temp:";
				}
				return ":memory:";
			}
		}

		public bool CloseOnDispose => false;

		public StreamFactory(Stream stream, string password)
		{
			_stream = stream;
			_password = password;
		}

		public Stream GetStream(bool canWrite, bool sequencial)
		{
			if (_password == null)
			{
				return new ConcurrentStream(_stream, canWrite);
			}
			return new AesStream(_password, new ConcurrentStream(_stream, canWrite));
		}

		public long GetLength()
		{
			long length = _stream.Length;
			if (length % 8192 != 0L)
			{
				length -= length % 8192;
				_stream.SetLength(length);
				_stream.FlushToDisk();
			}
			if (length <= 0)
			{
				return 0L;
			}
			return length - ((_password != null) ? 8192 : 0);
		}

		public bool Exists()
		{
			return _stream.Length > 0;
		}

		public void Delete()
		{
		}

		public bool IsLocked()
		{
			return false;
		}
	}
}
