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
