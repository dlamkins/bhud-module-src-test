using System;
using System.Collections.Generic;

namespace MysticCrafting.Module.Services.API
{
	public interface IPlayerItemService : IApiService, IDisposable
	{
		int GetItemCount(int itemId);

		int GetMaterialItemCount(int itemId);

		int GetBankItemCount(int itemId);

		int GetSharedInventoryCount(int itemId);

		IDictionary<string, int> GetCharacterInventoryItemCount(int itemId);
	}
}
