using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class TimeFact : Fact
	{
		public override FactType Type { get; } = FactType.Time;


		public int Duration { get; init; }
	}
}
