using System;
using System.Collections;
using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class DocumentCacheEnumerable : IEnumerable<BsonDocument>, IEnumerable, IDisposable
	{
		private IEnumerator<BsonDocument> _enumerator;

		private readonly List<PageAddress> _cache = new List<PageAddress>();

		private readonly IDocumentLookup _lookup;

		public DocumentCacheEnumerable(IEnumerable<BsonDocument> source, IDocumentLookup lookup)
		{
			_enumerator = source.GetEnumerator();
			_lookup = lookup;
		}

		public void Dispose()
		{
			if (_enumerator != null)
			{
				while (_enumerator.MoveNext())
				{
				}
				_enumerator.Dispose();
				_enumerator = null;
			}
		}

		public IEnumerator<BsonDocument> GetEnumerator()
		{
			int index;
			for (index = 0; index < _cache.Count; index++)
			{
				PageAddress rawId = _cache[index];
				yield return _lookup.Load(rawId);
			}
			while (_enumerator != null && _enumerator.MoveNext())
			{
				BsonDocument current = _enumerator.Current;
				Constants.ENSURE(!current.RawId.IsEmpty, "rawId must have a valid value");
				_cache.Add(current.RawId);
				yield return current;
				index++;
			}
			if (_enumerator != null)
			{
				_enumerator.Dispose();
				_enumerator = null;
			}
			for (; index < _cache.Count; index++)
			{
				PageAddress rawId2 = _cache[index];
				yield return _lookup.Load(rawId2);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
