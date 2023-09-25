using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Newtonsoft.Json;
using SemVer;

namespace Kenedia.Modules.Core.Models
{
	public class DataEntry<Key, T> : BaseDataEntry
	{
		[DataMember]
		public new Dictionary<Key, T> Items { get; protected set; } = new Dictionary<Key, T>();


		public T this[Key key]
		{
			get
			{
				if (!Items.TryGetValue(key, out var value))
				{
					return default(T);
				}
				return value;
			}
			set
			{
				Items[key] = value;
			}
		}

		[JsonIgnore]
		public int Count => Items.Count;

		[JsonIgnore]
		public IEnumerable<Key> Keys => Items.Keys;

		[JsonIgnore]
		public IEnumerable<T> Values => Items.Values;

		public void Add(Key key, T value)
		{
			Items.Add(key, value);
		}

		public void Remove(Key key)
		{
			Items.Remove(key);
		}

		public void Clear()
		{
			Items.Clear();
		}

		public bool ContainsKey(Key key)
		{
			return Items.ContainsKey(key);
		}

		public bool TryGetValue(Key key, out T value)
		{
			return Items.TryGetValue(key, out value);
		}

		public override async Task<bool> LoadAndUpdate(string name, Version version, string path, Gw2ApiManager gw2ApiManager, CancellationToken cancellationToken)
		{
			return await Task.FromResult(DataLoaded);
		}
	}
	public class DataEntry<T> : BaseDataEntry
	{
		[DataMember]
		public new List<T> Items { get; protected set; } = new List<T>();


		public T this[int key]
		{
			get
			{
				T val = Items.ElementAt(key);
				if (val == null)
				{
					return default(T);
				}
				return val;
			}
			set
			{
				Items[key] = value;
			}
		}

		[JsonIgnore]
		public int Count => Items.Count;

		public void Add(T value)
		{
			Items.Add(value);
		}

		public void Remove(T item)
		{
			Items.Remove(item);
		}

		public void Clear()
		{
			Items.Clear();
		}

		public override async Task<bool> LoadAndUpdate(string name, Version version, string path, Gw2ApiManager gw2ApiManager, CancellationToken cancellationToken)
		{
			return await Task.FromResult(DataLoaded);
		}
	}
}
