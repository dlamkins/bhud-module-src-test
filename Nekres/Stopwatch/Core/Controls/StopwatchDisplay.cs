using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Stopwatch;

namespace Nekres.Stopwatch.Core.Controls
{
	internal class StopwatchDisplay : Control
	{
		private BitmapFont _font;

		private string _text;

		private FontSize _fontSize;

		private Color _color;

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _text, value, false, "Text");
			}
		}

		public FontSize FontSize
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _fontSize;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				if (((Control)this).SetProperty<FontSize>(ref _fontSize, value, false, "FontSize"))
				{
					_font = Control.get_Content().GetFont((FontFace)0, value, (FontStyle)0);
				}
			}
		}

		public Color Color
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _color;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _color, value, false, "Color");
			}
		}

		public StopwatchDisplay()
			: this()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			_font = Control.get_Content().GetFont((FontFace)0, StopwatchModule.ModuleInstance.FontSize.get_Value(), (FontStyle)0);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)1;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _text, _font, bounds, Color, false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)1);
		}
	}
}
