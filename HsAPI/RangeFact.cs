using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class RangeFact : Fact
	{
		public override FactType Type { get; } = FactType.Range;


		public float? Min { get; init; }

		public float Max { get; init; }
	}
}
