using System;
using Microsoft.AspNetCore.Shared;

namespace Microsoft.AspNetCore.Http.Features
{
	internal static class FeatureCollectionExtensions
	{
		public static TFeature GetRequiredFeature<TFeature>(this IFeatureCollection featureCollection) where TFeature : notnull
		{
			ArgumentNullThrowHelper.ThrowIfNull(featureCollection, "featureCollection");
			TFeature val = featureCollection.Get<TFeature>();
			if (val == null)
			{
				throw new InvalidOperationException($"Feature '{typeof(TFeature)}' is not present.");
			}
			return val;
		}

		public static object GetRequiredFeature(this IFeatureCollection featureCollection, Type key)
		{
			ArgumentNullThrowHelper.ThrowIfNull(featureCollection, "featureCollection");
			ArgumentNullThrowHelper.ThrowIfNull(key, "key");
			return featureCollection[key] ?? throw new InvalidOperationException($"Feature '{key}' is not present.");
		}
	}
}
