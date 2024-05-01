using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class DistanceFact : Fact
	{
		public override FactType Type { get; } = FactType.Distance;


		public float Distance { get; init; }
	}
}
