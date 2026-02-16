using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Newtonsoft.Json;
using RaidWeekPlanner.Domain;
using RaidWeekPlanner.UI.Controls;

namespace RaidWeekPlanner.Services
{
	public class BusinessService
	{
		private readonly ContentsManager _contentsManager;

		private readonly Gw2ApiService _gw2ApiService;

		private readonly Func<LoadingSpinner> _getSpinner;

		private readonly CornerIcon _cornerIcon;

		private readonly Logger _logger;

		private Data _data;

		private Rotation _rotation;

		private List<string> _raidClears;

		private string _accountName { get; set; }

		public BusinessService(ContentsManager contentsManager, Gw2ApiService gw2ApiService, Func<LoadingSpinner> getSpinner, CornerIcon cornerIcon, Logger logger)
		{
			_contentsManager = contentsManager;
			_gw2ApiService = gw2ApiService;
			_getSpinner = getSpinner;
			_cornerIcon = cornerIcon;
			_logger = logger;
		}

		public void LoadData()
		{
			using (StreamReader streamReader = new StreamReader(_contentsManager.GetFileStream("data.json")))
			{
				string content2 = streamReader.ReadToEnd();
				_data = JsonConvert.DeserializeObject<Data>(content2);
			}
			using StreamReader sr = new StreamReader(_contentsManager.GetFileStream("rotation.json"));
			string content = sr.ReadToEnd();
			_rotation = JsonConvert.DeserializeObject<Rotation>(content);
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

		public async Task<List<string>> GetAccountClears(bool forceRefresh = false)
		{
			if (_raidClears == null || forceRefresh)
			{
				await RefreshBaseData();
			}
			return _raidClears;
		}

		public List<Area> GetAreas()
		{
			return _data.Areas;
		}

		public List<string> GetNeverOnTheMenu()
		{
			return (from e in _data.Areas.SelectMany((Area a) => a.Encounters)
				where e.IsDisabled
				select e.Key).ToList();
		}

		public List<string> GetEventsForCurrentWeek()
		{
			DateTime monday = GetMonday(DateTime.UtcNow);
			List<string> weekEvents = new List<string>();
			for (int dayOffset = 0; dayOffset < 7; dayOffset++)
			{
				DateTime currentDay = monday.AddDays(dayOffset);
				weekEvents.AddRange(GetEventsForDate(currentDay));
			}
			return weekEvents.Distinct().ToList();
		}

		private async Task<bool> RefreshAccountName()
		{
			_accountName = await _gw2ApiService.GetAccountName();
			_cornerIcon.UpdateWarningState(string.IsNullOrWhiteSpace(_accountName));
			return !string.IsNullOrWhiteSpace(_accountName);
		}

		private async Task RefreshProgression()
		{
			_raidClears = await _gw2ApiService.GetClears();
		}

		private DateTime GetMonday(DateTime date)
		{
			int daysFromMonday = (int)(date.DayOfWeek - 1 + 7) % 7;
			return date.Date.AddDays(-daysFromMonday).AddHours(1.0);
		}

		private List<string> GetEventsForDate(DateTime targetDate)
		{
			int daysPassed = (targetDate - _rotation.StartDate).Days;
			if (daysPassed < 0)
			{
				throw new ArgumentException("La date cible ne peut pas être antérieure à la date de départ");
			}
			List<string> results = new List<string>();
			List<string>[] eventLists = _rotation.GetLists();
			for (int i = 0; i < eventLists.Length; i++)
			{
				if (eventLists[i] != null && eventLists[i].Count > 0)
				{
					int index = daysPassed % eventLists[i].Count;
					results.Add(eventLists[i][index]);
				}
			}
			return results;
		}
	}
}
