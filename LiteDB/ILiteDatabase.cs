using System;
using System.Collections.Generic;
using System.IO;
using LiteDB.Engine;

namespace LiteDB
{
	public interface ILiteDatabase : IDisposable
	{
		BsonMapper Mapper { get; }

		ILiteStorage<string> FileStorage { get; }

		int UserVersion { get; set; }

		TimeSpan Timeout { get; set; }

		bool UtcDate { get; set; }

		long LimitSize { get; set; }

		int CheckpointSize { get; set; }

		Collation Collation { get; }

		ILiteCollection<T> GetCollection<T>(string name, BsonAutoId autoId = BsonAutoId.ObjectId);

		ILiteCollection<T> GetCollection<T>();

		ILiteCollection<T> GetCollection<T>(BsonAutoId autoId);

		ILiteCollection<BsonDocument> GetCollection(string name, BsonAutoId autoId = BsonAutoId.ObjectId);

		bool BeginTrans();

		bool Commit();

		bool Rollback();

		ILiteStorage<TFileId> GetStorage<TFileId>(string filesCollection = "_files", string chunksCollection = "_chunks");

		IEnumerable<string> GetCollectionNames();

		bool CollectionExists(string name);

		bool DropCollection(string name);

		bool RenameCollection(string oldName, string newName);

		IBsonDataReader Execute(TextReader commandReader, BsonDocument parameters = null);

		IBsonDataReader Execute(string command, BsonDocument parameters = null);

		IBsonDataReader Execute(string command, params BsonValue[] args);

		void Checkpoint();

		long Rebuild(RebuildOptions options = null);

		BsonValue Pragma(string name);

		BsonValue Pragma(string name, BsonValue value);
	}
}
