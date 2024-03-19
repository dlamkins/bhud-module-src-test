using System;

namespace LiteDB
{
	public class BsonFieldAttribute : Attribute
	{
		public string Name { get; set; }

		public BsonFieldAttribute(string name)
		{
			Name = name;
		}

		public BsonFieldAttribute()
		{
		}
	}
}
