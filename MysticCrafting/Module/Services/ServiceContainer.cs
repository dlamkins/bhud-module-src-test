using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using MysticCrafting.Module.Repositories;
using MysticCrafting.Module.Repositories.Logging;
using MysticCrafting.Module.Services.API;

namespace MysticCrafting.Module.Services
{
	internal class ServiceContainer
	{
		internal static IDataService DataService { get; set; }

		internal static ISqliteDbService SqliteDbService { get; set; }

		internal static IItemRepository ItemRepository { get; set; }

		internal static IRecipeRepository RecipeRepository { get; set; }

		internal static IMenuRepository MenuItemRepository { get; set; }

		internal static IVendorRepository VendorRepository { get; set; }

		internal static ICurrencyRepository CurrencyRepository { get; set; }

		internal static IFavoritesRepository FavoritesRepository { get; set; }

		internal static IChoiceRepository ChoiceRepository { get; set; }

		internal static IWizardsVaultRepository WizardsVaultRepository { get; set; }

		internal static ITextureRepository TextureRepository { get; set; }

		internal static ITradingPostService TradingPostService { get; set; }

		internal static IPlayerItemService PlayerItemService { get; set; }

		internal static IPlayerUnlocksService PlayerUnlocksService { get; set; }

		internal static IPlayerAchievementsService PlayerAchievementsService { get; set; }

		internal static IItemSourceService ItemSourceService { get; set; }

		internal static IWalletService WalletService { get; set; }

		internal static IAudioService AudioService { get; set; }

		internal static IApiServiceManager ApiServiceManager { get; set; }

		internal static void Register(Gw2ApiManager apiManager, DirectoriesManager directoriesManager, ContentsManager contentsManager)
		{
			DataService = new DataService(directoriesManager);
			SqliteDbService = new SqliteDbService(directoriesManager);
			ItemRepository = new ItemRepository();
			MenuItemRepository = new LoggingMenuRepository(new MenuRepository());
			DataService.RegisterRepository(MenuItemRepository);
			RecipeRepository = new RecipeRepository();
			VendorRepository = new VendorRepository();
			CurrencyRepository = new CurrencyRepository();
			WizardsVaultRepository = new WizardsVaultRepository();
			FavoritesRepository = new LoggingFavoritesRepository(new FavoritesRepository(DataService));
			DataService.RegisterRepository(FavoritesRepository);
			ChoiceRepository = new LoggingChoiceRepository(new ChoiceRepository(DataService));
			DataService.RegisterRepository(ChoiceRepository);
			TextureRepository = new TextureRepository(contentsManager);
			TradingPostService = new TradingPostService(apiManager, SqliteDbService);
			PlayerItemService = new PlayerItemService(apiManager);
			PlayerUnlocksService = new PlayerUnlocksService(apiManager);
			PlayerAchievementsService = new PlayerAchievementsService(apiManager);
			ItemSourceService = new ItemSourceService(TradingPostService, RecipeRepository, VendorRepository, ChoiceRepository, ItemRepository, WizardsVaultRepository);
			WalletService = new WalletService(apiManager, CurrencyRepository, PlayerItemService);
			AudioService = new AudioService(contentsManager);
			ApiServiceManager = new ApiServiceManager();
			ApiServiceManager.RegisterService(PlayerItemService);
			ApiServiceManager.RegisterService(PlayerUnlocksService);
			ApiServiceManager.RegisterService(WalletService);
		}

		internal static async Task LoadAsync()
		{
			if (DataService != null)
			{
				await DataService.DownloadRepositoryFilesAsync();
				await DataService.LoadAsync();
				await SqliteDbService.InitializeDatabaseFile();
				DataService.DeleteFile("currency_data.json");
				DataService.DeleteFile("items_data.json");
				DataService.DeleteFile("items_data_de.json");
				DataService.DeleteFile("items_data_kr.json");
				DataService.DeleteFile("items_data_fr.json");
				DataService.DeleteFile("items_data_sp.json");
				DataService.DeleteFile("items_data_cn.json");
				DataService.DeleteFile("recipes_data.json");
				DataService.DeleteFile("vendor_data.json");
				DataService.DeleteFile("wiki_links_data.json");
				DataService.DeleteFile("data_version.txt");
				DataService.DeleteFile("data_version_1.2.0.txt");
				DataService.DeleteFile("data.json");
				DataService.DeleteFile("data_1.2.0.json");
				DataService.DeleteFile("menu_data.json");
				DataService.DeleteFile("menu_data_new.json");
			}
			if (SqliteDbService != null)
			{
				ItemRepository.Initialize(SqliteDbService);
				RecipeRepository.Initialize(SqliteDbService);
				CurrencyRepository.Initialize(SqliteDbService);
				VendorRepository.Initialize(SqliteDbService);
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
			ItemRepository.Dispose();
			ItemRepository = null;
			RecipeRepository.Dispose();
			RecipeRepository = null;
			VendorRepository.Dispose();
			VendorRepository = null;
			CurrencyRepository.Dispose();
			CurrencyRepository = null;
			MenuItemRepository = null;
			FavoritesRepository = null;
			FavoritesRepository = null;
			ApiServiceManager = null;
			TradingPostService = null;
			PlayerItemService?.Dispose();
			PlayerItemService = null;
			PlayerUnlocksService = null;
			ItemSourceService = null;
			WalletService = null;
			AudioService = null;
		}
	}
}
