using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class PercentHpSelfDamageFact : Fact
	{
		public override FactType Type { get; } = FactType.PercentHpSelfDamage;


		public float Percent { get; init; }
	}
}
