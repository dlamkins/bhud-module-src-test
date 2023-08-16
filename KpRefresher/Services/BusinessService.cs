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
using KpRefresher.Ressources;
using KpRefresher.UI.Controls;
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

		private readonly CornerIcon _cornerIcon;

		private string _accountName { get; set; }

		private string _kpId { get; set; }

		private bool _isRefreshingKpData { get; set; }

		private List<RaidBoss> _raidBossNames { get; set; }

		private DateTime _nextRefresh { get; set; }

		private List<int> _raidMapIds { get; set; }

		private List<int> _strikeMapIds { get; set; }

		private bool _playerWasInInstance { get; set; }

		private TimeSpan _nextRefreshInterval => _nextRefresh - DateTime.UtcNow;

		private bool _canRefreshByDates => _nextRefreshInterval.Ticks < 0;

		private double _nextRefreshSeconds { get; set; }

		public List<string> LinkedKpId { get; set; }

		public bool RefreshScheduled { get; set; }

		public double ScheduleTimer { get; set; }

		public double ScheduleTimerEndValue { get; set; }

		public bool NotificationNextRefreshAvailabledActivated { get; set; }

		public double NotificationNextRefreshAvailabledTimer { get; set; }

		public double NotificationNextRefreshAvailabledTimerEndValue { get; set; }

		public string KpId
		{
			get
			{
				if (!string.IsNullOrEmpty(_moduleSettings.CustomId.get_Value()))
				{
					return _moduleSettings.CustomId.get_Value();
				}
				return _kpId;
			}
		}

		public BusinessService(ModuleSettings moduleSettings, Gw2ApiService gw2ApiService, KpMeService kpMeService, Func<LoadingSpinner> getSpinner, CornerIcon cornerIcon)
		{
			_moduleSettings = moduleSettings;
			_gw2ApiService = gw2ApiService;
			_kpMeService = kpMeService;
			_getSpinner = getSpinner;
			_cornerIcon = cornerIcon;
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
			await RefreshAccountName();
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
				ScreenNotification.ShowNotification(strings.Notification_DataNotAvailable, (NotificationType)1, (Texture2D)null, 4);
				return;
			}
			if (!_canRefreshByDates)
			{
				double minutesUntilRefreshAvailable = _nextRefreshInterval.TotalMinutes;
				string baseMsg = string.Format(strings.Notification_NextRefreshAvailableIn, minutesUntilRefreshAvailable.ToString("0"), (minutesUntilRefreshAvailable > 1.0) ? "s" : string.Empty);
				if (_moduleSettings.EnableAutoRetry.get_Value())
				{
					ScheduleRefresh(Math.Ceiling(_nextRefreshInterval.TotalSeconds));
					if (!fromUpdateLoop || _moduleSettings.ShowScheduleNotification.get_Value())
					{
						ScreenNotification.ShowNotification(string.Format(strings.Notification_TryScheduled, baseMsg), (NotificationType)1, (Texture2D)null, 4);
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
				ScreenNotification.ShowNotification(strings.Notification_NoClearRefreshAborted, (NotificationType)0, (Texture2D)null, 4);
				return;
			}
			bool? refreshed = await _kpMeService.RefreshApi(KpId);
			if (refreshed.HasValue && refreshed.Value)
			{
				_nextRefresh = DateTime.UtcNow.AddHours(1.0);
				Task.Run(async delegate
				{
					bool profileNotRefreshed = true;
					while (profileNotRefreshed)
					{
						await Task.Delay(20000);
						await UpdateNextRefresh();
						profileNotRefreshed = _nextRefreshSeconds == 0.0;
					}
				});
				ScreenNotification.ShowNotification(strings.Notification_RefreshOk, (NotificationType)0, (Texture2D)null, 4);
			}
			else
			{
				if (!refreshed.HasValue || refreshed.Value)
				{
					return;
				}
				await UpdateNextRefresh();
				if (_moduleSettings.EnableAutoRetry.get_Value())
				{
					ScheduleRefresh();
					if (_moduleSettings.ShowScheduleNotification.get_Value())
					{
						ScreenNotification.ShowNotification(strings.Notification_RefreshNotAvailableRetry, (NotificationType)1, (Texture2D)null, 4);
					}
				}
				else
				{
					ScreenNotification.ShowNotification(strings.Notification_RefreshNotAvailable, (NotificationType)1, (Texture2D)null, 4);
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
				ScheduleRefresh(_moduleSettings.DelayBeforeRefreshOnMapChange.get_Value() * 60);
				ScreenNotification.ShowNotification(string.Format(strings.Notification_InstanceExitDetected, _moduleSettings.DelayBeforeRefreshOnMapChange.get_Value(), (_moduleSettings.DelayBeforeRefreshOnMapChange.get_Value() > 1) ? "s" : string.Empty), (NotificationType)0, (Texture2D)null, 4);
			}
		}

		public async Task CopyKpToClipboard()
		{
			if (await DataLoaded())
			{
				Clipboard.SetText("KillProof.me id : " + KpId);
				ScreenNotification.ShowNotification(strings.Notification_CopiedToClipboard, (NotificationType)0, (Texture2D)null, 4);
			}
			else
			{
				ScreenNotification.ShowNotification(strings.Notification_DataNotAvailable, (NotificationType)1, (Texture2D)null, 4);
			}
		}

		public async Task OpenKpUrl()
		{
			if (!(await DataLoaded()))
			{
				ScreenNotification.ShowNotification(strings.Notification_DataNotAvailable, (NotificationType)1, (Texture2D)null, 4);
			}
			else
			{
				Process.Start(_kpMeService.GetBaseUrl() + "proof/" + KpId);
			}
		}

		public async Task<bool> ActivateNotificationNextRefreshAvailable()
		{
			if (!(await DataLoaded()))
			{
				ScreenNotification.ShowNotification(strings.Notification_DataNotAvailable, (NotificationType)1, (Texture2D)null, 4);
				return false;
			}
			if (_canRefreshByDates)
			{
				ScreenNotification.ShowNotification(strings.Notification_RefreshAvailable, (NotificationType)0, (Texture2D)null, 4);
				return false;
			}
			double minutesUntilRefreshAvailable = _nextRefreshInterval.TotalMinutes;
			NotificationNextRefreshAvailabledActivated = true;
			NotificationNextRefreshAvailabledTimer = 0.0;
			NotificationNextRefreshAvailabledTimerEndValue = Math.Ceiling(_nextRefreshInterval.TotalSeconds) * 1000.0;
			ScreenNotification.ShowNotification(string.Format(strings.Notification_NotifyScheduled, minutesUntilRefreshAvailable.ToString("0"), (minutesUntilRefreshAvailable > 1.0) ? "s" : string.Empty), (NotificationType)0, (Texture2D)null, 4);
			return true;
		}

		public void ResetNotificationNextRefreshAvailable()
		{
			NotificationNextRefreshAvailabledActivated = false;
			NotificationNextRefreshAvailabledTimer = 0.0;
			NotificationNextRefreshAvailabledTimerEndValue = double.MaxValue;
		}

		public void NextRefreshIsAvailable()
		{
			ScreenNotification.ShowNotification(strings.Notification_RefreshAvailable, (NotificationType)0, (Texture2D)null, 4);
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
				ScreenNotification.ShowNotification(strings.Notification_DataNotAvailable, (NotificationType)1, (Texture2D)null, 4);
				return null;
			}
			List<RaidBoss> baseClears = await _kpMeService.GetClearData(KpId);
			List<RaidBoss> clears = await _gw2ApiService.GetClears();
			List<(string, Color?)> res = new List<(string, Color?)>();
			List<RaidBoss> encounters = _raidBossNames.OrderBy((RaidBoss x) => (int)x).ToList();
			foreach (int wingNumber in encounters.Select((RaidBoss ob) => ob.GetAttribute<WingAttribute>().WingNumber).Distinct())
			{
				res.Add(($"[{strings.BusinessService_Wing} {wingNumber}]\n", Color.get_White()));
				IEnumerable<RaidBoss> bossFromWing = encounters.Where((RaidBoss o) => o.GetAttribute<WingAttribute>().WingNumber == wingNumber);
				for (int i = 0; i < bossFromWing.Count(); i++)
				{
					RaidBoss boss = bossFromWing.ElementAt(i);
					Color bossColor = Colors.BaseColor;
					if (baseClears.Contains(boss))
					{
						bossColor = Colors.KpRefreshedColor;
					}
					else if (clears.Contains(boss))
					{
						bossColor = Colors.OnlyGw2;
					}
					res.Add((boss.GetDisplayName(), bossColor));
					res.Add(((i < bossFromWing.Count() - 1) ? " - " : string.Empty, Colors.BaseColor));
				}
				res.Add(("\n", null));
			}
			return res;
		}

		public async Task<string> DisplayCurrentKp()
		{
			if (!(await DataLoaded()))
			{
				ScreenNotification.ShowNotification(strings.Notification_DataNotAvailable, (NotificationType)1, (Texture2D)null, 4);
				return string.Empty;
			}
			return await _gw2ApiService.ScanAccountForKp();
		}

		public async Task<string> RefreshLinkedAccounts()
		{
			if (!(await DataLoaded()))
			{
				ScreenNotification.ShowNotification(strings.Notification_DataNotAvailable, (NotificationType)1, (Texture2D)null, 4);
				return string.Empty;
			}
			List<Task> tasks = new List<Task>();
			string res = string.Empty;
			foreach (string acc in LinkedKpId)
			{
				Task tt = Task.Run(async delegate
				{
					bool? refreshRes = await _kpMeService.RefreshApi(acc);
					res = res + "- " + acc + " : " + ((refreshRes == true) ? strings.BusinessService_Refreshed : ((refreshRes == false) ? strings.BusinessService_RefreshNotAvailable : strings.BusinessService_Error)) + "\n";
				});
				tasks.Add(tt);
			}
			await Task.WhenAll(tasks);
			return res;
		}

		public async Task<(bool, string)> SetCustomId(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				_moduleSettings.CustomId.set_Value(value);
				return (true, strings.BusinessService_CustomIdSet);
			}
			KpApiModel accountData = await _kpMeService.GetAccountData(value, showNotification: false);
			if (accountData == null)
			{
				return (false, string.Format(strings.BusinessService_CustomIdNoAccountFound, value));
			}
			if (accountData.AccountName != _accountName)
			{
				return (false, string.Format(strings.BusinessService_CustomIdAccountNotMatching, value, _accountName));
			}
			_moduleSettings.CustomId.set_Value(value);
			return (true, strings.BusinessService_CustomIdSet);
		}

		private async Task<bool> RefreshAccountName()
		{
			_accountName = await _gw2ApiService.GetAccountName();
			_cornerIcon.UpdateWarningState(string.IsNullOrWhiteSpace(_accountName));
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
			_nextRefresh = DateTime.UtcNow.AddHours(1.0);
			LinkedKpId = null;
			string accountName = (string.IsNullOrEmpty(_moduleSettings.CustomId.get_Value()) ? _accountName : _moduleSettings.CustomId.get_Value());
			KpApiModel accountData = await _kpMeService.GetAccountData(accountName);
			if (accountData == null)
			{
				if (isFromInit)
				{
					ScreenNotification.ShowNotification(strings.Notification_KPProfileFetchError, (NotificationType)1, (Texture2D)null, 4);
				}
				_isRefreshingKpData = false;
				return;
			}
			if (!string.IsNullOrEmpty(_moduleSettings.CustomId.get_Value()) && accountData.AccountName != _accountName)
			{
				_moduleSettings.CustomId.set_Value(string.Empty);
				ScreenNotification.ShowNotification(string.Format(strings.Notification_CustomIdAccountNotMatching, _moduleSettings.CustomId.get_Value(), _accountName), (NotificationType)1, (Texture2D)null, 4);
			}
			_kpId = accountData.Id;
			_nextRefresh = accountData.NextRefresh;
			_nextRefreshSeconds = accountData.NextRefreshSeconds;
			LinkedKpId = accountData.LinkedAccounts?.Select((KpApiModel l) => l.Id)?.ToList();
			_isRefreshingKpData = false;
		}

		private void ScheduleRefresh(double seconds = 300.0)
		{
			RefreshScheduled = true;
			ScheduleTimer = 0.0;
			ScheduleTimerEndValue = seconds * 1000.0;
		}

		private async Task<bool> CheckRaidClears()
		{
			List<RaidBoss> baseClears = await _kpMeService.GetClearData(KpId);
			List<RaidBoss> clears = await _gw2ApiService.GetClears();
			if (clears == null || baseClears == null)
			{
				return false;
			}
			IEnumerable<RaidBoss> result = clears.Where((RaidBoss p) => !baseClears.Any((RaidBoss p2) => p2 == p));
			if (!result.Any())
			{
				return false;
			}
			if (!_moduleSettings.RefreshOnKillOnlyBoss.get_Value())
			{
				return true;
			}
			foreach (RaidBoss res in result)
			{
				if (!_raidBossNames.Any((RaidBoss r) => r == res))
				{
					return true;
				}
				if (_raidBossNames.FirstOrDefault((RaidBoss r) => r == res).HasAttribute<FinalBossAttribute>())
				{
					return true;
				}
			}
			return false;
		}

		private async Task UpdateNextRefresh()
		{
			KpApiModel accountData = await _kpMeService.GetAccountData(KpId);
			_nextRefresh = accountData?.NextRefresh ?? DateTime.Now.AddHours(1.0);
			_nextRefreshSeconds = accountData?.NextRefreshSeconds ?? 3600.0;
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
