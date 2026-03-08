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
	internal class IgnoredView : View
	{
		private FlowPanel panel = new FlowPanel
		{
			WidthSizingMode = SizingMode.Fill,
			HeightSizingMode = SizingMode.Fill,
			FlowDirection = ControlFlowDirection.SingleTopToBottom,
			CanScroll = true
		};

		private Dictionary<int, AsyncTexture2D> itemTextures;

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
			update(panel.Title);
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

		private void build_ignored_panels(Panel rootPanel)
		{
			search.Parent = rootPanel;
			search.TextChanged += handle_text_input;
			foreach (int item in excludedItemIds)
			{
				int icon = 0;
				if (string.IsNullOrEmpty(hunt) || Magic.get_local_name(item).ToLower().Contains(hunt.ToLower()))
				{
					icon = ((!Magic.jsonLut.itemLut.ContainsKey(item)) ? Magic.unknown.IconId : Magic.jsonLut.itemLut[item].IconId);
					if (!itemTextures.ContainsKey(icon))
					{
						itemTextures.Add(icon, AsyncTexture2D.FromAssetId(icon));
					}
					ViewContainer standardPanel = GetStandardPanel(rootPanel, Magic.get_local_name(item), icon);
					standardPanel.Click += delegate
					{
						callback(item, arg2: false);
					};
					standardPanel.Show();
				}
			}
		}

		public void update(string title_)
		{
			search.Parent = null;
			search.TextChanged -= handle_text_input;
			panel.ClearChildren();
			build_ignored_panels(panel);
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
			build_ignored_panels(panel);
		}
	}
}
