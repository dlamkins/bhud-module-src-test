using System;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Core.DataModels;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ItemTypeExtension
	{
		public static ItemType ToItemType(this ApiEnum<ItemType> apiEnum)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected I4, but got Unknown
			ItemType itemType;
			if (apiEnum.get_IsUnknown())
			{
				return (apiEnum.get_RawValue() == "Mwcc") ? ItemType.Relic : (Enum.TryParse<ItemType>(apiEnum.get_RawValue(), out itemType) ? itemType : ItemType.Unknown);
			}
			ItemType value = apiEnum.get_Value();
			return (value - 1) switch
			{
				0 => ItemType.Armor, 
				1 => ItemType.Back, 
				2 => ItemType.Bag, 
				3 => ItemType.Consumable, 
				4 => ItemType.CraftingMaterial, 
				5 => ItemType.Container, 
				6 => ItemType.Gathering, 
				7 => ItemType.Gizmo, 
				8 => ItemType.MiniPet, 
				9 => ItemType.Tool, 
				11 => ItemType.Trinket, 
				12 => ItemType.Trophy, 
				13 => ItemType.UpgradeComponent, 
				14 => ItemType.Weapon, 
				10 => ItemType.Trait, 
				15 => ItemType.Key, 
				16 => ItemType.JadeTechModule, 
				17 => ItemType.PowerCore, 
				_ => ItemType.Unknown, 
			};
		}

		public static ItemType ToItemType(this ItemType apiItemType)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected I4, but got Unknown
			return (int)apiItemType switch
			{
				1 => ItemType.Armor, 
				2 => ItemType.Back, 
				3 => ItemType.Bag, 
				4 => ItemType.Consumable, 
				5 => ItemType.CraftingMaterial, 
				6 => ItemType.Container, 
				7 => ItemType.Gathering, 
				8 => ItemType.Gizmo, 
				9 => ItemType.MiniPet, 
				10 => ItemType.Tool, 
				12 => ItemType.Trinket, 
				13 => ItemType.Trophy, 
				14 => ItemType.UpgradeComponent, 
				15 => ItemType.Weapon, 
				11 => ItemType.Trait, 
				16 => ItemType.Key, 
				17 => ItemType.JadeTechModule, 
				18 => ItemType.PowerCore, 
				0 => ItemType.Unknown, 
				_ => ItemType.Unknown, 
			};
		}

		public static ItemType ToApiItemType(this ItemType itemType)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			return (ItemType)(itemType switch
			{
				ItemType.Armor => 1, 
				ItemType.Back => 2, 
				ItemType.Bag => 3, 
				ItemType.Consumable => 4, 
				ItemType.CraftingMaterial => 5, 
				ItemType.Container => 6, 
				ItemType.Gathering => 7, 
				ItemType.Gizmo => 8, 
				ItemType.MiniPet => 9, 
				ItemType.Tool => 10, 
				ItemType.Trinket => 12, 
				ItemType.Trophy => 13, 
				ItemType.UpgradeComponent => 14, 
				ItemType.Weapon => 15, 
				ItemType.Trait => 11, 
				ItemType.Key => 16, 
				ItemType.JadeTechModule => 17, 
				ItemType.PowerCore => 18, 
				ItemType.Relic => 0, 
				ItemType.Unknown => 0, 
				_ => 0, 
			});
		}
	}
}
