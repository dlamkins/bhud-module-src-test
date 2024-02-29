using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using MysticCrafting.Models;
using MysticCrafting.Module.Services.Recurring;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Services.API
{
	public class PlayerUnlocksService : RecurringService, IPlayerUnlocksService, IRecurringService
	{
		private readonly Gw2ApiManager _gw2ApiManager;

		public override string Name => Common.LoadingPlayerUnlocks;

		public IList<int> UnlockedMiniatures { get; set; } = new List<int>();


		public IList<int> UnlockedRecipes { get; set; } = new List<int>();


		public IList<int> UnlockedSkins { get; set; } = new List<int>();


		public IDictionary<int, int> UnlockedLegendaries { get; set; } = new Dictionary<int, int>();


		public PlayerUnlocksService(Gw2ApiManager apiManager)
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

		public bool RecipeUnlocked(MysticRecipe recipe)
		{
			if (recipe != null)
			{
				_ = recipe.Id;
				if (!recipe.IsMysticForgeRecipe && recipe.RecipeSheetIds != null && recipe.RecipeSheetIds.Any())
				{
					return RecipeUnlocked(recipe.GameId ?? 999);
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
			if (_gw2ApiManager == null)
			{
				throw new Exception("Gw2ApiManager object is null.");
			}
			if (_gw2ApiManager.HasPermissions(new TokenPermission[3]
			{
				TokenPermission.Account,
				TokenPermission.Unlocks,
				TokenPermission.Inventories
			}))
			{
				Task<IApiV2ObjectList<int>> miniatures = _gw2ApiManager.Gw2ApiClient.V2.Account.Minis.GetAsync();
				Task<IApiV2ObjectList<int>> recipes = _gw2ApiManager.Gw2ApiClient.V2.Account.Recipes.GetAsync();
				Task<IApiV2ObjectList<int>> skins = _gw2ApiManager.Gw2ApiClient.V2.Account.Skins.GetAsync();
				Task<IApiV2ObjectList<AccountLegendaryArmory>> legendaries = _gw2ApiManager.Gw2ApiClient.V2.Account.LegendaryArmory.GetAsync();
				await Task.WhenAll(miniatures, recipes, skins, legendaries);
				UnlockedMiniatures = (await miniatures).ToList();
				UnlockedRecipes = (await recipes).ToList();
				UnlockedSkins = (await skins).ToList();
				UnlockedLegendaries = (await legendaries).ToDictionary((AccountLegendaryArmory x) => x.Id, (AccountLegendaryArmory x) => x.Count);
				return $"{UnlockedRecipes.Count} recipes, {UnlockedSkins.Count} skins, {UnlockedLegendaries.Count} legendaries.";
			}
			throw new Exception("One or more of the required permissions are missing: Account, Inventories, Unlocks.");
		}
	}
}
