using System;
using System.IO;

namespace LiteDB.Engine
{
	public class TempStream : Stream
	{
		private Stream _stream = new MemoryStream();

		private string _filename;

		private readonly long _maxMemoryUsage;

		public bool InMemory => _stream is MemoryStream;

		public bool InDisk => _stream is FileStream;

		public string Filename => _filename;

		public override bool CanRead => _stream.CanRead;

		public override bool CanSeek => _stream.CanWrite;

		public override bool CanWrite => _stream.CanWrite;

		public override long Length => _stream.Length;

		public override long Position
		{
			get
			{
				return _stream.Position;
			}
			set
			{
				Seek(value, SeekOrigin.Begin);
			}
		}

		public TempStream(string filename = null, long maxMemoryUsage = 10485760L)
		{
			_maxMemoryUsage = maxMemoryUsage;
			_filename = filename;
		}

		public override void Flush()
		{
			_stream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return _stream.Read(buffer, offset, count);
		}

		public override void SetLength(long value)
		{
			_stream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_stream.Write(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			if (origin switch
			{
				SeekOrigin.Current => _stream.Position + offset, 
				SeekOrigin.Begin => offset, 
				_ => _stream.Position - offset, 
			} > _maxMemoryUsage && InMemory)
			{
				_filename = _filename ?? Path.Combine(Path.GetTempPath(), "litedb_" + Guid.NewGuid().ToString() + ".db");
				FileStream file = new FileStream(_filename, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None, 8192, FileOptions.RandomAccess);
				_stream.Position = 0L;
				_stream.CopyTo(file);
				_stream.Dispose();
				_stream = file;
			}
			return _stream.Seek(offset, origin);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			_stream.Dispose();
			if (InDisk)
			{
				File.Delete(_filename);
			}
		}
	}
}
