using System;
using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class IndexService
	{
		private static readonly Random _rnd = new Random();

		private readonly Snapshot _snapshot;

		private readonly Collation _collation;

		public Collation Collation => _collation;

		public IndexService(Snapshot snapshot, Collation collation)
		{
			_snapshot = snapshot;
			_collation = collation;
		}

		public CollectionIndex CreateIndex(string name, string expr, bool unique)
		{
			int keyLength;
			int bytesLength = IndexNode.GetNodeLength(32, BsonValue.MinValue, out keyLength);
			IndexPage indexPage = _snapshot.NewPage<IndexPage>();
			CollectionIndex index = _snapshot.CollectionPage.InsertCollectionIndex(name, expr, unique);
			IndexNode head = indexPage.InsertIndexNode(index.Slot, 32, BsonValue.MinValue, PageAddress.Empty, bytesLength);
			IndexNode tail = indexPage.InsertIndexNode(index.Slot, 32, BsonValue.MaxValue, PageAddress.Empty, bytesLength);
			head.SetNext(0, tail.Position);
			tail.SetPrev(0, head.Position);
			index.FreeIndexPageList = indexPage.PageID;
			indexPage.PageListSlot = 0;
			index.Head = head.Position;
			index.Tail = tail.Position;
			return index;
		}

		public IndexNode AddNode(CollectionIndex index, BsonValue key, PageAddress dataBlock, IndexNode last)
		{
			if (key.IsMaxValue || key.IsMinValue)
			{
				throw LiteException.InvalidIndexKey("BsonValue MaxValue/MinValue are not supported as index key");
			}
			byte level = Flip();
			if (level > index.MaxLevel)
			{
				_snapshot.CollectionPage.UpdateCollectionIndex(index.Name).MaxLevel = level;
			}
			return AddNode(index, key, dataBlock, level, last);
		}

		private IndexNode AddNode(CollectionIndex index, BsonValue key, PageAddress dataBlock, byte level, IndexNode last)
		{
			int keyLength;
			int bytesLength = IndexNode.GetNodeLength(level, key, out keyLength);
			if (keyLength > 1023)
			{
				throw LiteException.InvalidIndexKey($"Index key must be less than {1023} bytes.");
			}
			IndexNode node = _snapshot.GetFreeIndexPage(bytesLength, ref index.FreeIndexPageList).InsertIndexNode(index.Slot, level, key, dataBlock, bytesLength);
			IndexNode cur = GetNode(index.Head);
			IndexNode cache = null;
			for (int i = index.MaxLevel - 1; i >= 0; i--)
			{
				cache = ((cache != null && cache.Position == cur.Next[i]) ? cache : GetNode(cur.Next[i]));
				while (!cur.Next[i].IsEmpty)
				{
					cache = ((cache != null && cache.Position == cur.Next[i]) ? cache : GetNode(cur.Next[i]));
					int num = cache.Key.CompareTo(key, _collation);
					if (num == 0 && index.Unique)
					{
						throw LiteException.IndexDuplicateKey(index.Name, key);
					}
					if (num == 1)
					{
						break;
					}
					cur = cache;
				}
				if (i <= level - 1)
				{
					node.SetNext((byte)i, cur.Next[i]);
					node.SetPrev((byte)i, cur.Position);
					cur.SetNext((byte)i, node.Position);
					GetNode(node.Next[i])?.SetPrev((byte)i, node.Position);
				}
			}
			if (last != null)
			{
				Constants.ENSURE(last.NextNode == PageAddress.Empty, "last index node must point to null");
				last = GetNode(last.Position);
				last.SetNextNode(node.Position);
			}
			_snapshot.AddOrRemoveFreeIndexList(node.Page, ref index.FreeIndexPageList);
			return node;
		}

		public byte Flip()
		{
			byte level = 1;
			int R = _rnd.Next();
			while ((R & 1) == 1)
			{
				level = (byte)(level + 1);
				if (level == 32)
				{
					break;
				}
				R >>= 1;
			}
			return level;
		}

		public IndexNode GetNode(PageAddress address)
		{
			if (address.PageID == uint.MaxValue)
			{
				return null;
			}
			return _snapshot.GetPage<IndexPage>(address.PageID).GetIndexNode(address.Index);
		}

		public IEnumerable<IndexNode> GetNodeList(PageAddress nodeAddress)
		{
			for (IndexNode node = GetNode(nodeAddress); node != null; node = GetNode(node.NextNode))
			{
				yield return node;
			}
		}

		public void DeleteAll(PageAddress pkAddress)
		{
			IndexNode node = GetNode(pkAddress);
			CollectionIndex[] indexes = _snapshot.CollectionPage.GetCollectionIndexesSlots();
			while (node != null)
			{
				DeleteSingleNode(node, indexes[node.Slot]);
				node = GetNode(node.NextNode);
			}
		}

		public IndexNode DeleteList(PageAddress pkAddress, HashSet<PageAddress> toDelete)
		{
			IndexNode last = GetNode(pkAddress);
			IndexNode node = GetNode(last.NextNode);
			CollectionIndex[] indexes = _snapshot.CollectionPage.GetCollectionIndexesSlots();
			while (node != null)
			{
				if (toDelete.Contains(node.Position))
				{
					DeleteSingleNode(node, indexes[node.Slot]);
					last.SetNextNode(node.NextNode);
				}
				else
				{
					last = node;
				}
				node = GetNode(node.NextNode);
			}
			return last;
		}

		private void DeleteSingleNode(IndexNode node, CollectionIndex index)
		{
			for (int i = node.Level - 1; i >= 0; i--)
			{
				IndexNode prevNode = GetNode(node.Prev[i]);
				IndexNode nextNode = GetNode(node.Next[i]);
				prevNode?.SetNext((byte)i, node.Next[i]);
				nextNode?.SetPrev((byte)i, node.Prev[i]);
			}
			node.Page.DeleteIndexNode(node.Position.Index);
			_snapshot.AddOrRemoveFreeIndexList(node.Page, ref index.FreeIndexPageList);
		}

		public void DropIndex(CollectionIndex index)
		{
			byte slot = index.Slot;
			CollectionIndex pkIndex = _snapshot.CollectionPage.PK;
			foreach (IndexNode item in FindAll(pkIndex, 1))
			{
				PageAddress next = item.NextNode;
				IndexNode last = item;
				while (next != PageAddress.Empty)
				{
					IndexNode node = GetNode(next);
					if (node.Slot == slot)
					{
						node.Page.DeleteIndexNode(node.Position.Index);
						last.SetNextNode(node.NextNode);
					}
					else
					{
						last = node;
					}
					next = node.NextNode;
				}
			}
			GetNode(index.Head).Page.DeleteIndexNode(index.Head.Index);
			GetNode(index.Tail).Page.DeleteIndexNode(index.Tail.Index);
		}

		public IEnumerable<IndexNode> FindAll(CollectionIndex index, int order)
		{
			IndexNode cur = ((order == 1) ? GetNode(index.Head) : GetNode(index.Tail));
			while (!cur.GetNextPrev(0, order).IsEmpty)
			{
				cur = GetNode(cur.GetNextPrev(0, order));
				if (cur.Key.IsMinValue || cur.Key.IsMaxValue)
				{
					break;
				}
				yield return cur;
			}
		}

		public IndexNode Find(CollectionIndex index, BsonValue value, bool sibling, int order)
		{
			IndexNode cur = ((order == 1) ? GetNode(index.Head) : GetNode(index.Tail));
			for (int i = index.MaxLevel - 1; i >= 0; i--)
			{
				while (!cur.GetNextPrev((byte)i, order).IsEmpty)
				{
					IndexNode next = GetNode(cur.GetNextPrev((byte)i, order));
					int diff = next.Key.CompareTo(value, _collation);
					if (diff == order && (i > 0 || !sibling))
					{
						break;
					}
					if (diff == order && i == 0 && sibling)
					{
						if (!next.Key.IsMinValue && !next.Key.IsMaxValue)
						{
							return next;
						}
						return null;
					}
					if (diff == 0)
					{
						return next;
					}
					cur = GetNode(cur.GetNextPrev((byte)i, order));
				}
			}
			return null;
		}
	}
}
