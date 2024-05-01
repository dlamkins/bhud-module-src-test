using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class PercentFact : Fact
	{
		public override FactType Type { get; } = FactType.Percent;


		public float Percent { get; init; }
	}
}
