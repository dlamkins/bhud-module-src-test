using System;
using System.Collections.Generic;
using System.Linq;
using JsonFlatFileDataStore;
using MysticCrafting.Models;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class ItemRepository : IItemRepository
	{
		private List<int> IgnoreIds = new List<int>
		{
			85380, 31049, 85406, 91205, 91235, 90986, 91183, 91255, 91231, 91218,
			91196, 91206, 91115, 91178, 91197, 37104
		};

		private IDocumentCollection<MysticItem> Items { get; } = ServiceContainer.Store.GetCollection<MysticItem>();


		public MysticItem GetItem(int itemId)
		{
			return Items.AsQueryable().FirstOrDefault((MysticItem i) => i.GameId == itemId);
		}

		public IList<MysticItem> GetItems()
		{
			return Items.AsQueryable().ToList();
		}

		public IEnumerable<MysticItem> FilterItems(MysticItemFilter filter)
		{
			IEnumerable<MysticItem> items = Items.AsQueryable();
			if (filter == null)
			{
				return FilterByAvailable(items);
			}
			if (filter.IsFavorite)
			{
				items = items.Where((MysticItem i) => ServiceContainer.FavoritesRepository.IsFavorite(i.GameId));
			}
			if (!string.IsNullOrEmpty(filter.Type))
			{
				items = items.Where((MysticItem i) => !string.IsNullOrEmpty(i.Type) && i.Type.Equals(filter.Type, StringComparison.InvariantCultureIgnoreCase));
			}
			if (!string.IsNullOrEmpty(filter.DetailsType))
			{
				items = items.Where((MysticItem i) => !string.IsNullOrEmpty(i.DetailsType) && i.DetailsType.Equals(filter.DetailsType, StringComparison.InvariantCultureIgnoreCase));
			}
			if (filter.Flags != null && filter.Flags.Any())
			{
				items = items.Where((MysticItem i) => i.Flags != null && i.Flags.Intersect(filter.Flags).Any());
			}
			if (!string.IsNullOrEmpty(filter.Rarity))
			{
				items = items.Where((MysticItem i) => i.Rarity != null && i.Rarity.Equals(filter.Rarity, StringComparison.InvariantCultureIgnoreCase));
			}
			if (!string.IsNullOrEmpty(filter.NameContainsText))
			{
				items = items.Where((MysticItem i) => i.Name != null && i.Name.ToLower().Contains(filter.NameContainsText.ToLower()));
			}
			if (filter.Weight != 0 && !filter.WeightFilterDisabled)
			{
				items = items.Where((MysticItem i) => i.WeightClass == filter.Weight);
			}
			if (!string.IsNullOrEmpty(filter.LegendaryType) && !filter.WeightFilterDisabled)
			{
				string type = filter.LegendaryType.Split(' ')[0];
				if (!string.IsNullOrEmpty(type))
				{
					items = items.Where((MysticItem i) => i.Name.ToLower().StartsWith(type.ToLower()));
				}
			}
			return FilterByAvailable(items);
		}

		private IEnumerable<MysticItem> FilterByAvailable(IEnumerable<MysticItem> items)
		{
			return items?.Where((MysticItem i) => ServiceContainer.RecipeRepository.GetRecipes(i.GameId).FirstOrDefault((MysticRecipe r) => r.IsMysticForgeRecipe) != null || (i.Rarity.Equals("Legendary", StringComparison.InvariantCultureIgnoreCase) && i.Name.StartsWith("Obsidian") && !IgnoreIds.Contains(i.GameId) && !i.Name.Contains("Mistforged") && !i.Name.Contains("Infusion") && !i.Name.Contains("Infused") && !i.Name.Contains("Attuned") && !i.Name.Contains("Rurik") && !i.Name.Contains("Eingestimmt") && !i.Name.Contains("eingestimmt") && !i.Name.Contains("Infundiert") && !i.Name.Contains("infundiert") && !i.Name.Contains("Nebelgeschmiedeter")));
		}
	}
}
