using System;
using System.Collections.Generic;
using MysticCrafting.Models.Vendor;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public interface IVendorRepository : IDisposable
	{
		void Initialize(ISqliteDbService service);

		IList<VendorSellsItem> GetVendorItems(int itemId);
	}
}
