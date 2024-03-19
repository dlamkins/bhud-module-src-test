using System;
using System.IO;

namespace LiteDB.Engine
{
	internal class ConcurrentStream : Stream
	{
		private readonly Stream _stream;

		private readonly bool _canWrite;

		private long _position;

		public override bool CanRead => _stream.CanRead;

		public override bool CanSeek => _stream.CanSeek;

		public override bool CanWrite => _canWrite;

		public override long Length => _stream.Length;

		public override long Position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
			}
		}

		public ConcurrentStream(Stream stream, bool canWrite)
		{
			_stream = stream;
			_canWrite = canWrite;
		}

		public override void Flush()
		{
			_stream.Flush();
		}

		public override void SetLength(long value)
		{
			_stream.SetLength(value);
		}

		protected override void Dispose(bool disposing)
		{
			_stream.Dispose();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			lock (_stream)
			{
				long position = (_position = origin switch
				{
					SeekOrigin.Current => _position + offset, 
					SeekOrigin.Begin => offset, 
					_ => _position - offset, 
				});
				return _position;
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			lock (_stream)
			{
				_stream.Position = _position;
				int result = _stream.Read(buffer, offset, count);
				_position = _stream.Position;
				return result;
			}
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (!_canWrite)
			{
				throw new NotSupportedException("Current stream are readonly");
			}
			lock (_stream)
			{
				_stream.Position = _position;
				_stream.Write(buffer, offset, count);
				_position = _stream.Position;
			}
		}
	}
}
