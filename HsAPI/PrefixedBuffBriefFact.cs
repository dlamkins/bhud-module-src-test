using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class PrefixedBuffBriefFact : Fact
	{
		public override FactType Type { get; } = FactType.PrefixedBuffBrief;


		public int Buff { get; init; }

		public int Prefix { get; init; }
	}
}
