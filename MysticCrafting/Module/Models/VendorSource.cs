using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Vendor;
using MysticCrafting.Models.Vendor;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Models
{
	public class VendorSource : ItemSource
	{
		public IList<VendorSellsItem> VendorItems { get; set; }

		public IList<VendorSellsItemGroup> VendorGroups { get; set; }

		public VendorSource(string uniqueId)
			: base(uniqueId)
		{
			Icon = ServiceContainer.TextureRepository.Textures.VendorIcon;
		}
	}
}
