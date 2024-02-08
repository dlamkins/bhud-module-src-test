using System.Collections.Generic;
using MysticCrafting.Module.Services.Recurring;

namespace MysticCrafting.Module.Services.API
{
	public interface IPlayerItemService : IRecurringService
	{
		int GetItemCount(int itemId);

		int GetMaterialItemCount(int itemId);

		int GetBankItemCount(int itemId);

		int GetSharedInventoryCount(int itemId);

		IDictionary<string, int> GetCharacterInventoryItemCount(int itemId);
	}
}
