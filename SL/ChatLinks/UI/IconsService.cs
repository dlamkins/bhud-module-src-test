using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Microsoft.Xna.Framework.Graphics;
using SL.Common;

namespace SL.ChatLinks.UI
{
	public sealed class IconsService
	{
		[CompilerGenerated]
		private HttpClient _003ChttpClient_003EP;

		[CompilerGenerated]
		private IconsCache _003Ccache_003EP;

		public IconsService(HttpClient httpClient, IconsCache cache)
		{
			_003ChttpClient_003EP = httpClient;
			_003Ccache_003EP = cache;
			base._002Ector();
		}

		public AsyncTexture2D? GetIcon(Uri? iconUrl)
		{
			if ((object)iconUrl == null)
			{
				return null;
			}
			return (GameService.Content.GetRenderServiceTexture(iconUrl) ?? _003Ccache_003EP.GetOrAdd(iconUrl, delegate(Uri url)
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
				}, TaskScheduler.Default);
				return newTexture;
			})).Duplicate();
		}
	}
}
