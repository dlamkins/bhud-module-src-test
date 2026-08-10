using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Concurrent
{
	[DebuggerDisplay("Count = {Count}")]
	internal sealed class MultiProducerMultiConsumerQueue<T> : ConcurrentQueue<T>, IProducerConsumerQueue<T>, IEnumerable<T>, IEnumerable
	{
		bool IProducerConsumerQueue<T>.IsEmpty => base.IsEmpty;

		int IProducerConsumerQueue<T>.Count => base.Count;

		void IProducerConsumerQueue<T>.Enqueue(T item)
		{
			Enqueue(item);
		}

		bool IProducerConsumerQueue<T>.TryDequeue([MaybeNullWhen(false)] out T result)
		{
			return TryDequeue(out result);
		}

		int IProducerConsumerQueue<T>.GetCountSafe(object syncObj)
		{
			return base.Count;
		}
	}
}
