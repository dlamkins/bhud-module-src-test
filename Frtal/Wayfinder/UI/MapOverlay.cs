using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Frtal.Wayfinder.UI
{
	public class MapOverlay : Control
	{
		public struct Dot
		{
			public Vector2 Pos;

			public Color Color;

			public Dot(Vector2 pos, Color color)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				Pos = pos;
				Color = color;
			}
		}

		private readonly BitmapFont _font = GameService.Content.get_DefaultFont14();

		private IReadOnlyList<Dot> _dots = new List<Dot>();

		public string DebugText { get; set; }

		public MapOverlay()
			: this()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_ZIndex(5);
			((Control)this).set_Location(Point.get_Zero());
			((Control)this).set_Size(((Control)GameService.Graphics.get_SpriteScreen()).get_Size());
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public void SetDots(IReadOnlyList<Dot> dots)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			_dots = dots ?? new List<Dot>();
			((Control)this).set_Size(((Control)GameService.Graphics.get_SpriteScreen()).get_Size());
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			Texture2D pixel = Textures.get_Pixel();
			foreach (Dot d in _dots)
			{
				int x = (int)d.Pos.X;
				int y = (int)d.Pos.Y;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(x - 5, y, 11, 1), d.Color);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(x, y - 5, 1, 11), d.Color);
			}
			int cx = bounds.Width / 2;
			int cy = bounds.Height / 2;
			Color magenta = default(Color);
			((Color)(ref magenta))._002Ector(255, 0, 255);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(cx - 12, cy, 25, 1), magenta);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, new Rectangle(cx, cy - 12, 1, 25), magenta);
			if (!string.IsNullOrEmpty(DebugText))
			{
				int tw = 760;
				int th = 24;
				Rectangle r = default(Rectangle);
				((Rectangle)(ref r))._002Ector(bounds.Width / 2 - tw / 2, bounds.Height - th - 48, tw, th);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, pixel, r, new Color(0, 0, 0) * 0.8f);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, DebugText, GameService.Content.get_DefaultFont16(), r, new Color(255, 240, 120), false, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
		}
	}
}
