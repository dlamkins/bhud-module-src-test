using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDB
{
	internal class BsonExpressionFunctions
	{
		public static IEnumerable<BsonValue> MAP(BsonDocument root, Collation collation, BsonDocument parameters, IEnumerable<BsonValue> input, BsonExpression mapExpr)
		{
			foreach (BsonValue item in input)
			{
				IEnumerable<BsonValue> values = mapExpr.Execute(new BsonDocument[1] { root }, root, item, collation);
				foreach (BsonValue item2 in values)
				{
					yield return item2;
				}
			}
		}

		public static IEnumerable<BsonValue> FILTER(BsonDocument root, Collation collation, BsonDocument parameters, IEnumerable<BsonValue> input, BsonExpression filterExpr)
		{
			foreach (BsonValue item in input)
			{
				BsonValue c = filterExpr.ExecuteScalar(new BsonDocument[1] { root }, root, item, collation);
				if (c.IsBoolean && c.AsBoolean)
				{
					yield return item;
				}
			}
		}

		public static IEnumerable<BsonValue> SORT(BsonDocument root, Collation collation, BsonDocument parameters, IEnumerable<BsonValue> input, BsonExpression sortExpr, BsonValue order)
		{
			if ((!order.IsInt32 || order.AsInt32 <= 0) && (!order.IsString || !order.AsString.Equals("asc", StringComparison.OrdinalIgnoreCase)))
			{
				return from x in source().OrderByDescending((Tuple<BsonValue, BsonValue> x) => x.Item2, collation)
					select x.Item1;
			}
			return from x in source().OrderBy((Tuple<BsonValue, BsonValue> x) => x.Item2, collation)
				select x.Item1;
			IEnumerable<Tuple<BsonValue, BsonValue>> source()
			{
				foreach (BsonValue item in input)
				{
					BsonValue value = sortExpr.ExecuteScalar(new BsonDocument[1] { root }, root, item, collation);
					yield return new Tuple<BsonValue, BsonValue>(item, value);
				}
			}
		}

		public static IEnumerable<BsonValue> SORT(BsonDocument root, Collation collation, BsonDocument parameters, IEnumerable<BsonValue> input, BsonExpression sortExpr)
		{
			return SORT(root, collation, parameters, input, sortExpr, 1);
		}
	}
}
