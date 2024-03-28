using NAudio.Wave;

namespace OpNop.EnterTheSandstorm
{
	public class LoopingAudioStream : WaveStream
	{
		private readonly WaveStream _sourceStream;

		private bool _stopLoop;

		public override WaveFormat WaveFormat => _sourceStream.WaveFormat;

		public override long Length => _sourceStream.Length;

		public override long Position
		{
			get
			{
				return _sourceStream.Position;
			}
			set
			{
				_sourceStream.Position = value;
			}
		}

		public LoopingAudioStream(WaveStream sourceStream)
		{
			_sourceStream = sourceStream;
		}

		public void StopLoop()
		{
			_stopLoop = true;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int totalBytesRead;
			int bytesRead;
			for (totalBytesRead = 0; totalBytesRead < count; totalBytesRead += bytesRead)
			{
				bytesRead = _sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
				if (bytesRead == 0)
				{
					if (_sourceStream.Position == 0L || _stopLoop)
					{
						break;
					}
					_sourceStream.Position = 0L;
				}
			}
			return totalBytesRead;
		}
	}
}
