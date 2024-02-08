using System.Collections.Generic;
using MysticCrafting.Models.Vendor;

namespace MysticCrafting.Module.Repositories.Logging
{
	public class LoggingVendorRepository : LoggingRepository, IVendorRepository, IRepository
	{
		private readonly IVendorRepository _vendorRepository;

		public LoggingVendorRepository(IVendorRepository vendorRepository)
			: base(vendorRepository)
		{
			_vendorRepository = vendorRepository;
		}

		public IList<VendorSellsItem> GetVendorItems()
		{
			return _vendorRepository.GetVendorItems();
		}

		public IList<VendorSellsItem> GetVendorItems(int itemId)
		{
			return _vendorRepository.GetVendorItems(itemId);
		}
	}
}
