using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atzie.MysticCrafting.Models.Account;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Services.API
{
	public class PlayerAchievementsService : ApiService, IPlayerAchievementsService, IApiService
	{
		private readonly Gw2ApiManager _gw2ApiManager;

		public override string Name => Common.LoadingPlayerAchievements;

		public IList<PlayerAchievement> Achievements { get; set; }

		public override List<TokenPermission> Permissions => new List<TokenPermission>
		{
			(TokenPermission)1,
			(TokenPermission)6
		};

		public PlayerAchievementsService(Gw2ApiManager apiManager)
			: base(apiManager)
		{
			_gw2ApiManager = apiManager;
		}

		public override async Task<string> LoadAsync()
		{
			if (!_gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)Permissions))
			{
				return string.Empty;
			}
			IApiV2ObjectList<AccountAchievement> unlockedAchievements = await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Achievements()).GetAsync(default(CancellationToken));
			Achievements = ((IEnumerable<AccountAchievement>)unlockedAchievements).Select(PlayerAchievement.From).ToList();
			return $"{((IReadOnlyCollection<AccountAchievement>)unlockedAchievements).Count} achievements.";
		}

		public PlayerAchievement GetAchievement(int id)
		{
			return Achievements.FirstOrDefault((PlayerAchievement a) => a.Id == id);
		}
	}
}
