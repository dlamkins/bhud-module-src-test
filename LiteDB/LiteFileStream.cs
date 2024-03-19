using System;
using System.IO;

namespace LiteDB
{
	public class LiteFileStream<TFileId> : Stream
	{
		public const int MAX_CHUNK_SIZE = 261120;

		private readonly ILiteCollection<LiteFileInfo<TFileId>> _files;

		private readonly ILiteCollection<BsonDocument> _chunks;

		private readonly LiteFileInfo<TFileId> _file;

		private readonly BsonValue _fileId;

		private readonly FileAccess _mode;

		private long _streamPosition;

		private int _currentChunkIndex;

		private byte[] _currentChunkData;

		private int _positionInChunk;

		private MemoryStream _buffer;

		private bool _disposed;

		public LiteFileInfo<TFileId> FileInfo => _file;

		public override long Length => _file.Length;

		public override bool CanRead => _mode == FileAccess.Read;

		public override bool CanWrite => _mode == FileAccess.Write;

		public override bool CanSeek => _mode == FileAccess.Read;

		public override long Position
		{
			get
			{
				return _streamPosition;
			}
			set
			{
				if (_mode == FileAccess.Read)
				{
					SetReadStreamPosition(value);
					return;
				}
				throw new NotSupportedException();
			}
		}

		internal LiteFileStream(ILiteCollection<LiteFileInfo<TFileId>> files, ILiteCollection<BsonDocument> chunks, LiteFileInfo<TFileId> file, BsonValue fileId, FileAccess mode)
		{
			_files = files;
			_chunks = chunks;
			_file = file;
			_fileId = fileId;
			_mode = mode;
			switch (mode)
			{
			case FileAccess.Read:
				_currentChunkData = GetChunkData(_currentChunkIndex);
				break;
			case FileAccess.Write:
				_buffer = new MemoryStream(261120);
				if (_file.Length > 0)
				{
					Constants.ENSURE(_chunks.DeleteMany("_id BETWEEN { f: @0, n: 0 } AND { f: @0, n: 99999999 }", _fileId) == _file.Chunks);
					_file.Length = 0L;
					_file.Chunks = 0;
				}
				break;
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			if (_mode == FileAccess.Write)
			{
				throw new NotSupportedException();
			}
			switch (origin)
			{
			case SeekOrigin.Begin:
				SetReadStreamPosition(offset);
				break;
			case SeekOrigin.Current:
				SetReadStreamPosition(_streamPosition + offset);
				break;
			case SeekOrigin.End:
				SetReadStreamPosition(Length + offset);
				break;
			}
			return _streamPosition;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (!_disposed)
			{
				if (disposing && CanWrite)
				{
					Flush();
					_buffer?.Dispose();
				}
				_disposed = true;
			}
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (_mode != FileAccess.Read)
			{
				throw new NotSupportedException();
			}
			int bytesLeft = count;
			while (_currentChunkData != null && bytesLeft > 0)
			{
				int bytesToCopy = Math.Min(bytesLeft, _currentChunkData.Length - _positionInChunk);
				Buffer.BlockCopy(_currentChunkData, _positionInChunk, buffer, offset, bytesToCopy);
				_positionInChunk += bytesToCopy;
				bytesLeft -= bytesToCopy;
				offset += bytesToCopy;
				_streamPosition += bytesToCopy;
				if (_positionInChunk >= _currentChunkData.Length)
				{
					_positionInChunk = 0;
					_currentChunkData = GetChunkData(++_currentChunkIndex);
				}
			}
			return count - bytesLeft;
		}

		private byte[] GetChunkData(int index)
		{
			return _chunks.FindOne("_id = { f: @0, n: @1 }", _fileId, index)?["data"].AsBinary;
		}

		private void SetReadStreamPosition(long newPosition)
		{
			if (newPosition < 0 && newPosition > Length)
			{
				throw new ArgumentOutOfRangeException();
			}
			_streamPosition = newPosition;
			_currentChunkIndex = (int)_streamPosition / 261120;
			_positionInChunk = (int)_streamPosition % 261120;
			_currentChunkData = GetChunkData(_currentChunkIndex);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_streamPosition += count;
			_buffer.Write(buffer, offset, count);
			if (_buffer.Length >= 261120)
			{
				WriteChunks(flush: false);
			}
		}

		public override void Flush()
		{
			WriteChunks(flush: true);
		}

		private void WriteChunks(bool flush)
		{
			byte[] buffer = new byte[261120];
			int read = 0;
			_buffer.Seek(0L, SeekOrigin.Begin);
			while ((read = _buffer.Read(buffer, 0, 261120)) > 0)
			{
				BsonDocument chunk = new BsonDocument { ["_id"] = new BsonDocument
				{
					["f"] = _fileId,
					["n"] = _file.Chunks++
				} };
				if (read != 261120)
				{
					byte[] bytes = new byte[read];
					Buffer.BlockCopy(buffer, 0, bytes, 0, read);
					chunk["data"] = bytes;
				}
				else
				{
					chunk["data"] = buffer;
				}
				_chunks.Insert(chunk);
			}
			if (flush)
			{
				_file.UploadDate = DateTime.Now;
				_file.Length = _streamPosition;
				_files.Upsert(_file);
			}
			_buffer = new MemoryStream();
		}
	}
}
