using System.Collections.Generic;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using gw2stacks_blish.data;

namespace views
{
	internal class FullCharacterView : View
	{
		private string characterName = "";

		private Dictionary<int, AsyncTexture2D> itemTextures;

		private List<ItemForDisplay> adviceList;

		private List<BagForDisplay> bags;

		private bool showBags;

		private FlowPanel panel = new FlowPanel
		{
			WidthSizingMode = SizingMode.Fill,
			HeightSizingMode = SizingMode.Fill,
			FlowDirection = ControlFlowDirection.LeftToRight,
			CanScroll = true
		};

		public void set_bag_flag(bool flag_)
		{
			showBags = flag_;
			update(itemTextures, adviceList, characterName, bags);
		}

		public void update(Dictionary<int, AsyncTexture2D> itemTextures_, List<ItemForDisplay> items_, string name_, List<BagForDisplay> bags_)
		{
			itemTextures = itemTextures_;
			adviceList = items_;
			characterName = name_;
			bags = bags_;
			if (showBags)
			{
				build_bag_inventory_panel();
			}
			else
			{
				build_inventory_panel();
			}
		}

		private void build_bag_inventory_panel()
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
					Height = 128,
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
						Size = new Point(40, 40),
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

		private void build_inventory_panel()
		{
			Container parent = panel.Parent;
			panel.Parent = null;
			panel = new FlowPanel
			{
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.LeftToRight,
				CanScroll = true
			};
			panel.Parent = parent;
			if (!itemTextures.ContainsKey(63172))
			{
				itemTextures.Add(63172, AsyncTexture2D.FromAssetId(63172));
			}
			List<ItemForDisplay> list = adviceList;
			panel.Title = characterName;
			foreach (ItemForDisplay advice in list)
			{
				if (!itemTextures.ContainsKey(advice.get_iconId()))
				{
					itemTextures.Add(advice.get_iconId(), AsyncTexture2D.FromAssetId(advice.get_iconId()));
				}
				Image container = new Image
				{
					Texture = itemTextures[advice.get_iconId()],
					Size = new Point(40, 40),
					Location = new Point(0, 0),
					Parent = panel
				};
				string text = advice.print(characterName);
				if (text != null)
				{
					container.BasicTooltipText = text;
				}
				container.Show();
			}
		}

		protected override void Build(Container buildPanel)
		{
			panel.Parent = buildPanel;
			build_inventory_panel();
		}
	}
}
