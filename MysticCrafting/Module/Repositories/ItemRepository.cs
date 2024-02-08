using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi;
using MysticCrafting.Models;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class ItemRepository : IItemRepository, IRepository
	{
		private readonly IDataService _dataService;

		private List<int> IgnoreIds = new List<int>
		{
			85380, 31049, 85406, 91205, 91235, 90986, 91183, 91255, 91231, 91218,
			91196, 91206, 91115, 91178, 91197, 37104
		};

		private IList<MysticItem> Items { get; set; }

		public string FileName => GameService.Overlay.UserLocale.Value switch
		{
			Locale.German => "items_data_de.json", 
			Locale.Spanish => "items_data_sp.json", 
			Locale.Korean => "items_data_kr.json", 
			Locale.Chinese => "items_data_cn.json", 
			Locale.French => "items_data_fr.json", 
			_ => "items_data.json", 
		};

		public bool LocalOnly => false;

		public bool Loaded { get; set; }

		public ItemRepository(IDataService dataService)
		{
			_dataService = dataService;
		}

		public async Task<string> LoadAsync()
		{
			Items = (await _dataService.LoadFromFileAsync<IList<MysticItem>>(FileName)) ?? new List<MysticItem>();
			Loaded = true;
			return $"{Items.Count} items loaded";
		}

		public MysticItem GetItem(int itemId)
		{
			return Items.FirstOrDefault((MysticItem i) => i.Id == itemId);
		}

		public IList<MysticItem> GetItems()
		{
			return Items;
		}

		public IEnumerable<MysticItem> FilterItems(MysticItemFilter filter)
		{
			IEnumerable<MysticItem> items = Items;
			if (filter == null)
			{
				return FilterByAvailable(items);
			}
			if (filter.IsFavorite)
			{
				items = items.Where((MysticItem i) => ServiceContainer.FavoritesRepository.IsFavorite(i.Id));
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
			return FilterByAvailable(items);
		}

		private IEnumerable<MysticItem> FilterByAvailable(IEnumerable<MysticItem> items)
		{
			return items?.Where((MysticItem i) => ServiceContainer.RecipeRepository.GetRecipes(i.Id).FirstOrDefault((MysticRecipe r) => r.MysticForgeId != 0) != null && !IgnoreIds.Contains(i.Id) && !i.Name.Contains("Mistforged") && !i.Name.Contains("Infusion") && !i.Name.Contains("Infused") && !i.Name.Contains("Attuned") && !i.Name.Contains("Rurik") && !i.Name.Contains("Eingestimmt") && !i.Name.Contains("eingestimmt") && !i.Name.Contains("Infundiert") && !i.Name.Contains("infundiert") && !i.Name.Contains("Nebelgeschmiedeter"));
		}
	}
}
