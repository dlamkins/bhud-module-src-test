using System;
using System.Collections.Generic;
using MysticCrafting.Models.Vendor;
using MysticCrafting.Module.Services;
using SQLite;

namespace MysticCrafting.Module.Repositories
{
	public class VendorRepository : IVendorRepository, IDisposable
	{
		private SQLiteConnection Connection { get; set; }

		public void Initialize(ISqliteDbService service)
		{
			Connection = new SQLiteConnection(service.DatabaseFilePath);
		}

		public IList<VendorSellsItem> GetVendorItems(int itemId)
		{
			return Connection.Query<VendorSellsItem>(string.Format("SELECT * FROM {0} WHERE {1} = {2} AND {3} = 0", "VendorSellsItem", "ItemId", itemId, "IsHistorical"), Array.Empty<object>());
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Connection?.Dispose();
			}
		}
	}
}
