using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LiteDB
{
	public interface ILiteCollection<T>
	{
		string Name { get; }

		BsonAutoId AutoId { get; }

		EntityMapper EntityMapper { get; }

		ILiteCollection<T> Include<K>(Expression<Func<T, K>> keySelector);

		ILiteCollection<T> Include(BsonExpression keySelector);

		bool Upsert(T entity);

		int Upsert(IEnumerable<T> entities);

		bool Upsert(BsonValue id, T entity);

		bool Update(T entity);

		bool Update(BsonValue id, T entity);

		int Update(IEnumerable<T> entities);

		int UpdateMany(BsonExpression transform, BsonExpression predicate);

		int UpdateMany(Expression<Func<T, T>> extend, Expression<Func<T, bool>> predicate);

		BsonValue Insert(T entity);

		void Insert(BsonValue id, T entity);

		int Insert(IEnumerable<T> entities);

		int InsertBulk(IEnumerable<T> entities, int batchSize = 5000);

		bool EnsureIndex(string name, BsonExpression expression, bool unique = false);

		bool EnsureIndex(BsonExpression expression, bool unique = false);

		bool EnsureIndex<K>(Expression<Func<T, K>> keySelector, bool unique = false);

		bool EnsureIndex<K>(string name, Expression<Func<T, K>> keySelector, bool unique = false);

		bool DropIndex(string name);

		ILiteQueryable<T> Query();

		IEnumerable<T> Find(BsonExpression predicate, int skip = 0, int limit = int.MaxValue);

		IEnumerable<T> Find(Query query, int skip = 0, int limit = int.MaxValue);

		IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int skip = 0, int limit = int.MaxValue);

		T FindById(BsonValue id);

		T FindOne(BsonExpression predicate);

		T FindOne(string predicate, BsonDocument parameters);

		T FindOne(BsonExpression predicate, params BsonValue[] args);

		T FindOne(Expression<Func<T, bool>> predicate);

		T FindOne(Query query);

		IEnumerable<T> FindAll();

		bool Delete(BsonValue id);

		int DeleteAll();

		int DeleteMany(BsonExpression predicate);

		int DeleteMany(string predicate, BsonDocument parameters);

		int DeleteMany(string predicate, params BsonValue[] args);

		int DeleteMany(Expression<Func<T, bool>> predicate);

		int Count();

		int Count(BsonExpression predicate);

		int Count(string predicate, BsonDocument parameters);

		int Count(string predicate, params BsonValue[] args);

		int Count(Expression<Func<T, bool>> predicate);

		int Count(Query query);

		long LongCount();

		long LongCount(BsonExpression predicate);

		long LongCount(string predicate, BsonDocument parameters);

		long LongCount(string predicate, params BsonValue[] args);

		long LongCount(Expression<Func<T, bool>> predicate);

		long LongCount(Query query);

		bool Exists(BsonExpression predicate);

		bool Exists(string predicate, BsonDocument parameters);

		bool Exists(string predicate, params BsonValue[] args);

		bool Exists(Expression<Func<T, bool>> predicate);

		bool Exists(Query query);

		BsonValue Min(BsonExpression keySelector);

		BsonValue Min();

		K Min<K>(Expression<Func<T, K>> keySelector);

		BsonValue Max(BsonExpression keySelector);

		BsonValue Max();

		K Max<K>(Expression<Func<T, K>> keySelector);
	}
}
