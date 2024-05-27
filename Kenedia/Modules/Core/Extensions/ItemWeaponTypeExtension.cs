using System;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ItemWeaponTypeExtension
	{
		public static bool IsTwoHanded(this ItemWeaponType itemWeaponType)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Invalid comparison between Unknown and I4
			if (itemWeaponType - 11 <= 5)
			{
				return true;
			}
			return false;
		}

		public static bool IsOneHanded(this ItemWeaponType itemWeaponType)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Invalid comparison between Unknown and I4
			if (itemWeaponType - 1 <= 5)
			{
				return true;
			}
			return false;
		}

		public static bool IsOffHand(this ItemWeaponType itemWeaponType)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Invalid comparison between Unknown and I4
			if (itemWeaponType - 7 <= 3)
			{
				return true;
			}
			return false;
		}

		public static SkillWeaponType ToSkillWeapon(this ItemWeaponType itemWeaponType)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			Enum.TryParse<SkillWeaponType>(((int)itemWeaponType == 17) ? "Spear" : ((object)(ItemWeaponType)(ref itemWeaponType)).ToString(), out SkillWeaponType skillWeaponType);
			return skillWeaponType;
		}
	}
}
