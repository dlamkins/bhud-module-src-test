using System;
using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Professions;

namespace Kenedia.Modules.BuildsManager.Utility
{
	public static class WeaponTypeConversion
	{
		public static ItemWeaponType ToItemWeaponType(this Weapon.WeaponType professionWeaponType)
		{
			Enum.TryParse<ItemWeaponType>(new Dictionary<Weapon.WeaponType, string> { 
			{
				Weapon.WeaponType.Longbow,
				"LongBow"
			} }.TryGetValue(professionWeaponType, out var streamlinedName) ? streamlinedName : professionWeaponType.ToString(), out var itemWeaponType);
			return itemWeaponType;
		}
	}
}
