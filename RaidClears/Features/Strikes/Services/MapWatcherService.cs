using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Localization;
using RaidClears.Settings.Enums;
using RaidClears.Shared.Controls;

namespace RaidClears.Features.Strikes.Services
{
	public class MapWatcherService : IDisposable
	{
		protected bool _isOnStrikeMap;

		protected StrikeMission? _strikeMission;

		protected string _strikeApiName = string.Empty;

		protected string _strikeName = string.Empty;

		public event EventHandler<string>? StrikeCompleted;

		public event EventHandler<List<string>>? CompletedStrikes;

		public MapWatcherService()
		{
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
		}

		public void DispatchCurrentStrikeClears()
		{
			Dictionary<string, DateTime> clears = new Dictionary<string, DateTime>();
			if (!Service.StrikePersistance.AccountClears.TryGetValue(Service.CurrentAccountName, out clears))
			{
				clears = new Dictionary<string, DateTime>();
			}
			List<string> clearedStrikesThisReset = new List<string>();
			foreach (KeyValuePair<string, DateTime> entry in clears)
			{
				if (Service.StrikeData.GetStrikeMissionResetById(entry.Key) == "daily")
				{
					if (entry.Value >= Service.ResetWatcher.LastDailyReset)
					{
						clearedStrikesThisReset.Add(entry.Key);
						clearedStrikesThisReset.Add("priority_" + entry.Key);
					}
					continue;
				}
				if (entry.Value >= Service.ResetWatcher.LastWeeklyReset)
				{
					clearedStrikesThisReset.Add(entry.Key);
				}
				if (entry.Value >= Service.ResetWatcher.LastDailyReset)
				{
					clearedStrikesThisReset.Add("priority_" + entry.Key);
				}
			}
			this.CompletedStrikes?.Invoke(this, clearedStrikesThisReset);
		}

		public void MarkStrikeCompleted(StrikeMission mission)
		{
			Service.StrikePersistance.SaveClear(Service.CurrentAccountName, mission);
			DispatchCurrentStrikeClears();
		}

		public void MarkStrikeNotCompleted(StrikeMission mission)
		{
			Service.StrikePersistance.RemoveClear(Service.CurrentAccountName, mission);
			DispatchCurrentStrikeClears();
		}

		protected void Reset()
		{
			_strikeMission = null;
			_isOnStrikeMap = false;
			_strikeApiName = string.Empty;
			_strikeName = string.Empty;
		}

		private async void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			StrikeMission _strikeMap = Service.StrikeData.GetStrikeMisisonByMapId(e.get_Value());
			if (_strikeMap != null)
			{
				Reset();
				_isOnStrikeMap = true;
				_strikeApiName = _strikeMap.Id;
				_strikeName = _strikeMap.Name;
				_strikeMission = _strikeMap;
			}
			else
			{
				if (!_isOnStrikeMap)
				{
					return;
				}
				switch (Service.Settings.StrikeSettings.StrikeCompletion.get_Value())
				{
				case StrikeComplete.MAP_CHANGE:
					this.StrikeCompleted?.Invoke(this, _strikeApiName);
					if (_strikeMission != null)
					{
						MarkStrikeCompleted(_strikeMission);
					}
					break;
				case StrikeComplete.POPUP:
				{
					ConfirmDialog dialog = new ConfirmDialog(_strikeName, Strings.Strike_Confirm_Message, new ButtonDefinition[2]
					{
						new ButtonDefinition(Strings.Strike_Confirm_Btn_Yes, DialogResult.OK),
						new ButtonDefinition(Strings.Strike_Confirm_Btn_No, DialogResult.Cancel)
					});
					DialogResult num = await dialog.ShowDialog();
					((Control)dialog).Dispose();
					if (num == DialogResult.OK)
					{
						this.StrikeCompleted?.Invoke(this, _strikeApiName);
						if (_strikeMission != null)
						{
							MarkStrikeCompleted(_strikeMission);
						}
					}
					break;
				}
				}
				Reset();
			}
		}

		public void Dispose()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
		}
	}
}
