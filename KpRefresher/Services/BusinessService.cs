using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using KpRefresher.Domain;
using KpRefresher.Domain.Attributes;
using KpRefresher.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KpRefresher.Services
{
	public class BusinessService
	{
		private readonly ModuleSettings _moduleSettings;

		private readonly Gw2ApiService _gw2ApiService;

		private readonly KpMeService _kpMeService;

		private readonly Func<LoadingSpinner> _getSpinner;

		private string _accountName { get; set; }

		private string _kpId { get; set; }

		private bool _isRefreshingKpData { get; set; }

		private List<RaidBoss> _raidBossNames { get; set; }

		private DateTime? _lastRefresh { get; set; }

		private DateTime? _refreshAvailable => _lastRefresh?.AddMinutes(61.0);

		private List<int> _raidMapIds { get; set; }

		private List<int> _strikeMapIds { get; set; }

		private bool _playerWasInInstance { get; set; }

		public List<string> LinkedKpId { get; set; }

		public bool RefreshScheduled { get; set; }

		public double ScheduleTimer { get; set; }

		public double ScheduleTimerEndValue { get; set; }

		public bool NotificationNextRefreshAvailabledActivated { get; set; }

		public double NotificationNextRefreshAvailabledTimer { get; set; }

		public double NotificationNextRefreshAvailabledTimerEndValue { get; set; }

		public BusinessService(ModuleSettings moduleSettings, Gw2ApiService gw2ApiService, KpMeService kpMeService, Func<LoadingSpinner> getSpinner)
		{
			_moduleSettings = moduleSettings;
			_gw2ApiService = gw2ApiService;
			_kpMeService = kpMeService;
			_getSpinner = getSpinner;
			_raidBossNames = Enum.GetValues(typeof(RaidBoss)).Cast<RaidBoss>().ToList();
			_raidMapIds = (from RaidMap m in Enum.GetValues(typeof(RaidMap))
				select (int)m).ToList();
			_strikeMapIds = (from StrikeMap m in Enum.GetValues(typeof(StrikeMap))
				select (int)m).ToList();
		}

		public async Task RefreshBaseData()
		{
			Func<LoadingSpinner> getSpinner = _getSpinner;
			if (getSpinner != null)
			{
				LoadingSpinner obj = getSpinner();
				if (obj != null)
				{
					((Control)obj).Show();
				}
			}
			await RefreshAccountName(isFromInit: true);
			await RefreshKpMeData(isFromInit: true);
			Func<LoadingSpinner> getSpinner2 = _getSpinner;
			if (getSpinner2 != null)
			{
				LoadingSpinner obj2 = getSpinner2();
				if (obj2 != null)
				{
					((Control)obj2).Hide();
				}
			}
		}

		public async Task RefreshKillproofMe(bool fromUpdateLoop = false)
		{
			CancelSchedule();
			if (!(await DataLoaded()))
			{
				ScreenNotification.ShowNotification("[KpRefresher] KillProof.me refresh was not available\nPlease retry later.", (NotificationType)1, (Texture2D)null, 4);
				return;
			}
			if (DateTime.UtcNow < _refreshAvailable.Value)
			{
				double minutesUntilRefreshAvailable = Math.Ceiling((_refreshAvailable.Value - DateTime.UtcNow).TotalMinutes);
				string baseMsg = $"[KpRefresher] Next refresh available in {minutesUntilRefreshAvailable} minutes";
				if (_moduleSettings.EnableAutoRetry.get_Value())
				{
					ScheduleRefresh(minutesUntilRefreshAvailable);
					if (!fromUpdateLoop || _moduleSettings.ShowScheduleNotification.get_Value())
					{
						ScreenNotification.ShowNotification(baseMsg + "\nA new try has been scheduled.", (NotificationType)1, (Texture2D)null, 4);
					}
				}
				else
				{
					ScreenNotification.ShowNotification(baseMsg, (NotificationType)1, (Texture2D)null, 4);
				}
				return;
			}
			if (_moduleSettings.EnableRefreshOnKill.get_Value() && !(await CheckRaidClears()))
			{
				ScreenNotification.ShowNotification("[KpRefresher] No new clear validating settings, refresh aborted !", (NotificationType)0, (Texture2D)null, 4);
				return;
			}
			bool? refreshed = await _kpMeService.RefreshApi(_kpId);
			if (refreshed.HasValue && refreshed.Value)
			{
				_lastRefresh = DateTime.UtcNow;
				ScreenNotification.ShowNotification("[KpRefresher] KillProof.me refresh successful !", (NotificationType)0, (Texture2D)null, 4);
			}
			else
			{
				if (!refreshed.HasValue || refreshed.Value)
				{
					return;
				}
				await UpdateLastRefresh();
				if (_moduleSettings.EnableAutoRetry.get_Value())
				{
					ScheduleRefresh();
					if (_moduleSettings.ShowScheduleNotification.get_Value())
					{
						ScreenNotification.ShowNotification("[KpRefresher] KillProof.me refresh was not available\nAuto-retry in 5 minutes.", (NotificationType)1, (Texture2D)null, 4);
					}
				}
				else
				{
					ScreenNotification.ShowNotification("[KpRefresher] KillProof.me refresh was not available\nPlease retry later.", (NotificationType)1, (Texture2D)null, 4);
				}
			}
		}

		public void CancelSchedule()
		{
			RefreshScheduled = false;
			ScheduleTimer = 0.0;
			ScheduleTimerEndValue = double.MaxValue;
		}

		public void MapChanged()
		{
			int mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			if (_raidMapIds.Contains(mapId) || _strikeMapIds.Contains(mapId))
			{
				_playerWasInInstance = true;
			}
			else if (_playerWasInInstance)
			{
				_playerWasInInstance = false;
				ScheduleRefresh(_moduleSettings.DelayBeforeRefreshOnMapChange.get_Value());
				ScreenNotification.ShowNotification(string.Format("[KpRefresher] Instance exit detected, refresh scheduled in {0} minute{1}", _moduleSettings.DelayBeforeRefreshOnMapChange.get_Value(), (_moduleSettings.DelayBeforeRefreshOnMapChange.get_Value() > 1) ? "s" : string.Empty), (NotificationType)0, (Texture2D)null, 4);
			}
		}

		public async Task CopyKpToClipboard()
		{
			if (await DataLoaded())
			{
				Clipboard.SetText("KpMe id : " + _kpId);
				ScreenNotification.ShowNotification("[KpRefresher] Id copied to clipboard !", (NotificationType)0, (Texture2D)null, 4);
			}
			else
			{
				ScreenNotification.ShowNotification("[KpRefresher] Id could not be loaded\nPlease try again later", (NotificationType)1, (Texture2D)null, 4);
			}
		}

		public async Task OpenKpUrl()
		{
			if (!(await DataLoaded()))
			{
				ScreenNotification.ShowNotification("[KpRefresher] KillProof.me id not loaded\nPlease retry later.", (NotificationType)1, (Texture2D)null, 4);
			}
			else
			{
				Process.Start(_kpMeService.GetBaseUrl() + "proof/" + _kpId);
			}
		}

		public async Task ActivateNotificationNextRefreshAvailable()
		{
			if (!(await DataLoaded()))
			{
				ScreenNotification.ShowNotification("[KpRefresher] KillProof.me refresh was not available\nPlease retry later.", (NotificationType)1, (Texture2D)null, 4);
				return;
			}
			if (DateTime.UtcNow > _refreshAvailable.Value)
			{
				ScreenNotification.ShowNotification("[KpRefresher] Next refresh is available !", (NotificationType)0, (Texture2D)null, 4);
				return;
			}
			double minutesUntilRefreshAvailable = Math.Ceiling((_refreshAvailable.Value - DateTime.UtcNow).TotalMinutes);
			NotificationNextRefreshAvailabledActivated = true;
			NotificationNextRefreshAvailabledTimer = 0.0;
			NotificationNextRefreshAvailabledTimerEndValue = minutesUntilRefreshAvailable * 60.0 * 1000.0;
			ScreenNotification.ShowNotification($"[KpRefresher] You will be notified when next refresh is available,\nin approx. {minutesUntilRefreshAvailable - 1.0} minutes.", (NotificationType)0, (Texture2D)null, 4);
		}

		public void ResetNotificationNextRefreshAvailable()
		{
			NotificationNextRefreshAvailabledActivated = false;
			NotificationNextRefreshAvailabledTimer = 0.0;
			NotificationNextRefreshAvailabledTimerEndValue = double.MaxValue;
		}

		public void NextRefreshIsAvailable()
		{
			ScreenNotification.ShowNotification("[KpRefresher] Next refresh is available !", (NotificationType)0, (Texture2D)null, 4);
			ResetNotificationNextRefreshAvailable();
		}

		public TimeSpan GetNextScheduledTimer()
		{
			if (!RefreshScheduled)
			{
				return TimeSpan.Zero;
			}
			double seconds = (ScheduleTimerEndValue - ScheduleTimer) / 1000.0;
			return new TimeSpan(0, 0, (int)seconds);
		}

		public async Task<List<(string, Color?)>> GetFullRaidStatus()
		{
			if (!(await DataLoaded()))
			{
				ScreenNotification.ShowNotification("[KpRefresher] KillProof.me refresh was not available\nPlease retry later.", (NotificationType)1, (Texture2D)null, 4);
				return null;
			}
			List<string> baseClears = await _kpMeService.GetClearData(_kpId);
			List<string> obj = await _gw2ApiService.GetClears();
			List<string> formattedGw2ApiClears = new List<string>();
			foreach (string item in obj)
			{
				Enum.TryParse<RaidBoss>(item, out var boss2);
				formattedGw2ApiClears.Add(boss2.GetDisplayName());
			}
			List<(string, Color?)> res = new List<(string, Color?)>();
			List<RaidBoss> encounters = _raidBossNames.OrderBy((RaidBoss x) => (int)x).ToList();
			foreach (int wingNumber in encounters.Select((RaidBoss ob) => ob.GetAttribute<WingAttribute>().WingNumber).Distinct())
			{
				res.Add(($"[Wing {wingNumber}]\n", Color.get_White()));
				IEnumerable<string> bossFromWing = from o in encounters
					where o.GetAttribute<WingAttribute>().WingNumber == wingNumber
					select o.GetDisplayName();
				for (int i = 0; i < bossFromWing.Count(); i++)
				{
					string boss = bossFromWing.ElementAt(i);
					Color bossColor = Colors.BaseColor;
					if (baseClears.Contains(boss, StringComparer.OrdinalIgnoreCase))
					{
						bossColor = Colors.KpRefreshedColor;
					}
					else if (formattedGw2ApiClears.Contains(boss, StringComparer.OrdinalIgnoreCase))
					{
						bossColor = Colors.OnlyGw2;
					}
					if (boss == "Statues Of Grenth")
					{
						boss = "Statues";
					}
					if (boss == "Voice In The Void")
					{
						boss = "Dhuum";
					}
					res.Add((boss + ((i < bossFromWing.Count() - 1) ? " - " : string.Empty), bossColor));
				}
				res.Add(("\n", null));
			}
			return res;
		}

		public async Task<string> DisplayCurrentKp()
		{
			if (!(await DataLoaded()))
			{
				ScreenNotification.ShowNotification("[KpRefresher] KillProof.me refresh was not available\nPlease retry later.", (NotificationType)1, (Texture2D)null, 4);
				return string.Empty;
			}
			return await _gw2ApiService.ScanAccountForKp();
		}

		public async Task<string> RefreshLinkedAccounts()
		{
			if (!(await DataLoaded()))
			{
				ScreenNotification.ShowNotification("[KpRefresher] KillProof.me refresh was not available\nPlease retry later.", (NotificationType)1, (Texture2D)null, 4);
				return string.Empty;
			}
			List<Task> tasks = new List<Task>();
			string res = string.Empty;
			foreach (string acc in LinkedKpId)
			{
				Task tt = Task.Run(async delegate
				{
					bool? refreshRes = await _kpMeService.RefreshApi(acc);
					res = res + "- " + acc + " : " + ((refreshRes == true) ? "Refreshed" : ((refreshRes == false) ? "Refresh not available" : "Error")) + "\n";
				});
				tasks.Add(tt);
			}
			await Task.WhenAll(tasks);
			return res;
		}

		private async Task<bool> RefreshAccountName(bool isFromInit = false)
		{
			_accountName = await _gw2ApiService.GetAccountName();
			if (string.IsNullOrWhiteSpace(_accountName) && isFromInit)
			{
				ScreenNotification.ShowNotification("[KpRefresher] Error while getting Account name from GW2 API.\nPlease retry later.", (NotificationType)2, (Texture2D)null, 4);
			}
			return !string.IsNullOrWhiteSpace(_accountName);
		}

		private async Task RefreshKpMeData(bool isFromInit = false)
		{
			_isRefreshingKpData = true;
			if (string.IsNullOrWhiteSpace(_accountName) && !(await RefreshAccountName()))
			{
				_isRefreshingKpData = false;
				return;
			}
			_kpId = string.Empty;
			_lastRefresh = null;
			LinkedKpId = null;
			KpApiModel accountData = await _kpMeService.GetAccountData(_accountName);
			if (accountData == null)
			{
				if (isFromInit)
				{
					ScreenNotification.ShowNotification("[KpRefresher] Error while loading KillProof.me profile.\nPlease retry later.", (NotificationType)1, (Texture2D)null, 4);
				}
				_isRefreshingKpData = false;
				return;
			}
			_kpId = accountData.Id;
			_lastRefresh = accountData.LastRefresh;
			LinkedKpId = accountData.LinkedAccounts?.Select((KpApiModel l) => l.Id)?.ToList();
			_isRefreshingKpData = false;
		}

		private void ScheduleRefresh(double minutes = 5.0)
		{
			RefreshScheduled = true;
			ScheduleTimer = 0.0;
			ScheduleTimerEndValue = minutes * 60.0 * 1000.0;
		}

		private async Task<bool> CheckRaidClears()
		{
			List<string> baseClears = await _kpMeService.GetClearData(_kpId);
			List<string> clears = await _gw2ApiService.GetClears();
			if (clears == null || baseClears == null)
			{
				return false;
			}
			List<string> formattedGw2ApiClears = new List<string>();
			foreach (string item in clears)
			{
				Enum.TryParse<RaidBoss>(item, out var boss);
				formattedGw2ApiClears.Add(boss.GetDisplayName());
			}
			IEnumerable<string> result = formattedGw2ApiClears.Where((string p) => !baseClears.Any((string p2) => p2 == p));
			if (!result.Any())
			{
				return false;
			}
			if (!_moduleSettings.RefreshOnKillOnlyBoss.get_Value())
			{
				return true;
			}
			foreach (string item2 in result)
			{
				if (Enum.TryParse<RaidBoss>(item2, out var raidBoss))
				{
					if (raidBoss.HasAttribute<FinalBossAttribute>())
					{
						return true;
					}
					continue;
				}
				return true;
			}
			return false;
		}

		private async Task UpdateLastRefresh(DateTime? date = null)
		{
			if (!date.HasValue)
			{
				date = (await _kpMeService.GetAccountData(_kpId))?.LastRefresh;
			}
			_lastRefresh = date.GetValueOrDefault();
		}

		private async Task<bool> DataLoaded(int retryCount = 0)
		{
			if (!string.IsNullOrWhiteSpace(_kpId))
			{
				return true;
			}
			if (retryCount >= 5)
			{
				return false;
			}
			if (_isRefreshingKpData)
			{
				await Task.Delay(1000);
			}
			else
			{
				if (retryCount > 0)
				{
					await Task.Delay(1000);
				}
				await RefreshKpMeData();
			}
			retryCount++;
			return await DataLoaded(retryCount);
		}
	}
}
