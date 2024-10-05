using System.Collections.Generic;
using System.IO;
using SemVer;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class ByteIntMapCollection
	{
		private readonly Paths _paths;

		public ByteIntMap Nourishments { get; } = new ByteIntMap();


		public ByteIntMap Enhancements { get; } = new ByteIntMap();


		public ByteIntMap PveRunes { get; } = new ByteIntMap();


		public ByteIntMap PvpRunes { get; } = new ByteIntMap();


		public ByteIntMap PveSigils { get; } = new ByteIntMap();


		public ByteIntMap PvpSigils { get; } = new ByteIntMap();


		public ByteIntMap Infusions { get; } = new ByteIntMap();


		public ByteIntMap Enrichments { get; } = new ByteIntMap();


		public ByteIntMap Trinkets { get; } = new ByteIntMap();


		public ByteIntMap Backs { get; } = new ByteIntMap();


		public ByteIntMap Weapons { get; } = new ByteIntMap();


		public ByteIntMap Armors { get; } = new ByteIntMap();


		public ByteIntMap PowerCores { get; } = new ByteIntMap();


		public ByteIntMap PveRelics { get; } = new ByteIntMap();


		public ByteIntMap PvpRelics { get; } = new ByteIntMap();


		public ByteIntMap PvpAmulets { get; } = new ByteIntMap();


		public ByteIntMap Stats { get; } = new ByteIntMap();


		public ByteIntMapCollection(Paths paths)
		{
			_paths = paths;
		}

		public ByteIntMapCollection(Version version, Paths paths)
			: this(paths)
		{
			using IEnumerator<KeyValuePair<string, ByteIntMap>> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value.Version = version;
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
		}

		public void Save()
		{
			try
			{
				using IEnumerator<KeyValuePair<string, ByteIntMap>> enumerator = GetEnumerator();
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, ByteIntMap> itemMap = enumerator.Current;
					string filePath = Path.Combine(_paths.ItemMapPath, itemMap.Key + ".json");
					itemMap.Value?.SaveToJson(filePath);
				}
			}
			catch
			{
			}
		}
	}
}
