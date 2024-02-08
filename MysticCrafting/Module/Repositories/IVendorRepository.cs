using System.Collections.Generic;
using MysticCrafting.Models.Vendor;

namespace MysticCrafting.Module.Repositories
{
	public interface IVendorRepository : IRepository
	{
		IList<VendorSellsItem> GetVendorItems();

		IList<VendorSellsItem> GetVendorItems(int itemId);
	}
}
