using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LiteDB.Engine
{
	internal class SortService : IDisposable
	{
		private readonly SortDisk _disk;

		private readonly List<SortContainer> _containers = new List<SortContainer>();

		private readonly int _containerSize;

		private readonly Done _done = new Done
		{
			Running = true
		};

		private readonly int _order;

		private readonly EnginePragmas _pragmas;

		private readonly BufferSlice _buffer;

		private readonly Lazy<Stream> _reader;

		public int Count => _done.Count;

		public IReadOnlyCollection<SortContainer> Containers => _containers;

		public SortService(SortDisk disk, int order, EnginePragmas pragmas)
		{
			_disk = disk;
			_order = order;
			_pragmas = pragmas;
			_containerSize = disk.ContainerSize;
			_reader = new Lazy<Stream>(() => _disk.GetReader());
			byte[] bytes = BufferPool.Rent(disk.ContainerSize);
			_buffer = new BufferSlice(bytes, 0, _containerSize);
		}

		public void Dispose()
		{
			foreach (SortContainer container in _containers)
			{
				container.Dispose();
				if (container.Position >= 0)
				{
					_disk.Return(container.Position);
				}
			}
			BufferPool.Return(_buffer.Array);
			if (_reader.IsValueCreated)
			{
				_disk.Return(_reader.Value);
			}
		}

		public void Insert(IEnumerable<KeyValuePair<BsonValue, PageAddress>> items)
		{
			foreach (IEnumerable<KeyValuePair<BsonValue, PageAddress>> containerItems in SliptValues(items, _done))
			{
				SortContainer container = new SortContainer(_pragmas.Collation, _containerSize);
				container.Insert(containerItems, _order, _buffer);
				_containers.Add(container);
				if (!_done.Running && _containers.Count == 1)
				{
					container.InitializeReader(null, _buffer, _pragmas.UtcDate);
					continue;
				}
				container.Position = _disk.GetContainerPosition();
				_disk.Write(container.Position, _buffer);
				container.InitializeReader(_reader.Value, null, _pragmas.UtcDate);
			}
		}

		public IEnumerable<KeyValuePair<BsonValue, PageAddress>> Sort()
		{
			if (_containers.Count == 0)
			{
				yield break;
			}
			SortContainer current = _containers[0];
			if (_containers.Count == 1)
			{
				do
				{
					yield return current.Current;
				}
				while (current.MoveNext());
				current.Dispose();
				yield break;
			}
			int diffOrder = _order * -1;
			while (_containers.Any((SortContainer x) => !x.IsEOF))
			{
				foreach (SortContainer container in _containers.Where((SortContainer x) => !x.IsEOF))
				{
					if (container.Current.Key.CompareTo(current.Current.Key, _pragmas.Collation) == diffOrder)
					{
						current = container;
					}
				}
				yield return current.Current;
				BsonValue lastKey = current.Current.Key;
				if (!current.MoveNext())
				{
					current = _containers.FirstOrDefault((SortContainer x) => !x.IsEOF);
				}
				while (current?.Current.Key == lastKey)
				{
					yield return current.Current;
					if (!current.MoveNext())
					{
						current = _containers.FirstOrDefault((SortContainer x) => !x.IsEOF);
					}
				}
			}
		}

		private IEnumerable<IEnumerable<KeyValuePair<BsonValue, PageAddress>>> SliptValues(IEnumerable<KeyValuePair<BsonValue, PageAddress>> source, Done done)
		{
			using IEnumerator<KeyValuePair<BsonValue, PageAddress>> enumerator = source.GetEnumerator();
			if (enumerator.MoveNext())
			{
				done.Count = 1;
				while (done.Running)
				{
					yield return YieldValues(enumerator, done);
				}
			}
		}

		private IEnumerable<KeyValuePair<BsonValue, PageAddress>> YieldValues(IEnumerator<KeyValuePair<BsonValue, PageAddress>> source, Done done)
		{
			int size = IndexNode.GetKeyLength(source.Current.Key, recalc: false) + 5;
			yield return source.Current;
			while (source.MoveNext())
			{
				int length = IndexNode.GetKeyLength(source.Current.Key, recalc: false) + 5;
				done.Count++;
				if (size + length > _containerSize)
				{
					yield break;
				}
				size += length;
				yield return source.Current;
			}
			done.Running = false;
		}
	}
}
