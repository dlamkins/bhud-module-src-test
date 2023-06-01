using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.Mounts
{
	public class TextureCache
	{
		private readonly ContentsManager contentsManager;

		private readonly Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();

		public static readonly string MouseTextureName = "255329.png";

		public static readonly string MountLogoTextureName = "514394-grey.png";

		public static readonly string TabBackgroundTextureName = "156006-big.png";

		public static readonly string SettingsIconTextureName = "155052.png";

		public static readonly string AnetIconTextureName = "1441452.png";

		public TextureCache(ContentsManager contentsManager)
		{
			this.contentsManager = contentsManager;
			PreCacheTextures();
		}

		private void PreCacheTextures()
		{
			Func<string, Texture2D> getTextureFromRef = (string textureName) => contentsManager.GetTexture(textureName);
			foreach (MountImageFile mountImageFile in Module._mountImageFiles)
			{
				PreCacheTexture(mountImageFile.Name, PremultiplyTexture);
			}
			PreCacheTexture(MouseTextureName, getTextureFromRef);
			PreCacheTexture(MountLogoTextureName, getTextureFromRef);
			PreCacheTexture(TabBackgroundTextureName, getTextureFromRef);
			PreCacheTexture(SettingsIconTextureName, getTextureFromRef);
			PreCacheTexture(AnetIconTextureName, getTextureFromRef);
		}

		private Texture2D PremultiplyTexture(string textureName)
		{
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				FileStream titleStream = File.OpenRead(Path.Combine(Module.mountsDirectory, textureName));
				Texture2D texture = Texture2D.FromStream(GameService.Graphics.get_GraphicsDevice(), (Stream)titleStream);
				titleStream.Close();
				Color[] buffer = (Color[])(object)new Color[texture.get_Width() * texture.get_Height()];
				texture.GetData<Color>(buffer);
				for (int i = 0; i < buffer.Length; i++)
				{
					buffer[i] = Color.FromNonPremultiplied((int)((Color)(ref buffer[i])).get_R(), (int)((Color)(ref buffer[i])).get_G(), (int)((Color)(ref buffer[i])).get_B(), (int)((Color)(ref buffer[i])).get_A());
				}
				texture.SetData<Color>(buffer);
				return texture;
			}
			catch
			{
				return Textures.get_Error();
			}
		}

		private void PreCacheTexture(string textureName, Func<string, Texture2D> getTextureAction)
		{
			if (!_textureCache.ContainsKey(textureName))
			{
				_textureCache[textureName] = getTextureAction(textureName);
			}
		}

		public Texture2D GetImgFile(string filename)
		{
			return GetTexture(filename);
		}

		public Texture2D GetMountImgFile(Mount mount)
		{
			return GetTexture(mount.ImageFileNameSetting.get_Value());
		}

		private Texture2D GetTexture(string filename)
		{
			if (_textureCache.ContainsKey(filename))
			{
				return _textureCache[filename];
			}
			return Textures.get_Error();
		}
	}
}
