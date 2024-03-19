using System.Collections.Generic;
using System.Linq;

namespace LiteDB.Engine
{
	internal class CollectionPage : BasePage
	{
		private const int P_INDEXES = 96;

		private const int P_INDEXES_COUNT = 8096;

		private readonly Dictionary<string, CollectionIndex> _indexes = new Dictionary<string, CollectionIndex>();

		public uint[] FreeDataPageList { get; } = new uint[5];


		public CollectionIndex PK => _indexes["_id"];

		public CollectionPage(PageBuffer buffer, uint pageID)
			: base(buffer, pageID, PageType.Collection)
		{
			for (int i = 0; i < 5; i++)
			{
				FreeDataPageList[i] = uint.MaxValue;
			}
		}

		public CollectionPage(PageBuffer buffer)
			: base(buffer)
		{
			Constants.ENSURE(base.PageType == PageType.Collection, "page type must be collection page");
			if (base.PageType != PageType.Collection)
			{
				throw LiteException.InvalidPageType(PageType.Collection, this);
			}
			BufferSlice area = _buffer.Slice(32, 8160);
			using BufferReader r = new BufferReader(new BufferSlice[1] { area });
			for (int j = 0; j < 5; j++)
			{
				FreeDataPageList[j] = r.ReadUInt32();
			}
			r.Skip(64 - r.Position);
			byte count = r.ReadByte();
			for (int i = 0; i < count; i++)
			{
				CollectionIndex index = new CollectionIndex(r);
				_indexes[index.Name] = index;
			}
		}

		public override PageBuffer UpdateBuffer()
		{
			if (base.PageType == PageType.Empty)
			{
				return base.UpdateBuffer();
			}
			using (BufferWriter w = new BufferWriter(_buffer.Slice(32, 8160)))
			{
				for (int i = 0; i < 5; i++)
				{
					w.Write(FreeDataPageList[i]);
				}
				w.Skip(64 - w.Position);
				w.Write((byte)_indexes.Count);
				foreach (CollectionIndex value in _indexes.Values)
				{
					value.UpdateBuffer(w);
				}
			}
			return base.UpdateBuffer();
		}

		public CollectionIndex GetCollectionIndex(string name)
		{
			if (_indexes.TryGetValue(name, out var index))
			{
				return index;
			}
			return null;
		}

		public ICollection<CollectionIndex> GetCollectionIndexes()
		{
			return _indexes.Values;
		}

		public CollectionIndex[] GetCollectionIndexesSlots()
		{
			CollectionIndex[] indexes = new CollectionIndex[_indexes.Max((KeyValuePair<string, CollectionIndex> x) => x.Value.Slot) + 1];
			foreach (CollectionIndex index in _indexes.Values)
			{
				indexes[index.Slot] = index;
			}
			return indexes;
		}

		public CollectionIndex InsertCollectionIndex(string name, string expr, bool unique)
		{
			int totalLength = 1 + _indexes.Sum((KeyValuePair<string, CollectionIndex> x) => CollectionIndex.GetLength(x.Value)) + CollectionIndex.GetLength(name, expr);
			if (_indexes.Count == 255 || totalLength >= 8096)
			{
				throw new LiteException(0, "This collection has no more space for new indexes");
			}
			CollectionIndex index = new CollectionIndex((byte)((_indexes.Count != 0) ? ((uint)(_indexes.Max((KeyValuePair<string, CollectionIndex> x) => x.Value.Slot) + 1)) : 0u), 0, name, expr, unique);
			_indexes[name] = index;
			base.IsDirty = true;
			return index;
		}

		public CollectionIndex UpdateCollectionIndex(string name)
		{
			base.IsDirty = true;
			return _indexes[name];
		}

		public void DeleteCollectionIndex(string name)
		{
			_indexes.Remove(name);
			base.IsDirty = true;
		}
	}
}
