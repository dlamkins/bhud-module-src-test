using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;

namespace SL.ChatLinks.UI
{
	public sealed class IconsCache : IDisposable
	{
		private readonly ConcurrentDictionary<string, AsyncTexture2D> WebCache = new ConcurrentDictionary<string, AsyncTexture2D>();

		public AsyncTexture2D GetOrAdd(string key, Func<string, AsyncTexture2D> valueFactory)
		{
			return WebCache.GetOrAdd(key, valueFactory);
		}

		public void Dispose()
		{
			foreach (KeyValuePair<string, AsyncTexture2D> item in WebCache.ToList())
			{
				item.Value.Dispose();
			}
			WebCache.Clear();
		}
	}
}
