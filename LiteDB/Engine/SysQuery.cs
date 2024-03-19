using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class SysQuery : SystemCollection
	{
		private readonly ILiteEngine _engine;

		public SysQuery(ILiteEngine engine)
			: base("$query")
		{
			_engine = engine;
		}

		public override IEnumerable<BsonDocument> Input(BsonValue options)
		{
			string query = options?.AsString ?? throw new LiteException(0, "Collection $query(sql) requires `sql` string parameter");
			SqlParser sql = new SqlParser(_engine, new Tokenizer(query), null);
			using IBsonDataReader reader = sql.Execute();
			while (reader.Read())
			{
				BsonValue value = reader.Current;
				yield return value.IsDocument ? value.AsDocument : new BsonDocument { ["expr"] = value };
			}
		}
	}
}
