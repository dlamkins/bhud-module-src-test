using System;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Items;
using MysticCrafting.Models;
using MysticCrafting.Models.Items;
using MysticCrafting.Models.TradingPost;
using MysticCrafting.Models.Vendor;
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

		private readonly int[] _wizardsVaultIds = new int[15]
		{
			96054, 96697, 93482, 99453, 97519, 19651, 29174, 19673, 19672, 29176,
			19652, 29180, 19654, 97519, 19655
		};

		public ItemSourceService(ITradingPostService tradingPostService, IRecipeRepository recipeRepository, IVendorRepository vendorRepository, IChoiceRepository choiceRepository, IItemRepository itemRepository, IWizardsVaultRepository wizardsVaultRepository)
		{
			_tradingPostService = tradingPostService;
			_recipeRepository = recipeRepository;
			_vendorRepository = vendorRepository;
			_choiceRepository = choiceRepository;
			_itemRepository = itemRepository;
			_wizardsVaultRepository = wizardsVaultRepository;
		}

		public IEnumerable<IItemSource> GetItemSources(MysticItem item)
		{
			return GetItemSources(item.GameId);
		}

		public IEnumerable<IItemSource> GetItemSources(int itemId)
		{
			if (itemId == 68063)
			{
				return new List<IItemSource> { GetTradingPostSource(itemId) };
			}
			List<IItemSource> sources = new List<IItemSource>();
			sources.AddRange(GetRecipeSources(itemId));
			TradingPostSource prices = GetTradingPostSource(itemId);
			if (prices != null)
			{
				sources.Add(prices);
			}
			VendorSource vendorPrices = GetVendorSource(itemId);
			if (vendorPrices != null)
			{
				sources.Add(vendorPrices);
			}
			IList<ItemContainerSource> itemContainerSource = GetItemContainerSource(itemId);
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
			List<MysticRecipe> recipes = (from s in sources
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

		private IEnumerable<RecipeSource> GetRecipeSources(int itemId)
		{
			return _recipeRepository.GetRecipes(itemId)?.Where((MysticRecipe r) => r.OutputQuantity < 2)?.Select((MysticRecipe recipe) => new RecipeSource(recipe.FullText)
			{
				Recipe = recipe,
				DisplayName = recipe.Source
			})?.ToList() ?? new List<RecipeSource>();
		}

		private TradingPostSource GetTradingPostSource(int itemId)
		{
			TradingPostItemPrices prices = _tradingPostService.GetItemPrices(itemId);
			if (prices == null)
			{
				return null;
			}
			return new TradingPostSource(prices.Id.ToString())
			{
				BuyPrice = prices.BuyPrice,
				SellPrice = prices.SellPrice,
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
				sources.Add(new ItemContainerSource($"item_container_{container.Id}")
				{
					Container = container,
					ContainerItem = _itemRepository.GetItem(container.ItemId)
				});
			}
			return sources;
		}

		private VendorSource GetVendorSource(int itemId)
		{
			IList<VendorSellsItem> vendorObjects = _vendorRepository.GetVendorItems(itemId);
			if (vendorObjects == null || !vendorObjects.Any())
			{
				return null;
			}
			return new VendorSource($"vendor_{itemId}")
			{
				VendorItems = vendorObjects,
				DisplayName = "Vendor"
			};
		}

		public string GetPreferredItemSource(string path)
		{
			return _choiceRepository.GetChoice(path, ChoiceType.ItemSource)?.Value;
		}

		public IItemSource GetPreferredItemSource(string path, int itemId)
		{
			NodeChoice choice = _choiceRepository.GetChoice(path, ChoiceType.ItemSource);
			if (choice == null)
			{
				return GetDefaultItemSource(itemId);
			}
			IItemSource itemSource = GetItemSources(itemId).FirstOrDefault((IItemSource i) => i.UniqueId.Equals(choice.Value, StringComparison.InvariantCultureIgnoreCase));
			return itemSource ?? GetDefaultItemSource(itemId);
		}

		public IItemSource GetDefaultItemSource(int itemId)
		{
			List<IItemSource> itemSources = GetItemSources(itemId).ToList();
			return itemSources.FirstOrDefault((IItemSource i) => (i as RecipeSource)?.Recipe.HasBaseIngredients ?? false) ?? itemSources.FirstOrDefault((IItemSource i) => i is TradingPostSource) ?? itemSources.FirstOrDefault();
		}
	}
}
