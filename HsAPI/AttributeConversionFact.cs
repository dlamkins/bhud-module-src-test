using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class AttributeConversionFact : Fact
	{
		public override FactType Type { get; } = FactType.AttributeConversion;


		public BaseAttribute Source { get; init; }

		public BaseAttribute Target { get; init; }

		public float Percent { get; init; }
	}
}
