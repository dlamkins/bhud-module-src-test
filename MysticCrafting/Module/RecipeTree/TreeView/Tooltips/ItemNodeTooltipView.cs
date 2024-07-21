using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.RecipeTree.TreeView.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Tooltips
{
	public class ItemNodeTooltipView : ItemTooltipView, ICountTooltipView, ITooltipView, IView
	{
		private Panel _linkedNodesPanel;

		private Label _rightClickOptionsLabel;

		private ItemIngredientNode Node { get; set; }

		public override string PlayerCountText => Node?.GetUnitCountText() ?? "";

		public ItemNodeTooltipView(ItemIngredientNode node)
			: base(node.Item, node.OrderUnitCount)
		{
			Node = node;
			base.PlayerItemCount = node.GetPlayerUnitCount();
		}

		public void UpdateLinkedNodes()
		{
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Expected O, but got Unknown
			if (Initialized)
			{
				base.PlayerItemCount = Node.GetPlayerUnitCount();
				int yPosition = base.Bottom + 15;
				List<IngredientNode> linkedNodes = Node.TreeView.IngredientNodes.ToList().GetByItemId(Node.Item.Id).ToList();
				if (linkedNodes.Count > 0)
				{
					UpdateLinkedNodes(linkedNodes, ref yPosition);
				}
				Label rightClickOptionsLabel = _rightClickOptionsLabel;
				if (rightClickOptionsLabel != null)
				{
					((Control)rightClickOptionsLabel).Dispose();
				}
				Label val = new Label();
				((Control)val).set_Parent(base.BuildPanel);
				val.set_Text(Recipe.TooltipOpenMenuText);
				((Control)val).set_Location(new Point(0, yPosition));
				val.set_Font(GameService.Content.get_DefaultFont14());
				val.set_TextColor(Color.get_LightGray());
				val.set_StrokeText(true);
				val.set_AutoSizeWidth(true);
				_rightClickOptionsLabel = val;
			}
		}

		public void UpdateLinkedNodes(IList<IngredientNode> linkedNodes, ref int yPosition)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Expected O, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			if (Node == null)
			{
				return;
			}
			Panel linkedNodesPanel = _linkedNodesPanel;
			if (linkedNodesPanel != null)
			{
				((Control)linkedNodesPanel).Dispose();
			}
			if (linkedNodes.Count <= 1)
			{
				return;
			}
			Panel val = new Panel();
			((Control)val).set_Parent(base.BuildPanel);
			((Control)val).set_Location(new Point(0, yPosition));
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			_linkedNodesPanel = val;
			int totalPlayerItemCount = ServiceContainer.PlayerItemService.GetItemCount(Node.Item.Id);
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_linkedNodesPanel);
			val2.set_Text(Recipe.AllRecipes);
			((Control)val2).set_Location(new Point(0, 0));
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_ShowShadow(true);
			val2.set_TextColor(Color.get_White());
			val2.set_AutoSizeWidth(true);
			Label titleLabel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)_linkedNodesPanel);
			val3.set_Text($"({totalPlayerItemCount}/{linkedNodes.Where((IngredientNode n) => n.Active).Sum((IngredientNode n) => n.TotalUnitCount)})");
			((Control)val3).set_Location(new Point(((Control)titleLabel).get_Right() + 5, 0));
			val3.set_Font(GameService.Content.get_DefaultFont16());
			val3.set_ShowShadow(true);
			val3.set_TextColor(Color.get_LightGray());
			val3.set_AutoSizeWidth(true);
			yPosition += 25;
			int xPosition = 0;
			int childPosition = 25;
			foreach (IngredientNode node in linkedNodes.Where((IngredientNode n) => n.Active))
			{
				if (node != null && node.TotalUnitCount != 0)
				{
					bool currentItem = node == Node;
					UpdateLinkedNode(node, ref xPosition, ref childPosition, currentItem);
				}
			}
			yPosition += childPosition - 15;
		}

		public void UpdateLinkedNode(IngredientNode node, ref int xPosition, ref int yPosition, bool isCurrentItem = false)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			if (node != null && node.TotalUnitCount != 0)
			{
				Image val = new Image(node.Icon.get_Texture());
				((Control)val).set_Parent((Container)(object)_linkedNodesPanel);
				((Control)val).set_Size(new Point(25, 25));
				((Control)val).set_Location(new Point(xPosition, yPosition));
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)_linkedNodesPanel);
				val2.set_Text(node.GetUnitCountText());
				((Control)val2).set_Location(new Point(xPosition + 30, yPosition + 3));
				val2.set_Font(GameService.Content.get_DefaultFont16());
				val2.set_ShowShadow(true);
				val2.set_TextColor(Color.get_LightGray());
				val2.set_AutoSizeWidth(true);
				Label countLabel = val2;
				if (isCurrentItem)
				{
					Label val3 = new Label();
					((Control)val3).set_Parent((Container)(object)_linkedNodesPanel);
					val3.set_Text("(" + Recipe.CurrentRecipe + ")");
					((Control)val3).set_Location(new Point(((Control)countLabel).get_Right() + 5, yPosition + 3));
					val3.set_Font(GameService.Content.get_DefaultFont14());
					val3.set_StrokeText(true);
					val3.set_TextColor(Color.get_LightGray());
					val3.set_AutoSizeWidth(true);
				}
				yPosition += 30;
			}
		}

		protected override void Unload()
		{
			Panel linkedNodesPanel = _linkedNodesPanel;
			if (linkedNodesPanel != null)
			{
				((Control)linkedNodesPanel).Dispose();
			}
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
