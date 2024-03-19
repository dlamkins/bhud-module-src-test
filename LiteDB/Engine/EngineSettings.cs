using System;
using System.IO;

namespace LiteDB.Engine
{
	public class EngineSettings
	{
		public Stream DataStream { get; set; }

		public Stream LogStream { get; set; }

		public Stream TempStream { get; set; }

		public string Filename { get; set; }

		public string Password { get; set; }

		public long InitialSize { get; set; }

		public Collation Collation { get; set; }

		public bool ReadOnly { get; set; }

		internal IStreamFactory CreateDataFactory()
		{
			if (DataStream != null)
			{
				return new StreamFactory(DataStream, Password);
			}
			if (Filename == ":memory:")
			{
				return new StreamFactory(new MemoryStream(), Password);
			}
			if (Filename == ":temp:")
			{
				return new StreamFactory(new TempStream(null, 10485760L), Password);
			}
			if (!string.IsNullOrEmpty(Filename))
			{
				return new FileStreamFactory(Filename, Password, ReadOnly, hidden: false);
			}
			throw new ArgumentException("EngineSettings must have Filename or DataStream as data source");
		}

		internal IStreamFactory CreateLogFactory()
		{
			if (LogStream != null)
			{
				return new StreamFactory(LogStream, Password);
			}
			if (Filename == ":memory:")
			{
				return new StreamFactory(new MemoryStream(), Password);
			}
			if (Filename == ":temp:")
			{
				return new StreamFactory(new TempStream(null, 10485760L), Password);
			}
			if (!string.IsNullOrEmpty(Filename))
			{
				return new FileStreamFactory(FileHelper.GetLogFile(Filename), Password, ReadOnly, hidden: false);
			}
			return new StreamFactory(new MemoryStream(), Password);
		}

		internal IStreamFactory CreateTempFactory()
		{
			if (TempStream != null)
			{
				return new StreamFactory(TempStream, Password);
			}
			if (Filename == ":memory:")
			{
				return new StreamFactory(new MemoryStream(), Password);
			}
			if (Filename == ":temp:")
			{
				return new StreamFactory(new TempStream(null, 10485760L), Password);
			}
			if (!string.IsNullOrEmpty(Filename))
			{
				return new FileStreamFactory(FileHelper.GetTempFile(Filename), Password, readOnly: false, hidden: true);
			}
			return new StreamFactory(new TempStream(null, 10485760L), Password);
		}
	}
}
