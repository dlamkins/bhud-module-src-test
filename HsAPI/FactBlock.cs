using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class FactBlock
	{
		public List<int> TraitRequirements { get; init; } = new List<int>();


		public string? Description { get; init; }

		public List<Fact> Facts { get; set; } = new List<Fact>();

	}
}
