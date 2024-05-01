using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class StunBreakFact : Fact
	{
		public override FactType Type { get; } = FactType.StunBreak;

	}
}
