using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using gw2stacks_blish.data;

namespace views
{
	internal class FullCharacterBagView : FullCharacterView
	{
		protected override void build_inventory_panel()
		{
			Container parent = panel.Parent;
			panel.Parent = null;
			panel = new FlowPanel
			{
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				CanScroll = true
			};
			panel.Parent = parent;
			if (!itemTextures.ContainsKey(63172))
			{
				itemTextures.Add(63172, AsyncTexture2D.FromAssetId(63172));
			}
			_ = adviceList;
			panel.Title = characterName;
			FlowPanel currentPanel = new FlowPanel
			{
				WidthSizingMode = SizingMode.AutoSize,
				HeightSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.LeftToRight,
				CanScroll = false
			};
			int i = 0;
			int index = 0;
			foreach (BagForDisplay bag in bags)
			{
				if (!itemTextures.ContainsKey(bag.get_icon_id()))
				{
					itemTextures.Add(bag.get_icon_id(), AsyncTexture2D.FromAssetId(bag.get_icon_id()));
				}
				ViewContainer wrapper = new ViewContainer
				{
					WidthSizingMode = SizingMode.Fill,
					Height = 160,
					ShowBorder = true,
					Parent = panel
				};
				wrapper.Icon = itemTextures[bag.get_icon_id()];
				wrapper.Title = bag.get_name();
				wrapper.BasicTooltipText = bag.get_advice();
				wrapper.Show();
				currentPanel = new FlowPanel
				{
					WidthSizingMode = SizingMode.Fill,
					HeightSizingMode = SizingMode.Fill,
					FlowDirection = ControlFlowDirection.LeftToRight,
					ControlPadding = new Vector2(5f, 5f),
					CanScroll = true
				};
				currentPanel.Show();
				currentPanel.Parent = wrapper;
				i = 0;
				while (i != bag.get_size())
				{
					ItemForDisplay advice = adviceList[index];
					if (!itemTextures.ContainsKey(advice.get_iconId()))
					{
						itemTextures.Add(advice.get_iconId(), AsyncTexture2D.FromAssetId(advice.get_iconId()));
					}
					Image container = new Image
					{
						Texture = itemTextures[advice.get_iconId()],
						Size = new Point(45, 45),
						Location = new Point(0, 0),
						Parent = currentPanel
					};
					string text = advice.print(characterName);
					if (text != null)
					{
						container.BasicTooltipText = text;
					}
					i++;
					index++;
				}
			}
		}
	}
}
