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
		private long _unitPrice;

		private CoinsControl _priceControl;

		private Label _coinLabel;

		public long UnitPrice
		{
			get
			{
				return _unitPrice;
			}
			set
			{
				if (_unitPrice != value)
				{
					_unitPrice = value;
					if (_priceControl != null)
					{
						_priceControl.UnitPrice = value;
					}
					if (_coinLabel != null)
					{
						((Control)_coinLabel).set_Visible(value == 0);
					}
				}
			}
		}

		public string DisplayName { get; }

		public string Name { get; set; }

		public override string PathName => Name ?? DisplayName;

		public TradingPostOptions Option { get; set; }

		public TradingPostNode(long unitPrice, string displayName, TradingPostOptions option, Container parent)
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
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Expected O, but got Unknown
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
			_priceControl = coinsControl;
			string coinLabelText = Recipe.Unavailable;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(coinLabelText);
			((Control)val2).set_Location(new Point(((Control)_priceControl).get_Right(), 10));
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_StrokeText(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Visible(UnitPrice == 0);
			_coinLabel = val2;
			base.Build();
		}
	}
}
