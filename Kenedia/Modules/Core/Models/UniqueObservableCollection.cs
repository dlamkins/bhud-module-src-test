using System;
using System.Collections.ObjectModel;

namespace Kenedia.Modules.Core.Models
{
	public class UniqueObservableCollection<T> : ObservableCollection<T> where T : class
	{
		public event EventHandler<T> ItemAdded;

		public event EventHandler<T> ItemRemoved;

		protected override void InsertItem(int index, T item)
		{
			if (!Contains(item))
			{
				base.InsertItem(index, item);
				this.ItemAdded?.Invoke(this, item);
			}
		}

		protected override void RemoveItem(int index)
		{
			T removedItem = base[index];
			base.RemoveItem(index);
			this.ItemRemoved?.Invoke(this, removedItem);
		}
	}
}
