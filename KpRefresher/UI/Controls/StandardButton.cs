using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using KpRefresher.Interfaces;
using KpRefresher.Services;

namespace KpRefresher.UI.Controls
{
	public class StandardButton : StandardButton, ILocalizable
	{
		private Func<string> _setLocalizedText;

		private Func<string> _setLocalizedTooltip;

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

		public StandardButton()
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

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
		}
	}
}
