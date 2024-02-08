using System.Collections.Generic;
using Blish_HUD.Graphics.UI;
using MysticCrafting.Models.Items;

namespace MysticCrafting.Module.Items
{
	public interface IItemListPresenter : IPresenter
	{
		void UpdateFilter(MysticItemFilter filter);

		void UpdateMenuBreadcrumbs(List<string> breadcrumbs);

		void Reload();
	}
}
