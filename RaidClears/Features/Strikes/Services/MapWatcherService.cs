using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;
using RaidClears.Localization;
using RaidClears.Settings.Enums;
using RaidClears.Shared.Controls;

namespace RaidClears.Features.Strikes.Services
{
	public class MapWatcherService : IDisposable
	{
		protected bool _isOnStrikeMap;

		protected Encounters.StrikeMission? _strikeMission;

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
			Dictionary<Encounters.StrikeMission, DateTime> clears = new Dictionary<Encounters.StrikeMission, DateTime>();
			if (!Service.StrikePersistance.AccountClears.TryGetValue(Service.CurrentAccountName, out clears))
			{
				clears = new Dictionary<Encounters.StrikeMission, DateTime>();
			}
			List<string> clearedStrikesThisReset = new List<string>();
			foreach (KeyValuePair<Encounters.StrikeMission, DateTime> entry in clears)
			{
				if (entry.Key.GetExpansionType() == StrikeMissionType.Ibs && entry.Value >= Service.ResetWatcher.LastDailyReset)
				{
					clearedStrikesThisReset.Add(entry.Key.GetApiLabel());
					clearedStrikesThisReset.Add("priority_" + entry.Key.GetApiLabel());
				}
				if ((entry.Key.GetExpansionType() == StrikeMissionType.Eod || entry.Key.GetExpansionType() == StrikeMissionType.SotO) && entry.Value >= Service.ResetWatcher.LastWeeklyReset)
				{
					clearedStrikesThisReset.Add(entry.Key.GetApiLabel());
					if (entry.Value >= Service.ResetWatcher.LastDailyReset)
					{
						clearedStrikesThisReset.Add("priority_" + entry.Key.GetApiLabel());
					}
				}
				if ((entry.Key.GetExpansionType() == StrikeMissionType.Eod || entry.Key.GetExpansionType() == StrikeMissionType.SotO) && entry.Value >= Service.ResetWatcher.LastDailyReset)
				{
					clearedStrikesThisReset.Add("priority_" + entry.Key.GetApiLabel());
				}
			}
			this.CompletedStrikes?.Invoke(this, clearedStrikesThisReset);
		}

		public void MarkStrikeCompleted(Encounters.StrikeMission mission)
		{
			Service.StrikePersistance.SaveClear(Service.CurrentAccountName, mission);
			DispatchCurrentStrikeClears();
		}

		public void MarkStrikeNotCompleted(Encounters.StrikeMission mission)
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
			if (Enum.IsDefined(typeof(MapIds.StrikeMaps), e.get_Value()))
			{
				Reset();
				_isOnStrikeMap = true;
				_strikeApiName = ((MapIds.StrikeMaps)e.get_Value()).GetApiLabel();
				_strikeName = ((MapIds.StrikeMaps)e.get_Value()).GetLabel();
				_strikeMission = ((MapIds.StrikeMaps)e.get_Value()).GetStrikeMission();
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
					if (_strikeMission.HasValue)
					{
						MarkStrikeCompleted(_strikeMission.Value);
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
						if (_strikeMission.HasValue)
						{
							MarkStrikeCompleted(_strikeMission.Value);
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
