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
		protected string characterName = "";

		protected Dictionary<int, AsyncTexture2D> itemTextures = new Dictionary<int, AsyncTexture2D>();

		protected List<ItemForDisplay> adviceList = new List<ItemForDisplay>();

		protected List<BagForDisplay> bags = new List<BagForDisplay>();

		protected AsyncTexture2D border = AsyncTexture2D.FromAssetId(683588);

		protected FlowPanel panel = new FlowPanel
		{
			WidthSizingMode = SizingMode.Fill,
			HeightSizingMode = SizingMode.Fill,
			FlowDirection = ControlFlowDirection.LeftToRight,
			CanScroll = true
		};

		public void update(List<ItemForDisplay> items_, string name_, List<BagForDisplay> bags_)
		{
			adviceList = items_;
			characterName = name_;
			bags = bags_;
			panel.Children.ToList().ForEach(delegate(Control item)
			{
				item.Dispose();
			});
			panel.ClearChildren();
			build_inventory_panel();
		}

		protected virtual void build_inventory_panel()
		{
			Container parent = panel.Parent;
			panel.Parent = null;
			panel = new FlowPanel
			{
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.LeftToRight,
				ControlPadding = new Vector2(5f, 5f),
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
					Size = new Point(45, 45),
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

		public void set_values(Dictionary<int, AsyncTexture2D> itemTextures_)
		{
			itemTextures = itemTextures_;
		}

		protected override void Build(Container buildPanel)
		{
			panel.Parent = buildPanel;
			build_inventory_panel();
		}
	}
}
