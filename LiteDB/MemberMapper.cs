using System;

namespace LiteDB
{
	public class MemberMapper
	{
		public bool AutoId { get; set; }

		public string MemberName { get; set; }

		public Type DataType { get; set; }

		public string FieldName { get; set; }

		public GenericGetter Getter { get; set; }

		public GenericSetter Setter { get; set; }

		public Func<object, BsonMapper, BsonValue> Serialize { get; set; }

		public Func<BsonValue, BsonMapper, object> Deserialize { get; set; }

		public bool IsDbRef { get; set; }

		public bool IsEnumerable { get; set; }

		public Type UnderlyingType { get; set; }
	}
}
