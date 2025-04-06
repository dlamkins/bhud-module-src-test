using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Blish_HUD.Content;

namespace SL.ChatLinks.UI
{
	public sealed class IconsCache : IDisposable
	{
		private readonly ConcurrentDictionary<string, AsyncTexture2D> _webCache = new ConcurrentDictionary<string, AsyncTexture2D>();

		public AsyncTexture2D GetOrAdd(string key, Func<string, AsyncTexture2D> valueFactory)
		{
			return _webCache.GetOrAdd(key, valueFactory);
		}

		public void Dispose()
		{
			ConcurrentDictionary<string, AsyncTexture2D> webCache = _webCache;
			List<KeyValuePair<string, AsyncTexture2D>> list = new List<KeyValuePair<string, AsyncTexture2D>>(webCache.Count);
			list.AddRange(webCache);
			foreach (KeyValuePair<string, AsyncTexture2D> item in list)
			{
				item.Value.Dispose();
			}
			_webCache.Clear();
		}
	}
}
