using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atzie.MysticCrafting.Models.Account;

namespace MysticCrafting.Module.Repositories
{
	public class AchievementRepository : IAchievementRepository
	{
		private readonly IItemRepository _itemRepository;

		private List<Achievement> _achievements = new List<Achievement>
		{
			new Achievement
			{
				Id = 12345678,
				RewardItemId = 103766,
				UnlockedByItemId = 103775,
				Name = "Klobjarne Geirr: Janthir Spear Kills"
			},
			new Achievement
			{
				Id = 22345678,
				RewardItemId = 103833,
				UnlockedByItemId = 104037,
				Name = "Klobjarne Geirr: Raid or Convergence Victories"
			},
			new Achievement
			{
				Id = 32345678,
				RewardItemId = 103793,
				UnlockedByItemId = 103855,
				Name = "Klobjarne Geirr: World Boss or Meta-Event Completions"
			},
			new Achievement
			{
				Id = 42345678,
				RewardItemId = 103578,
				Name = "Lowland Shore Mastery"
			},
			new Achievement
			{
				Id = 52345678,
				RewardItemId = 102814,
				Name = "Janthir Syntri Mastery"
			},
			new Achievement
			{
				Id = 62345678,
				RewardItemId = 102367,
				Name = "Mistburned Barrens Mastery"
			},
			new Achievement
			{
				Id = 72345678,
				RewardItemId = 104704,
				Name = "Askur Camping Cookout Backpiece"
			},
			new Achievement
			{
				Id = 82345678,
				RewardItemId = 104777,
				Name = "Unknown Nightmares: Experiments in the Shadows"
			},
			new Achievement
			{
				Id = 92345678,
				RewardItemId = 109363,
				Name = "Mists Research: Strife Unending"
			},
			new Achievement
			{
				Id = 102345678,
				RewardItemId = 109361,
				Name = "Castora: Forge Guard's Armor Collection"
			},
			new Achievement
			{
				Id = 112345678,
				RewardItemId = 109141,
				Name = "Eternity's Garden: Eternity's Garden Mastery"
			}
		};

		private bool _loaded;

		private Dictionary<int, Achievement> _achievementDictionary;

		public AchievementRepository(IItemRepository itemRepository)
		{
			_itemRepository = itemRepository;
			_achievementDictionary = _achievements.ToDictionary((Achievement a) => a.RewardItemId);
		}

		public async Task LoadAchievementsAsync()
		{
			if (_loaded)
			{
				await Task.CompletedTask;
				return;
			}
			List<Task> tasks = new List<Task>();
			foreach (Achievement achievement in _achievements)
			{
				tasks.Add(Task.Run(delegate
				{
					achievement.RewardItem = _itemRepository.GetItem(achievement.RewardItemId);
					achievement.UnlockedByItem = _itemRepository.GetItem(achievement.UnlockedByItemId);
				}));
			}
			await Task.WhenAll(tasks);
			_loaded = true;
		}

		public Achievement GetAchievement(int itemId)
		{
			_achievementDictionary.TryGetValue(itemId, out var achievement);
			return achievement;
		}
	}
}
