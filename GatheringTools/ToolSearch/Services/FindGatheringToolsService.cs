using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using GatheringTools.ToolSearch.Model;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace GatheringTools.ToolSearch.Services
{
	public static class FindGatheringToolsService
	{
		public static async Task<(AccountTools, bool apiAccessFailed)> GetToolsFromApi(List<GatheringTool> allGatheringTools, Gw2ApiManager gw2ApiManager, Logger logger)
		{
			if (!gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)gw2ApiManager.get_Permissions()))
			{
				return (new AccountTools(), true);
			}
			try
			{
				return (await Task.Run(() => GetToolsOnAccount(allGatheringTools, gw2ApiManager)), false);
			}
			catch (Exception e)
			{
				logger.Error(e, "Could not get gathering tools from API");
				return (new AccountTools(), true);
			}
		}

		private static async Task<AccountTools> GetToolsOnAccount(List<GatheringTool> allGatheringTools, Gw2ApiManager gw2ApiManager)
		{
			Task<IApiV2ObjectList<AccountItem>> sharedInventoryTask = ((IBlobClient<IApiV2ObjectList<AccountItem>>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Inventory()).GetAsync(default(CancellationToken));
			Task<IApiV2ObjectList<AccountItem>> bankTask = ((IBlobClient<IApiV2ObjectList<AccountItem>>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
				.get_Bank()).GetAsync(default(CancellationToken));
			Task<IApiV2ObjectList<Character>> charactersTask = ((IAllExpandableClient<Character>)(object)gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(default(CancellationToken));
			await Task.WhenAll(sharedInventoryTask, bankTask, charactersTask);
			List<GatheringTool> bankGatheringTools = FindGatheringTools((IEnumerable<AccountItem>)bankTask.Result, allGatheringTools).ToList();
			List<GatheringTool> sharedInventoryGatheringTools = FindGatheringTools((IEnumerable<AccountItem>)sharedInventoryTask.Result, allGatheringTools).ToList();
			AccountTools accountTools = new AccountTools(bankGatheringTools, sharedInventoryGatheringTools);
			foreach (Character characterResponse in (IEnumerable<Character>)charactersTask.Result)
			{
				List<GatheringTool> inventoryGatheringTools = FindInventoryGatheringTools(characterResponse, allGatheringTools);
				List<GatheringTool> equippedGatheringTools = FindEquippedGatheringTools(allGatheringTools, characterResponse).ToList();
				CharacterTools character = new CharacterTools(characterResponse.get_Name(), inventoryGatheringTools, equippedGatheringTools);
				accountTools.Characters.Add(character);
			}
			await UnknownGatheringToolsService.UpdateUnknownEquippedGatheringTools(accountTools.Characters, gw2ApiManager);
			return accountTools;
		}

		private static List<GatheringTool> FindInventoryGatheringTools(Character characterResponse, List<GatheringTool> allGatheringTools)
		{
			return FindGatheringTools((from b in characterResponse.get_Bags().Where(IsNotEmptyBagSlot)
				select b.get_Inventory()).SelectMany((IReadOnlyList<AccountItem> i) => i), allGatheringTools).ToList();
		}

		private static bool IsNotEmptyBagSlot(CharacterInventoryBag bag)
		{
			return bag != null;
		}

		private static IEnumerable<GatheringTool> FindGatheringTools(IEnumerable<AccountItem> accountItems, List<GatheringTool> allGatheringTools)
		{
			List<int> itemIds = (from i in accountItems.Where(IsNotEmptyItemSlot)
				select i.get_Id()).ToList();
			foreach (int itemId in itemIds)
			{
				GatheringTool matchingGatheringTool = allGatheringTools.FindToolById(itemId);
				if (matchingGatheringTool != null)
				{
					yield return matchingGatheringTool;
				}
			}
		}

		private static bool IsNotEmptyItemSlot(AccountItem itemSlot)
		{
			return itemSlot != null;
		}

		private static IEnumerable<GatheringTool> FindEquippedGatheringTools(List<GatheringTool> allGatheringTools, Character characterResponse)
		{
			List<int> equippedGatheringToolIds = GetEquippedGatheringToolIds(characterResponse.get_Equipment()).ToList();
			foreach (int gatheringToolId in equippedGatheringToolIds)
			{
				yield return allGatheringTools.FindToolById(gatheringToolId) ?? UnknownGatheringToolsService.CreateUnknownGatheringTool(gatheringToolId);
			}
		}

		private static IEnumerable<int> GetEquippedGatheringToolIds(IReadOnlyList<CharacterEquipmentItem> equipmentItems)
		{
			foreach (CharacterEquipmentItem equipmentItem in equipmentItems ?? new List<CharacterEquipmentItem>())
			{
				ItemEquipmentSlotType value = equipmentItem.get_Slot().get_Value();
				if (value - 20 <= 2)
				{
					yield return equipmentItem.get_Id();
				}
			}
		}

		private static GatheringTool FindToolById(this List<GatheringTool> allGatheringTools, int itemId)
		{
			return allGatheringTools.SingleOrDefault((GatheringTool a) => a.Id == itemId);
		}
	}
}
