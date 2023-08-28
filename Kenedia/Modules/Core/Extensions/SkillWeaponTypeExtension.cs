using System;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.Core.Extensions
{
	public static class SkillWeaponTypeExtension
	{
		public static ItemWeaponType ToItemWeapon(this SkillWeaponType skillWeaponType)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			Enum.TryParse<ItemWeaponType>(((int)skillWeaponType == 18) ? "Harpoon" : ((object)(SkillWeaponType)(ref skillWeaponType)).ToString(), out ItemWeaponType itemWeaponType);
			return itemWeaponType;
		}
	}
}
