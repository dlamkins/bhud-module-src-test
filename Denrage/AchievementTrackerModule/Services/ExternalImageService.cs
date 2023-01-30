using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Denrage.AchievementTrackerModule.Interfaces;
using Flurl.Http;
using Microsoft.Xna.Framework.Graphics;

namespace Denrage.AchievementTrackerModule.Services
{
	public class ExternalImageService : IExternalImageService
	{
		private readonly GraphicsService graphicsService;

		private readonly Logger logger;

		public ExternalImageService(GraphicsService graphicsService, Logger logger)
		{
			this.graphicsService = graphicsService;
			this.logger = logger;
		}

		public AsyncTexture2D GetImage(string imageUrl)
		{
			return GetImageInternal((async () => await DownloadWikiContent(imageUrl).GetStreamAsync(default(CancellationToken), (HttpCompletionOption)0), imageUrl));
		}

		public async Task<string> GetDirectImageLink(string imagePath, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (imagePath.Contains("File:"))
			{
				try
				{
					string obj = await DownloadWikiContent(imagePath).GetStringAsync(cancellationToken, (HttpCompletionOption)0);
					int fillImageStartIndex = obj.IndexOf("fullImageLink");
					int hrefStartIndex = obj.IndexOf("href=", fillImageStartIndex);
					int linkStartIndex = obj.IndexOf("\"", hrefStartIndex) + 1;
					int linkEndIndex = obj.IndexOf("\"", linkStartIndex);
					return obj.Substring(linkStartIndex, linkEndIndex - linkStartIndex);
				}
				catch (Exception ex)
				{
					logger.Error(ex, "Exception occured on parsing an image path");
					return string.Empty;
				}
			}
			return imagePath;
		}

		public AsyncTexture2D GetImageFromIndirectLink(string imagePath)
		{
			return GetImageInternal((async delegate
			{
				string url = await GetDirectImageLink(imagePath);
				return await DownloadWikiContent(url).GetStreamAsync(default(CancellationToken), (HttpCompletionOption)0);
			}, imagePath));
		}

		private AsyncTexture2D GetImageInternal((Func<Task<Stream>> GetStream, string Url) getImageStream)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			AsyncTexture2D texture = new AsyncTexture2D(Textures.get_TransparentPixel());
			Stream imageStream;
			Task.Run(async delegate
			{
				try
				{
					imageStream = await getImageStream.GetStream();
					graphicsService.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice device)
					{
						try
						{
							texture.SwapTexture(TextureUtil.FromStreamPremultiplied(device, imageStream));
							imageStream.Close();
						}
						catch (Exception ex2)
						{
							logger.Error(ex2, "Exception occured on downloading/swapping image. URL: " + getImageStream.Url);
							graphicsService.QueueMainThreadRender((Action<GraphicsDevice>)delegate
							{
								texture.SwapTexture(Textures.get_Error());
							});
						}
					});
				}
				catch (Exception ex)
				{
					logger.Error(ex, "Exception occured on downloading/swapping image. URL: " + getImageStream.Url);
					graphicsService.QueueMainThreadRender((Action<GraphicsDevice>)delegate
					{
						texture.SwapTexture(Textures.get_Error());
					});
				}
			});
			return texture;
		}

		private IFlurlRequest DownloadWikiContent(string url)
		{
			return ("https://wiki.guildwars2.com" + url).WithHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36");
		}
	}
}
