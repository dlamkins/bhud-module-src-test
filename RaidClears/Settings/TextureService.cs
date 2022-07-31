using System;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Settings
{
	public class TextureService : IDisposable
	{
		public Texture2D CornerIconTexture { get; }

		public Texture2D CornerIconHoverTexture { get; }

		public Texture2D DungeonsCornerIconTexture { get; }

		public Texture2D DungeonsCornerIconHoverTexture { get; }

		public TextureService(ContentsManager contentsManager)
		{
			CornerIconTexture = contentsManager.GetTexture("raids\\textures\\raidIconDark.png");
			CornerIconHoverTexture = contentsManager.GetTexture("raids\\textures\\raidIconBright.png");
			DungeonsCornerIconTexture = contentsManager.GetTexture("raids\\textures\\dungeonIconDark.png");
			DungeonsCornerIconHoverTexture = contentsManager.GetTexture("raids\\textures\\dungeonIconBright.png");
		}

		public void Dispose()
		{
			Texture2D cornerIconTexture = CornerIconTexture;
			if (cornerIconTexture != null)
			{
				((GraphicsResource)cornerIconTexture).Dispose();
			}
			Texture2D cornerIconHoverTexture = CornerIconHoverTexture;
			if (cornerIconHoverTexture != null)
			{
				((GraphicsResource)cornerIconHoverTexture).Dispose();
			}
			Texture2D dungeonsCornerIconTexture = DungeonsCornerIconTexture;
			if (dungeonsCornerIconTexture != null)
			{
				((GraphicsResource)dungeonsCornerIconTexture).Dispose();
			}
			Texture2D dungeonsCornerIconHoverTexture = DungeonsCornerIconHoverTexture;
			if (dungeonsCornerIconHoverTexture != null)
			{
				((GraphicsResource)dungeonsCornerIconHoverTexture).Dispose();
			}
		}
	}
}
