using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Models;
using RaidClears.Localization;
using RaidClears.Settings.Controls;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews
{
	public class StrikeLabelCustomizationView : View
	{
		public StrikeLabelCustomizationView()
			: this()
		{
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel);
			((Panel)panel).set_CanScroll(true);
			new Dictionary<string, DateTime>();
			if (Service.DailyBountyData != null && Service.DailyBountyData.Enabled)
			{
				((Container)(object)panel).AddControl((Control)(object)new EncounterLabelCustomerizer(panel, Service.StrikeSettings, Service.StrikeData.Priority, Color.get_Gold()));
				((Container)(object)panel).AddControl((Control)(object)new EncounterLabelCustomerizer(panel, Service.StrikeSettings, Service.StrikeData.PriorityTomorrow, Color.get_Gold()));
				panel.AddSpace();
			}
			foreach (ExpansionStrikes expansion in Service.StrikeData.Expansions)
			{
				((Container)(object)panel).AddControl((Control)(object)new EncounterLabelCustomerizer(panel, Service.StrikeSettings, expansion, Color.get_Gold()));
				foreach (BossEncounter mission in expansion.Missions)
				{
					EncounterLabelCustomerizer customerizer = new EncounterLabelCustomerizer(panel, Service.StrikeSettings, mission);
					((Container)(object)panel).AddControl((Control)(object)customerizer);
				}
				panel.AddSpace();
			}
			panel.AddString(Strings.StrikeLabelCustomization_Heading);
		}
	}
}
