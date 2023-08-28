using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Kenedia.Modules.Core.Models
{
	public class DeepObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IDisposable where TValue : INotifyPropertyChanged
	{
		private bool _isDisposed;

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

		public event EventHandler<DictionaryItemChangedEventArgs<TKey, TValue>> ValueChanged;

		public event EventHandler<PropertyChangedEventArgs> ItemChanged;

		public event EventHandler<PropertyChangedEventArgs> CollectionChanged;

		private void OnItemProperty_Changed(object sender, PropertyChangedEventArgs e)
		{
			this.ItemChanged?.Invoke(sender, e);
		}

		private void OnValueChanged(TKey key, TValue v, TValue value, [CallerMemberName] string propName = null)
		{
			if ((!(value?.Equals(v))) ?? true)
			{
				if (v != null)
				{
					v.PropertyChanged -= ItemProperty_Changed;
				}
				if (value != null)
				{
					value.PropertyChanged += ItemProperty_Changed;
				}
				base[key] = value;
				this.ValueChanged?.Invoke(this, new DictionaryItemChangedEventArgs<TKey, TValue>(key, v, value));
				this.CollectionChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
			}
		}

		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			using Enumerator enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				TValue value = enumerator.Current.Value;
				value.PropertyChanged -= ItemProperty_Changed;
			}
		}

		public new void Add(TKey key, TValue value)
		{
			if (!ContainsKey(key))
			{
				value.PropertyChanged += ItemProperty_Changed;
				base.Add(key, value);
				this.ValueChanged?.Invoke(this, new DictionaryItemChangedEventArgs<TKey, TValue>(key, default(TValue), value));
				this.CollectionChanged?.Invoke(this, new PropertyChangedEventArgs("Items"));
			}
		}

		public new bool Remove(TKey key)
		{
			if (ContainsKey(key))
			{
				TValue val = this[key];
				val.PropertyChanged -= ItemProperty_Changed;
			}
			return base.Remove(key);
		}

		public virtual void Wipe()
		{
			foreach (TKey key in base.Keys.ToList())
			{
				this[key] = default(TValue);
			}
		}

		protected void ItemProperty_Changed(object sender, PropertyChangedEventArgs e)
		{
			OnItemProperty_Changed(sender, e);
		}
	}
}