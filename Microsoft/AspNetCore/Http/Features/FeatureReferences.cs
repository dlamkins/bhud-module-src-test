using System;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Http.Features
{
	internal struct FeatureReferences<TCache>
	{
		public TCache? Cache;

		public IFeatureCollection Collection { get; private set; }

		public int Revision { get; private set; }

		public FeatureReferences(IFeatureCollection collection)
		{
			Collection = collection;
			Cache = default(TCache);
			Revision = collection.Revision;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Initalize(IFeatureCollection collection)
		{
			Revision = collection.Revision;
			Collection = collection;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Initalize(IFeatureCollection collection, int revision)
		{
			Revision = revision;
			Collection = collection;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TFeature? Fetch<TFeature, TState>(ref TFeature? cached, TState state, Func<TState, TFeature?> factory) where TFeature : class?
		{
			bool flush = false;
			int num = Collection?.Revision ?? ContextDisposed();
			if (Revision != num)
			{
				cached = null;
				flush = true;
			}
			return cached ?? UpdateCached(ref cached, state, factory, num, flush);
		}

		private TFeature UpdateCached<TFeature, TState>(ref TFeature cached, TState state, Func<TState, TFeature> factory, int revision, bool flush) where TFeature : class
		{
			if (flush)
			{
				Cache = default(TCache);
			}
			cached = Collection.Get<TFeature>();
			if (cached == null)
			{
				cached = factory(state);
				Collection.Set(cached);
				Revision = Collection.Revision;
			}
			else if (flush)
			{
				Revision = revision;
			}
			return cached;
		}

		public TFeature? Fetch<TFeature>(ref TFeature? cached, Func<IFeatureCollection, TFeature?> factory) where TFeature : class?
		{
			return Fetch(ref cached, Collection, factory);
		}

		private static int ContextDisposed()
		{
			ThrowContextDisposed();
			return 0;
		}

		private static void ThrowContextDisposed()
		{
			throw new ObjectDisposedException("Collection", "IFeatureCollection has been disposed.");
		}
	}
}
