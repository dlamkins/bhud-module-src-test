using System.Threading.Tasks;
using Atzie.MysticCrafting.Models.Account;

namespace MysticCrafting.Module.Repositories
{
	public interface IAchievementRepository
	{
		Achievement GetAchievement(int itemId);

		Task LoadAchievementsAsync();
	}
}
