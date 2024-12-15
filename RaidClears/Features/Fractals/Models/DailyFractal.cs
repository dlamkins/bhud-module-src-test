using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Fractals.Models
{
	public class DailyFractal : Fractal
	{
		private readonly FractalSettings settings = Service.Settings.FractalSettings;

		private static FractalSettings Settings => Service.Settings.FractalSettings;

		public DailyFractal(Container panel)
			: base(Fractal.RecLabel, 1, Fractal.RecId, new List<BoxModel>())
		{
			Service.ResetWatcher.DailyReset += new EventHandler<DateTime>(ResetWatcher_DailyReset);
			InitGroup(panel);
			InitDailyRecFractals();
		}

		protected void InitGroup(Container panel)
		{
			GridGroup group = new GridGroup(panel, settings.Style.Layout);
			((FlowPanel)(object)group).VisiblityChanged(settings.DailyRecs);
			SetGridGroupReference(group);
			GridBox labelBox = new GridBox((Container)(object)group, shortName, name, settings.Style.LabelOpacity, settings.Style.FontSize);
			SetGroupLabelReference(labelBox);
			labelBox.LayoutChange(settings.Style.Layout);
			labelBox.LabelDisplayChange(settings.Style.LabelDisplay, shortName, shortName);
		}

		protected void InitDailyRecFractals()
		{
			IEnumerable<BoxModel> recommendedFractals = DailyRecommendedFractalService.GetRecommendedFractals();
			List<BoxModel> newList = new List<BoxModel>();
			foreach (BoxModel encounter in recommendedFractals)
			{
				GridBox encounterBox = new GridBox((Container)(object)base.GridGroup, encounter.shortName, encounter.name, Settings.Style.GridOpacity, Settings.Style.FontSize);
				encounterBox.TextColorSetting(Settings.Style.Color.Text);
				encounter.SetGridBoxReference(encounterBox);
				encounter.WatchColorSettings(Settings.Style.Color.Cleared, Settings.Style.Color.NotCleared);
				newList.Add(encounter);
			}
			boxes = newList;
		}

		private void ResetWatcher_DailyReset(object sender, DateTime e)
		{
			foreach (BoxModel box in boxes)
			{
				((Control)box.Box).Dispose();
			}
			InitDailyRecFractals();
		}

		public override void Dispose()
		{
			Service.ResetWatcher.DailyReset -= new EventHandler<DateTime>(ResetWatcher_DailyReset);
		}
	}
}
