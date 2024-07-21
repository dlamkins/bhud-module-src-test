using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.Settings;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
{
	public class TradingPostNode : TreeNodeSelect
	{
		public int UnitPrice { get; }

		public string DisplayName { get; }

		public string Name { get; set; }

		public override string PathName => Name ?? DisplayName;

		public TradingPostOptions Option { get; set; }

		public TradingPostNode(int unitPrice, string displayName, TradingPostOptions option, Container parent)
			: base(parent)
		{
			UnitPrice = unitPrice;
			DisplayName = displayName;
			Option = option;
			((Control)this).set_Parent(parent);
			Build();
		}

		public override void Build()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).ClearChildren();
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(DisplayName);
			((Control)val).set_Location(new Point(40, 10));
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_StrokeText(true);
			val.set_AutoSizeWidth(true);
			CoinsControl coinsControl = new CoinsControl((Container)(object)this);
			((Control)coinsControl).set_Location(new Point(380, 10));
			coinsControl.UnitPrice = UnitPrice;
			CoinsControl price = coinsControl;
			string coinLabelText = Recipe.PriceEach;
			if (UnitPrice == 0)
			{
				coinLabelText = Recipe.Unavailable;
			}
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(coinLabelText);
			((Control)val2).set_Location(new Point(((Control)price).get_Right(), 10));
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_StrokeText(true);
			val2.set_AutoSizeWidth(true);
			base.Build();
		}
	}
}
