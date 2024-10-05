using System.Collections.Generic;
using System.IO;
using Kenedia.Modules.Core.Models;
using Newtonsoft.Json;
using SemVer;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class ByteIntMap
	{
		public Dictionary<byte, int> Items { get; } = new Dictionary<byte, int>();


		public Dictionary<byte, int> Ignored { get; } = new Dictionary<byte, int>();


		[JsonIgnore]
		public string Name { get; set; }

		[JsonIgnore]
		public Version Version { get; set; } = new Version(0, 0, 0, (string)null, (string)null);


		[JsonProperty("Version")]
		private string VersionString
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

		public int this[byte key]
		{
			get
			{
				return Items[key];
			}
			set
			{
				Items[key] = value;
			}
		}

		[JsonIgnore]
		public int Count => Items.Count;

		[JsonIgnore]
		public IEnumerable<byte> Keys => Items.Keys;

		[JsonIgnore]
		public IEnumerable<int> Values => Items.Values;

		public ByteIntMap()
		{
		}//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown


		public ByteIntMap(Version version)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			Version = version;
		}

		public void Add(byte key, int value)
		{
			Items.Add(key, value);
		}

		public void Remove(byte key)
		{
			Items.Remove(key);
		}

		public void Clear()
		{
			Items.Clear();
		}

		public bool ContainsKey(byte key)
		{
			return Items.ContainsKey(key);
		}

		public bool TryGetValue(byte key, out int value)
		{
			return Items.TryGetValue(key, out value);
		}

		public IEnumerator<KeyValuePair<byte, int>> GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		public void SaveToJson(string path)
		{
			string json = JsonConvert.SerializeObject((object)this, SerializerSettings.Default);
			File.WriteAllText(path, json);
		}
	}
}
