using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LiteDB.Engine;

namespace LiteDB
{
	public class LiteQueryable<T> : ILiteQueryable<T>, ILiteQueryableResult<T>
	{
		protected readonly ILiteEngine _engine;

		protected readonly BsonMapper _mapper;

		protected readonly string _collection;

		protected readonly Query _query;

		private readonly bool _isSimpleType = Reflection.IsSimpleType(typeof(T));

		internal LiteQueryable(ILiteEngine engine, BsonMapper mapper, string collection, Query query)
		{
			_engine = engine;
			_mapper = mapper;
			_collection = collection;
			_query = query;
		}

		public ILiteQueryable<T> Include<K>(Expression<Func<T, K>> path)
		{
			_query.Includes.Add(_mapper.GetExpression(path));
			return this;
		}

		public ILiteQueryable<T> Include(BsonExpression path)
		{
			_query.Includes.Add(path);
			return this;
		}

		public ILiteQueryable<T> Include(List<BsonExpression> paths)
		{
			_query.Includes.AddRange(paths);
			return this;
		}

		public ILiteQueryable<T> Where(BsonExpression predicate)
		{
			_query.Where.Add(predicate);
			return this;
		}

		public ILiteQueryable<T> Where(string predicate, BsonDocument parameters)
		{
			_query.Where.Add(BsonExpression.Create(predicate, parameters));
			return this;
		}

		public ILiteQueryable<T> Where(string predicate, params BsonValue[] args)
		{
			_query.Where.Add(BsonExpression.Create(predicate, args));
			return this;
		}

		public ILiteQueryable<T> Where(Expression<Func<T, bool>> predicate)
		{
			return Where(_mapper.GetExpression(predicate));
		}

		public ILiteQueryable<T> OrderBy(BsonExpression keySelector, int order = 1)
		{
			if (_query.OrderBy != null)
			{
				throw new ArgumentException("ORDER BY already defined in this query builder");
			}
			_query.OrderBy = keySelector;
			_query.Order = order;
			return this;
		}

		public ILiteQueryable<T> OrderBy<K>(Expression<Func<T, K>> keySelector, int order = 1)
		{
			return OrderBy(_mapper.GetExpression(keySelector), order);
		}

		public ILiteQueryable<T> OrderByDescending(BsonExpression keySelector)
		{
			return OrderBy(keySelector, -1);
		}

		public ILiteQueryable<T> OrderByDescending<K>(Expression<Func<T, K>> keySelector)
		{
			return OrderBy(keySelector, -1);
		}

		public ILiteQueryable<T> GroupBy(BsonExpression keySelector)
		{
			if (_query.GroupBy != null)
			{
				throw new ArgumentException("GROUP BY already defined in this query");
			}
			_query.GroupBy = keySelector;
			return this;
		}

		public ILiteQueryable<T> Having(BsonExpression predicate)
		{
			if (_query.Having != null)
			{
				throw new ArgumentException("HAVING already defined in this query");
			}
			_query.Having = predicate;
			return this;
		}

		public ILiteQueryableResult<BsonDocument> Select(BsonExpression selector)
		{
			_query.Select = selector;
			return new LiteQueryable<BsonDocument>(_engine, _mapper, _collection, _query);
		}

		public ILiteQueryableResult<K> Select<K>(Expression<Func<T, K>> selector)
		{
			if (_query.GroupBy != null)
			{
				throw new ArgumentException("Use Select(BsonExpression selector) when using GroupBy query");
			}
			_query.Select = _mapper.GetExpression(selector);
			return new LiteQueryable<K>(_engine, _mapper, _collection, _query);
		}

		public ILiteQueryableResult<T> ForUpdate()
		{
			_query.ForUpdate = true;
			return this;
		}

		public ILiteQueryableResult<T> Offset(int offset)
		{
			_query.Offset = offset;
			return this;
		}

		public ILiteQueryableResult<T> Skip(int offset)
		{
			return Offset(offset);
		}

		public ILiteQueryableResult<T> Limit(int limit)
		{
			_query.Limit = limit;
			return this;
		}

		public IBsonDataReader ExecuteReader()
		{
			_query.ExplainPlan = false;
			return _engine.Query(_collection, _query);
		}

		public IEnumerable<BsonDocument> ToDocuments()
		{
			using IBsonDataReader reader = ExecuteReader();
			while (reader.Read())
			{
				yield return reader.Current as BsonDocument;
			}
		}

		public IEnumerable<T> ToEnumerable()
		{
			if (_isSimpleType)
			{
				return from x in ToDocuments()
					select x[x.Keys.First()] into x
					select (T)_mapper.Deserialize(typeof(T), x);
			}
			return from x in ToDocuments()
				select (T)_mapper.Deserialize(typeof(T), x);
		}

		public List<T> ToList()
		{
			return ToEnumerable().ToList();
		}

		public T[] ToArray()
		{
			return ToEnumerable().ToArray();
		}

		public BsonDocument GetPlan()
		{
			_query.ExplainPlan = true;
			return _engine.Query(_collection, _query).ToEnumerable().FirstOrDefault()?.AsDocument;
		}

		public T Single()
		{
			return ToEnumerable().Single();
		}

		public T SingleOrDefault()
		{
			return ToEnumerable().SingleOrDefault();
		}

		public T First()
		{
			return ToEnumerable().First();
		}

		public T FirstOrDefault()
		{
			return ToEnumerable().FirstOrDefault();
		}

		public int Count()
		{
			BsonExpression oldSelect = _query.Select;
			try
			{
				Select("{ count: COUNT(*._id) }");
				return ToDocuments().Single()["count"].AsInt32;
			}
			finally
			{
				_query.Select = oldSelect;
			}
		}

		public long LongCount()
		{
			BsonExpression oldSelect = _query.Select;
			try
			{
				Select("{ count: COUNT(*._id) }");
				return ToDocuments().Single()["count"].AsInt64;
			}
			finally
			{
				_query.Select = oldSelect;
			}
		}

		public bool Exists()
		{
			BsonExpression oldSelect = _query.Select;
			try
			{
				Select("{ exists: ANY(*._id) }");
				return ToDocuments().Single()["exists"].AsBoolean;
			}
			finally
			{
				_query.Select = oldSelect;
			}
		}

		public int Into(string newCollection, BsonAutoId autoId = BsonAutoId.ObjectId)
		{
			_query.Into = newCollection;
			_query.IntoAutoId = autoId;
			using IBsonDataReader reader = ExecuteReader();
			return reader.Current.AsInt32;
		}
	}
}
