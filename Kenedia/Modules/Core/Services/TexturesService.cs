using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Services
{
	public static class TexturesService
	{
		private static readonly Dictionary<string, Texture2D> s_loadedTextures = new Dictionary<string, Texture2D>();

		private static ContentsManager s_contentsManager;

		public static void Initilize(ContentsManager contentsManager)
		{
			s_contentsManager = contentsManager;
		}

		public static void Dispose()
		{
			if (s_loadedTextures.Count > 0)
			{
				((IEnumerable<IDisposable>)s_loadedTextures.Select((KeyValuePair<string, Texture2D> e) => e.Value)).DisposeAll();
			}
			s_loadedTextures.Clear();
		}

		public static Texture2D GetTextureFromRef(string path, string key)
		{
			if (s_contentsManager != null)
			{
				if (s_loadedTextures.ContainsKey(key))
				{
					return s_loadedTextures[key];
				}
				Texture2D texture = s_contentsManager.GetTexture(path);
				s_loadedTextures.Add(key, texture);
				return texture;
			}
			return null;
		}

		public static Texture2D GetTextureFromRef(Bitmap bitmap, string key)
		{
			if (s_loadedTextures.ContainsKey(key))
			{
				return s_loadedTextures[key];
			}
			Texture2D texture;
			s_loadedTextures.Add(key, texture = bitmap.CreateTexture2D());
			return texture;
		}

		public static AsyncTexture2D GetTextureFromDisk(string path)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			if (s_loadedTextures.ContainsKey(path))
			{
				return AsyncTexture2D.op_Implicit(s_loadedTextures[path]);
			}
			AsyncTexture2D texture = new AsyncTexture2D();
			if (File.Exists(path))
			{
				GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice graphicsDevice)
				{
					texture.SwapTexture(TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)));
				});
			}
			return texture;
		}
	}
}
