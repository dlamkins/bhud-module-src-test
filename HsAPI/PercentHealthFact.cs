using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class PercentHealthFact : Fact
	{
		public override FactType Type { get; } = FactType.PercentHealth;


		public float Percent { get; init; }
	}
}
