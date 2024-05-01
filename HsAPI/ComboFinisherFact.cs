using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class ComboFinisherFact : Fact
	{
		public override FactType Type { get; } = FactType.ComboFinisher;


		public ComboFinisherType FinisherType { get; init; }
	}
}
