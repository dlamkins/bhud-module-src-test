using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using rp.spark.Models;
using rp.spark.Services;

namespace rp.spark.UI.Views
{
	public class SparkOptionsView : View
	{
		private const int ContentWidth = 660;

		private const int ControlHeight = 30;

		private readonly SparkSettings _settings;

		private readonly Action _requestServerSync;

		private readonly Action<bool> _setNearbySharing;

		private Dropdown _regionDropdown;

		private StandardButton _matureProfilesButton;

		private readonly MatureProfilesConfirmation _matureProfilesConfirm;

		private Checkbox _shareCheckbox;

		private Checkbox _hideLocationCheckbox;

		private Checkbox _showNearbyCheckbox;

		private Checkbox _autoHideCheckbox;

		private Checkbox _cornerIconCheckbox;

		private bool _isUnloaded;

		public SparkOptionsView(SparkSettings settings, Action requestServerSync, Action<bool> setNearbySharing, Action<bool> maturePreferenceChanged)
			: this()
		{
			_settings = settings;
			_requestServerSync = requestServerSync;
			_setNearbySharing = setNearbySharing;
			_matureProfilesConfirm = new MatureProfilesConfirmation(settings, maturePreferenceChanged);
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel stack = SparkFormLayout.AddVerticalStack(buildPanel, 8, 8, 660, 560, 8, canScroll: true);
			SparkFormLayout.AddLabel((Container)(object)stack, "Privacy", 660, 28, GameService.Content.get_DefaultFont18(), (Color?)new Color(255, 233, 180), strokeText: true);
			_shareCheckbox = SparkFormLayout.AddCheckbox((Container)(object)stack, "Share my profile", _settings.BroadcastProfile.get_Value(), 220);
			_shareCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (_settings.BroadcastProfile.get_Value() != _shareCheckbox.get_Checked())
				{
					_settings.BroadcastProfile.set_Value(_shareCheckbox.get_Checked());
					_requestServerSync?.Invoke();
				}
			});
			_hideLocationCheckbox = SparkFormLayout.AddCheckbox((Container)(object)stack, "Hide my location", _settings.HideLocation.get_Value(), 220);
			_hideLocationCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (_settings.HideLocation.get_Value() != _hideLocationCheckbox.get_Checked())
				{
					_settings.HideLocation.set_Value(_hideLocationCheckbox.get_Checked());
					_requestServerSync?.Invoke();
				}
			});
			_showNearbyCheckbox = SparkFormLayout.AddCheckbox((Container)(object)stack, "Show me nearby", _settings.ShowNearbyPresence.get_Value(), 220);
			_showNearbyCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (_settings.ShowNearbyPresence.get_Value() != _showNearbyCheckbox.get_Checked())
				{
					if (_setNearbySharing != null)
					{
						_setNearbySharing(_showNearbyCheckbox.get_Checked());
					}
					else
					{
						_settings.ShowNearbyPresence.set_Value(_showNearbyCheckbox.get_Checked());
					}
				}
			});
			SparkFormLayout.AddSpacer((Container)(object)stack, 660, 8);
			SparkFormLayout.AddLabel((Container)(object)stack, "Profile Discovery", 660, 28, GameService.Content.get_DefaultFont18(), (Color?)new Color(255, 233, 180), strokeText: true);
			FlowPanel discoveryRow = SparkFormLayout.AddRow((Container)(object)stack, 660, 30, 8);
			SparkFormLayout.AddLabel((Container)(object)discoveryRow, "Region:", 58, 30, GameService.Content.get_DefaultFont14());
			_regionDropdown = SparkFormLayout.AddDropdown((Container)(object)discoveryRow, new string[2]
			{
				ProfileRegion.NA.ToString(),
				ProfileRegion.EU.ToString()
			}, _settings.RegionFilter.get_Value().ToString(), 90, 30);
			_regionDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (Enum.TryParse<ProfileRegion>(_regionDropdown.get_SelectedItem()?.ToString(), out var result) && _settings.RegionFilter.get_Value() != result)
				{
					_settings.RegionFilter.set_Value(result);
					_requestServerSync?.Invoke();
				}
			});
			_matureProfilesButton = SparkFormLayout.AddButton((Container)(object)discoveryRow, _matureProfilesConfirm.ButtonText, 190, 30);
			((Control)_matureProfilesButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_matureProfilesConfirm.Toggle(buildPanel);
				SyncMatureButtonFromSettings();
			});
			SparkFormLayout.AddSpacer((Container)(object)stack, 660, 8);
			SparkFormLayout.AddLabel((Container)(object)stack, "Interface", 660, 28, GameService.Content.get_DefaultFont18(), (Color?)new Color(255, 233, 180), strokeText: true);
			_autoHideCheckbox = SparkFormLayout.AddCheckbox((Container)(object)stack, "Auto-hide UI", _settings.AutoHideGameUi.get_Value(), 220);
			_autoHideCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (_settings.AutoHideGameUi.get_Value() != _autoHideCheckbox.get_Checked())
				{
					_settings.AutoHideGameUi.set_Value(_autoHideCheckbox.get_Checked());
				}
			});
			_cornerIconCheckbox = SparkFormLayout.AddCheckbox((Container)(object)stack, "Show SPARK icon", _settings.ShowCornerIcon.get_Value(), 220);
			_cornerIconCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (_settings.ShowCornerIcon.get_Value() != _cornerIconCheckbox.get_Checked())
				{
					_settings.ShowCornerIcon.set_Value(_cornerIconCheckbox.get_Checked());
				}
			});
			WatchSettings();
			SyncCheckboxesFromSettings();
		}

		private void WatchSettings()
		{
			_isUnloaded = false;
			_settings.BroadcastProfile.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.HideLocation.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.ShowNearbyPresence.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.AutoHideGameUi.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.ShowCornerIcon.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.RegionFilter.add_SettingChanged((EventHandler<ValueChangedEventArgs<ProfileRegion>>)OnRegionFilterChanged);
			_settings.ShowMatureProfiles.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
		}

		private void UnwatchSettings()
		{
			_settings.BroadcastProfile.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.HideLocation.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.ShowNearbyPresence.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.AutoHideGameUi.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.ShowCornerIcon.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.RegionFilter.remove_SettingChanged((EventHandler<ValueChangedEventArgs<ProfileRegion>>)OnRegionFilterChanged);
			_settings.ShowMatureProfiles.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
		}

		private void OnSettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isUnloaded)
				{
					SyncCheckboxesFromSettings();
				}
			});
		}

		private void SyncCheckboxesFromSettings()
		{
			SetChecked(_shareCheckbox, _settings.BroadcastProfile.get_Value());
			SetChecked(_hideLocationCheckbox, _settings.HideLocation.get_Value());
			SetChecked(_showNearbyCheckbox, _settings.ShowNearbyPresence.get_Value());
			SetChecked(_autoHideCheckbox, _settings.AutoHideGameUi.get_Value());
			SetChecked(_cornerIconCheckbox, _settings.ShowCornerIcon.get_Value());
			SyncRegionDropdownFromSettings();
			SyncMatureButtonFromSettings();
		}

		private static void SetChecked(Checkbox checkbox, bool value)
		{
			if (checkbox != null && checkbox.get_Checked() != value)
			{
				checkbox.set_Checked(value);
			}
		}

		private void OnRegionFilterChanged(object sender, ValueChangedEventArgs<ProfileRegion> e)
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isUnloaded)
				{
					SyncRegionDropdownFromSettings();
				}
			});
		}

		private void SyncRegionDropdownFromSettings()
		{
			if (_regionDropdown != null)
			{
				string label = _settings.RegionFilter.get_Value().ToString();
				if (!string.Equals(_regionDropdown.get_SelectedItem()?.ToString(), label, StringComparison.Ordinal))
				{
					_regionDropdown.set_SelectedItem(label);
				}
			}
		}

		private void SyncMatureButtonFromSettings()
		{
			if (_matureProfilesButton != null)
			{
				_matureProfilesButton.set_Text(_matureProfilesConfirm.ButtonText);
			}
		}

		protected override void Unload()
		{
			_isUnloaded = true;
			UnwatchSettings();
			_shareCheckbox = null;
			_hideLocationCheckbox = null;
			_showNearbyCheckbox = null;
			_autoHideCheckbox = null;
			_cornerIconCheckbox = null;
			_matureProfilesConfirm.Dispose();
			_regionDropdown = null;
			_matureProfilesButton = null;
		}
	}
}
