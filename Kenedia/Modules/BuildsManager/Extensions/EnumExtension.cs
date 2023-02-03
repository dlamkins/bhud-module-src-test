using Gw2Sharp.Models;
using Kenedia.Modules.BuildsManager.Enums;

namespace Kenedia.Modules.BuildsManager.Extensions
{
	public static class EnumExtension
	{
		public static EquipmentSlots GetEquipmentSlot(this ArmorSlot slot)
		{
			return slot switch
			{
				ArmorSlot.Helm => EquipmentSlots.Helmet, 
				ArmorSlot.Shoulders => EquipmentSlots.Shoulders, 
				ArmorSlot.Coat => EquipmentSlots.Chest, 
				ArmorSlot.Gloves => EquipmentSlots.Gloves, 
				ArmorSlot.Leggings => EquipmentSlots.Leggings, 
				ArmorSlot.Boots => EquipmentSlots.Boots, 
				_ => EquipmentSlots.Unkown, 
			};
		}

		public static ArmorSlot GetArmorSlot(this EquipmentSlots slot)
		{
			return slot switch
			{
				EquipmentSlots.Helmet => ArmorSlot.Helm, 
				EquipmentSlots.Shoulders => ArmorSlot.Shoulders, 
				EquipmentSlots.Chest => ArmorSlot.Coat, 
				EquipmentSlots.Gloves => ArmorSlot.Gloves, 
				EquipmentSlots.Leggings => ArmorSlot.Leggings, 
				EquipmentSlots.Boots => ArmorSlot.Boots, 
				_ => ArmorSlot.Unkown, 
			};
		}

		public static ArmorWeight GetArmorWeight(this ProfessionType profession)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected I4, but got Unknown
			switch (profession - 1)
			{
			case 5:
			case 6:
			case 7:
				return ArmorWeight.Light;
			case 2:
			case 3:
			case 4:
				return ArmorWeight.Medium;
			case 0:
			case 1:
			case 8:
				return ArmorWeight.Heavy;
			default:
				return ArmorWeight.Unkown;
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
