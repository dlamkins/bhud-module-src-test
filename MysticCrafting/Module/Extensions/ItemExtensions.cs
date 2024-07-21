using System;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD;

namespace MysticCrafting.Module.Extensions
{
	public static class ItemExtensions
	{
		private static List<string> _oneHandedWeapons = new List<string> { "axe", "dagger", "mace", "pistol", "sword" };

		public static int? GetMaxCount(this Item item)
		{
			if (item.Rarity != ItemRarity.Legendary)
			{
				return int.MaxValue;
			}
			if (item.DetailsType.Equals("Rune", StringComparison.InvariantCultureIgnoreCase))
			{
				return 7;
			}
			if (item.DetailsType.Equals("Sigil", StringComparison.InvariantCultureIgnoreCase))
			{
				return 8;
			}
			if (item.Id == 93105)
			{
				return 2;
			}
			if (item.Type == ItemType.Trinket)
			{
				return 1;
			}
			if (item.Type == ItemType.Weapon)
			{
				if (_oneHandedWeapons.Contains(item.DetailsType.ToLower()))
				{
					return 4;
				}
				return 2;
			}
			return 1;
		}

		public static string LocalizedName(this Item item)
		{
			return item.Localizations?.FirstOrDefault((ItemLocalization l) => l.Locale == GameService.Overlay.get_UserLocale().get_Value())?.Name ?? item.Name;
		}

		public static string LocalizedDescription(this Item item)
		{
			return item.Localizations?.FirstOrDefault((ItemLocalization l) => l.Locale == GameService.Overlay.get_UserLocale().get_Value())?.Description ?? item.Description;
		}

		public static bool HasSkin(this Item item)
		{
			if (item.Type != ItemType.Armor && item.Type != ItemType.Weapon)
			{
				return item.Type == ItemType.Back;
			}
			return true;
		}
	}
}
