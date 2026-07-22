using System;
using System.Runtime.CompilerServices;
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
		private bool Clicked
		{
			get
			{
				if (base.MouseOver)
				{
					if (Control.Input.Mouse.State.LeftButton != ButtonState.Pressed)
					{
						return Control.Input.Mouse.State.RightButton == ButtonState.Pressed;
					}
					return true;
				}
				return false;
			}
		}

		public Action<MouseEventArgs> ClickAction { get; set; }

		public Color? ColorHovered { get; set; }

		public Color? ColorClicked { get; set; }

		public Color? ImageColor { get; set; } = Color.White;


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
			[CompilerGenerated]
			get
			{
				return _003CSetLocalizedTooltip_003Ek__BackingField;
			}
			set
			{
				_003CSetLocalizedTooltip_003Ek__BackingField = value;
				base.BasicTooltipText = value?.Invoke();
			}
		}

		public ImageButton()
		{
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
			AsyncTexture2D texture = GetTexture();
			Color? color = ((ColorHovered.HasValue && base.MouseOver) ? ColorHovered : ((ColorClicked.HasValue && Clicked) ? ColorClicked : ImageColor));
			if (texture != null && color.HasValue)
			{
				spriteBatch.DrawOnCtrl(this, texture, SizeRectangle.GetValueOrDefault(bounds), TextureRectangle ?? texture.Bounds, color.Value, TextureRotation.GetValueOrDefault(), default(Vector2));
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
