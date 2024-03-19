using System;

namespace LiteDB
{
	public class BsonIdAttribute : Attribute
	{
		public bool AutoId { get; private set; }

		public BsonIdAttribute()
		{
			AutoId = true;
		}

		public BsonIdAttribute(bool autoId)
		{
			AutoId = autoId;
		}
	}
}
