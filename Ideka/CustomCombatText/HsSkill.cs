using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ideka.CustomCombatText
{
	public class HsSkill
	{
		[JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
		public uint Id { get; set; }

		[JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
		public string Name { get; set; }

		[JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
		public string? Icon { get; set; }

		public HsSkill(uint id, string name)
		{
			Id = id;
			Name = name;
			base._002Ector();
		}
	}
}
