using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.Core.DataModels
{
	public class SkillConnection
	{
		public int Id { get; set; }

		public int? Default { get; set; }

		public int? AssetId { get; set; }

		public int? EnviromentalCounterskill { get; set; }

		public Enviroment Enviroment { get; set; } = Enviroment.Terrestrial;


		public SkillWeaponType? Weapon { get; set; }

		public Specializations? Specialization { get; set; }

		public DualSkill DualSkill { get; set; }

		public AttunementSkill AttunementSkill { get; set; }

		public Burst Burst { get; set; }

		public Stealth Stealth { get; set; }

		public Transform Transform { get; set; }

		public Bundle Bundle { get; set; }

		public int? Unleashed { get; set; }

		public int? Toolbelt { get; set; }

		public List<int> Pets { get; set; }

		public Chain Chain { get; set; }

		public FlipSkills FlipSkills { get; set; }

		public Dictionary<int, List<int>> Traited { get; set; }

		public void Clear()
		{
			Default = null;
			EnviromentalCounterskill = null;
			Enviroment = Enviroment.Terrestrial;
			Weapon = null;
			Specialization = null;
			DualSkill = null;
			AttunementSkill = null;
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
