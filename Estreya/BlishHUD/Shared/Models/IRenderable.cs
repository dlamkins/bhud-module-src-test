using System;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Estreya.BlishHUD.Shared.Models
{
	public interface IRenderable : IDisposable
	{
		RectangleF Render(SpriteBatch spriteBatch, RectangleF bounds);
	}
}
