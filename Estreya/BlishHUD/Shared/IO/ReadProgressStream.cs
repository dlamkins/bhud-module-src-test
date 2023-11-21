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

		protected Stream ContainedStream { get; }

		public override bool CanRead => ContainedStream.CanRead;

		public override bool CanSeek => ContainedStream.CanSeek;

		public override bool CanWrite => ContainedStream.CanWrite;

		public override long Length => ContainedStream.Length;

		public override long Position
		{
			get
			{
				return ContainedStream.Position;
			}
			set
			{
				ContainedStream.Position = value;
			}
		}

		public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

		public ReadProgressStream(Stream stream)
		{
			ContainedStream = stream ?? throw new ArgumentNullException("stream");
			if (stream.Length <= 0 || !stream.CanRead)
			{
				throw new ArgumentException("stream");
			}
		}

		public override void Flush()
		{
			ContainedStream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int result = ContainedStream.Read(buffer, offset, count);
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
			return ContainedStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			ContainedStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			ContainedStream.Write(buffer, offset, count);
		}
	}
}
