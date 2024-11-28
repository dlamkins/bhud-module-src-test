using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace MysticCrafting.Module.RecipeTree
{
	public interface IRecipeDetailsViewPresenter : IPresenter
	{
		void HandleServiceLoading(Container parent);

		void SaveScrollDistance();

		void UpdateScrollDistance();

		void InitializeScrollbar();
	}
}
