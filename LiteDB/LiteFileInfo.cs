using System;
using System.IO;

namespace LiteDB
{
	public class LiteFileInfo<TFileId>
	{
		private BsonValue _fileId;

		private ILiteCollection<LiteFileInfo<TFileId>> _files;

		private ILiteCollection<BsonDocument> _chunks;

		public TFileId Id { get; internal set; }

		[BsonField("filename")]
		public string Filename { get; internal set; }

		[BsonField("mimeType")]
		public string MimeType { get; internal set; }

		[BsonField("length")]
		public long Length { get; internal set; }

		[BsonField("chunks")]
		public int Chunks { get; internal set; }

		[BsonField("uploadDate")]
		public DateTime UploadDate { get; internal set; } = DateTime.Now;


		[BsonField("metadata")]
		public BsonDocument Metadata { get; set; } = new BsonDocument();


		internal void SetReference(BsonValue fileId, ILiteCollection<LiteFileInfo<TFileId>> files, ILiteCollection<BsonDocument> chunks)
		{
			_fileId = fileId;
			_files = files;
			_chunks = chunks;
		}

		public LiteFileStream<TFileId> OpenRead()
		{
			return new LiteFileStream<TFileId>(_files, _chunks, this, _fileId, FileAccess.Read);
		}

		public LiteFileStream<TFileId> OpenWrite()
		{
			return new LiteFileStream<TFileId>(_files, _chunks, this, _fileId, FileAccess.Write);
		}

		public void CopyTo(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			using LiteFileStream<TFileId> reader = OpenRead();
			reader.CopyTo(stream);
		}

		public void SaveAs(string filename, bool overwritten = true)
		{
			if (filename.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("filename");
			}
			using FileStream file = File.Open(filename, (!overwritten) ? FileMode.CreateNew : FileMode.Create);
			using LiteFileStream<TFileId> stream = OpenRead();
			stream.CopyTo(file);
		}
	}
}
