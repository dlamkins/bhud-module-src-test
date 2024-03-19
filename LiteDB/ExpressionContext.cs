using System.Collections.Generic;
using System.Linq.Expressions;

namespace LiteDB
{
	internal class ExpressionContext
	{
		public ParameterExpression Source { get; }

		public ParameterExpression Root { get; }

		public ParameterExpression Current { get; }

		public ParameterExpression Collation { get; }

		public ParameterExpression Parameters { get; }

		public ExpressionContext()
		{
			Source = Expression.Parameter(typeof(IEnumerable<BsonDocument>), "source");
			Root = Expression.Parameter(typeof(BsonDocument), "root");
			Current = Expression.Parameter(typeof(BsonValue), "current");
			Collation = Expression.Parameter(typeof(Collation), "collation");
			Parameters = Expression.Parameter(typeof(BsonDocument), "parameters");
		}
	}
}
