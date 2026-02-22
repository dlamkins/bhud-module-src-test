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

namespace RaidWeekPlanner.Services
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

		public async Task<List<string>> GetClears()
		{
			if (!_gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)_gw2ApiManager.get_Permissions()))
			{
				_logger.Warn("Permissions not granted.");
				return null;
			}
			try
			{
				IApiV2ObjectList<string> raidData = await ((IBlobClient<IApiV2ObjectList<string>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Raids()).GetAsync(default(CancellationToken));
				List<string> strikeData = await GetStrikeClear();
				if (raidData == null && strikeData == null)
				{
					return null;
				}
				List<string> obj = ((IEnumerable<string>)raidData)?.Select((string d) => d)?.ToList() ?? new List<string>();
				obj.AddRange(strikeData ?? new List<string>());
				return obj;
			}
			catch (Exception ex)
			{
				_logger.Warn("Error while getting raid clears : " + ex.Message);
				return null;
			}
		}

		private async Task<List<string>> GetStrikeClear()
		{
			AccountAchievement strikeWeeklyClearAchievement = ((IEnumerable<AccountAchievement>)(await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)_gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Achievements()).GetAsync(default(CancellationToken))))?.FirstOrDefault((AccountAchievement a) => a.get_Id() == 9125);
			if (strikeWeeklyClearAchievement == null || strikeWeeklyClearAchievement.get_Bits() == null)
			{
				return null;
			}
			return strikeWeeklyClearAchievement.get_Bits().Select(GetStrikeName).ToList();
		}

		private string GetStrikeName(int bit)
		{
			return bit switch
			{
				0 => "shiverpeaks_pass", 
				1 => "fraenir_of_jormag", 
				2 => "voice_and_claw", 
				3 => "whisper_of_jormag", 
				4 => "boneskinner", 
				5 => "cold_war", 
				6 => "aetherblade_hideout", 
				7 => "xunlai_jade_junkyard", 
				8 => "kaineng_overlook", 
				9 => "harvest_temple", 
				10 => "cosmic_observatory", 
				11 => "temple_of_febe", 
				12 => "old_lion_court", 
				13 => "kela", 
				_ => string.Empty, 
			};
		}
	}
}
