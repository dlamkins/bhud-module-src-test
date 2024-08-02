using Atzie.MysticCrafting.Models.Account;

namespace MysticCrafting.Module.Services.API
{
	public interface IPlayerAchievementsService : IApiService
	{
		PlayerAchievement GetAchievement(int id);
	}
}
