using System.Linq;

namespace LiteDB.Engine
{
	internal class IndexCost
	{
		public uint Cost { get; }

		public BsonExpression Expression { get; }

		public string IndexExpression { get; }

		public Index Index { get; }

		public IndexCost(CollectionIndex index, BsonExpression expr, BsonExpression value, Collation collation)
		{
			IndexCost indexCost = this;
			IndexExpression = index.Expression;
			Expression = expr;
			BsonExpressionType exprType = expr.Type;
			if (expr.Left.IsValue)
			{
				switch (expr.Type)
				{
				case BsonExpressionType.GreaterThan:
					exprType = BsonExpressionType.LessThan;
					break;
				case BsonExpressionType.GreaterThanOrEqual:
					exprType = BsonExpressionType.LessThanOrEqual;
					break;
				case BsonExpressionType.LessThan:
					exprType = BsonExpressionType.GreaterThan;
					break;
				case BsonExpressionType.LessThanOrEqual:
					exprType = BsonExpressionType.GreaterThanOrEqual;
					break;
				}
			}
			Index = (from x in value.Execute(collation)
				select indexCost.CreateIndex(exprType, index.Name, x)).FirstOrDefault();
			Constants.ENSURE(Index != null, "index must be not null");
			Cost = Index.GetCost(index);
		}

		public IndexCost(CollectionIndex index)
		{
			Expression = BsonExpression.Create(index.Expression);
			Index = new IndexAll(index.Name, 1);
			Cost = Index.GetCost(index);
			IndexExpression = index.Expression;
		}

		private Index CreateIndex(BsonExpressionType type, string name, BsonValue value)
		{
			switch (type)
			{
			case BsonExpressionType.Equal:
				return new IndexEquals(name, value);
			case BsonExpressionType.Between:
				return new IndexRange(name, value.AsArray[0], value.AsArray[1], startEquals: true, endEquals: true, 1);
			case BsonExpressionType.Like:
				return new IndexLike(name, value.AsString, 1);
			case BsonExpressionType.GreaterThan:
				return new IndexRange(name, value, BsonValue.MaxValue, startEquals: false, endEquals: true, 1);
			case BsonExpressionType.GreaterThanOrEqual:
				return new IndexRange(name, value, BsonValue.MaxValue, startEquals: true, endEquals: true, 1);
			case BsonExpressionType.LessThan:
				return new IndexRange(name, BsonValue.MinValue, value, startEquals: true, endEquals: false, 1);
			case BsonExpressionType.LessThanOrEqual:
				return new IndexRange(name, BsonValue.MinValue, value, startEquals: true, endEquals: true, 1);
			case BsonExpressionType.NotEqual:
				return new IndexScan(name, (BsonValue x) => x.CompareTo(value) != 0, 1);
			case BsonExpressionType.In:
				if (!value.IsArray)
				{
					return new IndexEquals(name, value);
				}
				return new IndexIn(name, value.AsArray, 1);
			default:
				return null;
			}
		}
	}
}
