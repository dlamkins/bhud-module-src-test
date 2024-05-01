using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class NoDataFact : Fact
	{
		public override FactType Type { get; } = FactType.NoData;

	}
}
