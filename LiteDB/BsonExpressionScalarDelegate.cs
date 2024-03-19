using System.Collections.Generic;

namespace LiteDB
{
	internal delegate BsonValue BsonExpressionScalarDelegate(IEnumerable<BsonDocument> source, BsonDocument root, BsonValue current, Collation collation, BsonDocument parameters);
}
