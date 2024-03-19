using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using LiteDB.Engine;

namespace LiteDB
{
	public sealed class LiteCollection<T> : ILiteCollection<T>
	{
		private readonly string _collection;

		private readonly ILiteEngine _engine;

		private readonly List<BsonExpression> _includes;

		private readonly BsonMapper _mapper;

		private readonly EntityMapper _entity;

		private readonly MemberMapper _id;

		private readonly BsonAutoId _autoId;

		public string Name => _collection;

		public BsonAutoId AutoId => _autoId;

		public EntityMapper EntityMapper => _entity;

		public int Count()
		{
			return Query().Count();
		}

		public int Count(BsonExpression predicate)
		{
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			return Query().Where(predicate).Count();
		}

		public int Count(string predicate, BsonDocument parameters)
		{
			return Count(BsonExpression.Create(predicate, parameters));
		}

		public int Count(string predicate, params BsonValue[] args)
		{
			return Count(BsonExpression.Create(predicate, args));
		}

		public int Count(Expression<Func<T, bool>> predicate)
		{
			return Count(_mapper.GetExpression(predicate));
		}

		public int Count(Query query)
		{
			return new LiteQueryable<T>(_engine, _mapper, _collection, query).Count();
		}

		public long LongCount()
		{
			return Query().LongCount();
		}

		public long LongCount(BsonExpression predicate)
		{
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			return Query().Where(predicate).LongCount();
		}

		public long LongCount(string predicate, BsonDocument parameters)
		{
			return LongCount(BsonExpression.Create(predicate, parameters));
		}

		public long LongCount(string predicate, params BsonValue[] args)
		{
			return LongCount(BsonExpression.Create(predicate, args));
		}

		public long LongCount(Expression<Func<T, bool>> predicate)
		{
			return LongCount(_mapper.GetExpression(predicate));
		}

		public long LongCount(Query query)
		{
			return new LiteQueryable<T>(_engine, _mapper, _collection, query).Count();
		}

		public bool Exists(BsonExpression predicate)
		{
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			return Query().Where(predicate).Exists();
		}

		public bool Exists(string predicate, BsonDocument parameters)
		{
			return Exists(BsonExpression.Create(predicate, parameters));
		}

		public bool Exists(string predicate, params BsonValue[] args)
		{
			return Exists(BsonExpression.Create(predicate, args));
		}

		public bool Exists(Expression<Func<T, bool>> predicate)
		{
			return Exists(_mapper.GetExpression(predicate));
		}

		public bool Exists(Query query)
		{
			return new LiteQueryable<T>(_engine, _mapper, _collection, query).Exists();
		}

		public BsonValue Min(BsonExpression keySelector)
		{
			if (string.IsNullOrEmpty(keySelector))
			{
				throw new ArgumentNullException("keySelector");
			}
			BsonDocument bsonDocument = Query().OrderBy(keySelector).Select(keySelector).ToDocuments()
				.First();
			return bsonDocument[bsonDocument.Keys.First()];
		}

		public BsonValue Min()
		{
			return Min("_id");
		}

		public K Min<K>(Expression<Func<T, K>> keySelector)
		{
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
			BsonExpression expr = _mapper.GetExpression(keySelector);
			BsonValue value = Min(expr);
			return (K)_mapper.Deserialize(typeof(K), value);
		}

		public BsonValue Max(BsonExpression keySelector)
		{
			if (string.IsNullOrEmpty(keySelector))
			{
				throw new ArgumentNullException("keySelector");
			}
			BsonDocument bsonDocument = Query().OrderByDescending(keySelector).Select(keySelector).ToDocuments()
				.First();
			return bsonDocument[bsonDocument.Keys.First()];
		}

		public BsonValue Max()
		{
			return Max("_id");
		}

		public K Max<K>(Expression<Func<T, K>> keySelector)
		{
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
			BsonExpression expr = _mapper.GetExpression(keySelector);
			BsonValue value = Max(expr);
			return (K)_mapper.Deserialize(typeof(K), value);
		}

		public bool Delete(BsonValue id)
		{
			if (id == null || id.IsNull)
			{
				throw new ArgumentNullException("id");
			}
			return _engine.Delete(_collection, new BsonValue[1] { id }) == 1;
		}

		public int DeleteAll()
		{
			return _engine.DeleteMany(_collection, null);
		}

		public int DeleteMany(BsonExpression predicate)
		{
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			return _engine.DeleteMany(_collection, predicate);
		}

		public int DeleteMany(string predicate, BsonDocument parameters)
		{
			return DeleteMany(BsonExpression.Create(predicate, parameters));
		}

		public int DeleteMany(string predicate, params BsonValue[] args)
		{
			return DeleteMany(BsonExpression.Create(predicate, args));
		}

		public int DeleteMany(Expression<Func<T, bool>> predicate)
		{
			return DeleteMany(_mapper.GetExpression(predicate));
		}

		public ILiteQueryable<T> Query()
		{
			return new LiteQueryable<T>(_engine, _mapper, _collection, new Query()).Include(_includes);
		}

		public IEnumerable<T> Find(BsonExpression predicate, int skip = 0, int limit = int.MaxValue)
		{
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			return Query().Include(_includes).Where(predicate).Skip(skip)
				.Limit(limit)
				.ToEnumerable();
		}

		public IEnumerable<T> Find(Query query, int skip = 0, int limit = int.MaxValue)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}
			if (skip != 0)
			{
				query.Offset = skip;
			}
			if (limit != int.MaxValue)
			{
				query.Limit = limit;
			}
			return new LiteQueryable<T>(_engine, _mapper, _collection, query).ToEnumerable();
		}

		public IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int skip = 0, int limit = int.MaxValue)
		{
			return Find(_mapper.GetExpression(predicate), skip, limit);
		}

		public T FindById(BsonValue id)
		{
			if (id == null || id.IsNull)
			{
				throw new ArgumentNullException("id");
			}
			return Find(BsonExpression.Create("_id = @0", id)).FirstOrDefault();
		}

		public T FindOne(BsonExpression predicate)
		{
			return Find(predicate).FirstOrDefault();
		}

		public T FindOne(string predicate, BsonDocument parameters)
		{
			return FindOne(BsonExpression.Create(predicate, parameters));
		}

		public T FindOne(BsonExpression predicate, params BsonValue[] args)
		{
			return FindOne(BsonExpression.Create(predicate, args));
		}

		public T FindOne(Expression<Func<T, bool>> predicate)
		{
			return FindOne(_mapper.GetExpression(predicate));
		}

		public T FindOne(Query query)
		{
			return Find(query).FirstOrDefault();
		}

		public IEnumerable<T> FindAll()
		{
			return Query().Include(_includes).ToEnumerable();
		}

		public ILiteCollection<T> Include<K>(Expression<Func<T, K>> keySelector)
		{
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
			BsonExpression path = _mapper.GetExpression(keySelector);
			return Include(path);
		}

		public ILiteCollection<T> Include(BsonExpression keySelector)
		{
			if (string.IsNullOrEmpty(keySelector))
			{
				throw new ArgumentNullException("keySelector");
			}
			LiteCollection<T> liteCollection = new LiteCollection<T>(_collection, _autoId, _engine, _mapper);
			liteCollection._includes.AddRange(_includes);
			liteCollection._includes.Add(keySelector);
			return liteCollection;
		}

		public bool EnsureIndex(string name, BsonExpression expression, bool unique = false)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			return _engine.EnsureIndex(_collection, name, expression, unique);
		}

		public bool EnsureIndex(BsonExpression expression, bool unique = false)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			string name = Regex.Replace(expression.Source, "[^a-z0-9]", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			return EnsureIndex(name, expression, unique);
		}

		public bool EnsureIndex<K>(Expression<Func<T, K>> keySelector, bool unique = false)
		{
			BsonExpression expression = GetIndexExpression(keySelector);
			return EnsureIndex(expression, unique);
		}

		public bool EnsureIndex<K>(string name, Expression<Func<T, K>> keySelector, bool unique = false)
		{
			BsonExpression expression = GetIndexExpression(keySelector);
			return EnsureIndex(name, expression, unique);
		}

		private BsonExpression GetIndexExpression<K>(Expression<Func<T, K>> keySelector)
		{
			BsonExpression expression = _mapper.GetIndexExpression(keySelector);
			if (typeof(K).IsEnumerable() && expression.IsScalar)
			{
				if (expression.Type != BsonExpressionType.Path)
				{
					throw new LiteException(0, "Expression `" + expression.Source + "` must return a enumerable expression");
				}
				expression = expression.Source + "[*]";
			}
			return expression;
		}

		public bool DropIndex(string name)
		{
			return _engine.DropIndex(_collection, name);
		}

		public BsonValue Insert(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			BsonDocument doc = _mapper.ToDocument(entity);
			bool removed = RemoveDocId(doc);
			_engine.Insert(_collection, new BsonDocument[1] { doc }, _autoId);
			BsonValue id = doc["_id"];
			if (removed)
			{
				_id.Setter(entity, id.RawValue);
			}
			return id;
		}

		public void Insert(BsonValue id, T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			if (id == null || id.IsNull)
			{
				throw new ArgumentNullException("id");
			}
			BsonDocument doc = _mapper.ToDocument(entity);
			doc["_id"] = id;
			_engine.Insert(_collection, new BsonDocument[1] { doc }, _autoId);
		}

		public int Insert(IEnumerable<T> entities)
		{
			if (entities == null)
			{
				throw new ArgumentNullException("entities");
			}
			return _engine.Insert(_collection, GetBsonDocs(entities), _autoId);
		}

		[Obsolete("Use normal Insert()")]
		public int InsertBulk(IEnumerable<T> entities, int batchSize = 5000)
		{
			if (entities == null)
			{
				throw new ArgumentNullException("entities");
			}
			return _engine.Insert(_collection, GetBsonDocs(entities), _autoId);
		}

		private IEnumerable<BsonDocument> GetBsonDocs(IEnumerable<T> documents)
		{
			foreach (T document in documents)
			{
				BsonDocument doc = _mapper.ToDocument(document);
				bool removed = RemoveDocId(doc);
				yield return doc;
				if (removed && _id != null)
				{
					_id.Setter(document, doc["_id"].RawValue);
				}
			}
		}

		private bool RemoveDocId(BsonDocument doc)
		{
			if (_id != null && doc.TryGetValue("_id", out var id) && ((_autoId == BsonAutoId.Int32 && id.IsInt32 && id.AsInt32 == 0) || (_autoId == BsonAutoId.ObjectId && (id.IsNull || (id.IsObjectId && id.AsObjectId == ObjectId.Empty))) || (_autoId == BsonAutoId.Guid && id.IsGuid && id.AsGuid == Guid.Empty) || (_autoId == BsonAutoId.Int64 && id.IsInt64 && id.AsInt64 == 0L)))
			{
				doc.Remove("_id");
				return true;
			}
			return false;
		}

		public bool Update(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			BsonDocument doc = _mapper.ToDocument(entity);
			return _engine.Update(_collection, new BsonDocument[1] { doc }) > 0;
		}

		public bool Update(BsonValue id, T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			if (id == null || id.IsNull)
			{
				throw new ArgumentNullException("id");
			}
			BsonDocument doc = _mapper.ToDocument(entity);
			doc["_id"] = id;
			return _engine.Update(_collection, new BsonDocument[1] { doc }) > 0;
		}

		public int Update(IEnumerable<T> entities)
		{
			if (entities == null)
			{
				throw new ArgumentNullException("entities");
			}
			return _engine.Update(_collection, entities.Select((T x) => _mapper.ToDocument(x)));
		}

		public int UpdateMany(BsonExpression transform, BsonExpression predicate)
		{
			if (transform == null)
			{
				throw new ArgumentNullException("transform");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			if (transform.Type != BsonExpressionType.Document)
			{
				throw new ArgumentException("Extend expression must return a document. Eg: `col.UpdateMany('{ Name: UPPER(Name) }', 'Age > 10')`");
			}
			return _engine.UpdateMany(_collection, transform, predicate);
		}

		public int UpdateMany(Expression<Func<T, T>> extend, Expression<Func<T, bool>> predicate)
		{
			if (extend == null)
			{
				throw new ArgumentNullException("extend");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			BsonExpression ext = _mapper.GetExpression(extend);
			BsonExpression pred = _mapper.GetExpression(predicate);
			if (ext.Type != BsonExpressionType.Document)
			{
				throw new ArgumentException("Extend expression must return an anonymous class to be merge with entities. Eg: `col.UpdateMany(x => new { Name = x.Name.ToUpper() }, x => x.Age > 10)`");
			}
			return _engine.UpdateMany(_collection, ext, pred);
		}

		public bool Upsert(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			return Upsert(new T[1] { entity }) == 1;
		}

		public int Upsert(IEnumerable<T> entities)
		{
			if (entities == null)
			{
				throw new ArgumentNullException("entities");
			}
			return _engine.Upsert(_collection, GetBsonDocs(entities), _autoId);
		}

		public bool Upsert(BsonValue id, T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			if (id == null || id.IsNull)
			{
				throw new ArgumentNullException("id");
			}
			BsonDocument doc = _mapper.ToDocument(entity);
			doc["_id"] = id;
			return _engine.Upsert(_collection, new BsonDocument[1] { doc }, _autoId) > 0;
		}

		internal LiteCollection(string name, BsonAutoId autoId, ILiteEngine engine, BsonMapper mapper)
		{
			_collection = name ?? mapper.ResolveCollectionName(typeof(T));
			_engine = engine;
			_mapper = mapper;
			_includes = new List<BsonExpression>();
			if (typeof(T) == typeof(BsonDocument))
			{
				_entity = null;
				_id = null;
				_autoId = autoId;
				return;
			}
			_entity = mapper.GetEntityMapper(typeof(T));
			_id = _entity.Id;
			if (_id != null && _id.AutoId)
			{
				_autoId = ((_id.DataType == typeof(int) || _id.DataType == typeof(int?)) ? BsonAutoId.Int32 : ((_id.DataType == typeof(long) || _id.DataType == typeof(long?)) ? BsonAutoId.Int64 : ((_id.DataType == typeof(Guid) || _id.DataType == typeof(Guid?)) ? BsonAutoId.Guid : BsonAutoId.ObjectId)));
			}
			else
			{
				_autoId = autoId;
			}
		}
	}
}
