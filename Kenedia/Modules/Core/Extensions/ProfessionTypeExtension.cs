using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ProfessionTypeExtension
	{
		public static ItemWeightType GetArmorType(this ProfessionType prof)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected I4, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			switch (prof - 1)
			{
			case 0:
			case 1:
			case 8:
				return (ItemWeightType)1;
			case 2:
			case 3:
			case 4:
				return (ItemWeightType)2;
			case 5:
			case 6:
			case 7:
				return (ItemWeightType)3;
			default:
				return (ItemWeightType)0;
			}
		}
	}
}
