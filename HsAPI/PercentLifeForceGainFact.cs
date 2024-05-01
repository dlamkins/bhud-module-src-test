using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class PercentLifeForceGainFact : Fact
	{
		public override FactType Type { get; } = FactType.PercentLifeForceGain;


		public float Percent { get; init; }
	}
}
