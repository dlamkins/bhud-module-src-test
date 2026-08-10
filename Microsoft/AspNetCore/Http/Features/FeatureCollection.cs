using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Shared;

namespace Microsoft.AspNetCore.Http.Features
{
	[DebuggerDisplay("Count = {GetCount()}")]
	[DebuggerTypeProxy(typeof(FeatureCollectionDebugView))]
	internal class FeatureCollection : IFeatureCollection, IEnumerable<KeyValuePair<Type, object>>, IEnumerable
	{
		private sealed class KeyComparer : IEqualityComparer<KeyValuePair<Type, object>>
		{
			public bool Equals(KeyValuePair<Type, object> x, KeyValuePair<Type, object> y)
			{
				return x.Key.Equals(y.Key);
			}

			public int GetHashCode(KeyValuePair<Type, object> obj)
			{
				return obj.Key.GetHashCode();
			}
		}

		private sealed class FeatureCollectionDebugView
		{
			private readonly FeatureCollection _features;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public KeyValuePair<string, object>[] Items => _features.Select<KeyValuePair<Type, object>, KeyValuePair<string, object>>((KeyValuePair<Type, object> pair) => new KeyValuePair<string, object>(pair.Key.FullName ?? string.Empty, pair.Value)).ToArray();

			public FeatureCollectionDebugView(FeatureCollection features)
			{
				_features = features;
				base._002Ector();
			}
		}

		private static readonly KeyComparer FeatureKeyComparer = new KeyComparer();

		private readonly IFeatureCollection _defaults;

		private readonly int _initialCapacity;

		private IDictionary<Type, object> _features;

		private volatile int _containerRevision;

		public virtual int Revision => _containerRevision + (_defaults?.Revision ?? 0);

		public bool IsReadOnly => false;

		public object? this[Type key]
		{
			get
			{
				ArgumentNullThrowHelper.ThrowIfNull(key, "key");
				if (_features == null || !_features.TryGetValue(key, out var value))
				{
					return _defaults?[key];
				}
				return value;
			}
			set
			{
				ArgumentNullThrowHelper.ThrowIfNull(key, "key");
				if (value == null)
				{
					if (_features != null && _features.Remove(key))
					{
						_containerRevision++;
					}
					return;
				}
				if (_features == null)
				{
					_features = new Dictionary<Type, object>(_initialCapacity);
				}
				_features[key] = value;
				_containerRevision++;
			}
		}

		public FeatureCollection()
		{
		}

		public FeatureCollection(int initialCapacity)
		{
			ArgumentOutOfRangeThrowHelper.ThrowIfNegative(initialCapacity, "initialCapacity");
			_initialCapacity = initialCapacity;
		}

		public FeatureCollection(IFeatureCollection defaults)
		{
			_defaults = defaults;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
		{
			if (_features != null)
			{
				foreach (KeyValuePair<Type, object> feature in _features)
				{
					yield return feature;
				}
			}
			if (_defaults == null)
			{
				yield break;
			}
			IEnumerable<KeyValuePair<Type, object>> enumerable;
			if (_features != null)
			{
				enumerable = _defaults.Except<KeyValuePair<Type, object>>(_features, FeatureKeyComparer);
			}
			else
			{
				IEnumerable<KeyValuePair<Type, object>> defaults = _defaults;
				enumerable = defaults;
			}
			foreach (KeyValuePair<Type, object> item in enumerable)
			{
				yield return item;
			}
		}

		public TFeature? Get<TFeature>()
		{
			if (typeof(TFeature).IsValueType)
			{
				object? obj = this[typeof(TFeature)];
				if (obj == null && (object)Nullable.GetUnderlyingType(typeof(TFeature)) == null)
				{
					throw new InvalidOperationException(typeof(TFeature).FullName + " does not exist in the feature collection and because it is a struct the method can't return null. Use 'featureCollection[typeof(" + typeof(TFeature).FullName + ")] is not null' to check if the feature exists.");
				}
				return (TFeature)obj;
			}
			return (TFeature)this[typeof(TFeature)];
		}

		public void Set<TFeature>(TFeature? instance)
		{
			this[typeof(TFeature)] = instance;
		}

		private int GetCount()
		{
			return this.Count();
		}
	}
}
