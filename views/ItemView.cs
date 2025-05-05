using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using gw2stacks_blish.data;

namespace views
{
	internal class ItemView : View
	{
		private FlowPanel panel = new FlowPanel
		{
			WidthSizingMode = SizingMode.Fill,
			HeightSizingMode = SizingMode.Fill,
			FlowDirection = ControlFlowDirection.SingleTopToBottom,
			CanScroll = true
		};

		private Dictionary<int, AsyncTexture2D> itemTextures;

		public List<ItemForDisplay> combinedAdvice = new List<ItemForDisplay>();

		private TextBox search = new TextBox
		{
			PlaceholderText = "Enter item name here ...",
			Size = new Point(830, 43),
			Font = GameService.Content.DefaultFont16,
			Location = new Point(0, 0)
		};

		private string hunt = "";

		public void set_search_string(string input_)
		{
			string sanitized = input_.Trim('[', ']', ' ', '\t', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9').TrimEnd('s');
			search.Text = sanitized;
			update(chatCode: true);
		}

		private void handle_text_input(object s_, EventArgs e_)
		{
			hunt = search.Text;
			update();
		}

		private ViewContainer GetStandardPanel(Panel rootPanel, string title, int id_)
		{
			return new ViewContainer
			{
				Icon = itemTextures[id_],
				Width = 830,
				HeightSizingMode = SizingMode.AutoSize,
				Title = title,
				ShowBorder = true,
				Parent = rootPanel
			};
		}

		private void build_item_panels(Panel rootPanel, bool chatCode)
		{
			search.Parent = rootPanel;
			search.TextChanged += handle_text_input;
			foreach (ItemForDisplay item in combinedAdvice)
			{
				if (string.IsNullOrEmpty(hunt) || Magic.get_local_name(item.get_id()).ToLower().Contains(hunt.ToLower()) || (chatCode && Magic.string_similar(hunt.ToLower(), Magic.get_local_name(item.get_id()).ToLower())))
				{
					if (!itemTextures.ContainsKey(item.get_iconId()))
					{
						itemTextures.Add(item.get_iconId(), AsyncTexture2D.FromAssetId(item.get_iconId()));
					}
					ViewContainer standardPanel = GetStandardPanel(rootPanel, Magic.get_local_name(item.get_id()), item.get_iconId());
					standardPanel.BasicTooltipText = item.ToString();
					standardPanel.Show();
				}
			}
		}

		public void update(bool chatCode = false)
		{
			search.Parent = null;
			panel.ClearChildren();
			search.TextChanged -= handle_text_input;
			build_item_panels(panel, chatCode);
			panel.Title = "Gw2stacks";
		}

		public void set_values(Dictionary<int, AsyncTexture2D> itemTextures_, List<ItemForDisplay> excludedItemIds_)
		{
			itemTextures = itemTextures_;
			combinedAdvice = excludedItemIds_;
		}

		protected override void Build(Container buildPanel)
		{
			panel.Parent = buildPanel;
			panel.ClearChildren();
			build_item_panels(panel, chatCode: false);
		}
	}
}
