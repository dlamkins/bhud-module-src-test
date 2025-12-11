using System;
using Blish_HUD;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Characters.Controls
{
	internal class ImageColorToggle : ImageGrayScaled, ILocalizable
	{
		private readonly Action<bool> _onChanged;

		public ProfessionType Profession { get; set; }

		public Func<string>? SetLocalizedTooltip
		{
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

		public ImageColorToggle()
		{
			LocalizingService.LocaleChanged += new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
		}

		public ImageColorToggle(Action<bool> onChanged)
			: this()
		{
			_onChanged = onChanged;
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			base.Active = !base.Active;
			_onChanged?.Invoke(base.Active);
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			GameService.Overlay.UserLocale.SettingChanged -= UserLocale_SettingChanged;
		}

		public void UserLocale_SettingChanged(object sender = null, ValueChangedEventArgs<Locale> e = null)
		{
			if (SetLocalizedTooltip != null)
			{
				base.BasicTooltipText = SetLocalizedTooltip?.Invoke();
			}
		}
	}
}
