using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Shared.Services;
using RaidClears.Features.Strikes.Models;
using RaidClears.Features.Strikes.Services;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Strikes
{
	public class StrikesPanel : GridPanel
	{
		private IEnumerable<Strike> _strikes;

		private readonly MapWatcherService _mapService;

		private static StrikeSettings Settings => Service.Settings.StrikeSettings;

		public StrikesPanel()
			: base(Settings.Generic, (Container)(object)GameService.Graphics.get_SpriteScreen())
		{
			_mapService = Service.MapWatcher;
			_strikes = StrikeMetaData.Create(this);
			_mapService.CompletedStrikes += new EventHandler<List<string>>(_mapService_CompletedStrikes);
			Service.ResetWatcher.DailyReset += new EventHandler<DateTime>(UpdateClearsAtReset);
			Service.ResetWatcher.WeeklyReset += new EventHandler<DateTime>(UpdateClearsAtReset);
			Service.ApiPollingService!.ApiPollingTrigger += new EventHandler<bool>(OnApiPollingTrigger);
			Settings.Style.Color.Cleared.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate
			{
				ApplyEncounterBackgroundColors();
			});
			Settings.Style.Color.NotCleared.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate
			{
				ApplyEncounterBackgroundColors();
			});
			Settings.StrikePanelColorNonWeeklyBounty.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate
			{
				ApplyEncounterBackgroundColors();
			});
			Settings.StrikePanelHighlightNonWeeklyBounty.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				ApplyEncounterBackgroundColors();
			});
			((FlowPanel)(object)this).LayoutChange(Settings.Style.Layout);
			this.BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);
			RegisterKeyBindService(new KeyBindHandlerService(Settings.Generic.ShowHideKeyBind, Settings.Generic.Visible));
			ApplyEncounterBackgroundColors();
		}

		private void OnApiPollingTrigger(object sender, bool _)
		{
			Task.Run(async delegate
			{
				await WeeklyStrikeClearsService.RefreshFromApiAsync();
				GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
				{
					Service.MapWatcher.DispatchCurrentStrikeClears();
				});
			});
		}

		private void UpdateClearsAtReset(object sender, DateTime reset)
		{
			Service.MapWatcher.DispatchCurrentStrikeClears();
		}

		private void _mapService_CompletedStrikes(object sender, List<string> strikesCompletedThisReset)
		{
			foreach (Strike group in _strikes)
			{
				if (group is DailyBountyTomorrow)
				{
					continue;
				}
				foreach (BoxModel encounter in group.boxes)
				{
					encounter.SetCleared(strikesCompletedThisReset.Contains(encounter.id));
				}
			}
			ApplyEncounterBackgroundColors();
		}

		private void ApplyEncounterBackgroundColors()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			Color clearedColor = Settings.Style.Color.Cleared.get_Value().HexToXnaColor();
			Color notClearedColor = Settings.Style.Color.NotCleared.get_Value().HexToXnaColor();
			Color nonWeeklyColor = Settings.StrikePanelColorNonWeeklyBounty.get_Value().HexToXnaColor();
			bool highlightNonWeekly = Settings.StrikePanelHighlightNonWeeklyBounty.get_Value();
			WeeklyBountyEncountersService weeklyBounties = Service.WeeklyBountyEncounters;
			foreach (Strike group in _strikes)
			{
				if (group is DailyBounty || group is DailyBountyTomorrow)
				{
					continue;
				}
				foreach (BoxModel encounter in group.boxes)
				{
					if (encounter.IsCleared)
					{
						encounter.Box.BackgroundColor = clearedColor;
					}
					else if (highlightNonWeekly && weeklyBounties != null && !weeklyBounties.IsWeeklyBounty(encounter.id))
					{
						encounter.Box.BackgroundColor = nonWeeklyColor;
					}
					else
					{
						encounter.Box.BackgroundColor = notClearedColor;
					}
				}
			}
			((Control)this).Invalidate();
		}

		public void ForceInvalidate()
		{
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			if (Service.ApiPollingService != null)
			{
				Service.ApiPollingService!.ApiPollingTrigger -= new EventHandler<bool>(OnApiPollingTrigger);
			}
			_mapService.CompletedStrikes -= new EventHandler<List<string>>(_mapService_CompletedStrikes);
			Service.ResetWatcher.DailyReset -= new EventHandler<DateTime>(UpdateClearsAtReset);
			Service.ResetWatcher.WeeklyReset -= new EventHandler<DateTime>(UpdateClearsAtReset);
			foreach (Strike strike in _strikes)
			{
				strike.Dispose();
			}
		}

		public void UpdateEncounterLabel(string encounterApiId, string newLabel)
		{
			foreach (Strike expansion in _strikes)
			{
				if (expansion.id == encounterApiId)
				{
					((Label)expansion.GroupLabel).set_Text(newLabel);
					((Control)expansion.GroupLabel).Invalidate();
				}
				foreach (BoxModel encounter in expansion.boxes)
				{
					if (encounter.id == encounterApiId)
					{
						encounter.SetLabel(newLabel);
					}
				}
			}
			((Control)this).Invalidate();
		}
	}
}
