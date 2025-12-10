using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
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

		[IteratorStateMachine(typeof(_003CGetEnumerator_003Ed__54))]
		public IEnumerator<KeyValuePair<string, ByteIntMap>> GetEnumerator()
		{
			return new _003CGetEnumerator_003Ed__54(0)
			{
				_003C_003E4__this = this
			};
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
