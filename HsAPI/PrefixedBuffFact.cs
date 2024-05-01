using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class PrefixedBuffFact : Fact
	{
		public override FactType Type { get; } = FactType.PrefixedBuff;


		public int ApplyCount { get; init; }

		public int Buff { get; init; }

		public int Prefix { get; init; }

		public int Duration { get; init; }
	}
}
