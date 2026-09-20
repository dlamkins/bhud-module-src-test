using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Flurl.Http;
using Microsoft.Xna.Framework.Graphics;
using Quarry.Interfaces;

namespace Quarry.Services
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
					string source = await DownloadWikiContent(imagePath).GetStringAsync(cancellationToken, (HttpCompletionOption)0);
					int fillImageStartIndex = source.IndexOf("fullImageLink", StringComparison.Ordinal);
					int hrefStartIndex = ((fillImageStartIndex < 0) ? (-1) : source.IndexOf("href=", fillImageStartIndex, StringComparison.Ordinal));
					int quoteIndex = ((hrefStartIndex < 0) ? (-1) : source.IndexOf("\"", hrefStartIndex, StringComparison.Ordinal));
					int linkStartIndex = ((quoteIndex < 0) ? (-1) : (quoteIndex + 1));
					int linkEndIndex = ((linkStartIndex < 0) ? (-1) : source.IndexOf("\"", linkStartIndex, StringComparison.Ordinal));
					if (linkStartIndex < 0 || linkEndIndex < 0)
					{
						logger.Debug("No full-image link on wiki page " + imagePath + "; showing the error texture.");
						return null;
					}
					return source.Substring(linkStartIndex, linkEndIndex - linkStartIndex);
				}
				catch (Exception ex)
				{
					logger.Debug(ex, "Couldn't resolve a wiki File: page to a direct image link.");
					return string.Empty;
				}
			}
			return imagePath;
		}

		public AsyncTexture2D GetImageFromIndirectLink(string imagePath)
		{
			return GetImageInternal((async delegate
			{
				string link = await GetDirectImageLink(imagePath);
				return (!string.IsNullOrEmpty(link)) ? (await DownloadWikiContent(link).GetStreamAsync(default(CancellationToken), (HttpCompletionOption)0)) : null;
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
					if (imageStream == null)
					{
						graphicsService.QueueMainThreadRender((Action<GraphicsDevice>)delegate
						{
							texture.SwapTexture(Textures.get_Error());
						});
					}
					else
					{
						graphicsService.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice device)
						{
							try
							{
								texture.SwapTexture(TextureUtil.FromStreamPremultiplied(device, imageStream));
								imageStream.Close();
							}
							catch (Exception ex2)
							{
								logger.Warn(ex2, "Couldn't decode a wiki image; showing the error texture. URL: " + getImageStream.Url);
								graphicsService.QueueMainThreadRender((Action<GraphicsDevice>)delegate
								{
									texture.SwapTexture(Textures.get_Error());
								});
							}
						});
					}
				}
				catch (Exception ex)
				{
					logger.Warn(ex, "Couldn't download a wiki image; showing the error texture. URL: " + getImageStream.Url);
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
