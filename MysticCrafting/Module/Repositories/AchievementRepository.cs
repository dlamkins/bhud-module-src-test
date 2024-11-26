using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Account;

namespace MysticCrafting.Module.Repositories
{
	public class AchievementRepository : IAchievementRepository
	{
		private readonly IItemRepository _itemRepository;

		private List<Achievement> Achievements => new List<Achievement>
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
			}
		};

		public AchievementRepository(IItemRepository itemRepository)
		{
			_itemRepository = itemRepository;
		}

		public Achievement GetAchievement(int itemId)
		{
			return Achievements.AsQueryable().FirstOrDefault((Achievement r) => r.RewardItemId == itemId);
		}
	}
}
