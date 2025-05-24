using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Blish_HUD.Content;

namespace SL.ChatLinks.UI
{
	public sealed class IconsCache : IDisposable
	{
		private readonly ConcurrentDictionary<Uri, AsyncTexture2D> _webCache = new ConcurrentDictionary<Uri, AsyncTexture2D>();

		public AsyncTexture2D GetOrAdd(Uri key, Func<Uri, AsyncTexture2D> valueFactory)
		{
			return _webCache.GetOrAdd(key, valueFactory);
		}

		public void Dispose()
		{
			ConcurrentDictionary<Uri, AsyncTexture2D> webCache = _webCache;
			List<KeyValuePair<Uri, AsyncTexture2D>> list = new List<KeyValuePair<Uri, AsyncTexture2D>>(webCache.Count);
			list.AddRange(webCache);
			foreach (KeyValuePair<Uri, AsyncTexture2D> item in list)
			{
				item.Value.Dispose();
			}
			_webCache.Clear();
		}
	}
}
