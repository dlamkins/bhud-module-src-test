using Atzie.MysticCrafting.Models.Crafting;

namespace MysticCrafting.Module.Services.API
{
	public interface IPlayerUnlocksService : IApiService
	{
		bool RecipeUnlocked(Recipe recipe);

		bool RecipeUnlocked(int itemId);

		bool ItemUnlocked(int itemId);

		bool SkinUnlocked(int itemId);

		bool LegendaryUnlocked(int itemId);

		int LegendaryUnlockedCount(int itemId);
	}
}
