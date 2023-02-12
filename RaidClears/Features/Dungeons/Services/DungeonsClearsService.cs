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

namespace RaidClears.Features.Dungeons.Services
{
	public class DungeonsClearsService
	{
		private const int FREQUENTER_ACHIEVEMENT_ID = 2963;

		private readonly List<TokenPermission> _necessaryApiTokenPermissions = new List<TokenPermission>
		{
			(TokenPermission)1,
			(TokenPermission)6
		};

		public async Task<List<string>> GetFrequenterPaths()
		{
			Gw2ApiManager gw2ApiManager = Service.Gw2ApiManager;
			Logger logger = Logger.GetLogger<Module>();
			if (!gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)_necessaryApiTokenPermissions))
			{
				logger.Warn("HasPermissions() returned false. Possible reasons: API subToken does not have the necessary permissions: " + string.Join(", ", _necessaryApiTokenPermissions) + ". Or module did not get API subToken from Blish yet. Or API key is missing.");
				return new List<string>();
			}
			try
			{
				AccountAchievement frequenter = ((IEnumerable<AccountAchievement>)(await ((IBlobClient<IApiV2ObjectList<AccountAchievement>>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Achievements()).GetAsync(default(CancellationToken)))).ToList().Find((AccountAchievement x) => x.get_Id() == 2963);
				List<string> list = new List<string>();
				if (frequenter != null)
				{
					list = ConvertFrequenterToPathId(frequenter.get_Bits()).ToList();
				}
				return list;
			}
			catch (Exception e)
			{
				logger.Warn(e, "Could not get current clears from API");
				return new List<string>();
			}
		}

		public async Task<List<string>> GetClearsFromApi()
		{
			Gw2ApiManager gw2ApiManager = Service.Gw2ApiManager;
			Logger logger = Logger.GetLogger<Module>();
			if (!gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)_necessaryApiTokenPermissions))
			{
				logger.Warn("HasPermissions() returned false. Possible reasons: API subToken does not have the necessary permissions: " + string.Join(", ", _necessaryApiTokenPermissions) + ". Or module did not get API subToken from Blish yet. Or API key is missing.");
				return new List<string>();
			}
			try
			{
				return ((IEnumerable<string>)(await ((IBlobClient<IApiV2ObjectList<string>>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
					.get_Dungeons()).GetAsync(default(CancellationToken)))).ToList();
			}
			catch (Exception e)
			{
				logger.Warn(e, "Could not get current clears from API");
				return new List<string>();
			}
		}

		private static IEnumerable<string> ConvertFrequenterToPathId(IEnumerable<int>? frequentedPaths)
		{
			return frequentedPaths?.Select(FrequentIdToPathString) ?? new List<string>();
		}

		private static string FrequentIdToPathString(int id)
		{
			return id switch
			{
				0 => "coe_story", 
				1 => "submarine", 
				2 => "teleporter", 
				3 => "front_door", 
				4 => "ac_story", 
				5 => "hodgins", 
				6 => "detha", 
				7 => "tzark", 
				8 => "jotun", 
				9 => "mursaat", 
				10 => "forgotten", 
				11 => "seer", 
				12 => "cm_story", 
				13 => "asura", 
				14 => "seraph", 
				15 => "butler", 
				16 => "se_story", 
				17 => "fergg", 
				18 => "rasalov", 
				19 => "koptev", 
				20 => "ta_story", 
				21 => "leurent", 
				22 => "vevina", 
				23 => "aetherpath", 
				24 => "hotw_story", 
				25 => "butcher", 
				26 => "plunderer", 
				27 => "zealot", 
				28 => "cof_story", 
				29 => "ferrah", 
				30 => "magg", 
				31 => "rhiannon", 
				_ => string.Empty, 
			};
		}
	}
}
