using System;
using System.IO;

namespace Estreya.BlishHUD.Shared.IO
{
	public class ReadProgressStream : Stream
	{
		public class ProgressChangedEventArgs
		{
			public double Progress { get; private set; }

			public ProgressChangedEventArgs(double progress)
			{
				Progress = progress;
			}
		}

		private double _lastProgress;

		private Stream _stream;

		protected Stream ContainedStream => _stream;

		public override bool CanRead => _stream.CanRead;

		public override bool CanSeek => _stream.CanSeek;

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
				_stream.Position = value;
			}
		}

		public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

		public ReadProgressStream(Stream stream)
		{
			_stream = stream ?? throw new ArgumentNullException("stream");
			if (stream.Length <= 0 || !stream.CanRead)
			{
				throw new ArgumentException("stream");
			}
		}

		public override void Flush()
		{
			_stream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int result = _stream.Read(buffer, offset, count);
			if (this.ProgressChanged != null)
			{
				double newProgress = (double)Position * 100.0 / (double)Length;
				if (newProgress > _lastProgress)
				{
					_lastProgress = newProgress;
					this.ProgressChanged(this, new ProgressChangedEventArgs(_lastProgress));
				}
			}
			return result;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return _stream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			_stream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_stream.Write(buffer, offset, count);
		}
	}
}
