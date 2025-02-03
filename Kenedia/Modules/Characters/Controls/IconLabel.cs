using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Characters.Controls
{
	public class IconLabel : Control, IFontControl
	{
		private AsyncTexture2D _icon;

		private BitmapFont _font = GameService.Content.DefaultFont14;

		private string _text;

		private Rectangle _iconRectangle = Rectangle.get_Empty();

		private Rectangle _textRectangle = Rectangle.get_Empty();

		public bool CaptureInput { get; set; }

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
				UpdateLayout();
			}
		}

		public Color TextColor { get; set; } = Color.get_White();


		public AsyncTexture2D Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				_icon = value;
				if (value != null)
				{
					UpdateLayout();
				}
			}
		}

		public BitmapFont Font
		{
			get
			{
				return _font;
			}
			set
			{
				_font = value;
				if (value != null)
				{
					UpdateLayout();
				}
			}
		}

		public bool AutoSizeWidth { get; set; }

		public bool AutoSizeHeight { get; set; }

		public Rectangle TextureRectangle { get; set; } = Rectangle.get_Empty();


		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D texture = Icon;
			if (texture != null)
			{
				spriteBatch.DrawOnCtrl(this, texture, _iconRectangle, (TextureRectangle == Rectangle.get_Empty()) ? texture.Bounds : TextureRectangle, Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
			spriteBatch.DrawStringOnCtrl(this, Text, Font, _textRectangle, TextColor);
		}

		protected override CaptureType CapturesInput()
		{
			if (!CaptureInput)
			{
				return CaptureType.None;
			}
			return base.CapturesInput();
		}

		private void UpdateLayout()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			Size2 textbounds = Font.MeasureString(Text);
			if (AutoSizeWidth)
			{
				base.Width = Math.Max((int)textbounds.Width + 4 + ((Icon != null) ? (base.Height + 5) : 0), base.Height);
			}
			if (AutoSizeHeight)
			{
				base.Height = Math.Max((int)textbounds.Height + 4, 0);
			}
			_iconRectangle = (Rectangle)((Icon == null) ? Rectangle.get_Empty() : new Rectangle(2, 2, base.LocalBounds.Height - 4, base.LocalBounds.Height - 4));
			_textRectangle = new Rectangle(((Rectangle)(ref _iconRectangle)).get_Right() + ((Icon != null) ? 5 : 0), 2, base.LocalBounds.Width - (((Rectangle)(ref _iconRectangle)).get_Right() + ((Icon != null) ? 5 : 0) + 2), base.LocalBounds.Height - 4);
		}
	}
}
