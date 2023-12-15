using System;

namespace Estreya.BlishHUD.PortalDistance.Models
{
	public class PortalDefinition
	{
		public int SkillID { get; }

		public Func<float> GetMaxDistance { get; }

		public PortalDefinition(int skillId, Func<float> getMaxDistance)
		{
			SkillID = skillId;
			GetMaxDistance = getMaxDistance;
		}
	}
}
