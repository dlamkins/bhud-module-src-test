using System.Collections.Generic;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.Mounts
{
	public class TextureCache
	{
		private readonly ContentsManager contentsManager;

		private readonly Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();

		public TextureCache(ContentsManager contentsManager)
		{
			this.contentsManager = contentsManager;
			PreCacheTextures();
		}

		private void PreCacheTextures()
		{
			foreach (Mount mount in Module._mounts)
			{
				string[] mountDisplay2 = Module._mountDisplay;
				foreach (string mountDisplay in mountDisplay2)
				{
					string textureName = GetTextureName(mount.ImageFileName, mountDisplay);
					if (!_textureCache.ContainsKey(textureName))
					{
						_textureCache[textureName] = contentsManager.GetTexture(textureName);
					}
				}
			}
		}

		public Texture2D GetImgFile(string filename)
		{
			string textureName = GetTextureName(filename, Module._settingDisplay.get_Value());
			return _textureCache[textureName];
		}

		private static string GetTextureName(string filename, string displaySetting)
		{
			string textureName = filename;
			return displaySetting switch
			{
				"Transparent" => textureName + "-trans.png", 
				"SolidText" => textureName + "-text.png", 
				_ => textureName + ".png", 
			};
		}
	}
}
