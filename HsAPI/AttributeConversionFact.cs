using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class AttributeConversionFact : Fact
	{
		public override FactType Type { get; } = FactType.AttributeConversion;


		public BaseAttribute Source { get; }

		public BaseAttribute Target { get; }

		public float Percent { get; }
	}
}
