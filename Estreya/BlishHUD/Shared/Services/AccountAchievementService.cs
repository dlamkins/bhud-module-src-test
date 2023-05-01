using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.Shared.Services
{
	public class AccountAchievementService : APIService<AccountAchievement>
	{
		public AccountAchievementService(Gw2ApiManager apiManager, APIServiceConfiguration configuration)
			: base(apiManager, configuration)
		{
		}

		protected override async Task<List<AccountAchievement>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress)
		{
			progress.Report("Loading achievements...");
			return ((IEnumerable<AccountAchievement>)(await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Achievements()).GetAsync(_cancellationTokenSource.Token))).ToList();
		}
	}
}
