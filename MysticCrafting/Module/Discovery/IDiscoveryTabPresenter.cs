using System.Collections.Generic;
using Blish_HUD.Graphics.UI;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Menu;

namespace MysticCrafting.Module.Discovery
{
	public interface IDiscoveryTabPresenter : IPresenter
	{
		void SearchAsync(string text);

		void ShowItemList(MysticItemFilter filter, List<string> breadcrumbs);

		void UpdateItemList(CategoryMenuItem menuItem);
	}
}
