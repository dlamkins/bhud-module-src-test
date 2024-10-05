using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Core.Controls
{
	public class Label : Blish_HUD.Controls.Label, ILocalizable
	{
		private Func<string>? _setLocalizedText;

		private Func<string>? _setLocalizedTooltip;

		public Func<string>? SetLocalizedText
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

		public Func<string>? SetLocalizedTooltip
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

		public Label()
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

		protected override void DisposeControl()
		{
			base.DisposeControl();
			GameService.Overlay.UserLocale.SettingChanged -= UserLocale_SettingChanged;
		}
	}
}
