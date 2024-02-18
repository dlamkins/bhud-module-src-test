using System.Collections.Generic;
using Blish_HUD.Controls;
using MysticCrafting.Module.Menu;

namespace MysticCrafting.Module.Discovery
{
	public interface IDiscoveryTabMenuModel
	{
		IList<CategoryMenuItem> GetMenuItems(Container parent = null);
	}
}
