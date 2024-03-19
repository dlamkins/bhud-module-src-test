namespace LiteDB.Engine
{
	internal interface IDocumentLookup
	{
		BsonDocument Load(IndexNode node);

		BsonDocument Load(PageAddress rawId);
	}
}
