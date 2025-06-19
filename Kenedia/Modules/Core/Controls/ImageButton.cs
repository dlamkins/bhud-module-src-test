using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.Core.Controls
{
	public class ImageButton : Control, ILocalizable
	{
		private Func<string> _setLocalizedTooltip;

		private bool Clicked
		{
			get
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Invalid comparison between Unknown and I4
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Invalid comparison between Unknown and I4
				if (base.MouseOver)
				{
					MouseState state = Control.Input.Mouse.State;
					if ((int)((MouseState)(ref state)).get_LeftButton() != 1)
					{
						state = Control.Input.Mouse.State;
						return (int)((MouseState)(ref state)).get_RightButton() == 1;
					}
					return true;
				}
				return false;
			}
		}

		public Action<MouseEventArgs> ClickAction { get; set; }

		public Color? ColorHovered { get; set; }

		public Color? ColorClicked { get; set; }

		public Color? ImageColor { get; set; } = Color.get_White();


		public Rectangle? SizeRectangle { get; set; }

		public Rectangle? TextureRectangle { get; set; }

		public AsyncTexture2D Texture { get; set; }

		public AsyncTexture2D DisabledTexture { get; set; }

		public AsyncTexture2D HoveredTexture { get; set; }

		public AsyncTexture2D ClickedTexture { get; set; }

		public bool ShowButton { get; set; }

		public bool ShowImageFrame { get; set; }

		public float? TextureRotation { get; set; }

		public Func<string> SetLocalizedTooltip
		{
			get
			{
				return _setLocalizedTooltip;
			}
			set
			{
				_setLocalizedTooltip = value;
				base.BasicTooltipText = value?.Invoke();
			}
		}

		public ImageButton()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			LocalizingService.LocaleChanged += new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			UserLocale_SettingChanged(null, null);
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedTooltip != null)
			{
				base.BasicTooltipText = SetLocalizedTooltip?.Invoke();
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Texture = null;
			DisabledTexture = null;
			HoveredTexture = null;
			ClickedTexture = null;
			GameService.Overlay.UserLocale.SettingChanged -= UserLocale_SettingChanged;
		}

		private AsyncTexture2D GetTexture()
		{
			if (base.Enabled || DisabledTexture == null)
			{
				if (!Clicked || ClickedTexture == null)
				{
					if (!base.MouseOver || HoveredTexture == null)
					{
						return Texture;
					}
					return HoveredTexture;
				}
				return ClickedTexture;
			}
			return DisabledTexture;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D texture = GetTexture();
			Color? color = ((ColorHovered.HasValue && base.MouseOver) ? ColorHovered : ((ColorClicked.HasValue && Clicked) ? ColorClicked : ImageColor));
			if (texture != null && color.HasValue)
			{
				spriteBatch.DrawOnCtrl(this, texture, SizeRectangle.GetValueOrDefault(bounds), (Rectangle)(((_003F?)TextureRectangle) ?? texture.Bounds), color.Value, TextureRotation.GetValueOrDefault(), default(Vector2), (SpriteEffects)0);
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			if (base.Enabled)
			{
				base.OnClick(e);
				ClickAction?.Invoke(e);
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
		}
	}
}
