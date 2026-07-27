using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frtal.LorebookReader
{
	public sealed class CodexWindow : StandardWindow
	{
		private const int MinWidth = 640;

		private const int MinHeight = 480;

		public CodexWindow(Texture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
			: this(background, windowRegion, contentRegion, windowSize)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)


		protected override Point HandleWindowResize(Point newSize)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			Screen screen = GameService.Graphics.get_SpriteScreen();
			int maxW = Math.Max(640, (screen != null) ? ((Control)screen).get_Width() : 640);
			int maxH = Math.Max(480, (screen != null) ? ((Control)screen).get_Height() : 480);
			return new Point(MathHelper.Clamp(newSize.X, 640, maxW), MathHelper.Clamp(newSize.Y, 480, maxH));
		}
	}
}
