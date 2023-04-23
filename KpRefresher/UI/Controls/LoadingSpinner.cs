using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using KpRefresher.Interfaces;
using KpRefresher.Services;

namespace KpRefresher.UI.Controls
{
	public class LoadingSpinner : LoadingSpinner, ILocalizable
	{
		private Func<string> _setLocalizedTooltip;

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

		public LoadingSpinner()
			: this()
		{
			LocalizingService.LocaleChanged += UserLocale_SettingChanged;
			UserLocale_SettingChanged(null, null);
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
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