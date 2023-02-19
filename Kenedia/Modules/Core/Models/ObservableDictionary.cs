using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kenedia.Modules.Core.Models
{
	public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
		public new TValue this[TKey key]
		{
			get
			{
				return base[key];
			}
			set
			{
				OnValueChanged(key, ContainsKey(key) ? base[key] : default(TValue), value, "Item");
			}
		}

		public event PropertyChangedEventHandler CollectionChanged;

		private void OnValueChanged(TKey key, TValue v, TValue value, [CallerMemberName] string propName = null)
		{
			if ((!(value?.Equals(v))) ?? true)
			{
				base[key] = value;
				this.CollectionChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
			}
		}
	}
}
