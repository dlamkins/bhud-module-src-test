using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Items
{
	public class ItemListView : View<IItemListPresenter>
	{
		public int ItemLimit { get; set; } = 50;


		public ViewContainer Container { get; set; }

		public FlowPanel Panel { get; set; }

		public MysticItemFilter ItemFilter { get; set; }

		public Dropdown RarityDropdown { get; set; }

		public Container BuildPanel { get; set; }

		public List<string> MenuBreadcrumbs { get; set; } = new List<string>();


		public ItemListView(MysticItemFilter itemFilter)
		{
			ItemFilter = itemFilter;
			WithPresenter(new ItemListPresenter(this, new ItemListModel(), ServiceContainer.ItemRepository));
		}

		protected override void Build(Container buildPanel)
		{
			BuildPanel = buildPanel;
			Container = new ViewContainer
			{
				FadeView = true,
				Parent = buildPanel,
				Size = new Point(buildPanel.Width, buildPanel.Height),
				Padding = new Thickness(0f),
				Location = new Point(0, 0)
			};
			BuildFilters();
			Panel = new FlowPanel
			{
				Parent = Container,
				Location = new Point(0, 40),
				Width = Container.Width - 5,
				Height = Container.Height - 50,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				CanScroll = true
			};
		}

		public void BuildColumnNames()
		{
			new Label
			{
				Parent = Container,
				Text = "Icon",
				AutoSizeWidth = true,
				StrokeText = true,
				TextColor = Color.LightYellow,
				Location = new Point(5, 50)
			};
			new Label
			{
				Parent = Container,
				Text = "Name",
				AutoSizeWidth = true,
				StrokeText = true,
				TextColor = Color.Orange,
				Location = new Point(80, 50)
			};
			new Label
			{
				Parent = Container,
				Text = "Type",
				AutoSizeWidth = true,
				StrokeText = true,
				TextColor = Color.Orange,
				Location = new Point(Container.Width - 350, 50)
			};
			new Label
			{
				Parent = Container,
				Text = "Weight Class",
				AutoSizeWidth = true,
				StrokeText = true,
				TextColor = Color.Orange,
				Location = new Point(Container.Width - 200, 50)
			};
		}

		public void BuildFilters()
		{
			Dropdown dropdown = new Dropdown
			{
				Parent = Container,
				Width = 120,
				Visible = true
			};
			dropdown.Items.Add("All rarities");
			dropdown.Items.Add("Basic");
			dropdown.Items.Add("Fine");
			dropdown.Items.Add("Masterwork");
			dropdown.Items.Add("Rare");
			dropdown.Items.Add("Exotic");
			dropdown.Items.Add("Ascended");
			dropdown.Items.Add("Legendary");
			if (string.IsNullOrEmpty(ItemFilter?.Rarity))
			{
				dropdown.SelectedItem = "All rarities";
			}
			else
			{
				dropdown.SelectedItem = ItemFilter.Rarity;
			}
			RarityDropdown = dropdown;
		}
	}
}
