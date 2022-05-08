using System.Collections.Generic;

namespace Kenedia.Modules.BuildsManager
{
	public class GearTemplate
	{
		public List<TemplateItem> Trinkets = new List<TemplateItem>
		{
			new TemplateItem
			{
				_Slot = "Back"
			},
			new TemplateItem
			{
				_Slot = "Amulet"
			},
			new TemplateItem
			{
				_Slot = "Ring1"
			},
			new TemplateItem
			{
				_Slot = "Ring2"
			},
			new TemplateItem
			{
				_Slot = "Accessoire1"
			},
			new TemplateItem
			{
				_Slot = "Accessoire2"
			}
		};

		public List<Armor_TemplateItem> Armor = new List<Armor_TemplateItem>
		{
			new Armor_TemplateItem
			{
				_Slot = "Helmet"
			},
			new Armor_TemplateItem
			{
				_Slot = "Shoulders"
			},
			new Armor_TemplateItem
			{
				_Slot = "Chest"
			},
			new Armor_TemplateItem
			{
				_Slot = "Gloves"
			},
			new Armor_TemplateItem
			{
				_Slot = "Leggings"
			},
			new Armor_TemplateItem
			{
				_Slot = "Boots"
			}
		};

		public List<Weapon_TemplateItem> Weapons = new List<Weapon_TemplateItem>
		{
			new Weapon_TemplateItem
			{
				Slot = _EquipmentSlots.Weapon1_MainHand
			},
			new Weapon_TemplateItem
			{
				Slot = _EquipmentSlots.Weapon1_OffHand
			},
			new Weapon_TemplateItem
			{
				Slot = _EquipmentSlots.Weapon2_MainHand
			},
			new Weapon_TemplateItem
			{
				Slot = _EquipmentSlots.Weapon2_OffHand
			}
		};

		public List<AquaticWeapon_TemplateItem> AquaticWeapons = new List<AquaticWeapon_TemplateItem>
		{
			new AquaticWeapon_TemplateItem
			{
				Slot = _EquipmentSlots.AquaticWeapon1
			},
			new AquaticWeapon_TemplateItem
			{
				Slot = _EquipmentSlots.AquaticWeapon2
			}
		};
	}
}
