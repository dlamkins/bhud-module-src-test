using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;

namespace Blish_HUD.Extended
{
	public class BitmapFontEx : BitmapFont, IDisposable
	{
		private readonly Texture2D _texture;

		public BitmapFontEx(string name, IEnumerable<BitmapFontRegion> regions, int lineHeight, Texture2D texture)
			: this(name, regions, lineHeight)
		{
			_texture = texture ?? throw new ArgumentNullException("texture");
		}

		public BitmapFontEx(string name, IReadOnlyList<BitmapFontRegion> regions, int lineHeight)
			: this(name, (IEnumerable<BitmapFontRegion>)regions, lineHeight)
		{
			object obj;
			if (regions == null)
			{
				obj = null;
			}
			else
			{
				BitmapFontRegion obj2 = regions.FirstOrDefault();
				if (obj2 == null)
				{
					obj = null;
				}
				else
				{
					TextureRegion2D textureRegion = obj2.get_TextureRegion();
					obj = ((textureRegion != null) ? textureRegion.get_Texture() : null);
				}
			}
			if (obj == null)
			{
				throw new ArgumentException("Parameter 'regions' was null or empty.");
			}
			_texture = (Texture2D)obj;
		}

		public void Dispose()
		{
			Texture2D texture = _texture;
			if (texture != null)
			{
				((GraphicsResource)texture).Dispose();
			}
		}
	}
}
