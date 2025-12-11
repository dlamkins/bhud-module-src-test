using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Core.Controls
{
	public class LoadingSpinner : Blish_HUD.Controls.LoadingSpinner, ILocalizable
	{
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

		public CaptureType? CaptureInput { get; set; }

		public LoadingSpinner()
		{
			LocalizingService.LocaleChanged += new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			UserLocale_SettingChanged(null, null);
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

		protected override CaptureType CapturesInput()
		{
			return CaptureInput ?? base.CapturesInput();
		}
	}
}
