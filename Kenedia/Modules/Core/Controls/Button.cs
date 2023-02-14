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
		private Func<string> _setLocalizedText;

		private Func<string> _setLocalizedTooltip;

		private bool _selected;

		private Rectangle _layoutIconBounds;

		private Rectangle _layoutTextBounds;

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

		public bool Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				_selected = value;
			}
		}

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
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
		}

		public override void RecalculateLayout()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			((StandardButton)this).RecalculateLayout();
			Size2 textDimensions = ((LabelBase)this).GetTextDimensions((string)null);
			int num = (int)((float)(((Control)this)._size.X / 2) - textDimensions.Width / 2f);
			if (((StandardButton)this).get_Icon() != null)
			{
				num = ((!(textDimensions.Width > 0f)) ? (num + 8) : (num + 10));
				_003F val;
				if (!((StandardButton)this).get_ResizeIcon())
				{
					Rectangle bounds = ((StandardButton)this).get_Icon().get_Texture().get_Bounds();
					val = ((Rectangle)(ref bounds)).get_Size();
				}
				else
				{
					val = new Point(16);
				}
				Point point = (Point)val;
				_layoutIconBounds = new Rectangle(num - point.X - 4, ((Control)this)._size.Y / 2 - point.Y / 2, point.X, point.Y);
			}
			_layoutTextBounds = new Rectangle(num, (int)textDimensions.Height + 2, (int)textDimensions.Width, 2);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			((StandardButton)this).Paint(spriteBatch, bounds);
			if (_selected)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _layoutTextBounds, Color.get_Black() * 0.6f);
			}
		}
	}
}
