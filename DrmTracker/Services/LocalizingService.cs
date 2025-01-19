using System;
using Blish_HUD;
using Gw2Sharp.WebApi;

namespace DrmTracker.Services
{
	public static class LocalizingService
	{
		public static bool Enabled { get; set; } = true;


		public static event EventHandler<ValueChangedEventArgs<Locale>> LocaleChanged;

		public static void OnLocaleChanged(object sender, ValueChangedEventArgs<Locale> eventArgs)
		{
			TriggerLocaleChanged(Enabled, sender, eventArgs);
		}

		public static void TriggerLocaleChanged(bool force = false, object sender = null, ValueChangedEventArgs<Locale> eventArgs = null)
		{
			if (force)
			{
				LocalizingService.LocaleChanged?.Invoke(sender, eventArgs);
			}
		}
	}
}
