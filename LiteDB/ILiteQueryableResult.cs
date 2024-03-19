using System.Collections.Generic;

namespace LiteDB
{
	public interface ILiteQueryableResult<T>
	{
		ILiteQueryableResult<T> Limit(int limit);

		ILiteQueryableResult<T> Skip(int offset);

		ILiteQueryableResult<T> Offset(int offset);

		ILiteQueryableResult<T> ForUpdate();

		BsonDocument GetPlan();

		IBsonDataReader ExecuteReader();

		IEnumerable<BsonDocument> ToDocuments();

		IEnumerable<T> ToEnumerable();

		List<T> ToList();

		T[] ToArray();

		int Into(string newCollection, BsonAutoId autoId = BsonAutoId.ObjectId);

		T First();

		T FirstOrDefault();

		T Single();

		T SingleOrDefault();

		int Count();

		long LongCount();

		bool Exists();
	}
}
