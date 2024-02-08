using System.Collections.Generic;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Models;

namespace MysticCrafting.Module.Services
{
	public interface IItemSourceService
	{
		IEnumerable<IItemSource> GetItemSources(MysticItem item);

		IEnumerable<IItemSource> GetItemSources(int itemId);

		string GetPreferredItemSource(string path);

		IItemSource GetPreferredItemSource(string path, int itemId);

		IItemSource GetDefaultItemSource(int itemId);
	}
}
