using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using CinemaModule.Services.Twitch;
using CinemaModule.Settings;
using CinemaModule.UI.Windows.Dialogs;
using Microsoft.Xna.Framework;

namespace CinemaModule.UI.Views
{
	public class ModuleSettingsView : View
	{
		private const int BUTTON_WIDTH = 200;

		private const int BUTTON_HEIGHT = 26;

		private const int PADDING = 10;

		private const int BUTTON_SPACING = 15;

		private readonly SettingCollection _settings;

		private readonly CinemaUserSettings _userSettings;

		private readonly CinemaSettings _cinemaSettings;

		private readonly TwitchAuthService _twitchAuthService;

		private readonly Action _showThirdPartyNoticesAction;

		private FlowPanel _settingsPanel;

		private StandardButton _twitchButton;

		private StandardButton _thirdPartyNoticesButton;

		private Checkbox _foregroundCheckbox;

		private Checkbox _keybindsEnabledCheckbox;

		private TwitchAuthWindow _twitchAuthWindow;

		public ModuleSettingsView(SettingCollection settings, CinemaUserSettings userSettings, CinemaSettings cinemaSettings, TwitchAuthService twitchAuthService, Action showThirdPartyNoticesAction)
			: this()
		{
			_settings = settings;
			_userSettings = userSettings;
			_cinemaSettings = cinemaSettings;
			_twitchAuthService = twitchAuthService;
			_showThirdPartyNoticesAction = showThirdPartyNoticesAction;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(((Control)buildPanel).get_Size());
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(5f, 8f));
			val.set_OuterControlPadding(new Vector2(10f, 10f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_AutoSizePadding(new Point(0, 15));
			((Control)val).set_Parent(buildPanel);
			_settingsPanel = val;
			BuildSettingsEntries();
			BuildExtraSettings();
			BuildButtons();
			_twitchAuthService.AuthStatusChanged += OnTwitchAuthStatusChanged;
		}

		private void BuildSettingsEntries()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			foreach (SettingEntry setting in _settings)
			{
				if (setting.get_SessionDefined())
				{
					IView settingView = SettingView.FromType(setting, ((Control)_settingsPanel).get_Width());
					if (settingView != null)
					{
						ViewContainer val = new ViewContainer();
						((Container)val).set_WidthSizingMode((SizingMode)2);
						((Container)val).set_HeightSizingMode((SizingMode)1);
						((Control)val).set_Parent((Container)(object)_settingsPanel);
						val.Show(settingView);
					}
				}
			}
		}

		private void BuildExtraSettings()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			Checkbox val = new Checkbox();
			val.set_Text("Window Display in Foreground");
			((Control)val).set_BasicTooltipText("When enabled, the on-screen window renders on top of all other overlay elements");
			val.set_Checked(_userSettings.WindowInForeground);
			((Control)val).set_Parent((Container)(object)_settingsPanel);
			_foregroundCheckbox = val;
			_foregroundCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_userSettings.WindowInForeground = _foregroundCheckbox.get_Checked();
			});
			BuildKeybindSection();
		}

		private void BuildKeybindSection()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			Checkbox val = new Checkbox();
			val.set_Text("Enable Keybinds");
			((Control)val).set_BasicTooltipText("Master toggle — enables or disables all CinemaHUD keybinds");
			val.set_Checked(_cinemaSettings.KeybindsEnabled.get_Value());
			((Control)val).set_Parent((Container)(object)_settingsPanel);
			_keybindsEnabledCheckbox = val;
			_keybindsEnabledCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_cinemaSettings.KeybindsEnabled.set_Value(_keybindsEnabledCheckbox.get_Checked());
			});
			AddKeybindRow(_cinemaSettings.KeybindPlayPause);
			AddKeybindRow(_cinemaSettings.KeybindLockWindow);
			AddKeybindRow(_cinemaSettings.KeybindMuteToggle);
			AddKeybindRow(_cinemaSettings.KeybindToggleEnabled);
		}

		private void AddKeybindRow(SettingEntry<KeyBinding> keybindSetting)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			IView settingView = SettingView.FromType((SettingEntry)(object)keybindSetting, ((Control)_settingsPanel).get_Width());
			if (settingView != null)
			{
				ViewContainer val = new ViewContainer();
				((Container)val).set_WidthSizingMode((SizingMode)2);
				((Container)val).set_HeightSizingMode((SizingMode)1);
				((Control)val).set_Parent((Container)(object)_settingsPanel);
				val.Show(settingView);
			}
		}

		private void BuildButtons()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)2);
			val.set_ControlPadding(new Vector2(15f, 0f));
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)_settingsPanel);
			FlowPanel buttonPanel = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text(GetTwitchButtonText());
			((Control)val2).set_Width(200);
			((Control)val2).set_Height(26);
			((Control)val2).set_Parent((Container)(object)buttonPanel);
			_twitchButton = val2;
			((Control)_twitchButton).add_Click((EventHandler<MouseEventArgs>)OnTwitchButtonClick);
			StandardButton val3 = new StandardButton();
			val3.set_Text("Third-Party Notices");
			((Control)val3).set_Width(200);
			((Control)val3).set_Height(26);
			((Control)val3).set_Parent((Container)(object)buttonPanel);
			_thirdPartyNoticesButton = val3;
			((Control)_thirdPartyNoticesButton).add_Click((EventHandler<MouseEventArgs>)OnThirdPartyNoticesButtonClick);
		}

		private string GetTwitchButtonText()
		{
			if (_twitchAuthService.IsAuthenticated && !string.IsNullOrEmpty(_twitchAuthService.Username))
			{
				return "Twitch: " + _twitchAuthService.Username;
			}
			return "Twitch Login";
		}

		private void OnTwitchButtonClick(object sender, MouseEventArgs e)
		{
			ShowTwitchAuthWindow();
		}

		private void ShowTwitchAuthWindow()
		{
			if (_twitchAuthWindow == null)
			{
				_twitchAuthWindow = new TwitchAuthWindow(_twitchAuthService, OnTwitchTokensChanged);
			}
			((Control)_twitchAuthWindow).Show();
		}

		private void OnTwitchTokensChanged(string accessToken, string refreshToken)
		{
			_userSettings.TwitchAccessToken = accessToken;
			_userSettings.TwitchRefreshToken = refreshToken;
		}

		private void OnTwitchAuthStatusChanged(object sender, TwitchAuthStatusEventArgs e)
		{
			if (_twitchButton != null)
			{
				_twitchButton.set_Text(GetTwitchButtonText());
			}
		}

		private void OnThirdPartyNoticesButtonClick(object sender, MouseEventArgs e)
		{
			_showThirdPartyNoticesAction?.Invoke();
		}

		protected override void Unload()
		{
			_twitchAuthService.AuthStatusChanged -= OnTwitchAuthStatusChanged;
			if (_twitchButton != null)
			{
				((Control)_twitchButton).remove_Click((EventHandler<MouseEventArgs>)OnTwitchButtonClick);
			}
			if (_thirdPartyNoticesButton != null)
			{
				((Control)_thirdPartyNoticesButton).remove_Click((EventHandler<MouseEventArgs>)OnThirdPartyNoticesButtonClick);
			}
			TwitchAuthWindow twitchAuthWindow = _twitchAuthWindow;
			if (twitchAuthWindow != null)
			{
				((Control)twitchAuthWindow).Dispose();
			}
		}
	}
}
