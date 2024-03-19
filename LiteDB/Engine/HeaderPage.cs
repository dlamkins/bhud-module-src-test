using System;
using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class HeaderPage : BasePage
	{
		public const string HEADER_INFO = "** This is a LiteDB file **";

		public const byte FILE_VERSION = 8;

		public const int P_HEADER_INFO = 32;

		public const int P_FILE_VERSION = 59;

		private const int P_FREE_EMPTY_PAGE_ID = 60;

		private const int P_LAST_PAGE_ID = 64;

		private const int P_CREATION_TIME = 68;

		private const int P_PRAGMAS = 76;

		private const int P_COLLECTIONS = 192;

		private const int COLLECTIONS_SIZE = 8000;

		private BsonDocument _collections;

		private bool _isCollectionsChanged;

		public uint FreeEmptyPageList { get; set; }

		public uint LastPageID { get; set; }

		public DateTime CreationTime { get; }

		public EnginePragmas Pragmas { get; set; }

		public HeaderPage(PageBuffer buffer, uint pageID)
			: base(buffer, 0u, PageType.Header)
		{
			CreationTime = DateTime.UtcNow;
			FreeEmptyPageList = uint.MaxValue;
			LastPageID = 0u;
			Pragmas = new EnginePragmas(this);
			_buffer.Write("** This is a LiteDB file **", 32);
			((BufferSlice)_buffer).Write((byte)8, 59);
			_buffer.Write(CreationTime, 68);
			_collections = new BsonDocument();
		}

		public HeaderPage(PageBuffer buffer)
			: base(buffer)
		{
			CreationTime = _buffer.ReadDateTime(68);
			LoadPage();
		}

		private void LoadPage()
		{
			string strA = _buffer.ReadString(32, "** This is a LiteDB file **".Length);
			byte ver = _buffer[59];
			if (string.CompareOrdinal(strA, "** This is a LiteDB file **") != 0 || ver != 8)
			{
				throw LiteException.InvalidDatabase();
			}
			FreeEmptyPageList = _buffer.ReadUInt32(60);
			LastPageID = _buffer.ReadUInt32(64);
			Pragmas = new EnginePragmas(_buffer, this);
			BufferSlice area = _buffer.Slice(192, 8000);
			using (BufferReader r = new BufferReader(new BufferSlice[1] { area }))
			{
				_collections = r.ReadDocument();
			}
			_isCollectionsChanged = false;
		}

		public override PageBuffer UpdateBuffer()
		{
			_buffer.Write(FreeEmptyPageList, 60);
			_buffer.Write(LastPageID, 64);
			Pragmas.UpdateBuffer(_buffer);
			if (_isCollectionsChanged)
			{
				using (BufferWriter w = new BufferWriter(_buffer.Slice(192, 8000)))
				{
					w.WriteDocument(_collections, recalc: true);
				}
				_isCollectionsChanged = false;
			}
			return base.UpdateBuffer();
		}

		public PageBuffer Savepoint()
		{
			UpdateBuffer();
			PageBuffer savepoint = new PageBuffer(new byte[8192], 0, 0);
			System.Buffer.BlockCopy(_buffer.Array, _buffer.Offset, savepoint.Array, savepoint.Offset, 8192);
			return savepoint;
		}

		public void Restore(PageBuffer savepoint)
		{
			System.Buffer.BlockCopy(savepoint.Array, savepoint.Offset, _buffer.Array, _buffer.Offset, 8192);
			LoadPage();
		}

		public uint GetCollectionPageID(string collection)
		{
			if (_collections.TryGetValue(collection, out var pageID))
			{
				return (uint)pageID.AsInt32;
			}
			return uint.MaxValue;
		}

		public IEnumerable<KeyValuePair<string, uint>> GetCollections()
		{
			foreach (KeyValuePair<string, BsonValue> el in _collections.GetElements())
			{
				yield return new KeyValuePair<string, uint>(el.Key, (uint)el.Value.AsInt32);
			}
		}

		public void InsertCollection(string name, uint pageID)
		{
			_collections[name] = (int)pageID;
			_isCollectionsChanged = true;
		}

		public void DeleteCollection(string name)
		{
			_collections.Remove(name);
			_isCollectionsChanged = true;
		}

		public void RenameCollection(string oldName, string newName)
		{
			BsonValue pageID = _collections[oldName];
			_collections.Remove(oldName);
			_collections.Add(newName, pageID);
			_isCollectionsChanged = true;
		}

		public int GetAvailableCollectionSpace()
		{
			return 8000 - _collections.GetBytesCount(recalc: true) - 1 - 1 - 4 - 8;
		}
	}
}
