using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.Core.DataModels
{
	public class OldSkillConnection
	{
		public int Id { get; set; }

		public int? Default { get; set; }

		public int? AssetId { get; set; }

		public int? Pvp { get; set; }

		public int? EnviromentalCounterskill { get; set; }

		public Enviroment Enviroment { get; set; } = Enviroment.Terrestrial;


		public AttunementType Attunement { get; set; }

		public SkillWeaponType? Weapon { get; set; }

		public SpecializationType? Specialization { get; set; }

		public DualSkill DualSkill { get; set; }

		public Burst Burst { get; set; }

		public Stealth Stealth { get; set; }

		public Transform Transform { get; set; }

		public Bundle Bundle { get; set; }

		public int? Unleashed { get; set; }

		public int? Toolbelt { get; set; }

		public Pets Pets { get; set; }

		public Chain Chain { get; set; }

		public FlipSkills FlipSkills { get; set; }

		public Traited Traited { get; set; }

		public void Clear()
		{
			Default = null;
			EnviromentalCounterskill = null;
			Enviroment = Enviroment.Terrestrial;
			Weapon = null;
			Specialization = null;
			DualSkill = null;
			Burst = null;
			Stealth = null;
			Transform = null;
			Bundle = null;
			Unleashed = null;
			Toolbelt = null;
			Pets = null;
			Chain = null;
			FlipSkills = null;
			Traited = null;
		}
	}
}
