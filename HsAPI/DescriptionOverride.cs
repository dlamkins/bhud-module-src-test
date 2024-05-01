using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class DescriptionOverride
	{
		public ProfessionId Profession { get; init; }

		public string Description { get; init; } = "";

	}
}
