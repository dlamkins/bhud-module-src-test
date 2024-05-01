using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class ConditionalSkill
	{
		public int Id { get; init; }

		public int? Spec { get; init; }
	}
}
