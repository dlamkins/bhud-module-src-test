using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class BuffBriefFact : Fact
	{
		public override FactType Type { get; } = FactType.BuffBrief;


		public int Buff { get; init; }
	}
}
