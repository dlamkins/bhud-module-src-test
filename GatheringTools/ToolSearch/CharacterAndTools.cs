using System.Collections.Generic;
using System.Linq;

namespace GatheringTools.ToolSearch
{
	public class CharacterAndTools
	{
		public string CharacterName { get; set; } = string.Empty;


		public List<GatheringTool> GatheringTools { get; set; } = new List<GatheringTool>();


		public List<GatheringTool> UnlimitedGatheringTools => GatheringTools.Where((GatheringTool g) => g.IsUnlimited).ToList();

		public bool HasTools()
		{
			return GatheringTools.Any();
		}

		public bool HasUnlimitedTools()
		{
			return GatheringTools.Any((GatheringTool g) => g.IsUnlimited);
		}
	}
}
