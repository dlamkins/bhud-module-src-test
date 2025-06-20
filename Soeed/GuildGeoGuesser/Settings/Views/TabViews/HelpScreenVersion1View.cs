using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Settings.Views.TabViews
{
	public class HelpScreenVersion1View : View
	{
		private HelpScreen HelpScreens = new HelpScreen();

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			progress.Report("Load helpscreenData");
			HelpScreen data = await Service.GeoServerWrapper.GetHelpScreens();
			if (data != null)
			{
				HelpScreens = data;
			}
			return true;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Expected O, but got Unknown
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel panel = FlowPanelExtensions.BeginFlowFill(new FlowPanel(), buildPanel);
			panel.set_OuterControlPadding(new Vector2(0f, 0f));
			((Control)panel).set_Height(((Control)buildPanel).get_Height());
			((Panel)panel).set_CanScroll(true);
			if (HelpScreens.SchemaVersion != 1)
			{
				panel.AddString($"Error Loading Help Screens with schema version {HelpScreens.SchemaVersion}").AddString("This module is only compatible with schema version 1");
				return;
			}
			foreach (HelpScreenPanel screen in HelpScreens.Screens)
			{
				FlowPanel val = new FlowPanel();
				((Panel)val).set_Title(screen.Name);
				((Panel)val).set_Collapsed(false);
				val.set_FlowDirection((ControlFlowDirection)3);
				((Container)val).set_WidthSizingMode((SizingMode)2);
				((Container)val).set_HeightSizingMode((SizingMode)1);
				val.set_ControlPadding(new Vector2(5f, 5f));
				FlowPanel screenPanel = val;
				foreach (HelpScreenContent line in screen.Content)
				{
					if (line.Type == "string")
					{
						screenPanel.AddString(line.Value);
					}
				}
				panel.AddControl<FlowPanel>(screenPanel);
			}
		}

		public HelpScreenVersion1View()
			: this()
		{
		}
	}
}
