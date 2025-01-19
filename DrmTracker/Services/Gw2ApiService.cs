using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace DrmTracker.Services
{
	public class Gw2ApiService
	{
		private readonly Gw2ApiManager _gw2ApiManager;

		private readonly Logger _logger;

		public Gw2ApiService(Gw2ApiManager gw2ApiManager, Logger logger)
		{
			_gw2ApiManager = gw2ApiManager;
			_logger = logger;
		}

		public async Task<string> GetAccountName()
		{
			if (!_gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)_gw2ApiManager.get_Permissions()))
			{
				_logger.Warn("Permissions not granted.");
				return string.Empty;
			}
			try
			{
				Account obj = await ((IBlobClient<Account>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()).GetAsync(default(CancellationToken));
				return (obj != null) ? obj.get_Name() : null;
			}
			catch (Exception ex)
			{
				_logger.Warn("Error while getting account name : " + ex.Message);
				return null;
			}
		}

		public async Task<List<AccountAchievement>> GetAchievements(List<int> ids)
		{
			if (!_gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)_gw2ApiManager.get_Permissions()))
			{
				_logger.Warn("Permissions not granted.");
				return null;
			}
			try
			{
				return ((IEnumerable<AccountAchievement>)(await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Achievements()).GetAsync(default(CancellationToken)))).Where((AccountAchievement a) => ids.Contains(a.get_Id())).ToList();
			}
			catch (Exception ex)
			{
				_logger.Warn("Error while getting achievements : " + ex.Message);
				return null;
			}
		}
	}
}
