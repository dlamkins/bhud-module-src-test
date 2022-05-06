using System.Collections.Generic;
using System.Linq;
using GatheringTools.ToolSearch.Model;

namespace GatheringTools.ToolSearch.Services
{
	public class FilterGatheringToolsService
	{
		public static void FilterTools(Account account, bool showOnlyUnlimitedTools, bool showBank, bool showSharedInventory)
		{
			if (showOnlyUnlimitedTools)
			{
				FilterUnlimitedTools(account);
			}
			if (!showBank)
			{
				account.BankGatheringTools.Clear();
			}
			if (!showSharedInventory)
			{
				account.SharedInventoryGatheringTools.Clear();
			}
		}

		private static void FilterUnlimitedTools(Account account)
		{
			FilterTools(account.BankGatheringTools);
			FilterTools(account.SharedInventoryGatheringTools);
			foreach (Character character in account.Characters)
			{
				FilterTools(character.EquippedGatheringTools);
				FilterTools(character.InventoryGatheringTools);
			}
		}

		private static void FilterTools(List<GatheringTool> gatheringTools)
		{
			List<GatheringTool> filteredTools = gatheringTools.Where((GatheringTool g) => g.IsUnlimited).ToList();
			gatheringTools.Clear();
			gatheringTools.AddRange(filteredTools);
		}
	}
}
