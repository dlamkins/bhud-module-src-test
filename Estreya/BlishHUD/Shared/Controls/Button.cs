using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.Shared.Controls
{
	public class Button : LabelBase
	{
		public const int STANDARD_CONTROL_HEIGHT = 26;

		public const int DEFAULT_CONTROL_WIDTH = 128;

		private const int ICON_SIZE = 16;

		private const int ICON_TEXT_OFFSET = 4;

		private const int ATLAS_SPRITE_WIDTH = 350;

		private const int ATLAS_SPRITE_HEIGHT = 20;

		private const int ANIM_FRAME_COUNT = 8;

		private const float ANIM_FRAME_TIME = 0.25f;

		private static readonly Texture2D _textureButtonIdle = Control.get_Content().GetTexture("common/button-states");

		private static readonly Texture2D _textureButtonBorder = Control.get_Content().GetTexture("button-border");

		private Tween _animIn;

		private Tween _animOut;

		private AsyncTexture2D _icon;

		private Rectangle _layoutIconBounds = Rectangle.get_Empty();

		private Rectangle _layoutTextBounds = Rectangle.get_Empty();

		private bool _resizeIcon;

		public string Text
		{
			get
			{
				return base._text;
			}
			set
			{
				((Control)this).SetProperty<string>(ref base._text, value, true, "Text");
			}
		}

		public AsyncTexture2D Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				((Control)this).SetProperty<AsyncTexture2D>(ref _icon, value, true, "Icon");
			}
		}

		public bool ResizeIcon
		{
			get
			{
				return _resizeIcon;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _resizeIcon, value, true, "ResizeIcon");
			}
		}

		public BitmapFont Font
		{
			get
			{
				return base._font;
			}
			set
			{
				((Control)this).SetProperty<BitmapFont>(ref base._font, value, true, "Font");
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public int AnimationState { get; set; }

		public Button()
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			base._textColor = Color.get_Black();
			base._horizontalAlignment = (HorizontalAlignment)0;
			base._verticalAlignment = (VerticalAlignment)1;
			((Control)this).set_Size(new Point(128, 26));
		}

		private void TriggerAnimation(bool directionIn)
		{
			Tween animIn = _animIn;
			if (animIn != null)
			{
				animIn.Pause();
			}
			Tween animOut = _animOut;
			if (animOut != null)
			{
				animOut.Pause();
			}
			if (directionIn)
			{
				Tweener tweener = GameService.Animation.get_Tweener();
				var anon = new
				{
					AnimationState = 8
				};
				Tween animOut2 = _animOut;
				_animIn = ((TweenerImpl)tweener).Tween<Button>(this, (object)anon, 0.25f - ((animOut2 != null) ? animOut2.get_TimeRemaining() : 0f), 0f, true);
			}
			else
			{
				Tweener tweener2 = GameService.Animation.get_Tweener();
				var anon2 = new
				{
					AnimationState = 0
				};
				Tween animIn2 = _animIn;
				_animOut = ((TweenerImpl)tweener2).Tween<Button>(this, (object)anon2, 0.25f - ((animIn2 != null) ? animIn2.get_TimeRemaining() : 0f), 0f, true);
			}
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			TriggerAnimation(directionIn: true);
			((Control)this).OnMouseEntered(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			TriggerAnimation(directionIn: false);
			((Control)this).OnMouseLeft(e);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			Control.get_Content().PlaySoundEffectByName("audio\\button-click");
			((Control)this).OnClick(e);
		}

		public override void RecalculateLayout()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			Size2 textDimensions = ((LabelBase)this).GetTextDimensions((string)null);
			int num = (int)((float)(((Control)this)._size.X / 2) - textDimensions.Width / 2f);
			if (_icon != null)
			{
				num = ((!(textDimensions.Width > 0f)) ? (num + 8) : (num + 10));
				_003F val;
				if (!_resizeIcon)
				{
					Rectangle bounds = _icon.get_Texture().get_Bounds();
					val = ((Rectangle)(ref bounds)).get_Size();
				}
				else
				{
					val = new Point(16);
				}
				Point point = (Point)val;
				_layoutIconBounds = new Rectangle(num - point.X - 4, ((Control)this)._size.Y / 2 - point.Y / 2, point.X, point.Y);
			}
			_layoutTextBounds = new Rectangle(num, 0, ((Control)this)._size.X - num, ((Control)this)._size.Y);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this)._enabled)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureButtonIdle, new Rectangle(3, 3, ((Control)this)._size.X - 6, ((Control)this)._size.Y - 5), (Rectangle?)new Rectangle(AnimationState * 350, 0, 350, 20));
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(3, 3, ((Control)this)._size.X - 6, ((Control)this)._size.Y - 5), Color.FromNonPremultiplied(121, 121, 121, 255));
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureButtonBorder, new Rectangle(2, 0, ((Control)this).get_Width() - 5, 4), (Rectangle?)new Rectangle(0, 0, 1, 4));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureButtonBorder, new Rectangle(((Control)this).get_Width() - 4, 2, 4, ((Control)this).get_Height() - 3), (Rectangle?)new Rectangle(0, 1, 4, 1));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureButtonBorder, new Rectangle(3, ((Control)this).get_Height() - 4, ((Control)this).get_Width() - 6, 4), (Rectangle?)new Rectangle(1, 0, 1, 4));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureButtonBorder, new Rectangle(0, 2, 4, ((Control)this).get_Height() - 3), (Rectangle?)new Rectangle(0, 3, 4, 1));
			if (_icon != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon), _layoutIconBounds);
			}
			base._textColor = (((Control)this)._enabled ? Color.get_Black() : Color.FromNonPremultiplied(51, 51, 51, 255));
			((LabelBase)this).DrawText(spriteBatch, _layoutTextBounds, (string)null);
		}
	}
}
