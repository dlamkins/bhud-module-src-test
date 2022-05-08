using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager
{
	public class API
	{
		public enum traitType
		{
			Minor = 1,
			Major
		}

		public enum skillSlot
		{
			Weapon_1 = 1,
			Weapon_2,
			Weapon_3,
			Weapon_4,
			Weapon_5,
			Profession_1,
			Profession_2,
			Profession_3,
			Profession_4,
			Profession_5,
			Heal,
			Utility,
			Elite
		}

		public enum armorSlot
		{
			Helm,
			Shoulders,
			Coat,
			Gloves,
			Leggings,
			Boots
		}

		public enum weaponHand
		{
			Mainhand,
			TwoHand,
			DualWielded,
			Offhand,
			Aquatic
		}

		public enum weaponSlot
		{
			Axe = 2,
			Dagger = 2,
			Mace = 2,
			Pistol = 2,
			Scepter = 0,
			Sword = 2,
			Focus = 3,
			Shield = 3,
			Torch = 3,
			Warhorn = 3,
			Greatsword = 1,
			Hammer = 1,
			Longbow = 1,
			Rifle = 1,
			Shortbow = 1,
			Staff = 1,
			Harpoon = 4,
			Speargun = 4,
			Trident = 4
		}

		public enum trinketType
		{
			Back,
			Amulet,
			Accessory,
			Ring
		}

		public enum armorWeight
		{
			Heavy = 1,
			Medium,
			Light
		}

		public enum upgradeType
		{
			Rune = 3,
			Sigil
		}

		public enum weaponType
		{
			Unkown = -1,
			Axe = 0,
			Dagger = 1,
			Mace = 2,
			Pistol = 3,
			Scepter = 4,
			Sword = 5,
			Focus = 6,
			Shield = 7,
			Torch = 8,
			Warhorn = 9,
			Greatsword = 10,
			Hammer = 11,
			Longbow = 12,
			Rifle = 13,
			Shortbow = 14,
			Staff = 0xF,
			Harpoon = 0x10,
			Speargun = 17,
			Trident = 18,
			Spear = 0x10,
			ShortBow = 14,
			LongBow = 12
		}

		public class Icon
		{
			public string Path;

			public string Url;

			public Texture2D Texture;

			public bool Loaded;
		}

		public class Item
		{
			public string Name;

			public int Id;

			public Icon Icon;

			public string ChatLink;
		}

		public class EquipmentItem : Item
		{
			public double AttributeAdjustment;
		}

		public class ArmorItem : EquipmentItem
		{
			public armorSlot Slot;

			public armorWeight ArmorWeight;
		}

		public class WeaponItem : EquipmentItem
		{
			public weaponType WeaponType;

			public weaponSlot Slot;
		}

		public class TrinketItem : EquipmentItem
		{
			public trinketType TrinketType;
		}

		public class RuneItem : Item
		{
			public upgradeType Type = upgradeType.Rune;

			public List<string> Bonuses;
		}

		public class SigilItem : Item
		{
			public upgradeType Type = upgradeType.Sigil;

			public string Description;
		}

		public class Skill
		{
			public string Name;

			public int Id;

			public int Specialization;

			public int PaletteId;

			public Icon Icon;

			public string ChatLink;

			public string Description;

			public skillSlot Slot;

			public List<string> Flags;

			public List<string> Categories;
		}

		public class Trait
		{
			public string Name;

			public int Id;

			public Icon Icon;

			public int Specialization;

			public string Description;

			public int Tier;

			public int Order;

			public traitType Type;
		}

		public class Specialization
		{
			public string Name;

			public int Id;

			public Icon Icon;

			public Icon Background;

			public Icon ProfessionIcon;

			public Icon ProfessionIconBig;

			public string Profession;

			public bool Elite;

			public Trait WeaponTrait;

			public List<Trait> MinorTraits = new List<Trait>();

			public List<Trait> MajorTraits = new List<Trait>();
		}

		public class ProfessionWeapon
		{
			public int Specialization;

			public weaponType Weapon;

			public List<weaponHand> Wielded;
		}

		public class Profession
		{
			public string Name;

			public string Id;

			public Icon Icon;

			public Icon IconBig;

			public List<Specialization> Specializations = new List<Specialization>();

			public List<ProfessionWeapon> Weapons = new List<ProfessionWeapon>();

			public List<Skill> Skills = new List<Skill>();
		}

		public class StatAttribute
		{
			public int Id;

			public string Name;

			public double Multiplier;

			public int Value;

			public Icon Icon;
		}

		public class Stat
		{
			public int Id;

			public string Name;

			public List<StatAttribute> Attributes = new List<StatAttribute>();

			public Icon Icon;
		}

		public static string UniformAttributeName(string statName)
		{
			return statName switch
			{
				"ConditionDamage" => "Condition Damage", 
				"BoonDuration" => "Concentration", 
				"ConditionDuration" => "Expertise", 
				"Healing" => "Healing Power", 
				"CritDamage" => "Ferocity", 
				_ => statName, 
			};
		}
	}
}
