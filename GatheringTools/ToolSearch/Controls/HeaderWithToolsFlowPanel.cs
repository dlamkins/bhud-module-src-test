using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using GatheringTools.ToolSearch.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GatheringTools.ToolSearch.Controls
{
	public class HeaderWithToolsFlowPanel : FlowPanel
	{
		private const int ICON_WIDTH_HEIGHT = 50;

		private const int WIDTH_OF_3_GATHERING_TOOLS = 155;

		private const int PADDING_TO_PREVENT_LINE_BREAK = 5;

		public HeaderWithToolsFlowPanel(string headerText, Texture2D headerTexture, List<GatheringTool> gatheringTools, Logger logger)
			: this()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)2);
			Image val = new Image(AsyncTexture2D.op_Implicit(headerTexture));
			((Control)val).set_BasicTooltipText(headerText);
			((Control)val).set_Size(new Point(30, 30));
			((Control)val).set_Parent((Container)(object)this);
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)0);
			((Control)val2).set_Size(new Point(155, 0));
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Parent((Container)(object)this);
			FlowPanel toolsFlowPanel = val2;
			foreach (GatheringTool gatheringTool in gatheringTools)
			{
				ShowGatheringToolImageOrFallbackLabel(logger, gatheringTool, toolsFlowPanel);
			}
		}

		private static void ShowGatheringToolImageOrFallbackLabel(Logger logger, GatheringTool gatheringTool, FlowPanel toolsFlowPanel)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Image val = new Image(GameService.Content.GetRenderServiceTexture(gatheringTool.IconUrl));
				((Control)val).set_BasicTooltipText(gatheringTool.Name);
				((Control)val).set_Size(new Point(50, 50));
				((Control)val).set_Parent((Container)(object)toolsFlowPanel);
			}
			catch (Exception e)
			{
				Label val2 = new Label();
				val2.set_Text(gatheringTool.Name);
				((Control)val2).set_BasicTooltipText(gatheringTool.Name);
				val2.set_Font(GameService.Content.get_DefaultFont18());
				val2.set_ShowShadow(true);
				val2.set_AutoSizeHeight(true);
				val2.set_AutoSizeWidth(true);
				((Control)val2).set_Parent((Container)(object)toolsFlowPanel);
				logger.Error(e, "Could not get gathering tool icon from API. Show gathering tool name instead.");
			}
		}
	}
}
