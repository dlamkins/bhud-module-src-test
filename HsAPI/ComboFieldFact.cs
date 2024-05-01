using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class ComboFieldFact : Fact
	{
		public override FactType Type { get; } = FactType.ComboField;


		public ComboFieldType FieldType { get; init; }
	}
}
