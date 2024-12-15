using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using RaidClears.Features.Strikes.Models;
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
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel);
			((Panel)panel).set_CanScroll(true);
			Dictionary<string, DateTime> clears = new Dictionary<string, DateTime>();
			if (!Service.StrikePersistance.AccountClears.TryGetValue(Service.CurrentAccountName, out clears))
			{
				clears = new Dictionary<string, DateTime>();
			}
			foreach (ExpansionStrikes expansion in Service.StrikeData.Expansions.OrderBy((ExpansionStrikes x) => x.Name))
			{
				panel.AddString(expansion.Name, Color.get_Gold());
				foreach (StrikeMission mission in expansion.Missions.OrderBy((StrikeMission x) => x.Name))
				{
					if (clears.ContainsKey(mission.Id))
					{
						panel.AddEncounterClearStatus(mission, clears[mission.Id]);
					}
					else
					{
						panel.AddEncounterClearStatus(mission, default(DateTime));
					}
				}
			}
			panel.AddString("Last Strike Mission Clears (Profile: " + Service.CurrentAccountName + ")");
		}
	}
}
