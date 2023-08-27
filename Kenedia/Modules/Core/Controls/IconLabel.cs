using System;
using Blish_HUD;
using Blish_HUD.Content;
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

		private BitmapFont _font = GameService.Content.get_DefaultFont14();

		private string _text = string.Empty;

		private DetailedTexture _texture;

		private int _innerPadding = 5;

		private RectangleDimensions _outerPadding = new RectangleDimensions(2);

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				Common.SetProperty(ref _text, value, ((Control)this).RecalculateLayout);
			}
		}

		public DetailedTexture Texture
		{
			get
			{
				return _texture;
			}
			set
			{
				Common.SetProperty(ref _texture, value, ((Control)this).RecalculateLayout);
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
				Common.SetProperty(ref _outerPadding, value, ((Control)this).RecalculateLayout);
			}
		}

		public int InnerPadding
		{
			get
			{
				return _innerPadding;
			}
			set
			{
				Common.SetProperty(ref _innerPadding, value, ((Control)this).RecalculateLayout);
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
				Common.SetProperty(ref _font, value, ((Control)this).RecalculateLayout);
			}
		}

		public Color FontColor { get; set; } = Color.get_White();


		public bool CaptureInput { get; set; }

		public override void RecalculateLayout()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RecalculateLayout();
			RectangleDimensions p = OuterPadding;
			RectangleF size = Font.GetStringRectangle(Text);
			int imagePadding = ((Texture != null) ? ((Math.Max(Texture.Bounds.Height, (int)size.Height) - p.Vertical - Texture.Bounds.Height) / 2) : 0);
			int textPadding = (Math.Max(Texture?.Bounds.Height ?? 0, (int)size.Height) - p.Vertical - (int)size.Height) / 2;
			_iconBounds = (Rectangle)((Texture == null) ? Rectangle.get_Empty() : new Rectangle(p.Left, p.Top + imagePadding, Texture.Bounds.Width, Texture.Bounds.Height));
			_textBounds = new Rectangle(((Rectangle)(ref _iconBounds)).get_Right() + InnerPadding, p.Top + textPadding, (int)size.Width, (int)size.Height);
			_totalBounds = new Rectangle(Point.get_Zero(), new Point(_iconBounds.Width + _textBounds.Width + InnerPadding + p.Right, Math.Max(_iconBounds.Height, _textBounds.Height) + p.Bottom));
			if (AutoSize)
			{
				if (((Control)this).get_Width() != _totalBounds.Width)
				{
					((Control)this).set_Width(_totalBounds.Width);
				}
				if (((Control)this).get_Height() != _totalBounds.Height)
				{
					((Control)this).set_Height(_totalBounds.Height);
				}
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			if (Texture != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(Texture.Texture), _iconBounds, (Rectangle?)Texture.TextureRegion, (Color)(((_003F?)Texture.DrawColor) ?? Color.get_White()), 0f, default(Vector2), (SpriteEffects)0);
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Text, Font, _textBounds, FontColor, false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		protected override CaptureType CapturesInput()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (!CaptureInput)
			{
				return (CaptureType)0;
			}
			return ((Control)this).CapturesInput();
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			Texture = null;
		}

		public IconLabel()
			: this()
		{
		}//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)

	}
}
