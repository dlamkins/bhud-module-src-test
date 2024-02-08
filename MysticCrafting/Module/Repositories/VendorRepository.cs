using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MysticCrafting.Models.Vendor;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class VendorRepository : IVendorRepository, IRepository
	{
		private readonly IDataService _dataService;

		private IList<VendorSellsItem> VendorItems { get; set; }

		public bool LocalOnly => false;

		public bool Loaded { get; private set; }

		public string FileName => "vendor_data.json";

		public VendorRepository(IDataService dataService)
		{
			_dataService = dataService;
		}

		public async Task<string> LoadAsync()
		{
			VendorItems = (await _dataService.LoadFromFileAsync<List<VendorSellsItem>>(FileName)) ?? new List<VendorSellsItem>();
			Loaded = true;
			return $"{VendorItems.Count} vendor items loaded";
		}

		public IList<VendorSellsItem> GetVendorItems()
		{
			return VendorItems;
		}

		public IList<VendorSellsItem> GetVendorItems(int itemId)
		{
			return VendorItems.Where((VendorSellsItem v) => v.ItemId.HasValue && v.ItemId == itemId).ToList();
		}
	}
}
