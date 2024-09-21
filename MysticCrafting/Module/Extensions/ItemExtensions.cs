using System;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Crafting;
using Atzie.MysticCrafting.Models.Items;
using Atzie.MysticCrafting.Models.Vendor;
using Blish_HUD;
using MysticCrafting.Models.Vendor;

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
				if (item.Id == 30691)
				{
					return 4;
				}
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

		public static bool IsDecoration(this Item item)
		{
			return new List<int> { 28, 29, 30, 31, 32, 34, 35, 36, 37 }.Contains(item.CategoryId);
		}

		public static bool IsDye(this Item item)
		{
			return new List<int> { 108, 109, 110, 111, 112 }.Contains(item.CategoryId);
		}

		public static List<Item> FlattenItems(this Item item)
		{
			return new List<Item> { item }.FlattenItems();
		}

		public static List<Item> FlattenItems(this List<Item> items)
		{
			List<Item> recipeItems = (from i in items.Where((Item i) => i != null && i.Recipes != null).SelectMany((Item i) => i.Recipes.Where((Recipe r) => r.Ingredients != null).SelectMany((Recipe r) => r.Ingredients.Select((Ingredient ing) => ing.Item).Concat(r.RecipeSheets))).Except(items)
				where i != null
				select i).ToList();
			List<Item> vendorItems = (from i in items.Where((Item i) => i != null && i.Recipes != null).SelectMany((Item i) => i.VendorListings.Where((VendorSellsItem v) => v.ItemCosts != null).SelectMany((VendorSellsItem v) => from c in v.ItemCosts
					where c.Item != null
					select c.Item)).Except(items)
					.Except(recipeItems)
				where i != null
				select i).ToList();
			if (recipeItems.Any())
			{
				items.AddRange(recipeItems);
			}
			if (vendorItems.Any())
			{
				items.AddRange(vendorItems);
			}
			if (!recipeItems.Any() && !vendorItems.Any())
			{
				return items;
			}
			items.FlattenItems();
			return items;
		}
	}
}
