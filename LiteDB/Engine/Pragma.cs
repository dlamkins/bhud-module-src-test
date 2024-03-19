using System;

namespace LiteDB.Engine
{
	internal class Pragma
	{
		public string Name { get; set; }

		public Func<BsonValue> Get { get; set; }

		public Action<BsonValue> Set { get; set; }

		public Action<BufferSlice> Read { get; set; }

		public Action<BsonValue, HeaderPage> Validate { get; set; }

		public Action<BufferSlice> Write { get; set; }
	}
}
