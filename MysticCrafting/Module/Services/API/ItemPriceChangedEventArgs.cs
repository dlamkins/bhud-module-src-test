using System;
using Atzie.MysticCrafting.Models.Items;

namespace MysticCrafting.Module.Services.API
{
	public class ItemPriceChangedEventArgs : EventArgs
	{
		public Item Item;
	}
}
