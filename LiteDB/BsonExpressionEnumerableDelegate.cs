using System.Collections.Generic;

namespace LiteDB
{
	internal delegate IEnumerable<BsonValue> BsonExpressionEnumerableDelegate(IEnumerable<BsonDocument> source, BsonDocument root, BsonValue current, Collation collation, BsonDocument parameters);
}
