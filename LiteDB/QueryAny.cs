using System;

namespace LiteDB
{
	public class QueryAny
	{
		public BsonExpression EQ(string arrayField, BsonValue value)
		{
			if (arrayField.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("arrayField");
			}
			return BsonExpression.Create($"{arrayField} ANY = {value ?? BsonValue.Null}");
		}

		public BsonExpression LT(string arrayField, BsonValue value)
		{
			if (arrayField.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("arrayField");
			}
			return BsonExpression.Create($"{arrayField} ANY < {value ?? BsonValue.Null}");
		}

		public BsonExpression LTE(string arrayField, BsonValue value)
		{
			if (arrayField.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("arrayField");
			}
			return BsonExpression.Create($"{arrayField} ANY <= {value ?? BsonValue.Null}");
		}

		public BsonExpression GT(string arrayField, BsonValue value)
		{
			if (arrayField.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("arrayField");
			}
			return BsonExpression.Create($"{arrayField} ANY > {value ?? BsonValue.Null}");
		}

		public BsonExpression GTE(string arrayField, BsonValue value)
		{
			if (arrayField.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("arrayField");
			}
			return BsonExpression.Create($"{arrayField} ANY >= {value ?? BsonValue.Null}");
		}

		public BsonExpression Between(string arrayField, BsonValue start, BsonValue end)
		{
			if (arrayField.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("arrayField");
			}
			return BsonExpression.Create($"{arrayField} ANY BETWEEN {start ?? BsonValue.Null} AND {end ?? BsonValue.Null}");
		}

		public BsonExpression StartsWith(string arrayField, string value)
		{
			if (arrayField.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("arrayField");
			}
			if (value.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("value");
			}
			return BsonExpression.Create(string.Format("{0} ANY LIKE {1}", arrayField, new BsonValue(value + "%")));
		}

		public BsonExpression Not(string arrayField, BsonValue value)
		{
			if (arrayField.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("arrayField");
			}
			return BsonExpression.Create($"{arrayField} ANY != {value ?? BsonValue.Null}");
		}
	}
}
