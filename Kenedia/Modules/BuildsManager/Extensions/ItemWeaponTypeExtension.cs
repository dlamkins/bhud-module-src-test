using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Professions;

namespace Kenedia.Modules.BuildsManager.Extensions
{
	public static class ItemWeaponTypeExtension
	{
		public static bool IsWeaponType(this ItemWeaponType type, Weapon.WeaponType weaponType)
		{
			return type switch
			{
				ItemWeaponType.LongBow => weaponType == Weapon.WeaponType.Longbow, 
				ItemWeaponType.ShortBow => weaponType == Weapon.WeaponType.Shortbow, 
				ItemWeaponType.Harpoon => weaponType == Weapon.WeaponType.Harpoon, 
				_ => weaponType.ToString() == type.ToString(), 
			};
		}
	}
}
