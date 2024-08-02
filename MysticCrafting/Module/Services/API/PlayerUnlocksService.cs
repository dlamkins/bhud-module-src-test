using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atzie.MysticCrafting.Models.Crafting;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Services.API
{
	public class PlayerUnlocksService : ApiService, IPlayerUnlocksService, IApiService
	{
		private readonly Gw2ApiManager _gw2ApiManager;

		public override string Name => Common.LoadingPlayerUnlocks;

		public IList<int> UnlockedMiniatures { get; set; } = new List<int>();


		public IList<int> UnlockedRecipes { get; set; } = new List<int>();


		public IList<int> UnlockedSkins { get; set; } = new List<int>();


		public IDictionary<int, int> UnlockedLegendaries { get; set; } = new Dictionary<int, int>();


		public override List<TokenPermission> Permissions => new List<TokenPermission>
		{
			(TokenPermission)1,
			(TokenPermission)5,
			(TokenPermission)9
		};

		public PlayerUnlocksService(Gw2ApiManager apiManager)
			: base(apiManager)
		{
			_gw2ApiManager = apiManager;
		}

		public bool MiniUnlocked(int itemId)
		{
			return UnlockedMiniatures.Contains(itemId);
		}

		public bool RecipeUnlocked(int itemId)
		{
			return UnlockedRecipes.Contains(itemId);
		}

		public bool RecipeUnlocked(Atzie.MysticCrafting.Models.Crafting.Recipe recipe)
		{
			if (recipe != null)
			{
				_ = recipe.Id;
				if (!recipe.IsMysticForgeRecipe && recipe.RecipeSheets != null && recipe.RecipeSheets.Any())
				{
					return RecipeUnlocked(recipe.Id);
				}
			}
			return true;
		}

		public bool ItemUnlocked(int itemId)
		{
			if (!SkinUnlocked(itemId) && !LegendaryUnlocked(itemId))
			{
				return MiniUnlocked(itemId);
			}
			return true;
		}

		public bool SkinUnlocked(int itemId)
		{
			return UnlockedSkins.Contains(itemId);
		}

		public bool LegendaryUnlocked(int itemId)
		{
			return UnlockedLegendaries.ContainsKey(itemId);
		}

		public int LegendaryUnlockedCount(int itemId)
		{
			UnlockedLegendaries.TryGetValue(itemId, out var count);
			return count;
		}

		public override async Task<string> LoadAsync()
		{
			if (!_gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)Permissions))
			{
				return string.Empty;
			}
			Task<IApiV2ObjectList<int>> miniatures = ((IBlobClient<IApiV2ObjectList<int>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Minis()).GetAsync(default(CancellationToken));
			Task<IApiV2ObjectList<int>> recipes = ((IBlobClient<IApiV2ObjectList<int>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Recipes()).GetAsync(default(CancellationToken));
			Task<IApiV2ObjectList<int>> skins = ((IBlobClient<IApiV2ObjectList<int>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Skins()).GetAsync(default(CancellationToken));
			Task<IApiV2ObjectList<AccountLegendaryArmory>> legendaries = ((IBlobClient<IApiV2ObjectList<AccountLegendaryArmory>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_LegendaryArmory()).GetAsync(default(CancellationToken));
			await Task.WhenAll(miniatures, recipes, skins, legendaries);
			UnlockedMiniatures = ((IEnumerable<int>)(await miniatures)).ToList();
			UnlockedRecipes = ((IEnumerable<int>)(await recipes)).ToList();
			UnlockedSkins = ((IEnumerable<int>)(await skins)).ToList();
			UnlockedLegendaries = ((IEnumerable<AccountLegendaryArmory>)(await legendaries)).ToDictionary((AccountLegendaryArmory x) => x.get_Id(), (AccountLegendaryArmory x) => x.get_Count());
			return $"{UnlockedRecipes.Count} recipes, {UnlockedSkins.Count} skins, {UnlockedLegendaries.Count} legendaries.";
		}
	}
}
