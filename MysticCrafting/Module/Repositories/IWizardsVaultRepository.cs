using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Items;

namespace MysticCrafting.Module.Repositories
{
	public interface IWizardsVaultRepository
	{
		IEnumerable<MysticVaultContainer> GetContainers(int itemId);
	}
}
