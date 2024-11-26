using Atzie.MysticCrafting.Models.Account;

namespace MysticCrafting.Module.Repositories
{
	public interface IAchievementRepository
	{
		Achievement GetAchievement(int itemId);
	}
}
