using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Recipe.TreeView.Controls;
using MysticCrafting.Module.Settings;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Recipe.TreeView.Nodes
{
	public class TradingPostNode : TreeNodeSelect
	{
		public int UnitPrice { get; }

		public string DisplayName { get; }

		public string Name { get; set; }

		public override string PathName => Name ?? DisplayName;

		public TradingPostOptions Option { get; set; }

		public TradingPostNode(int unitPrice, string displayName, TradingPostOptions option, Container parent)
		{
			UnitPrice = unitPrice;
			DisplayName = displayName;
			Option = option;
			base.Parent = parent;
			Build();
		}

		public override void Build()
		{
			ClearChildren();
			new Label
			{
				Parent = this,
				Text = DisplayName,
				Location = new Point(30, 10),
				Font = GameService.Content.DefaultFont16,
				StrokeText = true,
				AutoSizeWidth = true
			};
			CoinsControl price = new CoinsControl(this)
			{
				Location = new Point(350, 10),
				UnitPrice = UnitPrice
			};
			string coinLabelText = MysticCrafting.Module.Strings.Recipe.PriceEach;
			if (UnitPrice == 0)
			{
				coinLabelText = MysticCrafting.Module.Strings.Recipe.Unavailable;
			}
			new Label
			{
				Parent = this,
				Text = coinLabelText,
				Location = new Point(price.Right, 10),
				Font = GameService.Content.DefaultFont16,
				StrokeText = true,
				AutoSizeWidth = true
			};
			base.Build();
		}
	}
}
