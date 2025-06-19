using System.Collections.Generic;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.Core.DataModels
{
	public class SkillConnection
	{
		public Enviroment Enviroment { get; set; } = Enviroment.Terrestrial;


		public AttunementType? Attunement { get; set; }

		public SkillWeaponType? Weapon { get; set; }

		public SpecializationType? Specialization { get; set; }

		public SkillSlot? Slot { get; set; }

		public int? AssetId { get; set; }

		public int Id { get; set; }

		public int? Default { get; set; }

		public int? EnvCounter { get; set; }

		public int? Stealth { get; set; }

		public int? Malicious { get; set; }

		public int? Ambush { get; set; }

		public int? Unleashed { get; set; }

		public int? Toolbelt { get; set; }

		public int? PvP { get; set; }

		public int? WvW { get; set; }

		public List<int?> Chain { get; set; }

		public List<int?> FlipSkills { get; set; }

		public Burst Burst { get; set; }

		public Bundle Bundle { get; set; }

		public Pets Pets { get; set; }

		public Traited Traited { get; set; }

		public Transform Transform { get; set; }

		public List<ProfessionType> Professions { get; set; } = new List<ProfessionType>();


		public SkillConnection()
		{
		}

		public SkillConnection(OldSkillConnection c)
		{
			Id = c.Id;
			Default = ((c.Default != c.Id) ? c.Default : null);
			Enviroment = c.Enviroment;
			Attunement = c.Attunement;
			PvP = c.Pvp;
			AssetId = c.AssetId;
			Weapon = c.Weapon;
			Specialization = c.Specialization;
			EnvCounter = c.EnviromentalCounterskill;
			Stealth = ((!(c.Stealth?.Default).HasValue) ? c.Chain?.Stealth : c.Stealth?.Default);
			Malicious = ((!(c.Stealth?.Malicious).HasValue) ? c.Chain?.Malicious : c.Stealth?.Malicious);
			Ambush = c.Chain?.Ambush;
			Unleashed = c.Chain?.Unleashed;
			if (c.Chain != null)
			{
				Chain = new List<int?>(5)
				{
					c.Chain.First,
					c.Chain.Second,
					c.Chain.Third,
					c.Chain.Fourth,
					c.Chain.Fifth
				};
				Chain.RemoveAll((int? e) => !e.HasValue);
			}
			if (c.FlipSkills != null)
			{
				FlipSkills = new List<int?>(6)
				{
					c.FlipSkills.Default,
					c.FlipSkills.State1,
					c.FlipSkills.State2,
					c.FlipSkills.State3,
					c.FlipSkills.State4,
					c.FlipSkills.State5
				};
				FlipSkills.RemoveAll((int? e) => !e.HasValue);
			}
			Transform = c.Transform;
			Burst = c.Burst;
			Bundle = c.Bundle;
			Pets = c.Pets;
			Traited = c.Traited;
		}
	}
}
