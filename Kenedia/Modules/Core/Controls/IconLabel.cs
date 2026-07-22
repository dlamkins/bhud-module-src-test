using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Core.Controls
{
	internal class IconLabel : Control
	{
		private Rectangle _iconBounds;

		private Rectangle _textBounds;

		private Rectangle _totalBounds;

		public bool AutoSize;

		private RectangleDimensions _outerPadding = new RectangleDimensions(2);

		public string Text
		{
			[CompilerGenerated]
			get
			{
				return _003CText_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CText_003Ek__BackingField, value, delegate(string v)
				{
					_003CText_003Ek__BackingField = v;
				}, new Action(RecalculateLayout));
			}
		}

		public DetailedTexture Texture
		{
			[CompilerGenerated]
			get
			{
				return _003CTexture_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CTexture_003Ek__BackingField, value, delegate(DetailedTexture v)
				{
					_003CTexture_003Ek__BackingField = v;
				}, new Action(RecalculateLayout));
			}
		}

		public RectangleDimensions OuterPadding
		{
			get
			{
				return _outerPadding;
			}
			set
			{
				Common.SetProperty(ref _outerPadding, value, new Action(RecalculateLayout));
			}
		}

		public int InnerPadding
		{
			[CompilerGenerated]
			get
			{
				return _003CInnerPadding_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CInnerPadding_003Ek__BackingField, value, delegate(int v)
				{
					_003CInnerPadding_003Ek__BackingField = v;
				}, new Action(RecalculateLayout));
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
				Common.SetProperty(_003CFont_003Ek__BackingField, value, delegate(BitmapFont v)
				{
					_003CFont_003Ek__BackingField = v;
				}, new Action(RecalculateLayout));
			}
		}

		public Color FontColor { get; set; }

		public bool CaptureInput { get; set; }

		public CaptureType? Capture { get; set; }

		public bool ShowIcon
		{
			[CompilerGenerated]
			get
			{
				return _003CShowIcon_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CShowIcon_003Ek__BackingField, value, delegate(bool v)
				{
					_003CShowIcon_003Ek__BackingField = v;
				}, new Action(RecalculateLayout));
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			RectangleDimensions p = OuterPadding;
			RectangleF size = Font.GetStringRectangle(Text);
			int imagePadding = ((Texture != null) ? ((Math.Max(Texture.Bounds.Height, (int)size.Height) - p.Vertical - Texture.Bounds.Height) / 2) : 0);
			int textPadding = (Math.Max(Texture?.Bounds.Height ?? 0, (int)size.Height) - p.Vertical - (int)size.Height) / 2;
			_iconBounds = ((!ShowIcon || Texture == null) ? Rectangle.Empty : new Rectangle(p.Left, p.Top + imagePadding, Texture.Bounds.Width, Texture.Bounds.Height));
			_textBounds = new Rectangle(_iconBounds.Right + InnerPadding, p.Top + textPadding, (int)size.Width, (int)size.Height);
			_totalBounds = new Rectangle(Point.Zero, new Point(_iconBounds.Width + _textBounds.Width + InnerPadding + p.Right, Math.Max(_iconBounds.Height, _textBounds.Height) + p.Bottom));
			if (AutoSize)
			{
				if (base.Width != _totalBounds.Width)
				{
					base.Width = _totalBounds.Width;
				}
				if (base.Height != _totalBounds.Height)
				{
					base.Height = _totalBounds.Height;
				}
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			base.DoUpdate(gameTime);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (Texture != null)
			{
				spriteBatch.DrawOnCtrl(this, Texture.Texture, _iconBounds, Texture.TextureRegion, Texture.DrawColor ?? Color.White, 0f, default(Vector2));
			}
			spriteBatch.DrawStringOnCtrl(this, Text, Font, _textBounds, FontColor, wrap: false, stroke: true);
		}

		protected override CaptureType CapturesInput()
		{
			CaptureType? capture = Capture;
			if (!capture.HasValue)
			{
				if (!CaptureInput)
				{
					return CaptureType.None;
				}
				return base.CapturesInput();
			}
			return capture.GetValueOrDefault();
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Texture = null;
		}

		public IconLabel()
		{
			_003CText_003Ek__BackingField = string.Empty;
			_003CInnerPadding_003Ek__BackingField = 5;
			_003CFont_003Ek__BackingField = GameService.Content.DefaultFont14;
			FontColor = Color.White;
			CaptureInput = true;
			_003CShowIcon_003Ek__BackingField = true;
			base._002Ector();
		}
	}
}
