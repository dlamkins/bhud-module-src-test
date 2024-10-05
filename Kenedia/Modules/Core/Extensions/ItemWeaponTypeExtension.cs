using System;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ItemWeaponTypeExtension
	{
		public static bool IsTwoHanded(this ItemWeaponType itemWeaponType)
		{
			if ((uint)(itemWeaponType - 11) <= 8u)
			{
				return true;
			}
			return false;
		}

		public static bool IsOneHanded(this ItemWeaponType itemWeaponType)
		{
			if ((uint)(itemWeaponType - 1) <= 5u)
			{
				return true;
			}
			return false;
		}

		public static bool IsOffHand(this ItemWeaponType itemWeaponType)
		{
			if ((uint)(itemWeaponType - 7) <= 3u)
			{
				return true;
			}
			return false;
		}

		public static SkillWeaponType ToSkillWeapon(this ItemWeaponType itemWeaponType)
		{
			Enum.TryParse<SkillWeaponType>((itemWeaponType == ItemWeaponType.Harpoon) ? "Spear" : itemWeaponType.ToString(), out var skillWeaponType);
			return skillWeaponType;
		}
	}
}
