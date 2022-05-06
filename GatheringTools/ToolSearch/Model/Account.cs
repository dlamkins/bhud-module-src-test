using System.Collections.Generic;
using System.Linq;

namespace GatheringTools.ToolSearch.Model
{
	public class Account
	{
		public List<GatheringTool> SharedInventoryGatheringTools { get; } = new List<GatheringTool>();


		public List<GatheringTool> BankGatheringTools { get; } = new List<GatheringTool>();


		public List<Character> Characters { get; } = new List<Character>();


		public Account()
		{
		}

		public Account(List<GatheringTool> bankGatheringTools, List<GatheringTool> sharedInventoryGatheringTools)
		{
			BankGatheringTools = bankGatheringTools;
			SharedInventoryGatheringTools = sharedInventoryGatheringTools;
		}

		public bool HasTools()
		{
			if (!SharedInventoryGatheringTools.Any() && !BankGatheringTools.Any())
			{
				return Characters.Any((Character c) => c.HasTools());
			}
			return true;
		}
	}
}
