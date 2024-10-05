using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Models;
using Newtonsoft.Json;
using SemVer;

namespace Kenedia.Modules.BuildsManager.Services
{
	[DataContract]
	public class BaseMappedDataEntry : IDisposable
	{
		protected bool DataLoaded;

		public Version Version { get; set; } = new Version(0, 0, 0, (string)null, (string)null);


		[DataMember]
		[JsonProperty("Version")]
		public string VersionString
		{
			get
			{
				return ((object)Version).ToString();
			}
			set
			{
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Expected O, but got Unknown
				Version = new Version(value, false);
			}
		}

		[DataMember]
		public Dictionary<string, object> Items { get; set; } = new Dictionary<string, object>();


		public bool IsLoaded => DataLoaded;

		public ByteIntMap Map { get; set; }

		public virtual Task<bool> LoadAndUpdate(string name, ByteIntMap map, string path, Gw2ApiManager gw2ApiManager, CancellationToken token)
		{
			return Task.FromResult(result: false);
		}

		public static BaseMappedDataEntry FromGeneric<Key, T>(MappedDataEntry<Key, T> entry) where Key : notnull where T : IDataMember, new()
		{
			return new BaseMappedDataEntry
			{
				Version = entry.Version,
				Items = entry.Items.ToDictionary<KeyValuePair<Key, T>, string, object>((KeyValuePair<Key, T> kvp) => $"{kvp.Key}", (KeyValuePair<Key, T> kvp) => kvp.Value)
			};
		}

		public void Dispose()
		{
			Items.Clear();
			Items = null;
		}
	}
}
