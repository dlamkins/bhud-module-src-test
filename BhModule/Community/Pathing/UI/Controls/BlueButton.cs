using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace BhModule.Community.Pathing.UI.Controls
{
	internal class BlueButton : LabelBase
	{
		public const int STANDARD_CONTROL_HEIGHT = 26;

		public const int DEFAULT_CONTROL_WIDTH = 128;

		private const int ICON_SIZE = 16;

		private const int ICON_TEXT_OFFSET = 4;

		private const int ATLAS_SPRITE_WIDTH = 350;

		private const int ATLAS_SPRITE_HEIGHT = 20;

		private const int ANIM_FRAME_COUNT = 8;

		private const float ANIM_FRAME_TIME = 0.25f;

		private static readonly Texture2D _textureButtonIdle;

		private static readonly Texture2D _textureButtonBorder;

		private AsyncTexture2D _icon;

		private bool _resizeIcon;

		private Tween _animIn;

		private Tween _animOut;

		private Rectangle _layoutIconBounds = Rectangle.get_Empty();

		private Rectangle _layoutTextBounds = Rectangle.get_Empty();

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

		[EditorBrowsable(EditorBrowsableState.Never)]
		public int AnimationState { get; set; }

		static BlueButton()
		{
			_textureButtonIdle = Control.get_Content().GetTexture("common/button-states");
			_textureButtonBorder = Control.get_Content().GetTexture("button-border");
		}

		public BlueButton()
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
				_animIn = ((TweenerImpl)tweener).Tween<BlueButton>(this, (object)anon, 0.25f - ((animOut2 != null) ? animOut2.get_TimeRemaining() : 0f), 0f, true);
			}
			else
			{
				Tweener tweener2 = GameService.Animation.get_Tweener();
				var anon2 = new
				{
					AnimationState = 0
				};
				Tween animIn2 = _animIn;
				_animOut = ((TweenerImpl)tweener2).Tween<BlueButton>(this, (object)anon2, 0.25f - ((animIn2 != null) ? animIn2.get_TimeRemaining() : 0f), 0f, true);
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
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			Size2 textSize = ((LabelBase)this).GetTextDimensions((string)null);
			int textLeft = (int)((float)(((Control)this)._size.X / 2) - textSize.Width / 2f);
			if (_icon != null)
			{
				textLeft = ((!(textSize.Width > 0f)) ? (textLeft + 8) : (textLeft + 10));
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
				Point iconSize = (Point)val;
				_layoutIconBounds = new Rectangle(textLeft - iconSize.X - 4, ((Control)this)._size.Y / 2 - iconSize.Y / 2, iconSize.X, iconSize.Y);
			}
			_layoutTextBounds = new Rectangle(textLeft, 0, ((Control)this)._size.X - textLeft, ((Control)this)._size.Y);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this)._enabled)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureButtonIdle, new Rectangle(3, 3, ((Control)this)._size.X - 6, ((Control)this)._size.Y - 5), (Rectangle?)new Rectangle(AnimationState * 350, 0, 350, 20), Color.FromNonPremultiplied(123, 157, 192, 255));
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(3, 3, ((Control)this)._size.X - 6, ((Control)this)._size.Y - 5), Color.FromNonPremultiplied(73, 88, 111, 255));
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureButtonBorder, new Rectangle(2, 0, ((Control)this).get_Width() - 5, 4), (Rectangle?)new Rectangle(0, 0, 1, 4));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureButtonBorder, new Rectangle(((Control)this).get_Width() - 4, 2, 4, ((Control)this).get_Height() - 3), (Rectangle?)new Rectangle(0, 1, 4, 1));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureButtonBorder, new Rectangle(3, ((Control)this).get_Height() - 4, ((Control)this).get_Width() - 6, 4), (Rectangle?)new Rectangle(1, 0, 1, 4));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureButtonBorder, new Rectangle(0, 2, 4, ((Control)this).get_Height() - 3), (Rectangle?)new Rectangle(0, 3, 4, 1));
			if (_icon != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon), _layoutIconBounds);
			}
			base._textColor = (((Control)this)._enabled ? StandardColors.get_Default() : Color.FromNonPremultiplied(51, 51, 51, 255));
			((LabelBase)this).DrawText(spriteBatch, _layoutTextBounds, (string)null);
		}
	}
}
