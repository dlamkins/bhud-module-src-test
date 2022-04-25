using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace GatheringTools.ToolSearch
{
	public class GatheringToolsService
	{
		public static async Task<List<CharacterAndTools>> GetCharactersAndTools(Gw2ApiManager gw2ApiManager)
		{
			IApiV2ObjectList<Character> obj = await ((IAllExpandableClient<Character>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(default(CancellationToken));
			List<CharacterAndTools> charactersAndTools = new List<CharacterAndTools>();
			foreach (Character characterResponse in (IEnumerable<Character>)obj)
			{
				List<GatheringTool> gatheringTools = GetGatheringToolsWithIdAndType(characterResponse.get_Equipment()).ToList();
				CharacterAndTools characterAndTools = new CharacterAndTools();
				characterAndTools.CharacterName = characterResponse.get_Name();
				characterAndTools.GatheringTools.AddRange(gatheringTools);
				charactersAndTools.Add(characterAndTools);
			}
			await UpdateGatheringToolsNameEtc(charactersAndTools, gw2ApiManager);
			return charactersAndTools;
		}

		private static IEnumerable<GatheringTool> GetGatheringToolsWithIdAndType(IReadOnlyList<CharacterEquipmentItem> equipmentItems)
		{
			foreach (CharacterEquipmentItem equipmentItem in equipmentItems ?? new List<CharacterEquipmentItem>())
			{
				ItemEquipmentSlotType value = equipmentItem.get_Slot().get_Value();
				if (value - 20 <= 2)
				{
					yield return new GatheringTool
					{
						Id = equipmentItem.get_Id(),
						Type = equipmentItem.get_Slot().get_Value()
					};
				}
			}
		}

		private static async Task UpdateGatheringToolsNameEtc(List<CharacterAndTools> characterAndTools, Gw2ApiManager gw2ApiManager)
		{
			IEnumerable<int> gatheringToolIds = (from g in characterAndTools.SelectMany((CharacterAndTools c) => c.GatheringTools)
				select g.Id).Distinct();
			IReadOnlyList<Item> gatheringToolItems = await ((IBulkExpandableClient<Item, int>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Items()).ManyAsync(gatheringToolIds, default(CancellationToken));
			foreach (GatheringTool gatheringTool in characterAndTools.SelectMany((CharacterAndTools c) => c.GatheringTools))
			{
				Item matchingGatheringToolItem = gatheringToolItems.First((Item i) => i.get_Id() == gatheringTool.Id);
				gatheringTool.Name = matchingGatheringToolItem.get_Name();
				GatheringTool gatheringTool2 = gatheringTool;
				RenderUrl icon = matchingGatheringToolItem.get_Icon();
				gatheringTool2.IconUrl = ((RenderUrl)(ref icon)).get_Url().ToString();
				gatheringTool.IsUnlimited = matchingGatheringToolItem.get_Rarity() == ApiEnum<ItemRarity>.op_Implicit((ItemRarity)5);
			}
		}
	}
}
