using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
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

		private float _backgroundOpacity;

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

		public float BackgroundOpacity
		{
			get
			{
				return _backgroundOpacity;
			}
			set
			{
				((Control)this).SetProperty<float>(ref _backgroundOpacity, value, false, "BackgroundOpacity");
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
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * BackgroundOpacity);
			Size2 size = _font.MeasureString(_text);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _text, _font, new Rectangle((bounds.Width - (int)size.Width) / 2, (bounds.Height - (int)size.Height) / 2, (int)size.Width, (int)size.Height), Color, false, true, 2, (HorizontalAlignment)1, (VerticalAlignment)0);
		}
	}
}
