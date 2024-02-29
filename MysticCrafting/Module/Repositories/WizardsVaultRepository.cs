using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Items;
using JsonFlatFileDataStore;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class WizardsVaultRepository : IWizardsVaultRepository
	{
		private IDocumentCollection<MysticVaultContainer> ContainerItems { get; } = ServiceContainer.Store.GetCollection<MysticVaultContainer>();


		public IEnumerable<MysticVaultContainer> GetContainers(int itemId)
		{
			if (ContainerItems == null)
			{
				return new List<MysticVaultContainer>();
			}
			return from r in ContainerItems.AsQueryable()
				where (r.ContainedItemIds != null && r.ContainedItemIds.Contains(itemId)) || (r.ContainedChanceItemIds != null && r.ContainedChanceItemIds.Contains(itemId)) || (r.ContainedChoiceItemIds != null && r.ContainedChoiceItemIds.Contains(itemId))
				select r;
		}
	}
}
