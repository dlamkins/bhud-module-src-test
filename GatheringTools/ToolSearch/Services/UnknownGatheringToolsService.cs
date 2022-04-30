using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using GatheringTools.ToolSearch.Model;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace GatheringTools.ToolSearch.Services
{
	public class UnknownGatheringToolsService
	{
		private const string UNKNOWN_GATHERING_TOOL_NAME = "???";

		public static GatheringTool CreateUnknownGatheringTool(int gatheringToolId)
		{
			return new GatheringTool
			{
				Id = gatheringToolId,
				Name = "???"
			};
		}

		public static async Task UpdateUnknownEquippedGatheringTools(List<CharacterTools> characters, Gw2ApiManager gw2ApiManager)
		{
			List<int> unknownGatheringToolIds = (from g in characters.SelectMany((CharacterTools c) => c.EquippedGatheringTools)
				where g.Name == "???"
				select g.Id).Distinct().ToList();
			if (!unknownGatheringToolIds.Any())
			{
				return;
			}
			IReadOnlyList<Item> gatheringToolItems = await ((IBulkExpandableClient<Item, int>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Items()).ManyAsync((IEnumerable<int>)unknownGatheringToolIds, default(CancellationToken));
			foreach (GatheringTool unknownGatheringTool in from g in characters.SelectMany((CharacterTools c) => c.EquippedGatheringTools)
				where g.Name == "???"
				select g)
			{
				Item matchingGatheringToolItem = gatheringToolItems.Single((Item i) => i.get_Id() == unknownGatheringTool.Id);
				unknownGatheringTool.Name = matchingGatheringToolItem.get_Name();
				GatheringTool gatheringTool = unknownGatheringTool;
				RenderUrl icon = matchingGatheringToolItem.get_Icon();
				gatheringTool.IconUrl = ((RenderUrl)(ref icon)).get_Url().ToString();
				unknownGatheringTool.IsUnlimited = matchingGatheringToolItem.get_Rarity() == ApiEnum<ItemRarity>.op_Implicit((ItemRarity)5);
			}
		}
	}
}
