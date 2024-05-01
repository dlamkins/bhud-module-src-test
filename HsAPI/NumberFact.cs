using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class NumberFact : Fact
	{
		public override FactType Type { get; } = FactType.Number;


		public float Value { get; init; }
	}
}
