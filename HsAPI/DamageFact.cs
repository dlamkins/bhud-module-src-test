using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class DamageFact : Fact
	{
		public override FactType Type { get; } = FactType.Damage;


		public int HitCount { get; init; }

		public float DmgMultiplier { get; init; }
	}
}
