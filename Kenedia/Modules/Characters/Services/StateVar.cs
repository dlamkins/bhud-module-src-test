using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Kenedia.Modules.Characters.Services
{
	public sealed class StateVar<T>
	{
		private T _value;

		private INotifyPropertyChanged _observedObject;

		private INotifyCollectionChanged _observedCollection;

		private readonly HashSet<INotifyPropertyChanged> _observedCollectionItems = new HashSet<INotifyPropertyChanged>();

		public T Value
		{
			get
			{
				return _value;
			}
			set
			{
				Set(value);
			}
		}

		public event EventHandler<StateVarChangedEventArgs<T>> Changed;

		public bool Set(T value, bool forceNotify = false)
		{
			if (!forceNotify && EqualityComparer<T>.Default.Equals(_value, value))
			{
				return false;
			}
			T oldValue = _value;
			UnobserveCurrentValue();
			_value = value;
			ObserveCurrentValue();
			this.Changed?.Invoke(this, new StateVarChangedEventArgs<T>(oldValue, _value));
			return true;
		}

		private void ObserveCurrentValue()
		{
			INotifyPropertyChanged notifyPropertyChanged = _value as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				_observedObject = notifyPropertyChanged;
				_observedObject.PropertyChanged += ObservedObject_PropertyChanged;
			}
			INotifyCollectionChanged notifyCollectionChanged = _value as INotifyCollectionChanged;
			if (notifyCollectionChanged == null)
			{
				return;
			}
			_observedCollection = notifyCollectionChanged;
			_observedCollection.CollectionChanged += ObservedCollection_CollectionChanged;
			IEnumerable enumerable = _value as IEnumerable;
			if (enumerable == null)
			{
				return;
			}
			foreach (object item in enumerable)
			{
				AttachObservedItem(item as INotifyPropertyChanged);
			}
		}

		private void UnobserveCurrentValue()
		{
			if (_observedObject != null)
			{
				_observedObject.PropertyChanged -= ObservedObject_PropertyChanged;
				_observedObject = null;
			}
			if (_observedCollection != null)
			{
				_observedCollection.CollectionChanged -= ObservedCollection_CollectionChanged;
				_observedCollection = null;
			}
			INotifyPropertyChanged[] array = _observedCollectionItems.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].PropertyChanged -= ObservedItem_PropertyChanged;
			}
			_observedCollectionItems.Clear();
		}

		private void ObservedObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.Changed?.Invoke(this, new StateVarChangedEventArgs<T>(_value, _value));
		}

		private void ObservedCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
			{
				foreach (object item2 in e.OldItems)
				{
					DetachObservedItem(item2 as INotifyPropertyChanged);
				}
			}
			if (e.NewItems != null)
			{
				foreach (object item in e.NewItems)
				{
					AttachObservedItem(item as INotifyPropertyChanged);
				}
			}
			this.Changed?.Invoke(this, new StateVarChangedEventArgs<T>(_value, _value));
		}

		private void ObservedItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.Changed?.Invoke(this, new StateVarChangedEventArgs<T>(_value, _value));
		}

		private void AttachObservedItem(INotifyPropertyChanged item)
		{
			if (item != null && _observedCollectionItems.Add(item))
			{
				item.PropertyChanged += ObservedItem_PropertyChanged;
			}
		}

		private void DetachObservedItem(INotifyPropertyChanged item)
		{
			if (item != null && _observedCollectionItems.Remove(item))
			{
				item.PropertyChanged -= ObservedItem_PropertyChanged;
			}
		}
	}
}
