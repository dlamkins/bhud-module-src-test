using System.Drawing;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Utils
{
	public static class TextureUtil
	{
		public static Image ToImage(this Texture2D texture)
		{
			using MemoryStream ms = new MemoryStream();
			texture.SaveAsPng((Stream)ms, texture.get_Width(), texture.get_Height());
			ms.Seek(0L, SeekOrigin.Begin);
			return Image.FromStream(ms);
		}
	}
}
