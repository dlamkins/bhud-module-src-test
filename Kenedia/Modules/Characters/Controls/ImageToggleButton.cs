using System;
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

		private Func<string> _setLocalizedTooltip;

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

		public ImageToggleButton()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			if (Texture != null)
			{
				AsyncTexture2D texture = ((_clicked && ClickedTexture != null) ? ClickedTexture : ((Active && ActiveTexture != null) ? ActiveTexture : ((base.MouseOver && HoveredTexture != null) ? HoveredTexture : Texture)));
				_clicked = _clicked && base.MouseOver;
				spriteBatch.DrawOnCtrl(this, texture, (SizeRectangle != Rectangle.get_Empty()) ? SizeRectangle : bounds, (TextureRectangle == Rectangle.get_Empty()) ? texture.Bounds : TextureRectangle, Active ? ColorActive : (base.MouseOver ? ColorHovered : ((base.MouseOver && _clicked) ? ColorClicked : ColorDefault)), 0f, default(Vector2), (SpriteEffects)0);
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
