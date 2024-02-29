using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Recipe.TreeView.Extensions;
using MysticCrafting.Module.Recipe.TreeView.Nodes;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Recipe.TreeView.Tooltips
{
	public class ItemNodeTooltipView : ItemTooltipView
	{
		private Panel _linkedNodesPanel;

		private Label _rightClickOptionsLabel;

		private IngredientNode Node { get; set; }

		public override string PlayerCountText => Node?.GetPlayerCountAsText() ?? "";

		public ItemNodeTooltipView(IngredientNode node)
			: base(node.Item, node.RequiredQuantity)
		{
			Node = node;
			base.PlayerItemCount = node.GetPlayerItemCount();
		}

		public void UpdateLinkedNodes()
		{
			if (Initialized)
			{
				base.PlayerItemCount = Node.GetPlayerItemCount();
				int yPosition = base.Bottom + 15;
				List<IngredientNode> linkedNodes = Node.TreeView.IngredientNodes.ToList().GetByItemId(Node.Item.GameId).ToList();
				if (linkedNodes.Count > 0)
				{
					UpdateLinkedNodes(linkedNodes, ref yPosition);
				}
				_rightClickOptionsLabel?.Dispose();
				_rightClickOptionsLabel = new Label
				{
					Parent = base.BuildPanel,
					Text = MysticCrafting.Module.Strings.Recipe.TooltipOpenMenuText,
					Location = new Point(0, yPosition),
					Font = GameService.Content.DefaultFont14,
					TextColor = Color.LightGray,
					StrokeText = true,
					AutoSizeWidth = true
				};
			}
		}

		public void UpdateLinkedNodes(IList<IngredientNode> linkedNodes, ref int yPosition)
		{
			if (Node == null)
			{
				return;
			}
			_linkedNodesPanel?.Dispose();
			if (linkedNodes.Count <= 1)
			{
				return;
			}
			_linkedNodesPanel = new Panel
			{
				Parent = base.BuildPanel,
				Location = new Point(0, yPosition),
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.AutoSize
			};
			int totalPlayerItemCount = ServiceContainer.PlayerItemService.GetItemCount(Node.Item.GameId);
			Label titleLabel = new Label
			{
				Parent = _linkedNodesPanel,
				Text = MysticCrafting.Module.Strings.Recipe.AllRecipes,
				Location = new Point(0, 0),
				Font = GameService.Content.DefaultFont16,
				ShowShadow = true,
				TextColor = Color.White,
				AutoSizeWidth = true
			};
			new Label
			{
				Parent = _linkedNodesPanel,
				Text = $"({totalPlayerItemCount}/{linkedNodes.Sum((IngredientNode n) => n.TotalRequiredQuantity)})",
				Location = new Point(titleLabel.Right + 5, 0),
				Font = GameService.Content.DefaultFont16,
				ShowShadow = true,
				TextColor = Color.LightGray,
				AutoSizeWidth = true
			};
			yPosition += 25;
			int xPosition = 0;
			int childPosition = 25;
			foreach (IngredientNode node in linkedNodes)
			{
				if (node != null && node.TotalRequiredQuantity != 0)
				{
					bool currentItem = node == Node;
					UpdateLinkedNode(node, ref xPosition, ref childPosition, currentItem);
				}
			}
			yPosition += childPosition - 15;
		}

		public void UpdateLinkedNode(IngredientNode node, ref int xPosition, ref int yPosition, bool isCurrentItem = false)
		{
			if (node != null && node.TotalRequiredQuantity != 0)
			{
				new Image(ServiceContainer.TextureRepository.GetTexture(node.Item.Icon))
				{
					Parent = _linkedNodesPanel,
					Size = new Point(25, 25),
					Location = new Point(xPosition, yPosition)
				};
				Label countLabel = new Label
				{
					Parent = _linkedNodesPanel,
					Text = node.GetPlayerCountAsText(),
					Location = new Point(xPosition + 30, yPosition + 3),
					Font = GameService.Content.DefaultFont16,
					ShowShadow = true,
					TextColor = Color.LightGray,
					AutoSizeWidth = true
				};
				if (isCurrentItem)
				{
					new Label
					{
						Parent = _linkedNodesPanel,
						Text = "(" + MysticCrafting.Module.Strings.Recipe.CurrentRecipe + ")",
						Location = new Point(countLabel.Right + 5, yPosition + 3),
						Font = GameService.Content.DefaultFont14,
						StrokeText = true,
						TextColor = Color.LightGray,
						AutoSizeWidth = true
					};
				}
				yPosition += 30;
			}
		}

		protected override void Unload()
		{
			_linkedNodesPanel?.Dispose();
			Node = null;
			base.BuildPanel = null;
			base.Unload();
		}

		protected override void Build(Container buildPanel)
		{
			base.Build(buildPanel);
			UpdateLinkedNodes();
		}
	}
}
