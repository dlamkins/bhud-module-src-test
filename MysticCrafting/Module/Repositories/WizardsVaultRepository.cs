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
				ItemId = 103839,
				ContainedChoiceItemIds = new List<int> { 97852, 96007, 95957, 96764 }
			},
			new MysticVaultContainer
			{
				ItemId = 95957,
				ContainedItemIds = new List<int> { 29185, 19648 },
				ContainedChoiceItemIds = new List<int> { 19673, 19672 }
			},
			new MysticVaultContainer
			{
				ItemId = 96764,
				ContainedItemIds = new List<int> { 29171, 19657 },
				ContainedChoiceItemIds = new List<int> { 19673, 19672 }
			},
			new MysticVaultContainer
			{
				ItemId = 97852,
				ContainedItemIds = new List<int> { 29184, 19662 },
				ContainedChoiceItemIds = new List<int> { 19673, 19672 }
			},
			new MysticVaultContainer
			{
				ItemId = 96007,
				ContainedItemIds = new List<int> { 29172, 19644 },
				ContainedChoiceItemIds = new List<int> { 19673, 19672 }
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
