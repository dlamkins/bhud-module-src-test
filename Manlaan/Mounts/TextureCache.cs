using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Blish_HUD.Graphics;
using Blish_HUD.Modules.Managers;
using Manlaan.Mounts.Things;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.Mounts
{
	public class TextureCache
	{
		private readonly ContentsManager contentsManager;

		private readonly Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();

		public static readonly string MouseTextureName = "255329.png";

		public static readonly string MountLogoTextureName = "514394-grey-plus-plus100.png";

		public static readonly string TabBackgroundTextureName = "156006-big.png";

		public static readonly string SettingsTextureName = "155052.png";

		public static readonly string RadialSettingsTextureName = "1130623-32.png";

		public static readonly string IconSettingsTextureName = "2208345.png";

		public static readonly string SupportMeTabTextureName = "156127-32-grey.png";

		public static readonly string KofiTextureName = "kofi-small.png";

		public static readonly string AnetIconTextureName = "1441452.png";

		public TextureCache(ContentsManager contentsManager)
		{
			this.contentsManager = contentsManager;
			PreCacheTextures();
		}

		private void PreCacheTextures()
		{
			Func<string, Texture2D> getTextureFromRef = (string textureName) => contentsManager.GetTexture(textureName);
			foreach (ThingImageFile mountImageFile in Module._thingImageFiles)
			{
				PreCacheTexture(mountImageFile.Name, PremultiplyTexture);
			}
			PreCacheTexture(MouseTextureName, getTextureFromRef);
			PreCacheTexture(MountLogoTextureName, getTextureFromRef);
			PreCacheTexture(TabBackgroundTextureName, getTextureFromRef);
			PreCacheTexture(SettingsTextureName, getTextureFromRef);
			PreCacheTexture(RadialSettingsTextureName, getTextureFromRef);
			PreCacheTexture(IconSettingsTextureName, getTextureFromRef);
			PreCacheTexture(SupportMeTabTextureName, getTextureFromRef);
			PreCacheTexture(KofiTextureName, getTextureFromRef);
			PreCacheTexture(AnetIconTextureName, getTextureFromRef);
		}

		private Texture2D PremultiplyTexture(string textureName)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				using FileStream titleStream = File.OpenRead(Path.Combine(Module.mountsDirectory, textureName));
				GraphicsDeviceContext gdc = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					Texture2D texture = Texture2D.FromStream(((GraphicsDeviceContext)(ref gdc)).get_GraphicsDevice(), (Stream)titleStream);
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
				finally
				{
					((GraphicsDeviceContext)(ref gdc)).Dispose();
				}
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

		public Texture2D GetMountImgFile(Thing thing)
		{
			return GetTexture(thing.ImageFileNameSetting.get_Value());
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
