using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Denrage.AchievementTrackerModule.Interfaces;

namespace Denrage.AchievementTrackerModule.Services
{
	public class TextureService : ITextureService, IDisposable
	{
		private readonly ConcurrentDictionary<string, AsyncTexture2D> textures;

		private readonly ConcurrentDictionary<string, AsyncTexture2D> refTextures;

		private readonly ContentService contentService;

		private readonly ContentsManager contentsManager;

		public TextureService(ContentService contentService, ContentsManager contentsManager)
		{
			textures = new ConcurrentDictionary<string, AsyncTexture2D>();
			refTextures = new ConcurrentDictionary<string, AsyncTexture2D>();
			this.contentService = contentService;
			this.contentsManager = contentsManager;
		}

		public AsyncTexture2D GetTexture(string url)
		{
			AsyncTexture2D texture = textures.FirstOrDefault((KeyValuePair<string, AsyncTexture2D> t) => t.Key.Equals(url)).Value;
			if (texture != null)
			{
				return texture;
			}
			texture = contentService.GetRenderServiceTexture(url);
			if (texture != null)
			{
				textures.AddOrUpdate(url, texture, (string key, AsyncTexture2D value) => value = texture);
			}
			return texture;
		}

		public AsyncTexture2D GetRefTexture(string file)
		{
			AsyncTexture2D texture = refTextures.FirstOrDefault((KeyValuePair<string, AsyncTexture2D> t) => t.Key.Equals(file)).Value;
			if (texture != null)
			{
				return texture;
			}
			texture = AsyncTexture2D.op_Implicit(contentsManager.GetTexture(file));
			if (texture != null)
			{
				refTextures.AddOrUpdate(file, texture, (string key, AsyncTexture2D value) => value = texture);
			}
			return texture;
		}

		public void Dispose()
		{
			foreach (KeyValuePair<string, AsyncTexture2D> texture in textures)
			{
				texture.Value.Dispose();
			}
			foreach (KeyValuePair<string, AsyncTexture2D> refTexture in refTextures)
			{
				refTexture.Value.Dispose();
			}
			textures.Clear();
			refTextures.Clear();
		}
	}
}
