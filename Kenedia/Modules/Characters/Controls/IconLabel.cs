using System;
using System.Runtime.CompilerServices;
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
		private Rectangle _iconRectangle = Rectangle.Empty;

		private Rectangle _textRectangle = Rectangle.Empty;

		public bool CaptureInput { get; set; }

		public string Text
		{
			[CompilerGenerated]
			get
			{
				return _003CText_003Ek__BackingField;
			}
			set
			{
				_003CText_003Ek__BackingField = value;
				UpdateLayout();
			}
		}

		public Color TextColor { get; set; } = Color.White;


		public AsyncTexture2D Icon
		{
			[CompilerGenerated]
			get
			{
				return _003CIcon_003Ek__BackingField;
			}
			set
			{
				_003CIcon_003Ek__BackingField = value;
				if (value != null)
				{
					UpdateLayout();
				}
			}
		}

		public BitmapFont Font
		{
			[CompilerGenerated]
			get
			{
				return _003CFont_003Ek__BackingField;
			}
			set
			{
				_003CFont_003Ek__BackingField = value;
				if (value != null)
				{
					UpdateLayout();
				}
			}
		}

		public bool AutoSizeWidth { get; set; }

		public bool AutoSizeHeight { get; set; }

		public Rectangle TextureRectangle { get; set; }

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			AsyncTexture2D texture = Icon;
			if (texture != null)
			{
				spriteBatch.DrawOnCtrl(this, texture, _iconRectangle, (TextureRectangle == Rectangle.Empty) ? texture.Bounds : TextureRectangle, Color.White, 0f, default(Vector2));
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
			Size2 textbounds = Font.MeasureString(Text);
			if (AutoSizeWidth)
			{
				base.Width = Math.Max((int)textbounds.Width + 4 + ((Icon != null) ? (base.Height + 5) : 0), base.Height);
			}
			if (AutoSizeHeight)
			{
				base.Height = Math.Max((int)textbounds.Height + 4, 0);
			}
			_iconRectangle = ((Icon == null) ? Rectangle.Empty : new Rectangle(2, 2, base.LocalBounds.Height - 4, base.LocalBounds.Height - 4));
			_textRectangle = new Rectangle(_iconRectangle.Right + ((Icon != null) ? 5 : 0), 2, base.LocalBounds.Width - (_iconRectangle.Right + ((Icon != null) ? 5 : 0) + 2), base.LocalBounds.Height - 4);
		}

		public IconLabel()
		{
			_003CFont_003Ek__BackingField = GameService.Content.DefaultFont14;
			TextureRectangle = Rectangle.Empty;
			base._002Ector();
		}
	}
}
