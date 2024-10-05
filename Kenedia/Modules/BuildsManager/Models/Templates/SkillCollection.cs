using System;
using System.Collections.Generic;
using System.Linq;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.Core.DataModels;
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

		public bool HasSkill(int skillid, SkillSlotType state_enviroment)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<SkillSlotType, Skill> s = enumerator.Current;
					if (s.Key.HasFlag(state_enviroment))
					{
						Skill value = s.Value;
						if (value != null && value.Id == skillid)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public SkillSlotType GetSkillSlot(int skillid, SkillSlotType state_enviroment)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<SkillSlotType, Skill> s = enumerator.Current;
					if (s.Key.HasFlag(state_enviroment))
					{
						Skill value = s.Value;
						if (value != null && value.Id == skillid)
						{
							return s.Key;
						}
					}
				}
			}
			return SkillSlotType.Utility_1;
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

		public void SelectSkill(Skill skill, SkillSlotType targetSlot, Skill previousSkill = null)
		{
			SkillSlotType enviromentState = targetSlot.GetEnviromentState();
			if (HasSkill(skill, enviromentState))
			{
				SkillSlotType slot = GetSkillSlot(skill, enviromentState);
				base[slot] = previousSkill;
			}
			base[targetSlot] = skill;
		}

		public bool WipeInvalidRacialSkills(Races race)
		{
			bool wiped = false;
			List<int> invalidIds = new List<int>();
			foreach (Races r in Enum.GetValues(typeof(Races)))
			{
				if (r != Races.None && r != race && BuildsManager.Data.Races.TryGetValue(r, out var skillRace))
				{
					invalidIds.AddRange(skillRace?.Skills.Select<KeyValuePair<int, Skill>, int>((KeyValuePair<int, Skill> e) => e.Value.Id));
				}
			}
			foreach (SkillSlotType key in base.Keys.ToList())
			{
				if (invalidIds.Contains(base[key]?.Id ?? 0))
				{
					wiped = true;
					base[key] = null;
				}
			}
			return wiped;
		}

		public bool WipeSkills(Races? race)
		{
			bool wiped = false;
			Dictionary<int, Skill> racials = ((!race.HasValue) ? null : BuildsManager.Data?.Races?[race.Value]?.Skills);
			foreach (SkillSlotType key in base.Keys.ToList())
			{
				if (!race.HasValue || (racials != null && !racials.ContainsKey(base[key]?.Id ?? 0)))
				{
					wiped = true;
					base[key] = null;
				}
			}
			return wiped;
		}
	}
}
