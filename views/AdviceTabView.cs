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
	internal class AdviceTabView : View
	{
		private List<ItemForDisplay> adviceList = new List<ItemForDisplay>();

		private FlowPanel panel = new FlowPanel
		{
			WidthSizingMode = SizingMode.Fill,
			HeightSizingMode = SizingMode.Fill,
			FlowDirection = ControlFlowDirection.SingleTopToBottom,
			CanScroll = true
		};

		private Dictionary<int, AsyncTexture2D> itemTextures;

		private bool ignoredItemsFlag;

		private Action<int, bool> callback;

		public List<int> excludedItemIds = new List<int>();

		private TextBox search = new TextBox
		{
			PlaceholderText = "Enter item name here ...",
			Size = new Point(358, 43),
			Font = GameService.Content.DefaultFont16,
			Location = new Point(0, 0)
		};

		private string hunt = "";

		private void handle_text_input(object s_, EventArgs e_)
		{
			hunt = search.Text;
			refresh();
		}

		public void refresh()
		{
			update(adviceList, panel.Title, ignoredItemsFlag);
		}

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
			search.Parent = rootPanel;
			search.TextChanged += handle_text_input;
			search.Show();
			foreach (ItemForDisplay item in adviceList)
			{
				if (!itemTextures.ContainsKey(item.get_iconId()))
				{
					itemTextures.Add(item.get_iconId(), AsyncTexture2D.FromAssetId(item.get_iconId()));
				}
				if ((!string.IsNullOrEmpty(hunt) && !Magic.get_local_name(item.get_id()).ToLower().Contains(hunt.ToLower())) || excludedItemIds.Contains(item.get_id()))
				{
					continue;
				}
				ViewContainer container = GetStandardPanel(rootPanel, Magic.get_local_name(item.get_id()), item.get_iconId());
				container.BasicTooltipText = item.ToString();
				if (ignoredItemsFlag)
				{
					container.Click += delegate
					{
						callback(item.get_id(), arg2: true);
					};
				}
				container.Show();
			}
		}

		public void update(List<ItemForDisplay> items_, string title_, bool ignoredItemsFlag_)
		{
			search.Parent = null;
			search.TextChanged -= handle_text_input;
			panel.ClearChildren();
			ignoredItemsFlag = ignoredItemsFlag_;
			adviceList = items_;
			build_item_panels(panel);
			panel.Title = title_;
		}

		public void set_values(Dictionary<int, AsyncTexture2D> itemTextures_, Action<int, bool> callback_, List<int> excludedItemIds_)
		{
			itemTextures = itemTextures_;
			callback = callback_;
			excludedItemIds = excludedItemIds_;
		}

		protected override void Build(Container buildPanel)
		{
			panel.Parent = buildPanel;
			build_item_panels(panel);
		}
	}
}
