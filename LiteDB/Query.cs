using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteDB
{
	public class Query
	{
		public const int Ascending = 1;

		public const int Descending = -1;

		public BsonExpression Select { get; set; } = BsonExpression.Root;


		public List<BsonExpression> Includes { get; } = new List<BsonExpression>();


		public List<BsonExpression> Where { get; } = new List<BsonExpression>();


		public BsonExpression OrderBy { get; set; }

		public int Order { get; set; } = 1;


		public BsonExpression GroupBy { get; set; }

		public BsonExpression Having { get; set; }

		public int Offset { get; set; }

		public int Limit { get; set; } = int.MaxValue;


		public bool ForUpdate { get; set; }

		public string Into { get; set; }

		public BsonAutoId IntoAutoId { get; set; } = BsonAutoId.ObjectId;


		public bool ExplainPlan { get; set; }

		public static Query All()
		{
			return new Query();
		}

		public static Query All(int order = 1)
		{
			return new Query
			{
				OrderBy = "_id",
				Order = order
			};
		}

		public static Query All(string field, int order = 1)
		{
			return new Query
			{
				OrderBy = field,
				Order = order
			};
		}

		public static BsonExpression EQ(string field, BsonValue value)
		{
			if (field.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("field");
			}
			return BsonExpression.Create($"{field} = {value ?? BsonValue.Null}");
		}

		public static BsonExpression LT(string field, BsonValue value)
		{
			if (field.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("field");
			}
			return BsonExpression.Create($"{field} < {value ?? BsonValue.Null}");
		}

		public static BsonExpression LTE(string field, BsonValue value)
		{
			if (field.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("field");
			}
			return BsonExpression.Create($"{field} <= {value ?? BsonValue.Null}");
		}

		public static BsonExpression GT(string field, BsonValue value)
		{
			if (field.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("field");
			}
			return BsonExpression.Create($"{field} > {value ?? BsonValue.Null}");
		}

		public static BsonExpression GTE(string field, BsonValue value)
		{
			if (field.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("field");
			}
			return BsonExpression.Create($"{field} >= {value ?? BsonValue.Null}");
		}

		public static BsonExpression Between(string field, BsonValue start, BsonValue end)
		{
			if (field.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("field");
			}
			return BsonExpression.Create($"{field} BETWEEN {start ?? BsonValue.Null} AND {end ?? BsonValue.Null}");
		}

		public static BsonExpression StartsWith(string field, string value)
		{
			if (field.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("field");
			}
			if (value.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("value");
			}
			return BsonExpression.Create(string.Format("{0} LIKE {1}", field, new BsonValue(value + "%")));
		}

		public static BsonExpression Contains(string field, string value)
		{
			if (field.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("field");
			}
			if (value.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("value");
			}
			return BsonExpression.Create(string.Format("{0} LIKE {1}", field, new BsonValue("%" + value + "%")));
		}

		public static BsonExpression Not(string field, BsonValue value)
		{
			if (field.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("field");
			}
			return BsonExpression.Create($"{field} != {value ?? BsonValue.Null}");
		}

		public static BsonExpression In(string field, BsonArray value)
		{
			if (field.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("field");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return BsonExpression.Create($"{field} IN {value}");
		}

		public static BsonExpression In(string field, params BsonValue[] values)
		{
			return In(field, new BsonArray(values));
		}

		public static BsonExpression In(string field, IEnumerable<BsonValue> values)
		{
			return In(field, new BsonArray(values));
		}

		public static QueryAny Any()
		{
			return new QueryAny();
		}

		public static BsonExpression And(BsonExpression left, BsonExpression right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}
			if (right == null)
			{
				throw new ArgumentNullException("right");
			}
			return "(" + left.Source + " AND " + right.Source + ")";
		}

		public static BsonExpression And(params BsonExpression[] queries)
		{
			if (queries == null || queries.Length < 2)
			{
				throw new ArgumentException("At least two Query should be passed");
			}
			BsonExpression left = queries[0];
			for (int i = 1; i < queries.Length; i++)
			{
				left = And(left, queries[i]);
			}
			return left;
		}

		public static BsonExpression Or(BsonExpression left, BsonExpression right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}
			if (right == null)
			{
				throw new ArgumentNullException("right");
			}
			return "(" + left.Source + " OR " + right.Source + ")";
		}

		public static BsonExpression Or(params BsonExpression[] queries)
		{
			if (queries == null || queries.Length < 2)
			{
				throw new ArgumentException("At least two Query should be passed");
			}
			BsonExpression left = queries[0];
			for (int i = 1; i < queries.Length; i++)
			{
				left = Or(left, queries[i]);
			}
			return left;
		}

		public string ToSQL(string collection)
		{
			StringBuilder sb = new StringBuilder();
			if (ExplainPlan)
			{
				sb.AppendLine("EXPLAIN");
			}
			sb.AppendLine("SELECT " + Select.Source);
			if (Into != null)
			{
				sb.AppendLine("INTO " + Into + ":" + IntoAutoId.ToString().ToLower());
			}
			sb.AppendLine("FROM " + collection);
			if (Includes.Count > 0)
			{
				sb.AppendLine("INCLUDE " + string.Join(", ", Includes.Select((BsonExpression x) => x.Source)));
			}
			if (Where.Count > 0)
			{
				sb.AppendLine("WHERE " + string.Join(" AND ", Where.Select((BsonExpression x) => x.Source)));
			}
			if (GroupBy != null)
			{
				sb.AppendLine("GROUP BY " + GroupBy.Source);
			}
			if (Having != null)
			{
				sb.AppendLine("HAVING " + Having.Source);
			}
			if (OrderBy != null)
			{
				sb.AppendLine("ORDER BY " + OrderBy.Source + " " + ((Order == 1) ? "ASC" : "DESC"));
			}
			if (Limit != int.MaxValue)
			{
				sb.AppendLine($"LIMIT {Limit}");
			}
			if (Offset != 0)
			{
				sb.AppendLine($"OFFSET {Offset}");
			}
			if (ForUpdate)
			{
				sb.AppendLine("FOR UPDATE");
			}
			return sb.ToString().Trim();
		}
	}
}
