using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Currencies;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Tooltips
{
	public class AdvancedCurrencyTooltipView : View, ICountTooltipView, ITooltipView, IView
	{
		private Panel _linkedNodesPanel;

		private Label _rightClickOptionsLabel;

		private Label _itemCountLabel;

		private Label _moreRequiredLabel;

		private int _requiredQuantity;

		private int _playerItemCount;

		protected bool Initialized;

		private CurrencyQuantity Quantity { get; set; }

		private CurrencyIngredientNode Node { get; set; }

		public Container BuildPanel { get; set; }

		public virtual int RequiredQuantity
		{
			get
			{
				return _requiredQuantity;
			}
			set
			{
				_requiredQuantity = value;
				SetRequiredQuantityLabel();
			}
		}

		protected int Bottom { get; set; }

		public int PlayerItemCount
		{
			get
			{
				return _playerItemCount;
			}
			set
			{
				_playerItemCount = value;
				if (_itemCountLabel != null)
				{
					_itemCountLabel.set_Text(Node.GetUnitCountText());
				}
				SetRequiredQuantityLabel();
			}
		}

		private IList<Control> _controls { get; set; } = new List<Control>();


		public Point IconSize { get; set; } = new Point(30, 30);


		public AdvancedCurrencyTooltipView(CurrencyIngredientNode node)
			: this()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Expected O, but got Unknown
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Expected O, but got Unknown
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Expected O, but got Unknown
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Expected O, but got Unknown
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Expected O, but got Unknown
			Node = node;
			Quantity = node.CurrencyQuantity;
			if (Quantity.Currency?.Icon != null)
			{
				AsyncTexture2D texture = ServiceContainer.TextureRepository.GetTexture(Quantity.Currency.Icon);
				if (texture != null)
				{
					IList<Control> controls = _controls;
					Image val = new Image(texture);
					((Control)val).set_Size(IconSize);
					((Control)val).set_Location(new Point(0, 0));
					controls.Add((Control)val);
				}
			}
			IList<Control> controls2 = _controls;
			Label val2 = new Label();
			val2.set_Text(Quantity.Currency?.LocalizedName());
			val2.set_Font(GameService.Content.get_DefaultFont18());
			((Control)val2).set_Location(new Point(40, 5));
			val2.set_TextColor(ColorHelper.CurrencyName);
			val2.set_StrokeText(true);
			val2.set_AutoSizeWidth(true);
			controls2.Add((Control)val2);
			Label val3 = new Label();
			val3.set_Text(Quantity.Currency?.LocalizedDescription());
			val3.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val3).set_Location(new Point(0, 35));
			val3.set_TextColor(Color.get_White());
			((Control)val3).set_Width(400);
			val3.set_AutoSizeHeight(true);
			val3.set_WrapText(true);
			val3.set_StrokeText(true);
			Label descriptionLabel = val3;
			_controls.Add((Control)(object)descriptionLabel);
			if (Quantity.Currency.Id != 1)
			{
				int yPosition = 40 + ((Control)descriptionLabel).get_Height();
				PlayerItemCount = node.GetPlayerUnitCount();
				Label val4 = new Label();
				val4.set_Text(node.GetUnitCountText());
				val4.set_Font(GameService.Content.get_DefaultFont18());
				((Control)val4).set_Location(new Point(0, yPosition));
				val4.set_TextColor(Color.get_White());
				val4.set_StrokeText(true);
				val4.set_AutoSizeWidth(true);
				_itemCountLabel = val4;
				yPosition += ((Control)_itemCountLabel).get_Height() + 5;
				_controls.Add((Control)(object)_itemCountLabel);
				if (RequiredQuantity > 0)
				{
					Label val5 = new Label();
					val5.set_Text(string.Format(Recipe.MoreRequired, RequiredQuantity.ToString("N0")));
					val5.set_Font(GameService.Content.get_DefaultFont16());
					((Control)val5).set_Location(new Point(0, yPosition));
					val5.set_TextColor(Color.get_LightGray());
					val5.set_ShowShadow(true);
					val5.set_AutoSizeWidth(true);
					_moreRequiredLabel = val5;
					_controls.Add((Control)(object)_moreRequiredLabel);
					yPosition += 25;
				}
				yPosition += 5;
				IList<Control> controls3 = _controls;
				Label val6 = new Label();
				val6.set_Text(Recipe.WalletLabel);
				val6.set_Font(GameService.Content.get_DefaultFont16());
				((Control)val6).set_Location(new Point(0, yPosition));
				val6.set_TextColor(Color.get_LightGray());
				val6.set_ShowShadow(true);
				val6.set_AutoSizeWidth(true);
				controls3.Add((Control)val6);
				yPosition = (Bottom = yPosition + 25);
				Initialized = true;
			}
		}

		public void SetRequiredQuantityLabel()
		{
			if (_moreRequiredLabel != null)
			{
				_moreRequiredLabel.set_Text(string.Format(Recipe.MoreRequired, RequiredQuantity.ToString("N0")));
			}
			if (_itemCountLabel != null)
			{
				_itemCountLabel.set_Text(Node.GetUnitCountText());
			}
		}

		public void UpdateLinkedNodes()
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Expected O, but got Unknown
			if (Initialized)
			{
				PlayerItemCount = Node.GetPlayerUnitCount();
				int yPosition = Bottom + 15;
				List<IngredientNode> linkedNodes = Node.TreeView.IngredientNodes.ToList().GetByItemId(Node.Id).ToList();
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
				((Control)val).set_Parent(BuildPanel);
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
			((Control)val).set_Parent(BuildPanel);
			((Control)val).set_Location(new Point(0, yPosition));
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			_linkedNodesPanel = val;
			long totalPlayerItemCount = ServiceContainer.WalletService.GetQuantity(Node.Id).Count;
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
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			if (node != null && node.TotalUnitCount != 0)
			{
				Image icon = node.Icon;
				if (((icon != null) ? icon.get_Texture() : null) != null)
				{
					Image val = new Image(node.Icon.get_Texture());
					((Control)val).set_Parent((Container)(object)_linkedNodesPanel);
					((Control)val).set_Size(new Point(25, 25));
					((Control)val).set_Location(new Point(xPosition, yPosition));
				}
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

		protected override void Build(Container buildPanel)
		{
			BuildPanel = buildPanel;
			foreach (Control control in _controls.ToList())
			{
				if (control != null)
				{
					control.set_Parent(buildPanel);
				}
			}
			UpdateLinkedNodes();
		}

		protected override void Unload()
		{
			foreach (Control control in _controls)
			{
				control.Dispose();
			}
			_controls?.Clear();
			Quantity = null;
			BuildPanel = null;
			((View<IPresenter>)this).Unload();
		}
	}
}
