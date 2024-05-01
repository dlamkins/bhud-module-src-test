using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HsAPI
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class SlotGroup
	{
		public ProfessionId? Profession { get; init; }

		public SkillSlotType Slot { get; init; }

		public SkillInfo[] Candidates { get; init; } = Array.Empty<SkillInfo>();

	}
}
