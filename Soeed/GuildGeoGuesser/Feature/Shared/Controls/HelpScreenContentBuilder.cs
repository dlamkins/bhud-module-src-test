using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public static class HelpScreenContentBuilder
	{
		public static void Build(FlowPanel panel, HelpScreen helpScreens)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Expected O, but got Unknown
			if (helpScreens.SchemaVersion != 1)
			{
				panel.AddString($"Error Loading Help Screens with schema version {helpScreens.SchemaVersion}").AddString("This module is only compatible with schema version 1");
				return;
			}
			foreach (HelpScreenPanel screen in helpScreens.Screens)
			{
				FlowPanel val = new FlowPanel();
				((Panel)val).set_Title(screen.Name);
				((Panel)val).set_Collapsed(false);
				val.set_FlowDirection((ControlFlowDirection)3);
				((Container)val).set_WidthSizingMode((SizingMode)2);
				((Container)val).set_HeightSizingMode((SizingMode)1);
				val.set_ControlPadding(new Vector2(5f, 5f));
				val.set_OuterControlPadding(new Vector2(5f, 5f));
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
	}
}
