using MysticCrafting.Module.Services.Recurring;

namespace MysticCrafting.Module.Services.API
{
	public interface IPlayerUnlocksService : IRecurringService
	{
		bool RecipeUnlocked(int itemId);

		bool SkinUnlocked(int itemId);

		bool LegendaryUnlocked(int itemId);

		int LegendaryUnlockedCount(int itemId);
	}
}
