using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using RaidClears.Features.Fractals.Models;
using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Shared.Services;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Fractals
{
	public class FractalsPanel : GridPanel
	{
		private readonly IEnumerable<Fractal> _fractals;

		private readonly FractalMapWatcherService _mapService;

		private static FractalSettings Settings => Service.Settings.FractalSettings;

		public FractalsPanel()
			: base(Settings.Generic, (Container)(object)GameService.Graphics.get_SpriteScreen())
		{
			_mapService = Service.FractalMapWatcher;
			_fractals = FractalMetaData.Create(this);
			_mapService.CompletedFractal += new EventHandler<List<string>>(_mapService_CompletedStrikes);
			Service.ResetWatcher.DailyReset += new EventHandler<DateTime>(UpdateClearsAtReset);
			((FlowPanel)(object)this).LayoutChange(Settings.Style.Layout);
			this.BackgroundColorChange(Settings.Style.BgOpacity, Settings.Style.Color.Background);
			RegisterKeyBindService(new KeyBindHandlerService(Settings.Generic.ShowHideKeyBind, Settings.Generic.Visible));
		}

		private void UpdateClearsAtReset(object sender, DateTime reset)
		{
		}

		private void _mapService_CompletedStrikes(object sender, List<string> strikesCompletedThisReset)
		{
			foreach (Fractal group in _fractals)
			{
				if (group.GetType() == typeof(TierNTomorrow))
				{
					continue;
				}
				foreach (BoxModel encounter in group.boxes)
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
			_mapService.CompletedFractal -= new EventHandler<List<string>>(_mapService_CompletedStrikes);
			Service.ResetWatcher.DailyReset -= new EventHandler<DateTime>(UpdateClearsAtReset);
			Service.ResetWatcher.WeeklyReset -= new EventHandler<DateTime>(UpdateClearsAtReset);
			foreach (Fractal fractal in _fractals)
			{
				fractal.Dispose();
			}
		}

		public void UpdateEncounterLabel(string encounterApiId, string newLabel)
		{
			foreach (Fractal fractalGroup in _fractals)
			{
				if (fractalGroup.id == encounterApiId)
				{
					((Label)fractalGroup.GroupLabel).set_Text(newLabel);
					break;
				}
				foreach (BoxModel fractal in fractalGroup.boxes)
				{
					if (fractal.id == encounterApiId)
					{
						fractal.SetLabel(newLabel);
						return;
					}
				}
			}
		}
	}
}
