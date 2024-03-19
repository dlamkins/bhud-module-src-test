using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace LiteDB
{
	public class LiteRepository : ILiteRepository, IDisposable
	{
		private readonly ILiteDatabase _db;

		public ILiteDatabase Database => _db;

		public LiteRepository(ILiteDatabase database)
		{
			_db = database;
		}

		public LiteRepository(string connectionString, BsonMapper mapper = null)
		{
			_db = new LiteDatabase(connectionString, mapper);
		}

		public LiteRepository(ConnectionString connectionString, BsonMapper mapper = null)
		{
			_db = new LiteDatabase(connectionString, mapper);
		}

		public LiteRepository(Stream stream, BsonMapper mapper = null, Stream logStream = null)
		{
			_db = new LiteDatabase(stream, mapper, logStream);
		}

		public BsonValue Insert<T>(T entity, string collectionName = null)
		{
			return _db.GetCollection<T>(collectionName).Insert(entity);
		}

		public int Insert<T>(IEnumerable<T> entities, string collectionName = null)
		{
			return _db.GetCollection<T>(collectionName).Insert(entities);
		}

		public bool Update<T>(T entity, string collectionName = null)
		{
			return _db.GetCollection<T>(collectionName).Update(entity);
		}

		public int Update<T>(IEnumerable<T> entities, string collectionName = null)
		{
			return _db.GetCollection<T>(collectionName).Update(entities);
		}

		public bool Upsert<T>(T entity, string collectionName = null)
		{
			return _db.GetCollection<T>(collectionName).Upsert(entity);
		}

		public int Upsert<T>(IEnumerable<T> entities, string collectionName = null)
		{
			return _db.GetCollection<T>(collectionName).Upsert(entities);
		}

		public bool Delete<T>(BsonValue id, string collectionName = null)
		{
			return _db.GetCollection<T>(collectionName).Delete(id);
		}

		public int DeleteMany<T>(BsonExpression predicate, string collectionName = null)
		{
			return _db.GetCollection<T>(collectionName).DeleteMany(predicate);
		}

		public int DeleteMany<T>(Expression<Func<T, bool>> predicate, string collectionName = null)
		{
			return _db.GetCollection<T>(collectionName).DeleteMany(predicate);
		}

		public ILiteQueryable<T> Query<T>(string collectionName = null)
		{
			return _db.GetCollection<T>(collectionName).Query();
		}

		public bool EnsureIndex<T>(string name, BsonExpression expression, bool unique = false, string collectionName = null)
		{
			return _db.GetCollection<T>(collectionName).EnsureIndex(name, expression, unique);
		}

		public bool EnsureIndex<T>(BsonExpression expression, bool unique = false, string collectionName = null)
		{
			return _db.GetCollection<T>(collectionName).EnsureIndex(expression, unique);
		}

		public bool EnsureIndex<T, K>(Expression<Func<T, K>> keySelector, bool unique = false, string collectionName = null)
		{
			return _db.GetCollection<T>(collectionName).EnsureIndex(keySelector, unique);
		}

		public bool EnsureIndex<T, K>(string name, Expression<Func<T, K>> keySelector, bool unique = false, string collectionName = null)
		{
			return _db.GetCollection<T>(collectionName).EnsureIndex(name, keySelector, unique);
		}

		public T SingleById<T>(BsonValue id, string collectionName = null)
		{
			return _db.GetCollection<T>(collectionName).Query().Where("_id = @0", id)
				.Single();
		}

		public List<T> Fetch<T>(BsonExpression predicate, string collectionName = null)
		{
			return Query<T>(collectionName).Where(predicate).ToList();
		}

		public List<T> Fetch<T>(Expression<Func<T, bool>> predicate, string collectionName = null)
		{
			return Query<T>(collectionName).Where(predicate).ToList();
		}

		public T First<T>(BsonExpression predicate, string collectionName = null)
		{
			return Query<T>(collectionName).Where(predicate).First();
		}

		public T First<T>(Expression<Func<T, bool>> predicate, string collectionName = null)
		{
			return Query<T>(collectionName).Where(predicate).First();
		}

		public T FirstOrDefault<T>(BsonExpression predicate, string collectionName = null)
		{
			return Query<T>(collectionName).Where(predicate).FirstOrDefault();
		}

		public T FirstOrDefault<T>(Expression<Func<T, bool>> predicate, string collectionName = null)
		{
			return Query<T>(collectionName).Where(predicate).FirstOrDefault();
		}

		public T Single<T>(BsonExpression predicate, string collectionName = null)
		{
			return Query<T>(collectionName).Where(predicate).Single();
		}

		public T Single<T>(Expression<Func<T, bool>> predicate, string collectionName = null)
		{
			return Query<T>(collectionName).Where(predicate).Single();
		}

		public T SingleOrDefault<T>(BsonExpression predicate, string collectionName = null)
		{
			return Query<T>(collectionName).Where(predicate).SingleOrDefault();
		}

		public T SingleOrDefault<T>(Expression<Func<T, bool>> predicate, string collectionName = null)
		{
			return Query<T>(collectionName).Where(predicate).SingleOrDefault();
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~LiteRepository()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_db.Dispose();
			}
		}
	}
}
