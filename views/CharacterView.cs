using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using gw2stacks_blish.data;

namespace views
{
	internal class CharacterView : View
	{
		private string characterName = "";

		private Dictionary<int, AsyncTexture2D> itemTextures;

		private List<ItemForDisplay> adviceList;

		private FlowPanel panel = new FlowPanel
		{
			WidthSizingMode = SizingMode.Fill,
			HeightSizingMode = SizingMode.Fill,
			FlowDirection = ControlFlowDirection.SingleTopToBottom,
			CanScroll = true
		};

		public void update(Dictionary<int, AsyncTexture2D> itemTextures_, List<ItemForDisplay> items_, string names_)
		{
			itemTextures = itemTextures_;
			adviceList = items_;
			characterName = names_;
			build_inventory_panel();
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
			List<ItemForDisplay> list = adviceList.Where((ItemForDisplay item) => item.sources.Any((Source source) => source.place == characterName)).ToList();
			panel.Title = characterName;
			foreach (ItemForDisplay advice in list)
			{
				if (!itemTextures.ContainsKey(advice.get_id()))
				{
					itemTextures.Add(advice.get_id(), AsyncTexture2D.FromAssetId(advice.get_iconId()));
				}
				Image image = new Image();
				image.Texture = itemTextures[advice.get_id()];
				image.Size = new Point(40, 40);
				image.Location = new Point(0, 0);
				image.Parent = panel;
				image.BasicTooltipText = Magic.get_local_name(advice.get_id()) + "\n" + advice.ToString();
				image.Show();
			}
		}

		protected override void Build(Container buildPanel)
		{
			panel.Parent = buildPanel;
			build_inventory_panel();
		}
	}
}
