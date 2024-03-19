using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Nekres.ChatMacros.Core.Services.Speech
{
	internal class SpeechStream : Stream
	{
		private AutoResetEvent _writeEvent;

		private List<byte> _buffer;

		private int _buffersize;

		private int _readposition;

		private int _writeposition;

		private bool _reset;

		public override bool CanRead => true;

		public override bool CanSeek => false;

		public override bool CanWrite => true;

		public override long Length => -1L;

		public override long Position
		{
			get
			{
				return 0L;
			}
			set
			{
			}
		}

		public SpeechStream(int bufferSize)
		{
			_writeEvent = new AutoResetEvent(initialState: false);
			_buffersize = bufferSize;
			_buffer = new List<byte>(_buffersize);
			for (int i = 0; i < _buffersize; i++)
			{
				_buffer.Add(0);
			}
			_readposition = 0;
			_writeposition = 0;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return 0L;
		}

		public override void SetLength(long value)
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int i = 0;
			while (i < count && _writeEvent != null)
			{
				if (!_reset && _readposition >= _writeposition)
				{
					_writeEvent.WaitOne(5, exitContext: true);
					continue;
				}
				buffer[i] = _buffer[_readposition + offset];
				_readposition++;
				if (_readposition == _buffersize)
				{
					_readposition = 0;
					_reset = false;
				}
				i++;
			}
			return count;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			for (int i = offset; i < offset + count; i++)
			{
				_buffer[_writeposition] = buffer[i];
				_writeposition++;
				if (_writeposition == _buffersize)
				{
					_writeposition = 0;
					_reset = true;
				}
			}
			_writeEvent?.Set();
		}

		public override void Close()
		{
			_writeEvent?.Close();
			_writeEvent = null;
			base.Close();
		}

		public override void Flush()
		{
		}
	}
}
