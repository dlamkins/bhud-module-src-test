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
	public class TierNTomorrow : Fractal
	{
		private readonly FractalSettings settings = Service.Settings.FractalSettings;

		private static FractalSettings Settings => Service.Settings.FractalSettings;

		public TierNTomorrow(string name, int index, string shortName, IEnumerable<BoxModel> boxes, Container panel)
			: base(name, index, shortName, boxes)
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
			new GridBox((Container)(object)group, "Patch!", "Data may be wrong for up to two(2) weeks while the new fractal rotation is recorded", Settings.Style.GridOpacity, Settings.Style.FontSize);
		}

		protected void InitTierNFractals()
		{
			IEnumerable<BoxModel> tomorrowTierN = DailyTierNFractalService.GetTomorrowTierN();
			List<BoxModel> newList = new List<BoxModel>();
			foreach (BoxModel encounter in tomorrowTierN)
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
			InitTierNFractals();
		}

		public override void Dispose()
		{
			Service.ResetWatcher.DailyReset -= new EventHandler<DateTime>(ResetWatcher_DailyReset);
		}
	}
}
