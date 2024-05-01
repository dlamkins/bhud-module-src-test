using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class PercentLifeForceCostFact : Fact
	{
		public override FactType Type { get; } = FactType.PercentLifeForceCost;


		public float Percent { get; init; }
	}
}
