using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models.Templates;

namespace Kenedia.Modules.BuildsManager.Extensions
{
	public static class WeaponTypeExtension
	{
		public static bool IsItemWeaponType(this Weapon.WeaponType weaponType, ItemWeaponType type)
		{
			return type switch
			{
				ItemWeaponType.LongBow => weaponType == Weapon.WeaponType.Longbow, 
				ItemWeaponType.ShortBow => weaponType == Weapon.WeaponType.Shortbow, 
				ItemWeaponType.Harpoon => weaponType == Weapon.WeaponType.Harpoon, 
				_ => weaponType.ToString() == type.ToString(), 
			};
		}

		public static bool IsAquatic(this Weapon.WeaponType weaponType, TemplateSlotType templateSlotType = TemplateSlotType.Aquatic)
		{
			return weaponType switch
			{
				Weapon.WeaponType.Harpoon => (templateSlotType == TemplateSlotType.Aquatic || templateSlotType == TemplateSlotType.AltAquatic) ? true : false, 
				Weapon.WeaponType.Trident => true, 
				Weapon.WeaponType.Speargun => true, 
				_ => false, 
			};
		}

		public static bool IsTwoHanded(this Weapon.WeaponType weaponType)
		{
			return weaponType switch
			{
				Weapon.WeaponType.Greatsword => true, 
				Weapon.WeaponType.Hammer => true, 
				Weapon.WeaponType.Longbow => true, 
				Weapon.WeaponType.Rifle => true, 
				Weapon.WeaponType.Shortbow => true, 
				Weapon.WeaponType.Staff => true, 
				Weapon.WeaponType.Harpoon => true, 
				Weapon.WeaponType.Trident => true, 
				Weapon.WeaponType.Speargun => true, 
				_ => false, 
			};
		}
	}
}
