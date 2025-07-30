using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Soeed.WhatRoleAmIPlaying.Models;

namespace Soeed.WhatRoleAmIPlaying.Services
{
	public class Gw2ApiService
	{
		private static readonly Logger Logger = Logger.GetLogger<Gw2ApiService>();

		private readonly Gw2ApiManager _apiManager;

		public Gw2ApiService(Gw2ApiManager apiManager)
		{
			_apiManager = apiManager;
		}

		public async Task<List<Character>> GetUserCharactersAsync()
		{
			try
			{
				return ((IEnumerable<Character>)(await ((IAllExpandableClient<Character>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(default(CancellationToken)))).ToList();
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to get user characters");
				return new List<Character>();
			}
		}

		public Task<HashSet<string>> GetUnlockedEliteSpecsAsync()
		{
			HashSet<string> result = new HashSet<string>
			{
				"Dragonhunter", "Firebrand", "Willbender", "Berserker", "Spellbreaker", "Bladesworn", "Scrapper", "Holosmith", "Mechanist", "Druid",
				"Soulbeast", "Untamed", "Daredevil", "Deadeye", "Specter", "Tempest", "Weaver", "Catalyst", "Chronomancer", "Mirage",
				"Virtuoso", "Reaper", "Scourge", "Harbinger", "Herald", "Renegade", "Vindicator"
			};
			Logger.Info("Using all elite specs as unlocked (development mode)");
			return Task.FromResult(result);
		}

		public async Task<bool> HasUnlockedEliteSpecsAsync()
		{
			return (await GetUnlockedEliteSpecsAsync()).Count > 0;
		}

		public async Task<List<RoleSuggestion>> GetAvailableRolesAsync(RoleType roleType)
		{
			HashSet<string> unlockedSpecs = await GetUnlockedEliteSpecsAsync();
			List<RoleSuggestion> allRoles = WhatRoleAmIPlayingModule.RoleConfig.GetAllRoles(roleType);
			List<RoleSuggestion> availableRoles = allRoles.Where((RoleSuggestion role) => unlockedSpecs.Contains(role.EliteSpec)).ToList();
			Logger.Info($"Found {availableRoles.Count} available roles for {roleType} (from {allRoles.Count} total roles)");
			return availableRoles;
		}

		public async Task<int> GetCharacterCountAsync()
		{
			try
			{
				return (await GetUserCharactersAsync()).Count;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to get character count");
				return 0;
			}
		}
	}
}
