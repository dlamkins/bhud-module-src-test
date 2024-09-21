using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Items;

namespace MysticCrafting.Module.Repositories
{
	public class WizardsVaultRepository : IWizardsVaultRepository
	{
		private List<MysticVaultContainer> ContainerItems = new List<MysticVaultContainer>
		{
			new MysticVaultContainer
			{
				ItemId = 102946,
				ContainedChoiceItemIds = new List<int> { 95641, 95695, 95957, 96764 }
			},
			new MysticVaultContainer
			{
				ItemId = 95957,
				ContainedItemIds = new List<int> { 29185, 19648 },
				ContainedChanceItemIds = new List<int> { 19673, 19672 }
			},
			new MysticVaultContainer
			{
				ItemId = 96764,
				ContainedItemIds = new List<int> { 29171, 19657 },
				ContainedChanceItemIds = new List<int> { 19673, 19672 }
			},
			new MysticVaultContainer
			{
				ItemId = 95641,
				ContainedItemIds = new List<int> { 29170, 19649 },
				ContainedChanceItemIds = new List<int> { 19673, 19672 }
			},
			new MysticVaultContainer
			{
				ItemId = 95695,
				ContainedItemIds = new List<int> { 29167, 19645 },
				ContainedChanceItemIds = new List<int> { 19673, 19672 }
			},
			new MysticVaultContainer
			{
				ItemId = 101195,
				ContainedChoiceItemIds = new List<int> { 100098 }
			},
			new MysticVaultContainer
			{
				ItemId = 100547,
				ContainedChoiceItemIds = new List<int> { 100267 }
			},
			new MysticVaultContainer
			{
				ItemId = 100193,
				ContainedChoiceItemIds = new List<int> { 99964 }
			},
			new MysticVaultContainer
			{
				ItemId = 81346,
				ContainedChoiceItemIds = new List<int> { 80857, 80799, 80685, 80787, 80835, 80746 }
			}
		};

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
