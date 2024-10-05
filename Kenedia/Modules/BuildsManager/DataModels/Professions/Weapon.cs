using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Gw2Sharp;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.BuildsManager.DataModels.Professions
{
	[DataContract]
	public class Weapon : IDisposable
	{
		[Flags]
		public enum WieldingFlag
		{
			Unknown = 0x0,
			Mainhand = 0x1,
			TwoHand = 0x2,
			Offhand = 0x4,
			Aquatic = 0x8
		}

		public enum WeaponType
		{
			Unknown = 0,
			None = 1,
			Axe = 2,
			Dagger = 3,
			Mace = 4,
			Pistol = 5,
			Scepter = 6,
			Sword = 7,
			Focus = 8,
			Shield = 9,
			Torch = 10,
			Warhorn = 11,
			Greatsword = 12,
			Hammer = 13,
			Longbow = 14,
			Rifle = 0xF,
			Shortbow = 0x10,
			Staff = 17,
			Harpoon = 18,
			Speargun = 19,
			Trident = 20,
			LongBow = 14,
			ShortBow = 0x10,
			Spear = 18
		}

		private bool _isDisposed;

		[DataMember]
		public WieldingFlag Wielded { get; set; }

		[DataMember]
		public WeaponType Type { get; set; }

		[DataMember]
		public int Specialization { get; set; }

		[DataMember]
		public WieldingFlag? SpecializationWielded { get; set; }

		[DataMember]
		public List<int> Skills { get; set; } = new List<int>();


		public Weapon()
		{
		}

		public Weapon(KeyValuePair<string, ProfessionWeapon> weapon)
		{
			if (!Enum.TryParse<WeaponType>(weapon.Key, out var weaponType))
			{
				return;
			}
			Type = weaponType;
			foreach (ApiEnum<ProfessionWeaponFlag> flag in weapon.Value.Flags)
			{
				if (Enum.TryParse<WieldingFlag>(flag.Value.ToString(), out var wielded))
				{
					Wielded |= wielded;
				}
			}
			Specialization = weapon.Value.Specialization;
			Skills = new List<int>(weapon.Value.Skills.Select((ProfessionWeaponSkill e) => e.Id));
		}

		public Weapon(KeyValuePair<string, ProfessionWeapon> weapon, Dictionary<int, Skill> skills)
			: this(weapon)
		{
			if (!Enum.TryParse<WeaponType>(weapon.Key, out var _))
			{
				return;
			}
			foreach (ProfessionWeaponSkill s in weapon.Value.Skills)
			{
				if (skills.TryGetValue(s.Id, out var skill))
				{
					Skills.Add(skill.Id);
				}
			}
		}

		public void ApplyLanguage(Dictionary<int, Skill> skills)
		{
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
			}
		}
	}
}
