using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Discovery.ItemList
{
	public class ItemListView : View<IItemListPresenter>
	{
		private Panel _noResultsPanel;

		private Label _nothingLabel;

		private Image _nothingImage;

		private Label _limitLabel;

		public int ItemLimit { get; set; } = 50;


		public ViewContainer Container { get; set; }

		public FlowPanel Panel { get; set; }

		public ItemListModel Model { get; set; }

		public Dropdown RarityDropdown { get; set; }

		public bool LimitReached
		{
			get
			{
				return _limitLabel?.Visible ?? false;
			}
			set
			{
				if (_limitLabel != null)
				{
					_limitLabel.Visible = value;
				}
			}
		}

		public bool NoResults
		{
			get
			{
				return _noResultsPanel?.Visible ?? false;
			}
			set
			{
				if (_noResultsPanel != null)
				{
					_noResultsPanel.Visible = value;
					if (_noResultsPanel.Parent == null)
					{
						_noResultsPanel.Parent = Panel;
					}
				}
			}
		}

		public Container BuildPanel { get; set; }

		public ItemListView(ItemListModel itemListModel)
		{
			Model = itemListModel;
			WithPresenter(new ItemListPresenter(this, itemListModel));
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
			BuildNoResultsPanel();
			_limitLabel = new Label
			{
				Text = Common.MaximumResultsReached,
				Parent = buildPanel,
				Width = 800,
				TextColor = Color.Orange,
				Font = GameService.Content.DefaultFont16,
				Top = 5,
				Left = 160,
				StrokeText = true,
				Visible = false
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
			if (string.IsNullOrEmpty(Model?.Filter?.Rarity))
			{
				dropdown.SelectedItem = "All rarities";
			}
			else
			{
				dropdown.SelectedItem = Model?.Filter.Rarity;
			}
			RarityDropdown = dropdown;
		}

		public void BuildNoResultsPanel()
		{
			_noResultsPanel?.ClearChildren();
			_noResultsPanel?.Dispose();
			_noResultsPanel = new Panel
			{
				Parent = Panel,
				Size = Panel.Size,
				Visible = false
			};
			_nothingLabel = new Label
			{
				Text = Common.FavoritesEmpty,
				Parent = _noResultsPanel,
				Font = GameService.Content.DefaultFont32,
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				StrokeText = true,
				Location = new Point(120, 200)
			};
			_nothingImage = new Image
			{
				Texture = ServiceContainer.TextureRepository.Textures.HeartDisabled,
				Parent = _noResultsPanel,
				Size = new Point(50, 50),
				Location = new Point(350, 300)
			};
		}

		public void SetItemRows(IList<ItemRowView> rows)
		{
			Panel.ClearChildren();
			foreach (ItemRowView row in rows)
			{
				ViewContainer viewContainer = new ViewContainer();
				viewContainer.Size = new Point(Panel.Width, 65);
				viewContainer.Parent = Panel;
				viewContainer.Show(row);
			}
		}

		public void SetLimitLabelParent(Container parent)
		{
			if (_limitLabel != null)
			{
				_limitLabel.Parent = parent;
			}
		}
	}
}
