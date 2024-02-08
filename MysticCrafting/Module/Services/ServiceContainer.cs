using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using JsonFlatFileDataStore;
using MysticCrafting.Module.Repositories;
using MysticCrafting.Module.Repositories.Logging;
using MysticCrafting.Module.Services.API;
using MysticCrafting.Module.Services.Recurring;

namespace MysticCrafting.Module.Services
{
	internal class ServiceContainer
	{
		internal static IDataService DataService { get; set; }

		internal static IItemRepository ItemRepository { get; set; }

		internal static IRecipeRepository RecipeRepository { get; set; }

		internal static IMenuRepository MenuItemRepository { get; set; }

		internal static IVendorRepository VendorRepository { get; set; }

		internal static ICurrencyRepository CurrencyRepository { get; set; }

		internal static IWikiLinkRepository WikiLinkRepository { get; set; }

		internal static IFavoritesRepository FavoritesRepository { get; set; }

		internal static IChoiceRepository ChoiceRepository { get; set; }

		internal static ITextureRepository TextureRepository { get; set; }

		internal static ITradingPostService TradingPostService { get; set; }

		internal static IPlayerItemService PlayerItemService { get; set; }

		internal static IPlayerUnlocksService PlayerUnlocksService { get; set; }

		internal static IItemSourceService ItemSourceService { get; set; }

		internal static IWalletService WalletService { get; set; }

		internal static IAudioService AudioService { get; set; }

		internal static DataStore Store { get; private set; }

		internal static List<IRecurringService> RecurringServices { get; set; } = new List<IRecurringService>();


		internal static void Register(Gw2ApiManager apiManager, DirectoriesManager directoriesManager, ContentsManager contentsManager)
		{
			DataService = new DataService(directoriesManager);
			ItemRepository = new LoggingItemRepository(new ItemRepository(DataService));
			DataService.RegisterRepository(ItemRepository);
			RecipeRepository = new LoggingRecipeRepository(new RecipeRepository(DataService));
			DataService.RegisterRepository(RecipeRepository);
			MenuItemRepository = new LoggingMenuRepository(new MenuRepository(DataService));
			DataService.RegisterRepository(MenuItemRepository);
			VendorRepository = new LoggingVendorRepository(new VendorRepository(DataService));
			DataService.RegisterRepository(VendorRepository);
			CurrencyRepository = new FlatCurrencyRepository(DataService);
			DataService.RegisterRepository(CurrencyRepository);
			WikiLinkRepository = new LoggingWikiLinkRepository(new WikiLinkRepository(DataService));
			DataService.RegisterRepository(WikiLinkRepository);
			FavoritesRepository = new LoggingFavoritesRepository(new FavoritesRepository(DataService));
			DataService.RegisterRepository(FavoritesRepository);
			ChoiceRepository = new LoggingChoiceRepository(new ChoiceRepository(DataService));
			DataService.RegisterRepository(ChoiceRepository);
			TextureRepository = new TextureRepository(contentsManager);
			TradingPostService = new TradingPostService(apiManager, ItemRepository);
			PlayerItemService = new PlayerItemService(apiManager);
			PlayerUnlocksService = new PlayerUnlocksService(apiManager);
			ItemSourceService = new ItemSourceService(TradingPostService, RecipeRepository, VendorRepository, ChoiceRepository, ItemRepository);
			WalletService = new WalletService(apiManager, CurrencyRepository, PlayerItemService);
			AudioService = new AudioService(contentsManager);
			RecurringServices.Add(TradingPostService);
			RecurringServices.Add(PlayerItemService);
			RecurringServices.Add(PlayerUnlocksService);
			RecurringServices.Add(WalletService);
		}

		internal static async Task LoadAsync()
		{
			if (DataService != null)
			{
				await DataService.DownloadRepositoryFilesAsync();
				Store = new DataStore(DataService.GetFilePath("data.json"));
				await DataService.LoadAsync();
			}
			if (AudioService != null)
			{
				AudioService.Load();
			}
			await Task.CompletedTask;
		}

		internal static void Dispose()
		{
			DataService = null;
			ItemRepository = null;
			RecipeRepository = null;
			MenuItemRepository = null;
			FavoritesRepository = null;
			foreach (IRecurringService recurringService in RecurringServices)
			{
				recurringService?.StopTimedLoading();
			}
			RecurringServices = new List<IRecurringService>();
			TradingPostService = null;
			PlayerItemService = null;
			PlayerUnlocksService = null;
			ItemSourceService = null;
			WalletService = null;
			AudioService = null;
		}
	}
}
