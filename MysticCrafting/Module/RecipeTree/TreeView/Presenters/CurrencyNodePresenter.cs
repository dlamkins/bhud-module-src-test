using Atzie.MysticCrafting.Models.Currencies;
using Blish_HUD.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;

namespace MysticCrafting.Module.RecipeTree.TreeView.Presenters
{
	public class CurrencyNodePresenter
	{
		public CurrencyIngredientNode BuildNode(Container parent, CurrencyQuantity cost)
		{
			CurrencyIngredientNode obj = new CurrencyIngredientNode(cost, parent)
			{
				PanelHeight = 40
			};
			((Control)obj).set_Width(((Control)parent).get_Width() - 25);
			obj.PanelExtensionHeight = 0;
			obj.Build(parent);
			return obj;
		}
	}
}
