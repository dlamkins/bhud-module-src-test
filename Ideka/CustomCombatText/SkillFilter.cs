using System.Collections.Generic;

namespace Ideka.CustomCombatText
{
	public class SkillFilter
	{
		public bool Blacklist { get; set; }

		public HashSet<int> SkillIds { get; set; } = new HashSet<int>();


		public bool Allows(int skillId)
		{
			return Blacklist ^ SkillIds.Contains(skillId);
		}
	}
}
