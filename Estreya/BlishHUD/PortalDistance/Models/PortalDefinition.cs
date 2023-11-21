namespace Estreya.BlishHUD.PortalDistance.Models
{
	public class PortalDefinition
	{
		public int SkillID { get; set; }

		public float MaxDistance { get; set; }

		public PortalDefinition(int skillId, float maxDistance)
		{
			SkillID = skillId;
			MaxDistance = maxDistance;
		}
	}
}
