using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using MysticCrafting.Models.Items;

namespace MysticCrafting.Module.Extensions
{
	public static class MysticItemExtensions
	{
		private static List<string> _oneHandedWeapons = new List<string> { "axe", "dagger", "mace", "pistol", "sword" };

		public static int? GetMaxCount(this MysticItem item)
		{
			if (item.Rarity == "Legendary")
			{
				if (item.DetailsType.Equals("Rune", StringComparison.InvariantCultureIgnoreCase))
				{
					return 7;
				}
				if (item.DetailsType.Equals("Sigil", StringComparison.InvariantCultureIgnoreCase))
				{
					return 8;
				}
				if (item.GameId == 93105)
				{
					return 2;
				}
				if (item.Type.Equals("Trinket", StringComparison.InvariantCultureIgnoreCase))
				{
					return 1;
				}
				if (item.Type.Equals("Weapon", StringComparison.InvariantCultureIgnoreCase))
				{
					if (_oneHandedWeapons.Contains(item.DetailsType.ToLower()))
					{
						return 4;
					}
					return 2;
				}
				return 1;
			}
			return int.MaxValue;
		}

		public static string LocalizedName(this MysticItem item)
		{
			return item.Localizations?.FirstOrDefault((MysticItemLocalization l) => l.Locale == GameService.Overlay.UserLocale.Value)?.Name ?? item.Name;
		}

		public static string LocalizedDescription(this MysticItem item)
		{
			return item.Localizations?.FirstOrDefault((MysticItemLocalization l) => l.Locale == GameService.Overlay.UserLocale.Value)?.Description ?? item.Description;
		}

		public static bool HasSkin(this MysticItem item)
		{
			if (item.TypeEnum != MysticItemType.Armor && item.TypeEnum != MysticItemType.Weapon)
			{
				return item.TypeEnum == MysticItemType.Back;
			}
			return true;
		}
	}
}
