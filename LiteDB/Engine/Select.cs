namespace LiteDB.Engine
{
	internal class Select
	{
		public BsonExpression Expression { get; }

		public bool All { get; }

		public Select(BsonExpression expression, bool all)
		{
			Expression = expression;
			All = all;
		}
	}
}
