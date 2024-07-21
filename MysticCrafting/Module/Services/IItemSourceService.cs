using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Items;
using MysticCrafting.Module.Models;

namespace MysticCrafting.Module.Services
{
	public interface IItemSourceService
	{
		IEnumerable<IItemSource> GetItemSources(Item item);

		string GetPreferredItemSource(string path);
	}
}
