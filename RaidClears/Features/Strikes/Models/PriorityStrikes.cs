using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Services;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Strikes.Models
{
	public class PriorityStrikes : Strike
	{
		private readonly StrikeSettings settings = Service.Settings.StrikeSettings;

		private static StrikeSettings Settings => Service.Settings.StrikeSettings;

		public PriorityStrikes(string name, int index, string shortName, IEnumerable<BoxModel> boxes, Container panel)
			: base(name, index, shortName, boxes)
		{
			Service.ResetWatcher.DailyReset += new EventHandler<DateTime>(ResetWatcher_DailyReset);
			InitGroup(panel);
			InitPriorityStrikes();
		}

		protected void InitGroup(Container panel)
		{
			GridGroup group = new GridGroup(panel, settings.Style.Layout);
			((FlowPanel)(object)group).VisiblityChanged(settings.StrikeVisiblePriority);
			SetGridGroupReference(group);
			GridBox labelBox = new GridBox((Container)(object)group, shortName, name, settings.Style.LabelOpacity, settings.Style.FontSize);
			SetGroupLabelReference(labelBox);
			labelBox.LayoutChange(settings.Style.Layout);
			labelBox.LabelDisplayChange(settings.Style.LabelDisplay, shortName, shortName);
		}

		protected void InitPriorityStrikes()
		{
			IEnumerable<BoxModel> priorityEncounters = PriorityRotationService.GetPriorityEncounters();
			List<BoxModel> newList = new List<BoxModel>();
			foreach (BoxModel encounter in priorityEncounters)
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
			InitPriorityStrikes();
		}

		public override void Dispose()
		{
			Service.ResetWatcher.DailyReset -= new EventHandler<DateTime>(ResetWatcher_DailyReset);
		}
	}
}
