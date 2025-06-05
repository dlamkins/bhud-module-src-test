using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atzie.MysticCrafting.Models.Items;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class WizardsVaultRepository : IWizardsVaultRepository
	{
		private List<MysticVaultContainer> ContainerItems = new List<MysticVaultContainer>
		{
			new MysticVaultContainer
			{
				ItemId = 103827,
				ContainedChoiceItemIds = new List<int> { 99717, 93366, 96861, 97378 }
			},
			new MysticVaultContainer
			{
				ItemId = 103820,
				ContainedChoiceItemIds = new List<int> { 19673, 19672 }
			},
			new MysticVaultContainer
			{
				ItemId = 99717,
				ContainedItemIds = new List<int> { 29179, 19659, 103820 }
			},
			new MysticVaultContainer
			{
				ItemId = 93366,
				ContainedItemIds = new List<int> { 29182, 19656, 103820 }
			},
			new MysticVaultContainer
			{
				ItemId = 96861,
				ContainedItemIds = new List<int> { 29169, 19647, 103820 }
			},
			new MysticVaultContainer
			{
				ItemId = 97378,
				ContainedItemIds = new List<int> { 29168, 19646, 103820 }
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

		private bool _loaded;

		public async Task LoadContainersAsync()
		{
			if (_loaded)
			{
				await Task.CompletedTask;
				return;
			}
			List<Task> tasks = new List<Task>();
			foreach (MysticVaultContainer container in ContainerItems)
			{
				tasks.Add(Task.Run(delegate
				{
					container.Item = ServiceContainer.ItemRepository.GetItem(container.ItemId);
				}));
			}
			await Task.WhenAll(tasks);
			_loaded = true;
		}

		public IEnumerable<MysticVaultContainer> GetContainers(int itemId)
		{
			if (ContainerItems == null)
			{
				return new List<MysticVaultContainer>();
			}
			return ContainerItems.Where((MysticVaultContainer r) => (r.ContainedItemIds != null && r.ContainedItemIds.Contains(itemId)) || (r.ContainedChanceItemIds != null && r.ContainedChanceItemIds.Contains(itemId)) || (r.ContainedChoiceItemIds != null && r.ContainedChoiceItemIds.Contains(itemId)));
		}
	}
}
