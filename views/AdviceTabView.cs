using System.Collections.Generic;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using gw2stacks_blish.data;

namespace views
{
	internal class AdviceTabView : View
	{
		private List<ItemForDisplay> adviceList = new List<ItemForDisplay>();

		private FlowPanel panel;

		private Dictionary<int, AsyncTexture2D> itemTextures;

		private ViewContainer GetStandardPanel(Panel rootPanel, string title, int id_)
		{
			return new ViewContainer
			{
				Icon = itemTextures[id_],
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				Title = title,
				ShowBorder = true,
				Parent = rootPanel
			};
		}

		private void build_item_panels(Panel rootPanel)
		{
			foreach (ItemForDisplay item in adviceList)
			{
				if (!itemTextures.ContainsKey(item.item.itemId))
				{
					itemTextures.Add(item.item.itemId, AsyncTexture2D.FromAssetId(item.item.iconId));
				}
				ViewContainer standardPanel = GetStandardPanel(rootPanel, Magic.get_local_name(item.item.itemId), item.item.itemId);
				standardPanel.BasicTooltipText = Magic.get_current_translated_string(item.advice) + "\n" + item.get_source_string();
				standardPanel.Show();
			}
		}

		public void update(List<ItemForDisplay> items_, string title_, Dictionary<int, AsyncTexture2D> itemTextures_)
		{
			panel.ClearChildren();
			adviceList = items_;
			itemTextures = itemTextures_;
			build_item_panels(panel);
			panel.Title = title_;
		}

		protected override void Build(Container buildPanel)
		{
			panel = new FlowPanel
			{
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				CanScroll = true,
				Parent = buildPanel
			};
			build_item_panels(panel);
		}
	}
}
