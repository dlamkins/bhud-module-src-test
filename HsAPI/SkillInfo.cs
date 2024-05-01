using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class SkillInfo
	{
		public int Skill { get; init; }

		public int? MinLevel { get; init; }

		public float? Probability { get; init; }

		public SkillUsability[] Usability { get; init; } = Array.Empty<SkillUsability>();


		public WeaponType? WeaponMainhand { get; init; }

		public WeaponType? WeaponOffhand { get; init; }

		public ProfessionState? ProfessionState { get; init; }

		public ProfessionState? ProfessionState2 { get; init; }

		public int? Specialization { get; init; }

		public int? Trait { get; init; }

		public int? Buff { get; init; }

		public int? PreviousChainSkillIndex { get; init; }
	}
}
