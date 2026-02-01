using System;
using Blish_HUD.Controls;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.Shared.Controls
{
	public class UpdatingTextControl : Control
	{
		private readonly Func<string> _getText;

		private readonly Func<BitmapFont> _getFont;

		private readonly Func<Color> _getColor;

		public HorizontalAlignment HorizontalAlignment { get; set; }

		public VerticalAlignment VerticalAlignment { get; set; }

		public UpdatingTextControl(Func<string> getText, Func<BitmapFont> getFont, Func<Color> getColor)
			: this()
		{
			_getText = getText;
			_getFont = getFont;
			_getColor = getColor;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			string text = _getText();
			BitmapFont font = _getFont();
			Color color = _getColor() * ((Control)this).AbsoluteOpacity();
			spriteBatch.DrawStringOnCtrl((Control)(object)this, text, font, RectangleF.op_Implicit(bounds), color, wrap: false, 1f, HorizontalAlignment, VerticalAlignment);
		}
	}
}
