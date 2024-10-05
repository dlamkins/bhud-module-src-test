using System;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models.Templates;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class SkillChangedEventArgs : EventArgs
	{
		public SkillSlotType Slot { get; set; }

		public Skill? Skill { get; set; }

		public Skill? OldSkill { get; set; }

		public SkillChangedEventArgs(SkillSlotType slot, Skill? skill, Skill? oldSkill = null)
		{
			Slot = slot;
			Skill = skill;
			OldSkill = oldSkill;
		}
	}
}
