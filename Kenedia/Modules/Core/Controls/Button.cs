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
using MonoGame.Extended;

namespace Kenedia.Modules.Core.Controls
{
	public class Button : StandardButton, ILocalizable
	{
		private Texture2D _textureButtonIdle = Control.get_Content().GetTexture("common/button-states");

		private Texture2D _textureButtonBorder = Control.get_Content().GetTexture("button-border");

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
				((StandardButton)this).set_Text(value?.Invoke());
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
				((Control)this).set_BasicTooltipText(value?.Invoke());
			}
		}

		public bool Selected { get; set; }

		public bool SelectedTint { get; set; }

		public Button()
			: this()
		{
			LocalizingService.LocaleChanged += UserLocale_SettingChanged;
			UserLocale_SettingChanged(null, null);
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedText != null)
			{
				((StandardButton)this).set_Text(SetLocalizedText?.Invoke());
			}
			if (SetLocalizedTooltip != null)
			{
				((Control)this).set_BasicTooltipText(SetLocalizedTooltip?.Invoke());
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((StandardButton)this).OnClick(e);
			ClickAction?.Invoke();
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			_textureButtonBorder = null;
			_textureButtonIdle = null;
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
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
			((StandardButton)this).RecalculateLayout();
			Size2 textDimensions = ((LabelBase)this).GetTextDimensions((string)null);
			int num = (int)((float)(((Control)this)._size.X / 2) - textDimensions.Width / 2f);
			if (((StandardButton)this).get_Icon() != null)
			{
				num = ((!(textDimensions.Width > 0f)) ? (num + 8) : (num + 10));
				Point val2;
				if (!((StandardButton)this).get_ResizeIcon())
				{
					Rectangle bounds = ((StandardButton)this).get_Icon().get_Texture().get_Bounds();
					int val = Math.Min(((Rectangle)(ref bounds)).get_Size().X, ((Control)this).get_Width());
					bounds = ((StandardButton)this).get_Icon().get_Texture().get_Bounds();
					val2 = new Point(Math.Min(val, Math.Min(((Rectangle)(ref bounds)).get_Size().Y, ((Control)this).get_Height() - 7)));
				}
				else
				{
					val2 = new Point(16);
				}
				Point point = val2;
				_layoutIconBounds = new Rectangle(num - point.X - 4, ((Control)this)._size.Y / 2 - point.Y / 2, point.X, point.Y);
			}
			_layoutTextBounds = new Rectangle(num, (((Control)this).get_Height() - (int)textDimensions.Height) / 2, (int)textDimensions.Width, (int)textDimensions.Height);
			_underlineBounds = new Rectangle(num, ((Rectangle)(ref _layoutTextBounds)).get_Bottom() - 3, (int)textDimensions.Width, 2);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RecalculateLayout();
			if (((Control)this)._enabled)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureButtonIdle, new Rectangle(3, 3, ((Control)this)._size.X - 6, ((Control)this)._size.Y - 5), (Rectangle?)new Rectangle(((StandardButton)this).get_AnimationState() * 350, 0, 350, 20), (Color)((!SelectedTint || Selected) ? Color.get_White() : new Color(175, 175, 175)));
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(3, 3, ((Control)this)._size.X - 6, ((Control)this)._size.Y - 5), Color.FromNonPremultiplied(121, 121, 121, 255));
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureButtonBorder, new Rectangle(2, 0, ((Control)this).get_Width() - 5, 4), (Rectangle?)new Rectangle(0, 0, 1, 4));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureButtonBorder, new Rectangle(((Control)this).get_Width() - 4, 2, 4, ((Control)this).get_Height() - 3), (Rectangle?)new Rectangle(0, 1, 4, 1));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureButtonBorder, new Rectangle(3, ((Control)this).get_Height() - 4, ((Control)this).get_Width() - 6, 4), (Rectangle?)new Rectangle(1, 0, 1, 4));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureButtonBorder, new Rectangle(0, 2, 4, ((Control)this).get_Height() - 3), (Rectangle?)new Rectangle(0, 3, 4, 1));
			if (((StandardButton)this).get_Icon() != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(((StandardButton)this).get_Icon()), _layoutIconBounds);
			}
			((LabelBase)this)._textColor = (((Control)this)._enabled ? Color.get_Black() : Color.FromNonPremultiplied(51, 51, 51, 255));
			((LabelBase)this).DrawText(spriteBatch, _layoutTextBounds, (string)null);
			if (Selected)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _underlineBounds, Color.get_Black() * 0.6f);
			}
		}
	}
}
