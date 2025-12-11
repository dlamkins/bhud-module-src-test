using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Core.Controls
{
	public class Label : Blish_HUD.Controls.Label, ILocalizable
	{
		public Func<string>? SetLocalizedText
		{
			[CompilerGenerated]
			get
			{
				return _003CSetLocalizedText_003Ek__BackingField;
			}
			set
			{
				_003CSetLocalizedText_003Ek__BackingField = value;
				base.Text = value?.Invoke();
			}
		}

		public Func<string>? SetLocalizedTooltip
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
