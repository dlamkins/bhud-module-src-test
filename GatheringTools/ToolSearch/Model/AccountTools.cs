using System.Collections.Generic;
using System.Linq;

namespace GatheringTools.ToolSearch.Model
{
	public class AccountTools
	{
		public List<GatheringTool> SharedInventoryGatheringTools { get; } = new List<GatheringTool>();


		public List<GatheringTool> BankGatheringTools { get; } = new List<GatheringTool>();


		public List<CharacterTools> Characters { get; } = new List<CharacterTools>();


		public AccountTools()
		{
		}

		public AccountTools(List<GatheringTool> bankGatheringTools, List<GatheringTool> sharedInventoryGatheringTools)
		{
			BankGatheringTools = bankGatheringTools;
			SharedInventoryGatheringTools = sharedInventoryGatheringTools;
		}

		public bool HasTools()
		{
			if (!SharedInventoryGatheringTools.Any() && !BankGatheringTools.Any())
			{
				return Characters.Any((CharacterTools c) => c.HasTools());
			}
			return true;
		}
	}
}
