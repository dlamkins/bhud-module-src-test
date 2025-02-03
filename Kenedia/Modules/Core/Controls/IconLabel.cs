using System;
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

		private BitmapFont _font = GameService.Content.DefaultFont14;

		private string _text = string.Empty;

		private DetailedTexture _texture;

		private int _innerPadding = 5;

		private RectangleDimensions _outerPadding = new RectangleDimensions(2);

		private bool _showIcon = true;

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				Common.SetProperty(ref _text, value, new Action(RecalculateLayout));
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
				Common.SetProperty(ref _texture, value, new Action(RecalculateLayout));
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
			get
			{
				return _innerPadding;
			}
			set
			{
				Common.SetProperty(ref _innerPadding, value, new Action(RecalculateLayout));
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
				Common.SetProperty(ref _font, value, new Action(RecalculateLayout));
			}
		}

		public Color FontColor { get; set; } = Color.get_White();


		public bool CaptureInput { get; set; } = true;


		public CaptureType? Capture { get; set; }

		public bool ShowIcon
		{
			get
			{
				return _showIcon;
			}
			set
			{
				Common.SetProperty(ref _showIcon, value, new Action(RecalculateLayout));
			}
		}

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
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			RectangleDimensions p = OuterPadding;
			RectangleF size = Font.GetStringRectangle(Text);
			int imagePadding = ((Texture != null) ? ((Math.Max(Texture.Bounds.Height, (int)size.Height) - p.Vertical - Texture.Bounds.Height) / 2) : 0);
			int textPadding = (Math.Max(Texture?.Bounds.Height ?? 0, (int)size.Height) - p.Vertical - (int)size.Height) / 2;
			_iconBounds = (Rectangle)((!ShowIcon || Texture == null) ? Rectangle.get_Empty() : new Rectangle(p.Left, p.Top + imagePadding, Texture.Bounds.Width, Texture.Bounds.Height));
			_textBounds = new Rectangle(((Rectangle)(ref _iconBounds)).get_Right() + InnerPadding, p.Top + textPadding, (int)size.Width, (int)size.Height);
			_totalBounds = new Rectangle(Point.get_Zero(), new Point(_iconBounds.Width + _textBounds.Width + InnerPadding + p.Right, Math.Max(_iconBounds.Height, _textBounds.Height) + p.Bottom));
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
				spriteBatch.DrawOnCtrl(this, Texture.Texture, _iconBounds, Texture.TextureRegion, (Color)(((_003F?)Texture.DrawColor) ?? Color.get_White()), 0f, default(Vector2), (SpriteEffects)0);
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
	}
}
