using System.Collections.Generic;

namespace MysticCrafting.Module.Services.API
{
	public interface IPlayerItemService : IApiService
	{
		int GetItemCount(int itemId);

		int GetMaterialItemCount(int itemId);

		int GetBankItemCount(int itemId);

		int GetSharedInventoryCount(int itemId);

		IDictionary<string, int> GetCharacterInventoryItemCount(int itemId);
	}
}
