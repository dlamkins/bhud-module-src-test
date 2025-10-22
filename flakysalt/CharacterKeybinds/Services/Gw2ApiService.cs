using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace flakysalt.CharacterKeybinds.Services
{
	public class Gw2ApiService
	{
		private readonly Gw2ApiManager _apiManager;

		private readonly Logger _logger = Logger.GetLogger<Gw2ApiService>();

		public Gw2ApiService(Gw2ApiManager apiManager)
		{
			_apiManager = apiManager;
		}

		public bool HasRequiredPermissions()
		{
			List<TokenPermission> apiKeyPermissions = new List<TokenPermission>
			{
				(TokenPermission)1,
				(TokenPermission)3
			};
			return _apiManager.HasPermissions((IEnumerable<TokenPermission>)apiKeyPermissions);
		}

		public async Task<IEnumerable<Character>> GetCharactersAsync()
		{
			try
			{
				return (IEnumerable<Character>)(await ((IAllExpandableClient<Character>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(default(CancellationToken)));
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Failed to fetch characters from API");
				throw;
			}
		}

		public async Task<IEnumerable<Profession>> GetProfessionsAsync()
		{
			try
			{
				return (IEnumerable<Profession>)(await ((IAllExpandableClient<Profession>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Professions()).AllAsync(default(CancellationToken)));
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Failed to fetch professions from API");
				throw;
			}
		}

		public async Task<IEnumerable<Specialization>> GetSpecializationsAsync()
		{
			try
			{
				return (IEnumerable<Specialization>)(await ((IAllExpandableClient<Specialization>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Specializations()).AllAsync(default(CancellationToken)));
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Failed to fetch specializations from API");
				throw;
			}
		}

		public async Task<Specialization> GetSpecializationAsync(int specializationId)
		{
			try
			{
				return await ((IBulkExpandableClient<Specialization, int>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Specializations()).GetAsync(specializationId, default(CancellationToken));
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Failed to fetch specialization {specializationId} from API");
				throw;
			}
		}
	}
}
