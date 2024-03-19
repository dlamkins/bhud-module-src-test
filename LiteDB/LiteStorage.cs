using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace LiteDB
{
	public class LiteStorage<TFileId> : ILiteStorage<TFileId>
	{
		private readonly ILiteDatabase _db;

		private readonly ILiteCollection<LiteFileInfo<TFileId>> _files;

		private readonly ILiteCollection<BsonDocument> _chunks;

		public LiteStorage(ILiteDatabase db, string filesCollection, string chunksCollection)
		{
			_db = db;
			_files = db.GetCollection<LiteFileInfo<TFileId>>(filesCollection);
			_chunks = db.GetCollection(chunksCollection);
		}

		public LiteFileInfo<TFileId> FindById(TFileId id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			BsonValue fileId = _db.Mapper.Serialize(typeof(TFileId), id);
			LiteFileInfo<TFileId> file = _files.FindById(fileId);
			if (file == null)
			{
				return null;
			}
			file.SetReference(fileId, _files, _chunks);
			return file;
		}

		public IEnumerable<LiteFileInfo<TFileId>> Find(BsonExpression predicate)
		{
			ILiteQueryable<LiteFileInfo<TFileId>> query = _files.Query();
			if (predicate != null)
			{
				query = query.Where(predicate);
			}
			foreach (LiteFileInfo<TFileId> file in query.ToEnumerable())
			{
				BsonValue fileId = _db.Mapper.Serialize(typeof(TFileId), file.Id);
				file.SetReference(fileId, _files, _chunks);
				yield return file;
			}
		}

		public IEnumerable<LiteFileInfo<TFileId>> Find(string predicate, BsonDocument parameters)
		{
			return Find(BsonExpression.Create(predicate, parameters));
		}

		public IEnumerable<LiteFileInfo<TFileId>> Find(string predicate, params BsonValue[] args)
		{
			return Find(BsonExpression.Create(predicate, args));
		}

		public IEnumerable<LiteFileInfo<TFileId>> Find(Expression<Func<LiteFileInfo<TFileId>, bool>> predicate)
		{
			return Find(_db.Mapper.GetExpression(predicate));
		}

		public IEnumerable<LiteFileInfo<TFileId>> FindAll()
		{
			return Find((BsonExpression)null);
		}

		public bool Exists(TFileId id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			BsonValue fileId = _db.Mapper.Serialize(typeof(TFileId), id);
			return _files.Exists("_id = @0", fileId);
		}

		public LiteFileStream<TFileId> OpenWrite(TFileId id, string filename, BsonDocument metadata = null)
		{
			BsonValue fileId = _db.Mapper.Serialize(typeof(TFileId), id);
			LiteFileInfo<TFileId> file = FindById(id);
			if (file == null)
			{
				file = new LiteFileInfo<TFileId>
				{
					Id = id,
					Filename = Path.GetFileName(filename),
					MimeType = MimeTypeConverter.GetMimeType(filename),
					Metadata = (metadata ?? new BsonDocument())
				};
				file.SetReference(fileId, _files, _chunks);
			}
			else
			{
				file.Filename = Path.GetFileName(filename);
				file.MimeType = MimeTypeConverter.GetMimeType(filename);
				file.Metadata = metadata ?? file.Metadata;
			}
			return file.OpenWrite();
		}

		public LiteFileInfo<TFileId> Upload(TFileId id, string filename, Stream stream, BsonDocument metadata = null)
		{
			using LiteFileStream<TFileId> writer = OpenWrite(id, filename, metadata);
			stream.CopyTo(writer);
			return writer.FileInfo;
		}

		public LiteFileInfo<TFileId> Upload(TFileId id, string filename)
		{
			if (filename.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("filename");
			}
			using FileStream stream = File.OpenRead(filename);
			return Upload(id, Path.GetFileName(filename), stream);
		}

		public bool SetMetadata(TFileId id, BsonDocument metadata)
		{
			LiteFileInfo<TFileId> file = FindById(id);
			if (file == null)
			{
				return false;
			}
			file.Metadata = metadata ?? new BsonDocument();
			_files.Update(file);
			return true;
		}

		public LiteFileStream<TFileId> OpenRead(TFileId id)
		{
			return (FindById(id) ?? throw LiteException.FileNotFound(id.ToString())).OpenRead();
		}

		public LiteFileInfo<TFileId> Download(TFileId id, Stream stream)
		{
			LiteFileInfo<TFileId> obj = FindById(id) ?? throw LiteException.FileNotFound(id.ToString());
			obj.CopyTo(stream);
			return obj;
		}

		public LiteFileInfo<TFileId> Download(TFileId id, string filename, bool overwritten)
		{
			LiteFileInfo<TFileId> obj = FindById(id) ?? throw LiteException.FileNotFound(id.ToString());
			obj.SaveAs(filename, overwritten);
			return obj;
		}

		public bool Delete(TFileId id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			BsonValue fileId = _db.Mapper.Serialize(typeof(TFileId), id);
			bool deleted = _files.Delete(fileId);
			if (deleted)
			{
				_chunks.DeleteMany("_id BETWEEN { f: @0, n: 0} AND {f: @0, n: @1 }", fileId, int.MaxValue);
			}
			return deleted;
		}
	}
}
