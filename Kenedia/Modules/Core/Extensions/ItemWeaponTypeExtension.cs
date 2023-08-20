using System;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ItemWeaponTypeExtension
	{
		public static bool IsTwoHanded(this ItemWeaponType itemWeaponType)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Invalid comparison between Unknown and I4
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Invalid comparison between Unknown and I4
			if ((int)itemWeaponType != 11 && (int)itemWeaponType != 12 && (int)itemWeaponType != 13 && (int)itemWeaponType != 14 && (int)itemWeaponType != 15)
			{
				return (int)itemWeaponType == 16;
			}
			return true;
		}

		public static bool IsOneHanded(this ItemWeaponType itemWeaponType)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Invalid comparison between Unknown and I4
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Invalid comparison between Unknown and I4
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Invalid comparison between Unknown and I4
			if ((int)itemWeaponType != 1 && (int)itemWeaponType != 2 && (int)itemWeaponType != 3 && (int)itemWeaponType != 4 && (int)itemWeaponType != 5)
			{
				return (int)itemWeaponType == 6;
			}
			return true;
		}

		public static bool IsOffHand(this ItemWeaponType itemWeaponType)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Invalid comparison between Unknown and I4
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Invalid comparison between Unknown and I4
			if ((int)itemWeaponType != 7 && (int)itemWeaponType != 8 && (int)itemWeaponType != 9)
			{
				return (int)itemWeaponType == 10;
			}
			return true;
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
