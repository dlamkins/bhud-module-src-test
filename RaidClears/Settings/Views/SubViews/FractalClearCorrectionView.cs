using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews
{
	public class FractalClearCorrectionView : View
	{
		public FractalClearCorrectionView()
			: this()
		{
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel);
			Dictionary<string, DateTime> clears = new Dictionary<string, DateTime>();
			if (!Service.FractalPersistance.AccountClears.TryGetValue(Service.CurrentAccountName, out clears))
			{
				clears = new Dictionary<string, DateTime>();
			}
			foreach (KeyValuePair<string, DateTime> entry in clears.OrderBy((KeyValuePair<string, DateTime> p) => p.Key))
			{
				panel.AddEncounterClearStatus(Service.FractalMapData.GetFractalByApiName(entry.Key), entry.Value);
			}
			panel.AddString("Last Fractal Clears (Profile: " + Service.CurrentAccountName + ")");
		}
	}
}
