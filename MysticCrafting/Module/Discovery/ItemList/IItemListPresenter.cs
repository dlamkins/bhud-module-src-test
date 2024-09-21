using System.Collections.Generic;
using Blish_HUD.Graphics.UI;

namespace MysticCrafting.Module.Discovery.ItemList
{
	public interface IItemListPresenter : IPresenter
	{
		void Reload();

		IList<string> BuildBreadcrumbs();
	}
}
