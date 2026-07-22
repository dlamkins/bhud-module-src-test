using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ProfessionTypeExtension
	{
		public static ItemWeightType GetArmorType(this ProfessionType prof)
		{
			switch (prof)
			{
			case ProfessionType.Guardian:
			case ProfessionType.Warrior:
			case ProfessionType.Revenant:
				return ItemWeightType.Heavy;
			case ProfessionType.Engineer:
			case ProfessionType.Ranger:
			case ProfessionType.Thief:
				return ItemWeightType.Medium;
			case ProfessionType.Elementalist:
			case ProfessionType.Mesmer:
			case ProfessionType.Necromancer:
				return ItemWeightType.Light;
			default:
				return ItemWeightType.Unknown;
			}
		}
	}
}
