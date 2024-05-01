using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class TransformationAttributes
	{
		public float Power { get; init; }

		public float Precision { get; init; }

		public float Toughness { get; init; }

		public AttributesType Type { get; init; }

		public float Vitality { get; init; }
	}
}
