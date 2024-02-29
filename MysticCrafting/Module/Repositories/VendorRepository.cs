using System.Collections.Generic;
using System.Linq;
using JsonFlatFileDataStore;
using MysticCrafting.Models.Vendor;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class VendorRepository : IVendorRepository
	{
		private IDocumentCollection<VendorSellsItem> VendorItems { get; } = ServiceContainer.Store.GetCollection<VendorSellsItem>();


		public IList<VendorSellsItem> GetVendorItems()
		{
			return (from v in VendorItems.AsQueryable()
				where !v.IsHistorical
				select v).ToList();
		}

		public IList<VendorSellsItem> GetVendorItems(int itemId)
		{
			return (from v in VendorItems.AsQueryable().ToList()
				where v.ItemId.HasValue && v.ItemId == itemId && !v.IsHistorical
				select v).ToList();
		}
	}
}
