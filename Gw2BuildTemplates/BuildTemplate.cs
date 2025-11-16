using System.Collections.Generic;
using Gw2Sharp.Models;

namespace Gw2BuildTemplates
{
	public sealed class BuildTemplate
	{
		public ProfessionType Profession { get; set; }

		public SpecializationEntry[] Specializations { get; set; } = new SpecializationEntry[3]
		{
			new SpecializationEntry(),
			new SpecializationEntry(),
			new SpecializationEntry()
		};


		public ushort TerrestrialHeal { get; set; }

		public ushort AquaticHeal { get; set; }

		public ushort TerrestrialUtility1 { get; set; }

		public ushort AquaticUtility1 { get; set; }

		public ushort TerrestrialUtility2 { get; set; }

		public ushort AquaticUtility2 { get; set; }

		public ushort TerrestrialUtility3 { get; set; }

		public ushort AquaticUtility3 { get; set; }

		public ushort TerrestrialElite { get; set; }

		public ushort AquaticElite { get; set; }

		public RangerPetData RangerPets { get; set; }

		public RevenantLegendData RevenantLegends { get; set; }

		public List<TemplateWeaponType> SelectedWeapons { get; set; } = new List<TemplateWeaponType>();


		public List<uint> SkillOverrides { get; set; } = new List<uint>();

	}
}
