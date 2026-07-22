using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters.Controls
{
	public class ImageToggleButton : Control, ILocalizable
	{
		private readonly Action<bool> _onChanged;

		private bool _clicked;

		public bool Active { get; set; }

		public Color ColorHovered { get; set; } = new Color(255, 255, 255, 255);


		public Color ColorClicked { get; set; } = new Color(0, 0, 255, 255);


		public Color ColorDefault { get; set; } = new Color(255, 255, 255, 255);


		public Color ColorActive { get; set; } = new Color(255, 255, 255, 255);


		public AsyncTexture2D Texture { get; set; }

		public AsyncTexture2D HoveredTexture { get; set; }

		public AsyncTexture2D ActiveTexture { get; set; }

		public AsyncTexture2D ClickedTexture { get; set; }

		public Rectangle SizeRectangle { get; set; }

		public Rectangle TextureRectangle { get; set; }

		public Action ClickAction { get; set; }

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

		public ImageToggleButton()
		{
			GameService.Overlay.UserLocale.SettingChanged += UserLocale_SettingChanged;
			UserLocale_SettingChanged(null, null);
		}

		public ImageToggleButton(Action<bool> onChanged)
			: this()
		{
			_onChanged = onChanged;
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			Active = !Active;
			_onChanged?.Invoke(Active);
			ClickAction?.Invoke();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (Texture != null)
			{
				AsyncTexture2D texture = ((_clicked && ClickedTexture != null) ? ClickedTexture : ((Active && ActiveTexture != null) ? ActiveTexture : ((base.MouseOver && HoveredTexture != null) ? HoveredTexture : Texture)));
				_clicked = _clicked && base.MouseOver;
				spriteBatch.DrawOnCtrl(this, texture, (SizeRectangle != Rectangle.Empty) ? SizeRectangle : bounds, (TextureRectangle == Rectangle.Empty) ? texture.Bounds : TextureRectangle, Active ? ColorActive : (base.MouseOver ? ColorHovered : ((base.MouseOver && _clicked) ? ColorClicked : ColorDefault)), 0f, default(Vector2));
			}
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			base.OnLeftMouseButtonPressed(e);
			_clicked = true;
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			base.OnLeftMouseButtonReleased(e);
			_clicked = false;
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
			GameService.Overlay.UserLocale.SettingChanged -= UserLocale_SettingChanged;
		}
	}
}
