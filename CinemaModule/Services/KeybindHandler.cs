using System;
using Blish_HUD;
using CinemaModule.Settings;

namespace CinemaModule.Services
{
	public class KeybindHandler : IDisposable
	{
		private readonly CinemaSettings _cinemaSettings;

		private readonly Action _togglePause;

		private readonly Action _toggleLockWindow;

		private readonly Action _toggleMute;

		private readonly Action _toggleEnabled;

		private readonly EventHandler<ValueChangedEventArgs<bool>> _keybindsEnabledChangedHandler;

		public KeybindHandler(CinemaSettings cinemaSettings, Action togglePause, Action toggleLockWindow, Action toggleMute, Action toggleEnabled)
		{
			_cinemaSettings = cinemaSettings;
			_togglePause = togglePause;
			_toggleLockWindow = toggleLockWindow;
			_toggleMute = toggleMute;
			_toggleEnabled = toggleEnabled;
			ApplyEnabledState(cinemaSettings.KeybindsEnabled.get_Value());
			_keybindsEnabledChangedHandler = delegate(object s, ValueChangedEventArgs<bool> e)
			{
				ApplyEnabledState(e.get_NewValue());
			};
			_cinemaSettings.KeybindsEnabled.add_SettingChanged(_keybindsEnabledChangedHandler);
			SubscribeToKeybinds();
		}

		private void ApplyEnabledState(bool enabled)
		{
			_cinemaSettings.KeybindPlayPause.get_Value().set_Enabled(enabled);
			_cinemaSettings.KeybindLockWindow.get_Value().set_Enabled(enabled);
			_cinemaSettings.KeybindMuteToggle.get_Value().set_Enabled(enabled);
			_cinemaSettings.KeybindToggleEnabled.get_Value().set_Enabled(enabled);
		}

		private void SubscribeToKeybinds()
		{
			_cinemaSettings.KeybindPlayPause.get_Value().add_Activated((EventHandler<EventArgs>)OnPlayPauseActivated);
			_cinemaSettings.KeybindLockWindow.get_Value().add_Activated((EventHandler<EventArgs>)OnLockWindowActivated);
			_cinemaSettings.KeybindMuteToggle.get_Value().add_Activated((EventHandler<EventArgs>)OnMuteToggleActivated);
			_cinemaSettings.KeybindToggleEnabled.get_Value().add_Activated((EventHandler<EventArgs>)OnToggleEnabledActivated);
		}

		private void OnPlayPauseActivated(object sender, EventArgs e)
		{
			_togglePause();
		}

		private void OnLockWindowActivated(object sender, EventArgs e)
		{
			_toggleLockWindow();
		}

		private void OnMuteToggleActivated(object sender, EventArgs e)
		{
			_toggleMute();
		}

		private void OnToggleEnabledActivated(object sender, EventArgs e)
		{
			_toggleEnabled();
		}

		public void Dispose()
		{
			_cinemaSettings.KeybindsEnabled.remove_SettingChanged(_keybindsEnabledChangedHandler);
			_cinemaSettings.KeybindPlayPause.get_Value().remove_Activated((EventHandler<EventArgs>)OnPlayPauseActivated);
			_cinemaSettings.KeybindLockWindow.get_Value().remove_Activated((EventHandler<EventArgs>)OnLockWindowActivated);
			_cinemaSettings.KeybindMuteToggle.get_Value().remove_Activated((EventHandler<EventArgs>)OnMuteToggleActivated);
			_cinemaSettings.KeybindToggleEnabled.get_Value().remove_Activated((EventHandler<EventArgs>)OnToggleEnabledActivated);
		}
	}
}
