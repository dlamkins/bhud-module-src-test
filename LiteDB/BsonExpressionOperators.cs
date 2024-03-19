using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDB
{
	internal class BsonExpressionOperators
	{
		public static BsonValue ADD(BsonValue left, BsonValue right)
		{
			if (left.IsString && right.IsString)
			{
				return left.AsString + right.AsString;
			}
			if (left.IsString || right.IsString)
			{
				return BsonExpressionMethods.STRING(left).AsString + BsonExpressionMethods.STRING(right).AsString;
			}
			if (left.IsDateTime && right.IsNumber)
			{
				return left.AsDateTime.AddTicks(right.AsInt64);
			}
			if (left.IsNumber && right.IsDateTime)
			{
				return right.AsDateTime.AddTicks(left.AsInt64);
			}
			if (left.IsNumber && right.IsNumber)
			{
				return left + right;
			}
			return BsonValue.Null;
		}

		public static BsonValue MINUS(BsonValue left, BsonValue right)
		{
			if (left.IsDateTime && right.IsNumber)
			{
				return left.AsDateTime.AddTicks(-right.AsInt64);
			}
			if (left.IsNumber && right.IsDateTime)
			{
				return right.AsDateTime.AddTicks(-left.AsInt64);
			}
			if (left.IsNumber && right.IsNumber)
			{
				return left - right;
			}
			return BsonValue.Null;
		}

		public static BsonValue MULTIPLY(BsonValue left, BsonValue right)
		{
			if (left.IsNumber && right.IsNumber)
			{
				return left * right;
			}
			return BsonValue.Null;
		}

		public static BsonValue DIVIDE(BsonValue left, BsonValue right)
		{
			if (left.IsNumber && right.IsNumber)
			{
				return left / right;
			}
			return BsonValue.Null;
		}

		public static BsonValue MOD(BsonValue left, BsonValue right)
		{
			if (left.IsNumber && right.IsNumber)
			{
				return (int)left % (int)right;
			}
			return BsonValue.Null;
		}

		public static BsonValue EQ(Collation collation, BsonValue left, BsonValue right)
		{
			return collation.Equals(left, right);
		}

		public static BsonValue EQ_ALL(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.All((BsonValue x) => collation.Equals(x, right));
		}

		public static BsonValue EQ_ANY(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.Any((BsonValue x) => collation.Equals(x, right));
		}

		public static BsonValue GT(BsonValue left, BsonValue right)
		{
			return left > right;
		}

		public static BsonValue GT_ANY(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.Any((BsonValue x) => collation.Compare(x, right) > 0);
		}

		public static BsonValue GT_ALL(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.All((BsonValue x) => collation.Compare(x, right) > 0);
		}

		public static BsonValue GTE(BsonValue left, BsonValue right)
		{
			return left >= right;
		}

		public static BsonValue GTE_ANY(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.Any((BsonValue x) => collation.Compare(x, right) >= 0);
		}

		public static BsonValue GTE_ALL(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.All((BsonValue x) => collation.Compare(x, right) >= 0);
		}

		public static BsonValue LT(BsonValue left, BsonValue right)
		{
			return left < right;
		}

		public static BsonValue LT_ANY(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.Any((BsonValue x) => collation.Compare(x, right) < 0);
		}

		public static BsonValue LT_ALL(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.All((BsonValue x) => collation.Compare(x, right) < 0);
		}

		public static BsonValue LTE(Collation collation, BsonValue left, BsonValue right)
		{
			return left <= right;
		}

		public static BsonValue LTE_ANY(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.Any((BsonValue x) => collation.Compare(x, right) <= 0);
		}

		public static BsonValue LTE_ALL(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.All((BsonValue x) => collation.Compare(x, right) <= 0);
		}

		public static BsonValue NEQ(Collation collation, BsonValue left, BsonValue right)
		{
			return !collation.Equals(left, right);
		}

		public static BsonValue NEQ_ANY(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.Any((BsonValue x) => !collation.Equals(x, right));
		}

		public static BsonValue NEQ_ALL(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.All((BsonValue x) => !collation.Equals(x, right));
		}

		public static BsonValue LIKE(Collation collation, BsonValue left, BsonValue right)
		{
			if (left.IsString && right.IsString)
			{
				return left.AsString.SqlLike(right.AsString, collation);
			}
			return false;
		}

		public static BsonValue LIKE_ANY(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.Any((BsonValue x) => LIKE(collation, x, right));
		}

		public static BsonValue LIKE_ALL(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.All((BsonValue x) => LIKE(collation, x, right));
		}

		public static BsonValue BETWEEN(Collation collation, BsonValue left, BsonValue right)
		{
			if (!right.IsArray)
			{
				throw new InvalidOperationException("BETWEEN expression need an array with 2 values");
			}
			BsonArray asArray = right.AsArray;
			if (asArray.Count != 2)
			{
				throw new InvalidOperationException("BETWEEN expression need an array with 2 values");
			}
			BsonValue start = asArray[0];
			BsonValue end = asArray[1];
			return collation.Compare(left, start) >= 0 && collation.Compare(left, end) <= 0;
		}

		public static BsonValue BETWEEN_ANY(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.Any((BsonValue x) => BETWEEN(collation, x, right));
		}

		public static BsonValue BETWEEN_ALL(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.All((BsonValue x) => BETWEEN(collation, x, right));
		}

		public static BsonValue IN(Collation collation, BsonValue left, BsonValue right)
		{
			if (right.IsArray)
			{
				return right.AsArray.Contains(left, collation);
			}
			return left == right;
		}

		public static BsonValue IN_ANY(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.Any((BsonValue x) => IN(collation, x, right));
		}

		public static BsonValue IN_ALL(Collation collation, IEnumerable<BsonValue> left, BsonValue right)
		{
			return left.All((BsonValue x) => IN(collation, x, right));
		}

		public static BsonValue PARAMETER_PATH(BsonDocument doc, string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return doc;
			}
			if (doc.TryGetValue(name, out var item))
			{
				return item;
			}
			return BsonValue.Null;
		}

		public static BsonValue MEMBER_PATH(BsonValue value, string name)
		{
			if (value == null)
			{
				throw new LiteException(0, "Field '" + name + "' is invalid in the select list because it is not contained in either an aggregate function or the GROUP BY clause.");
			}
			if (string.IsNullOrEmpty(name))
			{
				return value;
			}
			if (value.IsDocument && value.AsDocument.TryGetValue(name, out var item))
			{
				return item;
			}
			return BsonValue.Null;
		}

		public static BsonValue ARRAY_INDEX(BsonValue value, int index, BsonExpression expr, BsonDocument root, Collation collation, BsonDocument parameters)
		{
			if (!value.IsArray)
			{
				return BsonValue.Null;
			}
			BsonArray arr = value.AsArray;
			if (expr.Type == BsonExpressionType.Parameter)
			{
				BsonValue bsonValue = expr.ExecuteScalar(root, collation);
				if (!bsonValue.IsNumber)
				{
					throw new LiteException(0, "Parameter expression must return number when called inside an array");
				}
				index = bsonValue.AsInt32;
			}
			int idx = ((index < 0) ? (arr.Count + index) : index);
			if (arr.Count > idx)
			{
				return arr[idx];
			}
			return BsonValue.Null;
		}

		public static IEnumerable<BsonValue> ARRAY_FILTER(BsonValue value, int index, BsonExpression filterExpr, BsonDocument root, Collation collation, BsonDocument parameters)
		{
			if (!value.IsArray)
			{
				yield break;
			}
			BsonArray arr = value.AsArray;
			if (index == int.MaxValue)
			{
				foreach (BsonValue item2 in arr)
				{
					yield return item2;
				}
				yield break;
			}
			foreach (BsonValue item in arr)
			{
				BsonValue c = filterExpr.ExecuteScalar(new BsonDocument[1] { root }, root, item, collation);
				if (c.IsBoolean && c.AsBoolean)
				{
					yield return item;
				}
			}
		}

		public static BsonValue DOCUMENT_INIT(string[] keys, BsonValue[] values)
		{
			Constants.ENSURE(keys.Length == values.Length, "both keys/value must contains same length");
			if (keys.Length == 1 && keys[0][0] == '$' && values[0].IsString)
			{
				string text = keys[0];
				if (text != null)
				{
					switch (text.Length)
					{
					case 5:
						switch (text[1])
						{
						case 'g':
							if (!(text == "$guid"))
							{
								break;
							}
							return BsonExpressionMethods.GUID(values[0]);
						case 'd':
							if (!(text == "$date"))
							{
								break;
							}
							return BsonExpressionMethods.DATE(Collation.Binary, values[0]);
						}
						break;
					case 9:
						switch (text[2])
						{
						case 'i':
							if (!(text == "$minValue"))
							{
								break;
							}
							return BsonExpressionMethods.MINVALUE();
						case 'a':
							if (!(text == "$maxValue"))
							{
								break;
							}
							return BsonExpressionMethods.MAXVALUE();
						}
						break;
					case 7:
						if (!(text == "$binary"))
						{
							break;
						}
						return BsonExpressionMethods.BINARY(values[0]);
					case 4:
						if (!(text == "$oid"))
						{
							break;
						}
						return BsonExpressionMethods.OBJECTID(values[0]);
					case 11:
						if (!(text == "$numberLong"))
						{
							break;
						}
						return BsonExpressionMethods.LONG(values[0]);
					case 14:
						if (!(text == "$numberDecimal"))
						{
							break;
						}
						return BsonExpressionMethods.DECIMAL(Collation.Binary, values[0]);
					}
				}
			}
			BsonDocument doc = new BsonDocument();
			for (int i = 0; i < keys.Length; i++)
			{
				doc[keys[i]] = values[i];
			}
			return doc;
		}

		public static BsonValue ARRAY_INIT(BsonValue[] values)
		{
			return new BsonArray(values);
		}
	}
}
