using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Crafting;
using Atzie.MysticCrafting.Models.Items;
using Atzie.MysticCrafting.Models.Vendor;
using MysticCrafting.Models.Vendor;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.Repositories;
using MysticCrafting.Module.Services.API;

namespace MysticCrafting.Module.Services
{
	public class ItemSourceService : IItemSourceService
	{
		private readonly ITradingPostService _tradingPostService;

		private readonly IRecipeRepository _recipeRepository;

		private readonly IVendorRepository _vendorRepository;

		private readonly IChoiceRepository _choiceRepository;

		private readonly IItemRepository _itemRepository;

		private readonly IWizardsVaultRepository _wizardsVaultRepository;

		private Dictionary<int, List<int>> SwappableItems = new Dictionary<int, List<int>> { 
		{
			79410,
			new List<int> { 24282, 19684, 19684, 19684, 19684 }
		} };

		public ItemSourceService(ITradingPostService tradingPostService, IRecipeRepository recipeRepository, IVendorRepository vendorRepository, IChoiceRepository choiceRepository, IItemRepository itemRepository, IWizardsVaultRepository wizardsVaultRepository)
		{
			_tradingPostService = tradingPostService;
			_recipeRepository = recipeRepository;
			_vendorRepository = vendorRepository;
			_choiceRepository = choiceRepository;
			_itemRepository = itemRepository;
			_wizardsVaultRepository = wizardsVaultRepository;
		}

		public IEnumerable<IItemSource> GetItemSources(Item item)
		{
			if (item.Id == 68063 || item.Id == 92687)
			{
				return new List<IItemSource> { GetTradingPostSource(item) };
			}
			List<IItemSource> sources = new List<IItemSource>();
			IEnumerable<RecipeSource> recipes = GetRecipeSources(item);
			if (recipes != null)
			{
				sources.AddRange(recipes);
			}
			if (item.CanBeTraded)
			{
				TradingPostSource prices = GetTradingPostSource(item);
				if (prices != null)
				{
					sources.Add(prices);
				}
			}
			VendorSource vendorPrices = GetVendorSource(item);
			if (vendorPrices != null)
			{
				sources.Add(vendorPrices);
			}
			IList<ItemContainerSource> itemContainerSource = GetItemContainerSource(item.Id);
			if (itemContainerSource != null && itemContainerSource.Any())
			{
				sources.AddRange(itemContainerSource);
			}
			return sources;
		}

		public IEnumerable<IItemSource> ConvertSourcesToSwap(IEnumerable<RecipeSource> sources, int itemId, int parentItemId)
		{
			if (itemId == 0 || parentItemId == 0 || !SwappableItems.Select((KeyValuePair<int, List<int>> s) => s.Key).Contains(parentItemId))
			{
				return sources;
			}
			List<Recipe> recipes = (from s in sources
				where s != null
				select s into r
				select r.Recipe).ToList();
			if (recipes == null || recipes.Count() <= 1)
			{
				return sources;
			}
			List<int> swapIds = SwappableItems[parentItemId];
			new SwapItemSource($"swap_{itemId}")
			{
				DisplayName = "Swap",
				Recipes = recipes,
				SwappableItemIds = swapIds
			};
			return null;
		}

		private IEnumerable<RecipeSource> GetRecipeSources(Item item)
		{
			return (from r in item.Recipes?.Where((Recipe r) => r.OutputQuantity < 2)
				select new RecipeSource(r.FullText)
				{
					Recipe = r,
					DisplayName = r.Source.ToString(),
					Source = r.Source
				});
		}

		private TradingPostSource GetTradingPostSource(Item item)
		{
			if (item == null || !item.CanBeTraded)
			{
				return null;
			}
			return new TradingPostSource(item.Id.ToString())
			{
				Item = item,
				DisplayName = "Trading Post"
			};
		}

		private IList<ItemContainerSource> GetItemContainerSource(int itemId)
		{
			IEnumerable<MysticVaultContainer> containers = _wizardsVaultRepository.GetContainers(itemId);
			if (containers == null)
			{
				return null;
			}
			List<ItemContainerSource> sources = new List<ItemContainerSource>();
			foreach (MysticVaultContainer container in containers)
			{
				sources.Add(new ItemContainerSource($"item_container_{container.ItemId}")
				{
					Container = container,
					ContainerItem = _itemRepository.GetItem(container.ItemId)
				});
			}
			return sources;
		}

		private VendorSource GetVendorSource(Item item)
		{
			if (item.VendorListings == null || item.VendorListings.All((VendorSellsItem v) => v.IsHistorical))
			{
				return null;
			}
			List<VendorSellsItem> vendorObjects = item.VendorListings.Where((VendorSellsItem v) => !v.IsHistorical).ToList();
			IList<VendorSellsItemGroup> groups = vendorObjects.CombineVendorSellsItems();
			return new VendorSource($"vendor_{item.Id}")
			{
				VendorItems = vendorObjects,
				VendorGroups = groups,
				DisplayName = "Vendor"
			};
		}

		public string GetPreferredItemSource(string path)
		{
			return _choiceRepository.GetChoice(path, ChoiceType.ItemSource)?.Value;
		}
	}
}