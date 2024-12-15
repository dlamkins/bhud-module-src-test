using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Models;
using RaidClears.Settings.Controls;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews
{
	public class FractalLabelCustomizationView : View
	{
		public FractalLabelCustomizationView()
			: this()
		{
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel);
			((Panel)panel).set_CanScroll(true);
			new Dictionary<string, DateTime>();
			foreach (EncounterInterface category in Service.FractalMapData.Categories)
			{
				((Container)(object)panel).AddControl((Control)(object)new EncounterLabelCustomerizer(panel, Service.FractalPersistance, category, Color.get_Gold()));
			}
			panel.AddSpace();
			using (Dictionary<string, RaidClears.Features.Fractals.Services.FractalMap>.Enumerator enumerator2 = Service.FractalMapData.Maps.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					((Container)(object)panel).AddControl((Control)(object)new EncounterLabelCustomerizer(encounter: enumerator2.Current.Value.ToEncounterInterface(), parent: panel, labelable: Service.FractalPersistance, labelColor: Color.get_White()));
				}
			}
			panel.AddString("Customize Fractal Labels");
		}
	}
}
