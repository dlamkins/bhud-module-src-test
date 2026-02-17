using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Shared.Controls;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Shared.Services;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Features.Strikes.Models
{
	public class DailyBounty : Strike
	{
		private const string BountyIdPrefix = "priority_";

		private readonly StrikeSettings settings = Service.Settings.StrikeSettings;

		private static StrikeSettings Settings => Service.Settings.StrikeSettings;

		public DailyBounty(string name, string id, int index, string shortName, IEnumerable<BoxModel> boxes, Container panel)
			: base(name, id, index, shortName, boxes)
		{
			Service.ResetWatcher.DailyReset += new EventHandler<DateTime>(ResetWatcher_DailyReset);
			if (Service.ApiPollingService != null)
			{
				Service.ApiPollingService!.ApiPollingTrigger += new EventHandler<bool>(OnApiPollingTrigger);
			}
			InitGroup(panel);
			InitBountyEncounters();
		}

		private void OnApiPollingTrigger(object sender, bool e)
		{
			Task.Run(async delegate
			{
				if (Service.DailyBountyProgress != null)
				{
					await Service.DailyBountyProgress.RefreshFromApiAsync();
					IReadOnlyCollection<int> completed = Service.DailyBountyProgress.CompletedDailyBountyAchievementIds;
					foreach (BoxModel box in boxes)
					{
						string encounterId = box.id;
						string apiId = (encounterId.StartsWith("priority_", StringComparison.Ordinal) ? encounterId.Substring("priority_".Length) : encounterId);
						int? bountyAchievementId = Service.RaidData.GetRaidEncounterByApiId(apiId).DailyBountyAchievementId;
						bool cleared = bountyAchievementId.HasValue && completed.Contains(bountyAchievementId.Value);
						box.SetCleared(cleared);
					}
				}
			});
		}

		protected void InitGroup(Container panel)
		{
			GridGroup group = new GridGroup(panel, settings.Style.Layout);
			((FlowPanel)(object)group).VisiblityChanged(Service.StrikeData.GetPriorityVisible());
			SetGridGroupReference(group);
			GridBox labelBox = new GridBox((Container)(object)group, shortName, name, settings.Style.LabelOpacity, settings.Style.FontSize);
			SetGroupLabelReference(labelBox);
			labelBox.LayoutChange(settings.Style.Layout);
			labelBox.LabelDisplayChange(settings.Style.LabelDisplay, shortName, shortName);
		}

		protected void InitBountyEncounters()
		{
			IEnumerable<Encounter> dailyBounties = DailyBountyService.GetDailyBounties();
			List<BoxModel> newList = new List<BoxModel>();
			foreach (Encounter encounter in dailyBounties)
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
			foreach (BoxModel box2 in boxes)
			{
				GridBox box = box2.Box;
				if (box != null)
				{
					((Control)box).Dispose();
				}
			}
			InitBountyEncounters();
		}

		public override void Dispose()
		{
			if (Service.ApiPollingService != null)
			{
				Service.ApiPollingService!.ApiPollingTrigger -= new EventHandler<bool>(OnApiPollingTrigger);
			}
			Service.ResetWatcher.DailyReset -= new EventHandler<DateTime>(ResetWatcher_DailyReset);
			base.Dispose();
		}
	}
}
