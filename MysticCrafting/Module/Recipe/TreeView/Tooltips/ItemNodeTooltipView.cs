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
		private IngredientNode Node { get; set; }

		private IList<Control> Controls { get; set; } = new List<Control>();


		public override int RequiredQuantity => Node?.RequiredQuantity ?? 0;

		public override string PlayerCountText => Node?.GetPlayerCountAsText() ?? "";

		public ItemNodeTooltipView(IngredientNode node)
			: base(node.Item, node.RequiredQuantity)
		{
			Node = node;
			base.PlayerItemCount = node.GetPlayerItemCount();
		}

		public void UpdateLinkedNodes()
		{
			if (!Initialized)
			{
				return;
			}
			base.PlayerItemCount = Node.GetPlayerItemCount();
			int yPosition = base.Bottom + 15;
			List<IngredientNode> linkedNodes = Node.TreeView.IngredientNodes.GetByItemId(Node.Item.Id).ToList();
			while (Controls.Count() > 0)
			{
				Control linkedNode = Controls.FirstOrDefault();
				if (linkedNode != null)
				{
					Controls.Remove(linkedNode);
					linkedNode.Parent = null;
					linkedNode.Dispose();
				}
			}
			if (linkedNodes.Count > 1)
			{
				UpdateLinkedNodes(linkedNodes, ref yPosition);
			}
		}

		public void UpdateLinkedNodes(IEnumerable<IngredientNode> linkedNodes, ref int yPosition)
		{
			if (Node == null)
			{
				return;
			}
			int totalPlayerItemCount = ServiceContainer.PlayerItemService.GetItemCount(Node.Item.Id);
			if (base.AccountTitleLabel != null)
			{
				Controls.Add(new Label
				{
					Parent = base.BuildPanel,
					Text = $"({totalPlayerItemCount})",
					Location = new Point(base.AccountTitleLabel.Right + 5, base.AccountTitleLabel.Top),
					Font = GameService.Content.DefaultFont16,
					ShowShadow = true,
					TextColor = Color.LightGray,
					AutoSizeWidth = true
				});
			}
			Label titleLabel = new Label
			{
				Parent = base.BuildPanel,
				Text = MysticCrafting.Module.Strings.Recipe.AllRecipes,
				Location = new Point(0, yPosition),
				Font = GameService.Content.DefaultFont16,
				ShowShadow = true,
				TextColor = Color.White,
				AutoSizeWidth = true
			};
			Controls.Add(titleLabel);
			Controls.Add(new Label
			{
				Parent = base.BuildPanel,
				Text = $"({totalPlayerItemCount}/{linkedNodes.Sum((IngredientNode n) => n.TotalRequiredQuantity)})",
				Location = new Point(titleLabel.Right + 5, yPosition),
				Font = GameService.Content.DefaultFont16,
				ShowShadow = true,
				TextColor = Color.LightGray,
				AutoSizeWidth = true
			});
			yPosition += 25;
			int xPosition = 0;
			foreach (IngredientNode node in linkedNodes)
			{
				if (node.TotalRequiredQuantity != 0)
				{
					node.GetParents().Reverse();
					bool currentItem = node == Node;
					UpdateLinkedNode(node, ref xPosition, ref yPosition, currentItem);
				}
			}
		}

		public void UpdateLinkedNode(IngredientNode node, ref int xPosition, ref int yPosition, bool isCurrentItem = false)
		{
			if (node != null && node.TotalRequiredQuantity != 0)
			{
				Controls.Add(new Image(ServiceContainer.TextureRepository.GetTexture(node.Item.Icon))
				{
					Parent = base.BuildPanel,
					Size = new Point(25, 25),
					Location = new Point(xPosition, yPosition)
				});
				Label countLabel = new Label
				{
					Parent = base.BuildPanel,
					Text = node.GetPlayerCountAsText(),
					Location = new Point(xPosition + 30, yPosition + 3),
					Font = GameService.Content.DefaultFont16,
					ShowShadow = true,
					TextColor = Color.LightGray,
					AutoSizeWidth = true
				};
				Controls.Add(countLabel);
				if (isCurrentItem)
				{
					Label currentItemLabel = new Label
					{
						Parent = base.BuildPanel,
						Text = "(" + MysticCrafting.Module.Strings.Recipe.CurrentRecipe + ")",
						Location = new Point(countLabel.Right + 5, yPosition + 3),
						Font = GameService.Content.DefaultFont14,
						StrokeText = true,
						TextColor = Color.LightGray,
						AutoSizeWidth = true
					};
					Controls.Add(currentItemLabel);
				}
				yPosition += 30;
			}
		}

		protected override void Build(Container buildPanel)
		{
			base.BuildPanel = buildPanel;
			base.Build(buildPanel);
			UpdateLinkedNodes();
		}
	}
}
