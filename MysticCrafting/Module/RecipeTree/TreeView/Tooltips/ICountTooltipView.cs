using Blish_HUD.Common.UI.Views;
using Blish_HUD.Graphics.UI;

namespace MysticCrafting.Module.RecipeTree.TreeView.Tooltips
{
	public interface ICountTooltipView : ITooltipView, IView
	{
		int RequiredQuantity { get; set; }

		void UpdateLinkedNodes();
	}
}
