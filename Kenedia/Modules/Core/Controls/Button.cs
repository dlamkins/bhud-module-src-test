using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Kenedia.Modules.Core.Controls
{
	public class Button : StandardButton, ILocalizable
	{
		private Texture2D _textureButtonIdle = Control.Content.GetTexture("common/button-states");

		private Texture2D _textureButtonBorder = Control.Content.GetTexture("button-border");

		private Func<string> _setLocalizedText;

		private Func<string> _setLocalizedTooltip;

		private Rectangle _layoutIconBounds;

		private Rectangle _layoutTextBounds;

		private Rectangle _underlineBounds;

		public Action ClickAction { get; set; }

		public Func<string> SetLocalizedText
		{
			get
			{
				return _setLocalizedText;
			}
			set
			{
				_setLocalizedText = value;
				base.Text = value?.Invoke();
			}
		}

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

		public bool Selected { get; set; }

		public bool SelectedTint { get; set; }

		public Button()
		{
			LocalizingService.LocaleChanged += new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			UserLocale_SettingChanged(null, null);
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedText != null)
			{
				base.Text = SetLocalizedText?.Invoke();
			}
			if (SetLocalizedTooltip != null)
			{
				base.BasicTooltipText = SetLocalizedTooltip?.Invoke();
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			ClickAction?.Invoke();
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_textureButtonBorder = null;
			_textureButtonIdle = null;
			GameService.Overlay.UserLocale.SettingChanged -= UserLocale_SettingChanged;
		}

		public override void RecalculateLayout()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			Size2 textDimensions = GetTextDimensions();
			int num = (int)((float)(_size.X / 2) - textDimensions.Width / 2f);
			if (base.Icon != null)
			{
				num = ((!(textDimensions.Width > 0f)) ? (num + 8) : (num + 10));
				Point val2;
				if (!base.ResizeIcon)
				{
					Rectangle bounds = base.Icon.Texture.get_Bounds();
					int val = Math.Min(((Rectangle)(ref bounds)).get_Size().X, base.Width);
					bounds = base.Icon.Texture.get_Bounds();
					val2 = new Point(Math.Min(val, Math.Min(((Rectangle)(ref bounds)).get_Size().Y, base.Height - 7)));
				}
				else
				{
					val2 = new Point(16);
				}
				Point point = val2;
				_layoutIconBounds = new Rectangle(num - point.X - 4, _size.Y / 2 - point.Y / 2, point.X, point.Y);
			}
			_layoutTextBounds = new Rectangle(num, (base.Height - (int)textDimensions.Height) / 2, (int)textDimensions.Width, (int)textDimensions.Height);
			_underlineBounds = new Rectangle(num, ((Rectangle)(ref _layoutTextBounds)).get_Bottom() - 3, (int)textDimensions.Width, 2);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			if (_enabled)
			{
				spriteBatch.DrawOnCtrl((Control)this, _textureButtonIdle, new Rectangle(3, 3, _size.X - 6, _size.Y - 5), (Rectangle?)new Rectangle(base.AnimationState * 350, 0, 350, 20), (Color)((!SelectedTint || Selected) ? Color.get_White() : new Color(175, 175, 175)));
			}
			else
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(3, 3, _size.X - 6, _size.Y - 5), Color.FromNonPremultiplied(121, 121, 121, 255));
			}
			spriteBatch.DrawOnCtrl((Control)this, _textureButtonBorder, new Rectangle(2, 0, base.Width - 5, 4), (Rectangle?)new Rectangle(0, 0, 1, 4));
			spriteBatch.DrawOnCtrl((Control)this, _textureButtonBorder, new Rectangle(base.Width - 4, 2, 4, base.Height - 3), (Rectangle?)new Rectangle(0, 1, 4, 1));
			spriteBatch.DrawOnCtrl((Control)this, _textureButtonBorder, new Rectangle(3, base.Height - 4, base.Width - 6, 4), (Rectangle?)new Rectangle(1, 0, 1, 4));
			spriteBatch.DrawOnCtrl((Control)this, _textureButtonBorder, new Rectangle(0, 2, 4, base.Height - 3), (Rectangle?)new Rectangle(0, 3, 4, 1));
			if (base.Icon != null)
			{
				spriteBatch.DrawOnCtrl((Control)this, (Texture2D)base.Icon, _layoutIconBounds);
			}
			_textColor = (_enabled ? Color.get_Black() : Color.FromNonPremultiplied(51, 51, 51, 255));
			DrawText(spriteBatch, _layoutTextBounds);
			if (Selected)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _underlineBounds, Color.get_Black() * 0.6f);
			}
		}
	}
}
