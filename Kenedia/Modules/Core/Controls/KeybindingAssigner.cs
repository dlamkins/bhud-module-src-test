using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Core.Controls
{
	public class KeybindingAssigner : KeybindingAssigner, ILocalizable
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
				((KeybindingAssigner)this).set_KeyBindingName(value?.Invoke());
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
				((Control)this).set_BasicTooltipText(value?.Invoke());
			}
		}

		public Action<KeyBinding> KeybindChangedAction { get; set; }

		public KeybindingAssigner()
			: this()
		{
			LocalizingService.LocaleChanged += UserLocale_SettingChanged;
			((KeybindingAssigner)this).add_BindingChanged((EventHandler<EventArgs>)KeybindingAssigner_BindingChanged);
			UserLocale_SettingChanged(null, null);
		}

		private void KeybindingAssigner_BindingChanged(object sender, EventArgs e)
		{
			KeybindChangedAction?.Invoke(((KeybindingAssigner)this).get_KeyBinding());
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedKeyBindingName != null)
			{
				((KeybindingAssigner)this).set_KeyBindingName(SetLocalizedKeyBindingName?.Invoke());
			}
			if (SetLocalizedTooltip != null)
			{
				((Control)this).set_BasicTooltipText(SetLocalizedTooltip?.Invoke());
			}
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
			((KeybindingAssigner)this).remove_BindingChanged((EventHandler<EventArgs>)KeybindingAssigner_BindingChanged);
		}
	}
}
