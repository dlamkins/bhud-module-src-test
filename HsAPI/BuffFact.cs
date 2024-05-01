using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class BuffFact : Fact
	{
		public override FactType Type { get; } = FactType.Buff;


		public int Buff { get; init; }

		public int ApplyCount { get; init; }

		public int Duration { get; init; }
	}
}
