using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace LiteDB
{
	public interface ILiteStorage<TFileId>
	{
		LiteFileInfo<TFileId> FindById(TFileId id);

		IEnumerable<LiteFileInfo<TFileId>> Find(BsonExpression predicate);

		IEnumerable<LiteFileInfo<TFileId>> Find(string predicate, BsonDocument parameters);

		IEnumerable<LiteFileInfo<TFileId>> Find(string predicate, params BsonValue[] args);

		IEnumerable<LiteFileInfo<TFileId>> Find(Expression<Func<LiteFileInfo<TFileId>, bool>> predicate);

		IEnumerable<LiteFileInfo<TFileId>> FindAll();

		bool Exists(TFileId id);

		LiteFileStream<TFileId> OpenWrite(TFileId id, string filename, BsonDocument metadata = null);

		LiteFileInfo<TFileId> Upload(TFileId id, string filename, Stream stream, BsonDocument metadata = null);

		LiteFileInfo<TFileId> Upload(TFileId id, string filename);

		bool SetMetadata(TFileId id, BsonDocument metadata);

		LiteFileStream<TFileId> OpenRead(TFileId id);

		LiteFileInfo<TFileId> Download(TFileId id, Stream stream);

		LiteFileInfo<TFileId> Download(TFileId id, string filename, bool overwritten);

		bool Delete(TFileId id);
	}
}
