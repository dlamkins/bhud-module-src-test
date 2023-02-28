using System;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ItemWeaponTypeExtension
	{
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
