using System.Collections.Generic;

namespace Ideka.CustomCombatText
{
	public class DoubleSortedSet<T>
	{
		private readonly SortedSet<T> _setA = new SortedSet<T>(comparerA);

		private readonly SortedSet<T> _setB = new SortedSet<T>(comparerB);

		private readonly object _lock = new object();

		public DoubleSortedSet(IComparer<T> comparerA, IComparer<T> comparerB)
		{
		}

		public IEnumerable<T> SortedA()
		{
			return _setA;
		}

		public IEnumerable<T> SortedB()
		{
			return _setB;
		}

		public IEnumerable<T> ReverseA()
		{
			return _setA.Reverse();
		}

		public IEnumerable<T> ReverseB()
		{
			return _setB.Reverse();
		}

		public void Add(T item)
		{
			lock (_lock)
			{
				_setA.Add(item);
				_setB.Add(item);
			}
		}

		public void Remove(T item)
		{
			lock (_lock)
			{
				_setA.Remove(item);
				_setB.Remove(item);
			}
		}

		public void Clear()
		{
			lock (_lock)
			{
				_setA.Clear();
				_setB.Clear();
			}
		}
	}
}
