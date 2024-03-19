using System.IO;

namespace LiteDB.Engine
{
	internal class FileStreamFactory : IStreamFactory
	{
		private readonly string _filename;

		private readonly string _password;

		private readonly bool _readonly;

		private readonly bool _hidden;

		public string Name => Path.GetFileName(_filename);

		public bool CloseOnDispose => true;

		public FileStreamFactory(string filename, string password, bool readOnly, bool hidden)
		{
			_filename = filename;
			_password = password;
			_readonly = readOnly;
			_hidden = hidden;
		}

		public Stream GetStream(bool canWrite, bool sequencial)
		{
			bool write = canWrite && !_readonly;
			bool num = write && !Exists();
			FileStream stream = new FileStream(_filename, _readonly ? FileMode.Open : FileMode.OpenOrCreate, (!write) ? FileAccess.Read : FileAccess.ReadWrite, write ? FileShare.Read : FileShare.ReadWrite, 8192, sequencial ? FileOptions.SequentialScan : FileOptions.RandomAccess);
			if (num && _hidden)
			{
				File.SetAttributes(_filename, FileAttributes.Hidden);
			}
			if (_password != null)
			{
				return new AesStream(_password, stream);
			}
			return stream;
		}

		public long GetLength()
		{
			if (!Exists())
			{
				return 0L;
			}
			long length = new FileInfo(_filename).Length;
			if (length % 8192 != 0L)
			{
				length -= length % 8192;
				using FileStream fs = new FileStream(_filename, FileMode.Open, FileAccess.Write, FileShare.None, 8192, FileOptions.SequentialScan);
				fs.SetLength(length);
				fs.FlushToDisk();
			}
			if (length <= 0)
			{
				return 0L;
			}
			return length - ((_password != null) ? 8192 : 0);
		}

		public bool Exists()
		{
			return File.Exists(_filename);
		}

		public void Delete()
		{
			File.Delete(_filename);
		}

		public bool IsLocked()
		{
			if (Exists())
			{
				return FileHelper.IsFileLocked(_filename);
			}
			return false;
		}
	}
}
