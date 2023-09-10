using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Kenedia.Modules.Core.Attributes;
using SemVer;

namespace Kenedia.Modules.Core.Models
{
	[DataContract]
	public class BaseDataEntry
	{
		protected bool DataLoaded;

		[DataMember]
		[JsonSemverVersion]
		public Version Version { get; set; } = new Version(0, 0, 0, (string)null, (string)null);


		[DataMember]
		public Dictionary<string, object> Items { get; set; } = new Dictionary<string, object>();


		public bool IsLoaded => DataLoaded;

		public virtual Task<bool> LoadAndUpdate(string name, Version version, string path, Gw2ApiManager gw2ApiManager, CancellationToken token)
		{
			return Task.FromResult(result: false);
		}

		public static BaseDataEntry FromGeneric<Key, T>(DataEntry<Key, T> entry)
		{
			return new BaseDataEntry
			{
				Version = entry.Version,
				Items = ((IEnumerable<KeyValuePair<Key, T>>)entry.Items).ToDictionary((Func<KeyValuePair<Key, T>, string>)((KeyValuePair<Key, T> kvp) => $"{kvp.Key}"), (Func<KeyValuePair<Key, T>, object>)((KeyValuePair<Key, T> kvp) => kvp.Value))
			};
		}

		public void Dispose()
		{
			Items.Clear();
			Items = null;
		}
	}
}
