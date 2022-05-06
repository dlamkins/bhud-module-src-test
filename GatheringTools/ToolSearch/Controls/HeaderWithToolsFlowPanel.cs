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

		public HeaderWithToolsFlowPanel(string headerText, List<GatheringTool> gatheringTools, Texture2D headerTexture, Texture2D unknownToolTexture, Logger logger)
			: this()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
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
				ShowGatheringToolImageOrFallbackControl(gatheringTool, unknownToolTexture, toolsFlowPanel, logger);
			}
		}

		private static void ShowGatheringToolImageOrFallbackControl(GatheringTool gatheringTool, Texture2D unknownToolTexture, FlowPanel toolsFlowPanel, Logger logger)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D obj = DetermineToolTexture(gatheringTool, unknownToolTexture, logger);
			string tooltipText = DetermineTooltipText(gatheringTool);
			Image val = new Image(obj);
			((Control)val).set_BasicTooltipText(tooltipText);
			((Control)val).set_Size(new Point(50, 50));
			((Control)val).set_Parent((Container)(object)toolsFlowPanel);
		}

		private static string DetermineTooltipText(GatheringTool gatheringTool)
		{
			return gatheringTool.ToolType switch
			{
				ToolType.Normal => gatheringTool.Name, 
				ToolType.UnknownId => $"No data in the API for this item ID: {gatheringTool.Id}. :(\n" + "Could be a very new or very old item. Or a bug in the API.", 
				ToolType.InventoryCanNotBeAccessedPlaceHolder => "Can not access inventory for this character. :(\nCould be an API error or API key has no 'inventories' permission.", 
				_ => $"Bug in module code. ToolType {gatheringTool.ToolType} is not handled. :(", 
			};
		}

		private static AsyncTexture2D DetermineToolTexture(GatheringTool gatheringTool, Texture2D unknownToolTexture, Logger logger)
		{
			if (gatheringTool.ToolType != 0)
			{
				return AsyncTexture2D.op_Implicit(unknownToolTexture);
			}
			try
			{
				return GameService.Content.GetRenderServiceTexture(gatheringTool.IconUrl);
			}
			catch (Exception e)
			{
				logger.Error(e, "Could not get gathering tool icon from API. Show placeholder icon instead.");
				return AsyncTexture2D.op_Implicit(unknownToolTexture);
			}
		}
	}
}
