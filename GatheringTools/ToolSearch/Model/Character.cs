using System.Collections.Generic;
using System.Linq;

namespace GatheringTools.ToolSearch.Model
{
	public class Character
	{
		public string CharacterName { get; }

		public List<GatheringTool> EquippedGatheringTools { get; }

		public List<GatheringTool> InventoryGatheringTools { get; }

		public Character(string characterName, List<GatheringTool> inventoryGatheringTools, List<GatheringTool> equippedGatheringTools)
		{
			CharacterName = characterName;
			InventoryGatheringTools = inventoryGatheringTools;
			EquippedGatheringTools = equippedGatheringTools;
		}

		public bool HasTools()
		{
			if (!EquippedGatheringTools.Any())
			{
				return InventoryGatheringTools.Any();
			}
			return true;
		}
	}
}
