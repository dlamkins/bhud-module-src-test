using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class AdjustByAttributeAndLevelFact : Fact
	{
		public override FactType Type { get; }

		public float Value { get; init; }

		public float LevelExponent { get; init; }

		public float LevelMultiplier { get; init; }

		public int HitCount { get; init; }

		public BaseAttribute? Attribute { get; init; }

		public float? AttributeMultiplier { get; init; }
	}
}
