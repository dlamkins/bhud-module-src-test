using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using DrmTracker.Domain;
using DrmTracker.UI.Controls;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;

namespace DrmTracker.Services
{
	public class BusinessService
	{
		private readonly ModuleSettings _moduleSettings;

		private readonly ContentsManager _contentsManager;

		private readonly Gw2ApiService _gw2ApiService;

		private readonly Func<LoadingSpinner> _getSpinner;

		private readonly CornerIcon _cornerIcon;

		private readonly Logger _logger;

		private Data _data;

		private List<DrmProgression> _accountDrm;

		private string _accountName { get; set; }

		public BusinessService(ModuleSettings moduleSettings, ContentsManager contentsManager, Gw2ApiService gw2ApiService, Func<LoadingSpinner> getSpinner, CornerIcon cornerIcon, Logger logger)
		{
			_moduleSettings = moduleSettings;
			_contentsManager = contentsManager;
			_gw2ApiService = gw2ApiService;
			_getSpinner = getSpinner;
			_cornerIcon = cornerIcon;
			_logger = logger;
		}

		public void LoadData()
		{
			using StreamReader sr = new StreamReader(_contentsManager.GetFileStream("data.json"));
			string content = sr.ReadToEnd();
			_data = JsonConvert.DeserializeObject<Data>(content);
		}

		public async Task RefreshBaseData()
		{
			Func<LoadingSpinner> getSpinner = _getSpinner;
			if (getSpinner != null)
			{
				LoadingSpinner loadingSpinner = getSpinner();
				if (loadingSpinner != null)
				{
					((Control)loadingSpinner).Show();
				}
			}
			await RefreshAccountName();
			await RefreshProgression();
			Func<LoadingSpinner> getSpinner2 = _getSpinner;
			if (getSpinner2 != null)
			{
				LoadingSpinner loadingSpinner2 = getSpinner2();
				if (loadingSpinner2 != null)
				{
					((Control)loadingSpinner2).Hide();
				}
			}
		}

		public async Task<List<DrmProgression>> GetAccountDrm(bool forceRefresh = false)
		{
			if (_accountDrm == null || forceRefresh)
			{
				await RefreshBaseData();
			}
			return _accountDrm;
		}

		public List<Faction> GetFactions()
		{
			return _data.Factions;
		}

		public List<Map> GetMaps()
		{
			return _data.Maps;
		}

		public List<Drm> GetDrms()
		{
			return _data.Drms;
		}

		private async Task<bool> RefreshAccountName()
		{
			_accountName = await _gw2ApiService.GetAccountName();
			_cornerIcon.UpdateWarningState(string.IsNullOrWhiteSpace(_accountName));
			return !string.IsNullOrWhiteSpace(_accountName);
		}

		private async Task RefreshProgression()
		{
			Dictionary<int, ApiIds> mapApiIdsDict = _data.Drms.ToDictionary((Drm drm) => drm.Map, (Drm drm) => drm.ApiIds);
			List<int> allApiIds = mapApiIdsDict.Values.SelectMany((ApiIds m) => m.GetIds()).ToList();
			List<AccountAchievement> accountProgression = await _gw2ApiService.GetAchievements(allApiIds);
			if (accountProgression == null)
			{
				return;
			}
			_accountDrm = mapApiIdsDict.Select((KeyValuePair<int, ApiIds> m) => new DrmProgression
			{
				Map = m.Key,
				AccountAchievement = new DrmAchievements
				{
					Clear = accountProgression.FirstOrDefault((AccountAchievement ap) => ap.get_Id() == m.Value.Clear),
					Factions = accountProgression.FirstOrDefault((AccountAchievement ap) => ap.get_Id() == m.Value.Factions),
					FullCM = accountProgression.FirstOrDefault((AccountAchievement ap) => ap.get_Id() == m.Value.FullCM)
				}
			}).ToList();
		}
	}
}
