using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using GatheringTools.ToolSearch.Model;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.IdentityModel.Tokens;

namespace GatheringTools.ToolSearch.Services
{
	public class UnknownGatheringToolsService
	{
		public static GatheringTool CreateNoInventoryAccessPlaceholderTool()
		{
			return new GatheringTool
			{
				Id = -1,
				IsUnlimited = true,
				ToolType = ToolType.InventoryCanNotBeAccessedPlaceHolder
			};
		}

		public static GatheringTool CreateUnknownGatheringTool(int id)
		{
			return new GatheringTool
			{
				Id = id,
				IsUnlimited = true,
				ToolType = ToolType.UnknownId
			};
		}

		public static async Task UpdateUnknownEquippedGatheringTools(List<Character> characters, Gw2ApiManager gw2ApiManager, Logger logger)
		{
			List<GatheringTool> unknownGatheringTools = GetUnknownGatheringTools(characters);
			if (!CollectionUtilities.IsNullOrEmpty<GatheringTool>((IEnumerable<GatheringTool>)unknownGatheringTools))
			{
				IReadOnlyList<Item> matchingGatheringToolItems = await GetGatheringToolItemsFromApi(unknownGatheringTools, characters, gw2ApiManager, logger);
				if (matchingGatheringToolItems.Any())
				{
					UpdateUnknownGatheringTools(unknownGatheringTools, matchingGatheringToolItems);
				}
			}
		}

		private static List<GatheringTool> GetUnknownGatheringTools(List<Character> charactersToolsList)
		{
			return (from g in charactersToolsList.SelectMany((Character c) => c.EquippedGatheringTools)
				where g.ToolType == ToolType.UnknownId
				select g).ToList();
		}

		private static async Task<IReadOnlyList<Item>> GetGatheringToolItemsFromApi(List<GatheringTool> unknownGatheringTools, List<Character> characters, Gw2ApiManager gw2ApiManager, Logger logger)
		{
			List<int> unknownGatheringToolIds = unknownGatheringTools.Select((GatheringTool g) => g.Id).Distinct().ToList();
			try
			{
				return await ((IBulkExpandableClient<Item, int>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Items()).ManyAsync((IEnumerable<int>)unknownGatheringToolIds, default(CancellationToken));
			}
			catch (Exception e)
			{
				List<string> characterNamesWithUnknownTools = (from c in characters
					where c.EquippedGatheringTools.Any((GatheringTool g) => unknownGatheringToolIds.Contains(g.Id))
					select c.CharacterName).ToList();
				logger.Error(e, "V2.Items.ManyAsync() for unknown gathering tool ids failed. This can be the case for old historical items. unknown ids: " + string.Join(", ", unknownGatheringToolIds) + ". Characters equipped with unknown tools: " + string.Join(", ", characterNamesWithUnknownTools) + ".");
				return new List<Item>().AsReadOnly();
			}
		}

		private static void UpdateUnknownGatheringTools(List<GatheringTool> unknownGatheringTools, IReadOnlyList<Item> matchingGatheringToolItems)
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			foreach (GatheringTool unknownGatheringTool in unknownGatheringTools)
			{
				Item matchingGatheringToolItem = matchingGatheringToolItems.Single((Item i) => i.get_Id() == unknownGatheringTool.Id);
				unknownGatheringTool.Name = matchingGatheringToolItem.get_Name();
				GatheringTool gatheringTool = unknownGatheringTool;
				RenderUrl icon = matchingGatheringToolItem.get_Icon();
				gatheringTool.IconUrl = ((RenderUrl)(ref icon)).get_Url().ToString();
				unknownGatheringTool.IsUnlimited = matchingGatheringToolItem.get_Rarity() == ApiEnum<ItemRarity>.op_Implicit((ItemRarity)5);
				unknownGatheringTool.ToolType = ToolType.Normal;
			}
		}
	}
}
