using Gw2Sharp.Models;

namespace Kenedia.Modules.BuildsManager
{
	public static class EnumConversion
	{
		public static _EquipmentSlots GetEquipmentSlot(this _ArmorSlot slot)
		{
			return slot switch
			{
				_ArmorSlot.Helm => _EquipmentSlots.Helmet, 
				_ArmorSlot.Shoulders => _EquipmentSlots.Shoulders, 
				_ArmorSlot.Coat => _EquipmentSlots.Chest, 
				_ArmorSlot.Gloves => _EquipmentSlots.Gloves, 
				_ArmorSlot.Leggings => _EquipmentSlots.Leggings, 
				_ArmorSlot.Boots => _EquipmentSlots.Boots, 
				_ => _EquipmentSlots.Unkown, 
			};
		}

		public static _ArmorSlot GetArmorSlot(this _EquipmentSlots slot)
		{
			return slot switch
			{
				_EquipmentSlots.Helmet => _ArmorSlot.Helm, 
				_EquipmentSlots.Shoulders => _ArmorSlot.Shoulders, 
				_EquipmentSlots.Chest => _ArmorSlot.Coat, 
				_EquipmentSlots.Gloves => _ArmorSlot.Gloves, 
				_EquipmentSlots.Leggings => _ArmorSlot.Leggings, 
				_EquipmentSlots.Boots => _ArmorSlot.Boots, 
				_ => _ArmorSlot.Unkown, 
			};
		}

		public static _ArmorWeight GetArmorWeight(this ProfessionType profession)
		{
			switch (profession)
			{
			case ProfessionType.Elementalist:
			case ProfessionType.Mesmer:
			case ProfessionType.Necromancer:
				return _ArmorWeight.Light;
			case ProfessionType.Engineer:
			case ProfessionType.Ranger:
			case ProfessionType.Thief:
				return _ArmorWeight.Medium;
			case ProfessionType.Guardian:
			case ProfessionType.Warrior:
			case ProfessionType.Revenant:
				return _ArmorWeight.Heavy;
			default:
				return _ArmorWeight.Unkown;
			}
		}

		public static string convertWeaponType(this GW2API.intDetails details)
		{
			if (details.Type != null)
			{
				switch (details.Type.RawValue)
				{
				case "Harpoon":
					return "Spear";
				case "Spear":
					return "Harpoon";
				case "LongBow":
					return "Longbow";
				case "Longbow":
					return "LongBow";
				case "ShortBow":
					return "Shortbow";
				case "Shortbow":
					return "ShortBow";
				}
			}
			if (details.Type == null)
			{
				return "Unkown";
			}
			return details.Type.RawValue;
		}
	}
}
