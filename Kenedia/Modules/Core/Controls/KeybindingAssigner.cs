using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Core.Controls
{
	public class KeybindingAssigner : Blish_HUD.Controls.KeybindingAssigner, ILocalizable
	{
		private Func<string> _setLocalizedKeyBindingName;

		private Func<string> _setLocalizedTooltip;

		public Func<string> SetLocalizedKeyBindingName
		{
			get
			{
				return _setLocalizedKeyBindingName;
			}
			set
			{
				_setLocalizedKeyBindingName = value;
				base.KeyBindingName = value?.Invoke();
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
				base.BasicTooltipText = value?.Invoke();
			}
		}

		public Action<KeyBinding> KeybindChangedAction { get; set; }

		public KeybindingAssigner()
		{
			LocalizingService.LocaleChanged += new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			base.BindingChanged += KeybindingAssigner_BindingChanged;
			UserLocale_SettingChanged(null, null);
		}

		private void KeybindingAssigner_BindingChanged(object sender, EventArgs e)
		{
			KeybindChangedAction?.Invoke(base.KeyBinding);
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedKeyBindingName != null)
			{
				base.KeyBindingName = SetLocalizedKeyBindingName?.Invoke();
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
			base.BindingChanged -= KeybindingAssigner_BindingChanged;
		}
	}
}
