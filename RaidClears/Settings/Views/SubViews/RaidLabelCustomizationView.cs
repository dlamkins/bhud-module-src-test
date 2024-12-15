using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using RaidClears.Features.Raids.Models;
using RaidClears.Settings.Controls;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews
{
	public class RaidLabelCustomizationView : View
	{
		public RaidLabelCustomizationView()
			: this()
		{
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel);
			((Panel)panel).set_CanScroll(true);
			new Dictionary<string, DateTime>();
			foreach (ExpansionRaid expansion in Service.RaidData.Expansions)
			{
				panel.AddString(expansion.Name, Color.get_Gold());
				foreach (RaidWing wing in expansion.Wings)
				{
					((Container)(object)panel).AddControl((Control)(object)new EncounterLabelCustomerizer(panel, Service.RaidSettings, wing, Color.get_Gray()));
					foreach (RaidEncounter encounter in wing.Encounters)
					{
						EncounterLabelCustomerizer customerizer = new EncounterLabelCustomerizer(panel, Service.RaidSettings, encounter);
						((Container)(object)panel).AddControl((Control)(object)customerizer);
					}
					panel.AddSpace();
				}
			}
			panel.AddString("Customize Raid Boss Labels");
		}
	}
}
