using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using GuildWars2.Items;
using Microsoft.Xna.Framework.Graphics;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public class ItemIcons
	{
		[CompilerGenerated]
		private HttpClient _003ChttpClient_003EP;

		private static readonly ConcurrentDictionary<string, AsyncTexture2D> WebCache = new ConcurrentDictionary<string, AsyncTexture2D>();

		public ItemIcons(HttpClient httpClient)
		{
			_003ChttpClient_003EP = httpClient;
			base._002Ector();
		}

		public AsyncTexture2D? GetIcon(Item item)
		{
			if (item.IconHref == null)
			{
				return null;
			}
			return (GameService.Content.GetRenderServiceTexture(item.IconHref) ?? WebCache.GetOrAdd(item.IconHref, (Func<string, AsyncTexture2D>)delegate(string url)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				AsyncTexture2D newTexture = new AsyncTexture2D();
				_003ChttpClient_003EP.GetStreamAsync(url).ContinueWith(delegate(Task<Stream> task)
				{
					if (task.Status == TaskStatus.RanToCompletion)
					{
						using Stream stream = task.Result;
						Texture2D val = TextureUtil.FromStreamPremultiplied(stream);
						newTexture.SwapTexture(val);
					}
				});
				return newTexture;
			})).Duplicate();
		}
	}
}
