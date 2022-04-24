using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace GatheringTools.ToolSearch
{
	public class CharacterAndToolsFlowPanel : FlowPanel
	{
		public CharacterAndToolsFlowPanel(CharacterAndTools characterAndTools, bool onlyUnlimitedToolsAreVisible, Logger logger)
			: this()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text(characterAndTools.CharacterName);
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_ShowShadow(true);
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			((Control)val).set_Parent((Container)(object)this);
			List<GatheringTool> obj = (onlyUnlimitedToolsAreVisible ? characterAndTools.UnlimitedGatheringTools : characterAndTools.GatheringTools);
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)2);
			((Container)val2).set_WidthSizingMode((SizingMode)1);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Parent((Container)(object)this);
			FlowPanel toolsFlowPanel = val2;
			foreach (GatheringTool gatheringTool in obj)
			{
				try
				{
					Image val3 = new Image(GameService.Content.GetRenderServiceTexture(gatheringTool.IconUrl));
					((Control)val3).set_BasicTooltipText(gatheringTool.Name);
					((Control)val3).set_Size(new Point(50, 50));
					((Control)val3).set_Parent((Container)(object)toolsFlowPanel);
				}
				catch (Exception e)
				{
					Label val4 = new Label();
					val4.set_Text(gatheringTool.Name);
					val4.set_Font(GameService.Content.get_DefaultFont18());
					val4.set_ShowShadow(true);
					val4.set_AutoSizeHeight(true);
					val4.set_AutoSizeWidth(true);
					((Control)val4).set_Parent((Container)(object)toolsFlowPanel);
					logger.Error("Could not get gathering tool icon from API. Show gathering tool name instead.", new object[1] { e });
				}
			}
		}
	}
}
