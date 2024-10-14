using System;
using System.Collections.Generic;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.BuildsManager.Models.Templates
{
	public class SkillCollection : ObservableDictionary<SkillSlotType, Skill>
	{
		public SkillCollection()
		{
			foreach (SkillSlotType slot in Enum.GetValues(typeof(SkillSlotType)))
			{
				if (slot < SkillSlotType.Heal)
				{
					continue;
				}
				SkillSlotType[] array = new SkillSlotType[2]
				{
					SkillSlotType.Active,
					SkillSlotType.Inactive
				};
				foreach (SkillSlotType state in array)
				{
					SkillSlotType[] array2 = new SkillSlotType[2]
					{
						SkillSlotType.Terrestrial,
						SkillSlotType.Aquatic
					};
					foreach (SkillSlotType enviroment in array2)
					{
						Add(state | enviroment | slot, new Skill());
					}
				}
			}
		}

		public ushort GetPaletteId(SkillSlotType slot)
		{
			Skill skill;
			return (ushort)((TryGetValue(slot, out skill) && skill != null) ? ((uint)skill.PaletteId) : 0u);
		}

		public bool HasSkill(Skill skill, SkillSlotType state_enviroment)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<SkillSlotType, Skill> s = enumerator.Current;
					if (s.Key.HasFlag(state_enviroment) && s.Value == skill)
					{
						return true;
					}
				}
			}
			return false;
		}

		public SkillSlotType GetSkillSlot(Skill skill, SkillSlotType state_enviroment)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<SkillSlotType, Skill> s = enumerator.Current;
					if (s.Key.HasFlag(state_enviroment) && s.Value == skill)
					{
						return s.Key;
					}
				}
			}
			return SkillSlotType.Utility_1;
		}
	}
}
