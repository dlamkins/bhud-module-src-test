using System;
using Blish_HUD;
using Blish_HUD.Controls;
using DrmTracker.Interfaces;
using DrmTracker.Services;
using Gw2Sharp.WebApi;

namespace DrmTracker.UI.Controls
{
	public class FlowPanel : FlowPanel, ILocalizable
	{
		private Func<string> _setLocalizedTooltip;

		private Func<string> _setLocalizedTitle;

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

		public Func<string> SetLocalizedTitle
		{
			get
			{
				return _setLocalizedTitle;
			}
			set
			{
				_setLocalizedTitle = value;
				((Panel)this).set_Title(value?.Invoke());
			}
		}

		public FlowPanel()
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
			if (SetLocalizedTitle != null)
			{
				((Panel)this).set_Title(SetLocalizedTitle?.Invoke());
			}
		}

		protected override void DisposeControl()
		{
			((FlowPanel)this).DisposeControl();
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
		}
	}
}
