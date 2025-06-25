using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;

namespace gw2stacks_blish.data
{
	public class Armory
	{
		public Dictionary<ItemArmorSlotType, bool> heavyArmor;

		public Dictionary<ItemArmorSlotType, bool> mediumArmor;

		public Dictionary<ItemArmorSlotType, bool> lightArmor;

		public bool backpack;

		public int rings;

		public int trinkets;

		public bool amulet;

		public int runes;

		public int sigils;

		public bool relic;

		public Dictionary<ItemWeaponType, int> weapons;

		public Armory()
		{
			heavyArmor = new Dictionary<ItemArmorSlotType, bool>
			{
				{
					ItemArmorSlotType.Helm,
					false
				},
				{
					ItemArmorSlotType.Shoulders,
					false
				},
				{
					ItemArmorSlotType.Coat,
					false
				},
				{
					ItemArmorSlotType.Gloves,
					false
				},
				{
					ItemArmorSlotType.Leggings,
					false
				},
				{
					ItemArmorSlotType.Boots,
					false
				}
			};
			mediumArmor = new Dictionary<ItemArmorSlotType, bool>
			{
				{
					ItemArmorSlotType.Helm,
					false
				},
				{
					ItemArmorSlotType.Shoulders,
					false
				},
				{
					ItemArmorSlotType.Coat,
					false
				},
				{
					ItemArmorSlotType.Gloves,
					false
				},
				{
					ItemArmorSlotType.Leggings,
					false
				},
				{
					ItemArmorSlotType.Boots,
					false
				}
			};
			lightArmor = new Dictionary<ItemArmorSlotType, bool>
			{
				{
					ItemArmorSlotType.Helm,
					false
				},
				{
					ItemArmorSlotType.Shoulders,
					false
				},
				{
					ItemArmorSlotType.Coat,
					false
				},
				{
					ItemArmorSlotType.Gloves,
					false
				},
				{
					ItemArmorSlotType.Leggings,
					false
				},
				{
					ItemArmorSlotType.Boots,
					false
				}
			};
			backpack = false;
			rings = 0;
			trinkets = 0;
			amulet = false;
			weapons = new Dictionary<ItemWeaponType, int>
			{
				{
					ItemWeaponType.Axe,
					0
				},
				{
					ItemWeaponType.Dagger,
					0
				},
				{
					ItemWeaponType.Mace,
					0
				},
				{
					ItemWeaponType.Pistol,
					0
				},
				{
					ItemWeaponType.Sword,
					0
				},
				{
					ItemWeaponType.Scepter,
					0
				},
				{
					ItemWeaponType.Focus,
					0
				},
				{
					ItemWeaponType.Shield,
					0
				},
				{
					ItemWeaponType.Torch,
					0
				},
				{
					ItemWeaponType.Warhorn,
					0
				},
				{
					ItemWeaponType.Greatsword,
					0
				},
				{
					ItemWeaponType.Hammer,
					0
				},
				{
					ItemWeaponType.LongBow,
					0
				},
				{
					ItemWeaponType.Rifle,
					0
				},
				{
					ItemWeaponType.ShortBow,
					0
				},
				{
					ItemWeaponType.Staff,
					0
				},
				{
					ItemWeaponType.Harpoon,
					0
				},
				{
					ItemWeaponType.Speargun,
					0
				},
				{
					ItemWeaponType.Trident,
					0
				}
			};
		}
	}
}
