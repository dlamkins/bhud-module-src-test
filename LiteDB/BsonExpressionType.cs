namespace LiteDB
{
	public enum BsonExpressionType : byte
	{
		Double = 1,
		Int,
		String,
		Boolean,
		Null,
		Array,
		Document,
		Parameter,
		Call,
		Path,
		Modulo,
		Add,
		Subtract,
		Multiply,
		Divide,
		Equal,
		Like,
		Between,
		GreaterThan,
		GreaterThanOrEqual,
		LessThan,
		LessThanOrEqual,
		NotEqual,
		In,
		Or,
		And,
		Map,
		Filter,
		Sort,
		Source
	}
}
