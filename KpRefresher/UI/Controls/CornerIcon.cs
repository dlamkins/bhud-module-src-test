using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi;
using KpRefresher.Interfaces;
using KpRefresher.Ressources;
using KpRefresher.Services;
using Microsoft.Xna.Framework.Graphics;

namespace KpRefresher.UI.Controls
{
	public class CornerIcon : CornerIcon, ILocalizable
	{
		private Func<string> _setLocalizedTooltip;

		private Texture2D _cornerIconTexture;

		private Texture2D _cornerIconWarningTexture;

		private Texture2D _cornerIconHoverTexture;

		private Texture2D _cornerIconHoverWarningTexture;

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

		public CornerIcon(ContentsManager contentsManager)
			: this()
		{
			LocalizingService.LocaleChanged += UserLocale_SettingChanged;
			UserLocale_SettingChanged(null, null);
			_cornerIconTexture = contentsManager.GetTexture("corner.png");
			_cornerIconWarningTexture = contentsManager.GetTexture("corner_warn.png");
			_cornerIconHoverTexture = contentsManager.GetTexture("corner-hover.png");
			_cornerIconHoverWarningTexture = contentsManager.GetTexture("corner-hover_warn.png");
			UpdateWarningState(isWarning: false);
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedTooltip != null)
			{
				((Control)this).set_BasicTooltipText(SetLocalizedTooltip?.Invoke());
			}
		}

		public void UpdateWarningState(bool isWarning)
		{
			if (isWarning)
			{
				((CornerIcon)this).set_Icon(AsyncTexture2D.op_Implicit(_cornerIconHoverWarningTexture));
				((CornerIcon)this).set_HoverIcon(AsyncTexture2D.op_Implicit(_cornerIconHoverWarningTexture));
				SetLocalizedTooltip = () => strings.CornerIcon_Tooltip_Warning;
			}
			else
			{
				((CornerIcon)this).set_Icon(AsyncTexture2D.op_Implicit(_cornerIconTexture));
				((CornerIcon)this).set_HoverIcon(AsyncTexture2D.op_Implicit(_cornerIconHoverTexture));
				SetLocalizedTooltip = () => strings.CornerIcon_Tooltip;
			}
		}

		protected override void DisposeControl()
		{
			Texture2D cornerIconTexture = _cornerIconTexture;
			if (cornerIconTexture != null)
			{
				((GraphicsResource)cornerIconTexture).Dispose();
			}
			Texture2D cornerIconWarningTexture = _cornerIconWarningTexture;
			if (cornerIconWarningTexture != null)
			{
				((GraphicsResource)cornerIconWarningTexture).Dispose();
			}
			Texture2D cornerIconHoverTexture = _cornerIconHoverTexture;
			if (cornerIconHoverTexture != null)
			{
				((GraphicsResource)cornerIconHoverTexture).Dispose();
			}
			Texture2D cornerIconHoverWarningTexture = _cornerIconHoverWarningTexture;
			if (cornerIconHoverWarningTexture != null)
			{
				((GraphicsResource)cornerIconHoverWarningTexture).Dispose();
			}
			((CornerIcon)this).DisposeControl();
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
		}
	}
}
