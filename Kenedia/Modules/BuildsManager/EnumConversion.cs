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
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected I4, but got Unknown
			switch (profession - 1)
			{
			case 5:
			case 6:
			case 7:
				return _ArmorWeight.Light;
			case 2:
			case 3:
			case 4:
				return _ArmorWeight.Medium;
			case 0:
			case 1:
			case 8:
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
