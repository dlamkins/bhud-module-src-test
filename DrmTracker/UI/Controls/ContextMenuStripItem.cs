using System;
using Blish_HUD;
using Blish_HUD.Controls;
using DrmTracker.Interfaces;
using DrmTracker.Services;
using Gw2Sharp.WebApi;

namespace DrmTracker.UI.Controls
{
	public class ContextMenuStripItem : ContextMenuStripItem, ILocalizable
	{
		private Func<string> _setLocalizedText;

		public Func<string> SetLocalizedText
		{
			get
			{
				return _setLocalizedText;
			}
			set
			{
				_setLocalizedText = value;
				((ContextMenuStripItem)this).set_Text(value?.Invoke());
			}
		}

		public ContextMenuStripItem()
			: this()
		{
			LocalizingService.LocaleChanged += UserLocale_SettingChanged;
			UserLocale_SettingChanged(null, null);
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedText != null)
			{
				((ContextMenuStripItem)this).set_Text(SetLocalizedText?.Invoke());
			}
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
		}
	}
}
