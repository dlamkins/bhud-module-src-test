using System.Collections.Generic;
using Kenedia.Modules.BuildsManager.DataModels.Professions;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class SkillDictionary : Dictionary<int, Skill>
	{
		public Skill? Get(int id)
		{
			TryGetValue(id, out var skill);
			return skill;
		}
	}
}
