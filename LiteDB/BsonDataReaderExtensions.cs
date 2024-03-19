using System.Collections.Generic;
using System.Linq;

namespace LiteDB
{
	public static class BsonDataReaderExtensions
	{
		public static IEnumerable<BsonValue> ToEnumerable(this IBsonDataReader reader)
		{
			try
			{
				while (reader.Read())
				{
					yield return reader.Current;
				}
			}
			finally
			{
				reader.Dispose();
			}
		}

		public static BsonValue[] ToArray(this IBsonDataReader reader)
		{
			return reader.ToEnumerable().ToArray();
		}

		public static IList<BsonValue> ToList(this IBsonDataReader reader)
		{
			return reader.ToEnumerable().ToList();
		}

		public static BsonValue First(this IBsonDataReader reader)
		{
			return reader.ToEnumerable().First();
		}

		public static BsonValue FirstOrDefault(this IBsonDataReader reader)
		{
			return reader.ToEnumerable().FirstOrDefault();
		}

		public static BsonValue Single(this IBsonDataReader reader)
		{
			return reader.ToEnumerable().Single();
		}

		public static BsonValue SingleOrDefault(this IBsonDataReader reader)
		{
			return reader.ToEnumerable().SingleOrDefault();
		}
	}
}
