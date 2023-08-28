using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Core.Controls
{
	public class TrackBar : TrackBar, ILocalizable
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

		public Action<int> ValueChangedAction { get; set; }

		public TrackBar()
			: this()
		{
			LocalizingService.LocaleChanged += UserLocale_SettingChanged;
			((TrackBar)this).add_ValueChanged((EventHandler<ValueEventArgs<float>>)TrackBar_ValueChanged);
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
			((TrackBar)this).DisposeControl();
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
		}

		private void TrackBar_ValueChanged(object sender, ValueEventArgs<float> e)
		{
			ValueChangedAction?.Invoke((int)((TrackBar)this).get_Value());
		}
	}
}
