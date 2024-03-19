using System.Diagnostics;

namespace LiteDB.Engine
{
	internal class CursorInfo
	{
		public string Collection { get; }

		public Query Query { get; set; }

		public int Fetched { get; set; }

		public Stopwatch Elapsed { get; } = new Stopwatch();


		public CursorInfo(string collection, Query query)
		{
			Collection = collection;
			Query = query;
		}
	}
}
