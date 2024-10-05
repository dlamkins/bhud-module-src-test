using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Kenedia.Modules.Core.Models;
using Newtonsoft.Json;
using SemVer;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class StaticVersion
	{
		private Version _version = new Version(0, 0, 0, (string)null, (string)null);

		[JsonIgnore]
		public Version Version
		{
			set
			{
				if (value == null)
				{
					return;
				}
				if (!(value > _version))
				{
					return;
				}
				_version = value;
				using IEnumerator<KeyValuePair<string, ByteIntMap>> enumerator = GetEnumerator();
				while (enumerator.MoveNext())
				{
					this[enumerator.Current.Key].Version = value;
				}
			}
		}

		public ByteIntMap Armors { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap Backs { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap Enhancements { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap Enrichments { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap Infusions { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap Nourishments { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap Pets { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap PowerCores { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap Professions { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap PveRunes { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap PvpRunes { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap PveSigils { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap PvpSigils { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap PvpAmulets { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap Races { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap PveRelics { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap PvpRelics { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap Stats { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap Trinkets { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap Weapons { get; set; } = new ByteIntMap(new Version(0, 0, 0, (string)null, (string)null));


		public ByteIntMap this[string propertyName]
		{
			get
			{
				PropertyInfo propertyInfo = GetType().GetProperty(propertyName);
				if (!(propertyInfo != null))
				{
					throw new ArgumentException("Property '" + propertyName + "' not found in StaticVersion class.");
				}
				return (ByteIntMap)propertyInfo.GetValue(this);
			}
			set
			{
				PropertyInfo propertyInfo = GetType().GetProperty(propertyName);
				if (propertyInfo != null)
				{
					propertyInfo.SetValue(this, value);
					return;
				}
				throw new ArgumentException("Property '" + propertyName + "' not found in StaticVersion class.");
			}
		}

		public StaticVersion()
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Expected O, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown


		public StaticVersion(Version version)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Expected O, but got Unknown
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Expected O, but got Unknown
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Expected O, but got Unknown
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Expected O, but got Unknown
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Expected O, but got Unknown
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Expected O, but got Unknown
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Expected O, but got Unknown
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Expected O, but got Unknown
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Expected O, but got Unknown
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Expected O, but got Unknown
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Expected O, but got Unknown
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Expected O, but got Unknown
			using IEnumerator<KeyValuePair<string, ByteIntMap>> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				this[enumerator.Current.Key].Version = version;
			}
		}

		public IEnumerator<KeyValuePair<string, ByteIntMap>> GetEnumerator()
		{
			yield return new KeyValuePair<string, ByteIntMap>("Nourishments", Nourishments);
			yield return new KeyValuePair<string, ByteIntMap>("Enhancements", Enhancements);
			yield return new KeyValuePair<string, ByteIntMap>("PveRunes", PveRunes);
			yield return new KeyValuePair<string, ByteIntMap>("PvpRunes", PvpRunes);
			yield return new KeyValuePair<string, ByteIntMap>("PveSigils", PveSigils);
			yield return new KeyValuePair<string, ByteIntMap>("PvpSigils", PvpSigils);
			yield return new KeyValuePair<string, ByteIntMap>("Infusions", Infusions);
			yield return new KeyValuePair<string, ByteIntMap>("Enrichments", Enrichments);
			yield return new KeyValuePair<string, ByteIntMap>("Trinkets", Trinkets);
			yield return new KeyValuePair<string, ByteIntMap>("Backs", Backs);
			yield return new KeyValuePair<string, ByteIntMap>("Weapons", Weapons);
			yield return new KeyValuePair<string, ByteIntMap>("Armors", Armors);
			yield return new KeyValuePair<string, ByteIntMap>("PowerCores", PowerCores);
			yield return new KeyValuePair<string, ByteIntMap>("PveRelics", PveRelics);
			yield return new KeyValuePair<string, ByteIntMap>("PvpRelics", PvpRelics);
			yield return new KeyValuePair<string, ByteIntMap>("PvpAmulets", PvpAmulets);
			yield return new KeyValuePair<string, ByteIntMap>("Stats", Stats);
			yield return new KeyValuePair<string, ByteIntMap>("Professions", Professions);
			yield return new KeyValuePair<string, ByteIntMap>("Pets", Pets);
			yield return new KeyValuePair<string, ByteIntMap>("Races", Races);
		}

		public void Save(string path)
		{
			string json = JsonConvert.SerializeObject((object)this, SerializerSettings.Default);
			File.WriteAllText(path, json);
		}

		public static StaticVersion LoadFromFile(Version version, string path)
		{
			StaticVersion staticVersion = JsonConvert.DeserializeObject<StaticVersion>(File.ReadAllText(path), SerializerSettings.Default) ?? new StaticVersion(version);
			staticVersion.Version = version;
			foreach (KeyValuePair<string, ByteIntMap> property in staticVersion)
			{
				staticVersion[property.Key].Name = property.Key;
			}
			return staticVersion;
		}

		public static StaticVersion LoadFromFile(string path)
		{
			StaticVersion staticVersion = JsonConvert.DeserializeObject<StaticVersion>(File.ReadAllText(path), SerializerSettings.Default) ?? new StaticVersion();
			foreach (KeyValuePair<string, ByteIntMap> property in staticVersion)
			{
				staticVersion[property.Key].Name = property.Key;
			}
			return staticVersion;
		}

		public Dictionary<string, Version> GetVersions()
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			Dictionary<string, Version> versions = new Dictionary<string, Version>();
			using IEnumerator<KeyValuePair<string, ByteIntMap>> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, ByteIntMap> property = enumerator.Current;
				versions.Add(property.Key, new Version(((object)property.Value.Version).ToString(), false));
			}
			return versions;
		}
	}
}
