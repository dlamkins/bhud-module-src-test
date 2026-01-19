using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
			foreach (KeyValuePair<Uri, AsyncTexture2D> item in _webCache.ToList())
			{
				item.Value.Dispose();
			}
			_webCache.Clear();
		}
	}
}
