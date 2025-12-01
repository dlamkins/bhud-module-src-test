using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;
using flakysalt.CharacterKeybinds.Data;

namespace flakysalt.CharacterKeybinds.Services
{
	public class Gw2ApiService
	{
		private readonly Gw2ApiManager _apiManager;

		private readonly Logger _logger = Logger.GetLogger<Gw2ApiService>();

		private readonly string apiStatusWebsiteUrl = "https://status.gw2efficiency.com/api/";

		private ApiStatusResponse statusResponse;

		private DateTime _lastApiStatusCheck = DateTime.MinValue;

		public event EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>> SubtokenUpdated;

		public Gw2ApiService(Gw2ApiManager apiManager)
		{
			_apiManager = apiManager;
			_apiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnApiManagerSubtokenUpdated);
		}

		private void OnApiManagerSubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			this.SubtokenUpdated?.Invoke(sender, e);
		}

		public async Task<bool> IsApiAvailable()
		{
			if (_lastApiStatusCheck.AddMinutes(5.0) < DateTime.Now)
			{
				await UpdateApiStatus();
			}
			int num = statusResponse.Data.Count((EndpointStatus e) => e.Status == 200);
			int totalCount = statusResponse.Data.Count;
			return (double)num / (double)totalCount > 0.9;
		}

		private async Task UpdateApiStatus()
		{
			HttpWebResponse response = (HttpWebResponse)WebRequest.Create(apiStatusWebsiteUrl).GetResponse();
			using Stream stream = response.GetResponseStream();
			using StreamReader reader = new StreamReader(stream);
			_lastApiStatusCheck = DateTime.Now;
			statusResponse = JsonConvert.DeserializeObject<ApiStatusResponse>(await reader.ReadToEndAsync());
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

		public bool HasSubtoken()
		{
			return _apiManager.get_HasSubtoken();
		}

		public async Task<IEnumerable<Character>> GetCharactersAsync()
		{
			try
			{
				return (IEnumerable<Character>)(await ((IAllExpandableClient<Character>)(object)_apiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(default(CancellationToken)));
			}
			catch (Exception ex)
			{
				_logger.Info(ex, "Failed to fetch characters from API");
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
				_logger.Info(ex, "Failed to fetch professions from API");
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
				_logger.Info(ex, "Failed to fetch specializations from API");
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
				_logger.Info(ex, $"Failed to fetch specialization {specializationId} from API");
				throw;
			}
		}
	}
}
