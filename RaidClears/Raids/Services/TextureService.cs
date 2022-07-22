using System;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Raids.Services
{
	public class TextureService : IDisposable
	{
		public Texture2D CornerIconTexture { get; }

		public Texture2D CornerIconHoverTexture { get; }

		public TextureService(ContentsManager contentsManager)
		{
			CornerIconTexture = contentsManager.GetTexture("raids\\textures\\raidIconDark.png");
			CornerIconHoverTexture = contentsManager.GetTexture("raids\\textures\\raidIconBright.png");
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
		}
	}
}
