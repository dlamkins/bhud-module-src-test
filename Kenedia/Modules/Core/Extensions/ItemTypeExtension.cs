using System;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Core.DataModels;

namespace Kenedia.Modules.Core.Extensions
{
	public static class ItemTypeExtension
	{
		public static Kenedia.Modules.Core.DataModels.ItemType ToItemType(this ApiEnum<Gw2Sharp.WebApi.V2.Models.ItemType> apiEnum)
		{
			Kenedia.Modules.Core.DataModels.ItemType itemType;
			if (apiEnum.IsUnknown)
			{
				return (apiEnum.RawValue == "Mwcc") ? Kenedia.Modules.Core.DataModels.ItemType.Relic : (Enum.TryParse<Kenedia.Modules.Core.DataModels.ItemType>(apiEnum.RawValue, out itemType) ? itemType : Kenedia.Modules.Core.DataModels.ItemType.Unknown);
			}
			return apiEnum.Value switch
			{
				Gw2Sharp.WebApi.V2.Models.ItemType.Armor => Kenedia.Modules.Core.DataModels.ItemType.Armor, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Back => Kenedia.Modules.Core.DataModels.ItemType.Back, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Bag => Kenedia.Modules.Core.DataModels.ItemType.Bag, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Consumable => Kenedia.Modules.Core.DataModels.ItemType.Consumable, 
				Gw2Sharp.WebApi.V2.Models.ItemType.CraftingMaterial => Kenedia.Modules.Core.DataModels.ItemType.CraftingMaterial, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Container => Kenedia.Modules.Core.DataModels.ItemType.Container, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Gathering => Kenedia.Modules.Core.DataModels.ItemType.Gathering, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Gizmo => Kenedia.Modules.Core.DataModels.ItemType.Gizmo, 
				Gw2Sharp.WebApi.V2.Models.ItemType.MiniPet => Kenedia.Modules.Core.DataModels.ItemType.MiniPet, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Tool => Kenedia.Modules.Core.DataModels.ItemType.Tool, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Trinket => Kenedia.Modules.Core.DataModels.ItemType.Trinket, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Trophy => Kenedia.Modules.Core.DataModels.ItemType.Trophy, 
				Gw2Sharp.WebApi.V2.Models.ItemType.UpgradeComponent => Kenedia.Modules.Core.DataModels.ItemType.UpgradeComponent, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Weapon => Kenedia.Modules.Core.DataModels.ItemType.Weapon, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Trait => Kenedia.Modules.Core.DataModels.ItemType.Trait, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Key => Kenedia.Modules.Core.DataModels.ItemType.Key, 
				Gw2Sharp.WebApi.V2.Models.ItemType.JadeTechModule => Kenedia.Modules.Core.DataModels.ItemType.JadeTechModule, 
				Gw2Sharp.WebApi.V2.Models.ItemType.PowerCore => Kenedia.Modules.Core.DataModels.ItemType.PowerCore, 
				_ => Kenedia.Modules.Core.DataModels.ItemType.Unknown, 
			};
		}

		public static Kenedia.Modules.Core.DataModels.ItemType ToItemType(this Gw2Sharp.WebApi.V2.Models.ItemType apiItemType)
		{
			return apiItemType switch
			{
				Gw2Sharp.WebApi.V2.Models.ItemType.Armor => Kenedia.Modules.Core.DataModels.ItemType.Armor, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Back => Kenedia.Modules.Core.DataModels.ItemType.Back, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Bag => Kenedia.Modules.Core.DataModels.ItemType.Bag, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Consumable => Kenedia.Modules.Core.DataModels.ItemType.Consumable, 
				Gw2Sharp.WebApi.V2.Models.ItemType.CraftingMaterial => Kenedia.Modules.Core.DataModels.ItemType.CraftingMaterial, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Container => Kenedia.Modules.Core.DataModels.ItemType.Container, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Gathering => Kenedia.Modules.Core.DataModels.ItemType.Gathering, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Gizmo => Kenedia.Modules.Core.DataModels.ItemType.Gizmo, 
				Gw2Sharp.WebApi.V2.Models.ItemType.MiniPet => Kenedia.Modules.Core.DataModels.ItemType.MiniPet, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Tool => Kenedia.Modules.Core.DataModels.ItemType.Tool, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Trinket => Kenedia.Modules.Core.DataModels.ItemType.Trinket, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Trophy => Kenedia.Modules.Core.DataModels.ItemType.Trophy, 
				Gw2Sharp.WebApi.V2.Models.ItemType.UpgradeComponent => Kenedia.Modules.Core.DataModels.ItemType.UpgradeComponent, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Weapon => Kenedia.Modules.Core.DataModels.ItemType.Weapon, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Trait => Kenedia.Modules.Core.DataModels.ItemType.Trait, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Key => Kenedia.Modules.Core.DataModels.ItemType.Key, 
				Gw2Sharp.WebApi.V2.Models.ItemType.JadeTechModule => Kenedia.Modules.Core.DataModels.ItemType.JadeTechModule, 
				Gw2Sharp.WebApi.V2.Models.ItemType.PowerCore => Kenedia.Modules.Core.DataModels.ItemType.PowerCore, 
				Gw2Sharp.WebApi.V2.Models.ItemType.Unknown => Kenedia.Modules.Core.DataModels.ItemType.Unknown, 
				_ => Kenedia.Modules.Core.DataModels.ItemType.Unknown, 
			};
		}

		public static Gw2Sharp.WebApi.V2.Models.ItemType ToApiItemType(this Kenedia.Modules.Core.DataModels.ItemType itemType)
		{
			return itemType switch
			{
				Kenedia.Modules.Core.DataModels.ItemType.Armor => Gw2Sharp.WebApi.V2.Models.ItemType.Armor, 
				Kenedia.Modules.Core.DataModels.ItemType.Back => Gw2Sharp.WebApi.V2.Models.ItemType.Back, 
				Kenedia.Modules.Core.DataModels.ItemType.Bag => Gw2Sharp.WebApi.V2.Models.ItemType.Bag, 
				Kenedia.Modules.Core.DataModels.ItemType.Consumable => Gw2Sharp.WebApi.V2.Models.ItemType.Consumable, 
				Kenedia.Modules.Core.DataModels.ItemType.CraftingMaterial => Gw2Sharp.WebApi.V2.Models.ItemType.CraftingMaterial, 
				Kenedia.Modules.Core.DataModels.ItemType.Container => Gw2Sharp.WebApi.V2.Models.ItemType.Container, 
				Kenedia.Modules.Core.DataModels.ItemType.Gathering => Gw2Sharp.WebApi.V2.Models.ItemType.Gathering, 
				Kenedia.Modules.Core.DataModels.ItemType.Gizmo => Gw2Sharp.WebApi.V2.Models.ItemType.Gizmo, 
				Kenedia.Modules.Core.DataModels.ItemType.MiniPet => Gw2Sharp.WebApi.V2.Models.ItemType.MiniPet, 
				Kenedia.Modules.Core.DataModels.ItemType.Tool => Gw2Sharp.WebApi.V2.Models.ItemType.Tool, 
				Kenedia.Modules.Core.DataModels.ItemType.Trinket => Gw2Sharp.WebApi.V2.Models.ItemType.Trinket, 
				Kenedia.Modules.Core.DataModels.ItemType.Trophy => Gw2Sharp.WebApi.V2.Models.ItemType.Trophy, 
				Kenedia.Modules.Core.DataModels.ItemType.UpgradeComponent => Gw2Sharp.WebApi.V2.Models.ItemType.UpgradeComponent, 
				Kenedia.Modules.Core.DataModels.ItemType.Weapon => Gw2Sharp.WebApi.V2.Models.ItemType.Weapon, 
				Kenedia.Modules.Core.DataModels.ItemType.Trait => Gw2Sharp.WebApi.V2.Models.ItemType.Trait, 
				Kenedia.Modules.Core.DataModels.ItemType.Key => Gw2Sharp.WebApi.V2.Models.ItemType.Key, 
				Kenedia.Modules.Core.DataModels.ItemType.JadeTechModule => Gw2Sharp.WebApi.V2.Models.ItemType.JadeTechModule, 
				Kenedia.Modules.Core.DataModels.ItemType.PowerCore => Gw2Sharp.WebApi.V2.Models.ItemType.PowerCore, 
				Kenedia.Modules.Core.DataModels.ItemType.Relic => Gw2Sharp.WebApi.V2.Models.ItemType.Unknown, 
				Kenedia.Modules.Core.DataModels.ItemType.Unknown => Gw2Sharp.WebApi.V2.Models.ItemType.Unknown, 
				_ => Gw2Sharp.WebApi.V2.Models.ItemType.Unknown, 
			};
		}
	}
}
