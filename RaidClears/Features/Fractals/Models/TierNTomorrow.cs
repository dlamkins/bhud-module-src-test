using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Shared.Services;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Fractals.Models
{
	public class TierNTomorrow : Fractal
	{
		private readonly FractalSettings settings = Service.Settings.FractalSettings;

		private static FractalSettings Settings => Service.Settings.FractalSettings;

		public TierNTomorrow(Container panel)
			: base(Fractal.TomorrowLabel, 2, Fractal.TomorrowId, new List<BoxModel>())
		{
			Service.ResetWatcher.DailyReset += new EventHandler<DateTime>(ResetWatcher_DailyReset);
			InitGroup(panel);
			InitTierNFractals();
		}

		protected void InitGroup(Container panel)
		{
			GridGroup group = new GridGroup(panel, settings.Style.Layout);
			((FlowPanel)(object)group).VisiblityChanged(settings.TomorrowTierN);
			SetGridGroupReference(group);
			GridBox labelBox = new GridBox((Container)(object)group, shortName, name, settings.Style.LabelOpacity, settings.Style.FontSize);
			SetGroupLabelReference(labelBox);
			labelBox.LayoutChange(settings.Style.Layout);
			labelBox.LabelDisplayChange(settings.Style.LabelDisplay, shortName, shortName);
		}

		protected void InitTierNFractals()
		{
			IEnumerable<(BoxModel box, FractalMap fractalMap, int scale)> tomorrowTierNForTooltip = DailyTierNFractalService.GetTomorrowTierNForTooltip();
			List<BoxModel> newList = new List<BoxModel>();
			foreach (var item in tomorrowTierNForTooltip)
			{
				BoxModel encounter = item.box;
				FractalMap map = item.fractalMap;
				int scale = item.scale;
				GridBox encounterBox = new GridBox((Container)(object)base.GridGroup, encounter.shortName, encounter.name, Settings.Style.GridOpacity, Settings.Style.FontSize);
				CmTooltip fractalTooptip = new CmTooltip();
				fractalTooptip.Fractal = new CMInterface(map, scale, DayOfYearIndexService.DayOfYearIndex());
				((Control)encounterBox).set_Tooltip((Tooltip)(object)fractalTooptip);
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
			InitTierNFractals();
		}

		public override void Dispose()
		{
			Service.ResetWatcher.DailyReset -= new EventHandler<DateTime>(ResetWatcher_DailyReset);
		}
	}
}
