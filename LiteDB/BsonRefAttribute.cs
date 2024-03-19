using System;

namespace LiteDB
{
	public class BsonRefAttribute : Attribute
	{
		public string Collection { get; set; }

		public BsonRefAttribute(string collection)
		{
			Collection = collection;
		}

		public BsonRefAttribute()
		{
			Collection = null;
		}
	}
}
