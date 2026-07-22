using System;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.Core.Extensions
{
	public static class SkillWeaponTypeExtension
	{
		public static ItemWeaponType ToItemWeapon(this SkillWeaponType skillWeaponType)
		{
			Enum.TryParse<ItemWeaponType>((skillWeaponType == SkillWeaponType.Spear) ? "Harpoon" : skillWeaponType.ToString(), out var itemWeaponType);
			return itemWeaponType;
		}
	}
}
