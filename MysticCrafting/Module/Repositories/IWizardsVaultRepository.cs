using System.Collections.Generic;
using System.Threading.Tasks;
using Atzie.MysticCrafting.Models.Items;

namespace MysticCrafting.Module.Repositories
{
	public interface IWizardsVaultRepository
	{
		Task LoadContainersAsync();

		IEnumerable<MysticVaultContainer> GetContainers(int itemId);
	}
}
