using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
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
			((FlowPanel)(object)this).LayoutChange(Settings.Style.Layout);
			this.BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);
			RegisterKeyBindService(new KeyBindHandlerService(Settings.Generic.ShowHideKeyBind, Settings.Generic.Visible));
		}

		private void UpdateClearsAtReset(object sender, DateTime reset)
		{
			Service.MapWatcher.DispatchCurrentStrikeClears();
		}

		private void _mapService_CompletedStrikes(object sender, List<string> strikesCompletedThisReset)
		{
			foreach (Strike strike in _strikes)
			{
				foreach (BoxModel encounter in strike.boxes)
				{
					encounter.SetCleared(strikesCompletedThisReset.Contains(encounter.id));
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
			_mapService.CompletedStrikes -= new EventHandler<List<string>>(_mapService_CompletedStrikes);
			Service.ResetWatcher.DailyReset -= new EventHandler<DateTime>(UpdateClearsAtReset);
			Service.ResetWatcher.WeeklyReset -= new EventHandler<DateTime>(UpdateClearsAtReset);
			foreach (Strike strike in _strikes)
			{
				strike.Dispose();
			}
		}
	}
}
