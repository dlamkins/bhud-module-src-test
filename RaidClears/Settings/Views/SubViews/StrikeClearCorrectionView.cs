using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using RaidClears.Features.Shared.Enums;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews
{
	public class StrikeClearCorrectionView : View
	{
		public StrikeClearCorrectionView()
			: this()
		{
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel);
			Dictionary<Encounters.StrikeMission, DateTime> clears = new Dictionary<Encounters.StrikeMission, DateTime>();
			if (!Service.StrikePersistance.AccountClears.TryGetValue(Service.CurrentAccountName, out clears))
			{
				clears = new Dictionary<Encounters.StrikeMission, DateTime>();
			}
			foreach (KeyValuePair<Encounters.StrikeMission, DateTime> entry in clears.OrderBy((KeyValuePair<Encounters.StrikeMission, DateTime> p) => p.Key))
			{
				panel.AddEncounterClearStatus(entry.Key, entry.Value);
			}
			panel.AddString("Last Strike Mission Clears");
		}
	}
}
