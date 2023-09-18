using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.Regions_Of_Tyria
{
	internal class BitmapFont : BitmapFont, IDisposable
	{
		private readonly Texture2D _texture;

		public BitmapFont(string name, IEnumerable<BitmapFontRegion> regions, int lineHeight, Texture2D texture)
			: this(name, regions, lineHeight)
		{
			_texture = texture;
		}

		public BitmapFont(string name, IReadOnlyList<BitmapFontRegion> regions, int lineHeight)
			: this(name, (IEnumerable<BitmapFontRegion>)regions, lineHeight)
		{
			_texture = regions[0].get_TextureRegion().get_Texture();
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
