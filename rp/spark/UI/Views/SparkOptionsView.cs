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

		private static readonly string[] ProfileTooltipLineOptions = new string[10] { "2", "4", "6", "8", "10", "12", "14", "16", "18", "20" };

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

		private Checkbox _showKnownForTooltipsCheckbox;

		private Checkbox _showCurrentlyTooltipsCheckbox;

		private Checkbox _showOocTooltipsCheckbox;

		private Checkbox _trimLongTooltipsCheckbox;

		private Dropdown _profileTooltipLinesDropdown;

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
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0397: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel stack = SparkFormLayout.AddVerticalStack(buildPanel, 8, 8, 660, 560, 8, canScroll: true);
			SparkFormLayout.AddLabel((Container)(object)stack, "Privacy", 660, 28, GameService.Content.get_DefaultFont18(), (Color?)new Color(255, 233, 180), strokeText: true);
			_shareCheckbox = SparkFormLayout.AddCheckbox((Container)(object)stack, "Share my profile", _settings.BroadcastProfile.get_Value(), 220);
			((Control)_shareCheckbox).set_BasicTooltipText("When unchecked, your profile will not be uploaded to SPARK. This means anyone viewing a local copy of your profile will not receive updates, even if you're set to Invisible.");
			_shareCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (_settings.BroadcastProfile.get_Value() != _shareCheckbox.get_Checked())
				{
					_settings.BroadcastProfile.set_Value(_shareCheckbox.get_Checked());
					_requestServerSync?.Invoke();
				}
			});
			_hideLocationCheckbox = SparkFormLayout.AddCheckbox((Container)(object)stack, "Hide my location", _settings.HideLocation.get_Value(), 220);
			((Control)_hideLocationCheckbox).set_BasicTooltipText("When checked, your location will be set to 'Hidden' for all location fields in SPARK.");
			_hideLocationCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (_settings.HideLocation.get_Value() != _hideLocationCheckbox.get_Checked())
				{
					_settings.HideLocation.set_Value(_hideLocationCheckbox.get_Checked());
					_requestServerSync?.Invoke();
				}
			});
			_showNearbyCheckbox = SparkFormLayout.AddCheckbox((Container)(object)stack, "Show me nearby", _settings.ShowNearbyPresence.get_Value(), 220);
			((Control)_showNearbyCheckbox).set_BasicTooltipText("When checked, others using the Nearby Players window will be able to see you if you're on the same map and how far away you are.");
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
			((Control)_regionDropdown).set_BasicTooltipText("Set which region to broadcast your profile to in order for players in the same region to be able to find and contact you.");
			_regionDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (Enum.TryParse<ProfileRegion>(_regionDropdown.get_SelectedItem()?.ToString(), out var result2) && _settings.RegionFilter.get_Value() != result2)
				{
					_settings.RegionFilter.set_Value(result2);
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
			((Control)_autoHideCheckbox).set_BasicTooltipText("Closes all SPARK windows when loading new maps or on character select.");
			_autoHideCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (_settings.AutoHideGameUi.get_Value() != _autoHideCheckbox.get_Checked())
				{
					_settings.AutoHideGameUi.set_Value(_autoHideCheckbox.get_Checked());
				}
			});
			_cornerIconCheckbox = SparkFormLayout.AddCheckbox((Container)(object)stack, "Show SPARK icon", _settings.ShowCornerIcon.get_Value(), 220);
			((Control)_cornerIconCheckbox).set_BasicTooltipText("Displays an icon with quick access to SPARK windows and settings at the top of the screen.");
			_cornerIconCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (_settings.ShowCornerIcon.get_Value() != _cornerIconCheckbox.get_Checked())
				{
					_settings.ShowCornerIcon.set_Value(_cornerIconCheckbox.get_Checked());
				}
			});
			SparkFormLayout.AddSpacer((Container)(object)stack, 660, 8);
			SparkFormLayout.AddLabel((Container)(object)stack, "Profile Tooltips", 660, 28, GameService.Content.get_DefaultFont18(), (Color?)new Color(255, 233, 180), strokeText: true);
			_showKnownForTooltipsCheckbox = SparkFormLayout.AddCheckbox((Container)(object)stack, "Include Known For in tooltips", _settings.ShowKnownForInProfileTooltips.get_Value(), 260);
			((Control)_showKnownForTooltipsCheckbox).set_BasicTooltipText("Shows Known For information in tooltips when a profile provides it.");
			_showKnownForTooltipsCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (_settings.ShowKnownForInProfileTooltips.get_Value() != _showKnownForTooltipsCheckbox.get_Checked())
				{
					_settings.ShowKnownForInProfileTooltips.set_Value(_showKnownForTooltipsCheckbox.get_Checked());
				}
			});
			_showCurrentlyTooltipsCheckbox = SparkFormLayout.AddCheckbox((Container)(object)stack, "Include Currently (in character) in tooltips", _settings.ShowCurrentlyInProfileTooltips.get_Value(), 260);
			((Control)_showCurrentlyTooltipsCheckbox).set_BasicTooltipText("Shows Currently (in character) information in tooltips when a profile provides it.");
			_showCurrentlyTooltipsCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (_settings.ShowCurrentlyInProfileTooltips.get_Value() != _showCurrentlyTooltipsCheckbox.get_Checked())
				{
					_settings.ShowCurrentlyInProfileTooltips.set_Value(_showCurrentlyTooltipsCheckbox.get_Checked());
				}
			});
			_showOocTooltipsCheckbox = SparkFormLayout.AddCheckbox((Container)(object)stack, "Include Player Information (out of character) in tooltips", _settings.ShowOocInfoInProfileTooltips.get_Value(), 260);
			((Control)_showOocTooltipsCheckbox).set_BasicTooltipText("Show Player Information (out of character) information in tooltips when a profile provides it.");
			_showOocTooltipsCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (_settings.ShowOocInfoInProfileTooltips.get_Value() != _showOocTooltipsCheckbox.get_Checked())
				{
					_settings.ShowOocInfoInProfileTooltips.set_Value(_showOocTooltipsCheckbox.get_Checked());
				}
			});
			FlowPanel tooltipLinesRow = SparkFormLayout.AddRow((Container)(object)stack, 660, 30, 8);
			_trimLongTooltipsCheckbox = SparkFormLayout.AddCheckbox((Container)(object)tooltipLinesRow, "Trim long profile tooltips", _settings.TrimLongProfileTooltips.get_Value(), 245);
			((Control)_trimLongTooltipsCheckbox).set_BasicTooltipText("Limits each enabled tooltip section to the configured number of wrapped lines.");
			_trimLongTooltipsCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (_settings.TrimLongProfileTooltips.get_Value() != _trimLongTooltipsCheckbox.get_Checked())
				{
					_settings.TrimLongProfileTooltips.set_Value(_trimLongTooltipsCheckbox.get_Checked());
				}
				SyncTooltipSettings();
			});
			_profileTooltipLinesDropdown = SparkFormLayout.AddDropdown((Container)(object)tooltipLinesRow, ProfileTooltipLineOptions, SparkSettings.ProfileTooltipLimit(_settings.ProfileTooltipLinesPerSection.get_Value()).ToString(), 70, 30);
			SparkFormLayout.AddLabel((Container)(object)tooltipLinesRow, "max lines per section", 145, 30, GameService.Content.get_DefaultFont14());
			((Control)_profileTooltipLinesDropdown).set_BasicTooltipText("Applied separately to Known For, Currently, and Out of Character.");
			_profileTooltipLinesDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (int.TryParse(_profileTooltipLinesDropdown.get_SelectedItem()?.ToString(), out var result))
				{
					result = SparkSettings.ProfileTooltipLimit(result);
					if (_settings.ProfileTooltipLinesPerSection.get_Value() != result)
					{
						_settings.ProfileTooltipLinesPerSection.set_Value(result);
					}
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
			_settings.ShowKnownForInProfileTooltips.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.ShowCurrentlyInProfileTooltips.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.ShowOocInfoInProfileTooltips.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.TrimLongProfileTooltips.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.ProfileTooltipLinesPerSection.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnTooltipLineLimitChanged);
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
			_settings.ShowKnownForInProfileTooltips.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.ShowCurrentlyInProfileTooltips.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.ShowOocInfoInProfileTooltips.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.TrimLongProfileTooltips.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSettingChanged);
			_settings.ProfileTooltipLinesPerSection.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnTooltipLineLimitChanged);
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
			SetChecked(_showKnownForTooltipsCheckbox, _settings.ShowKnownForInProfileTooltips.get_Value());
			SetChecked(_showCurrentlyTooltipsCheckbox, _settings.ShowCurrentlyInProfileTooltips.get_Value());
			SetChecked(_showOocTooltipsCheckbox, _settings.ShowOocInfoInProfileTooltips.get_Value());
			SetChecked(_trimLongTooltipsCheckbox, _settings.TrimLongProfileTooltips.get_Value());
			SyncTooltipSettings();
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

		private void OnTooltipLineLimitChanged(object sender, ValueChangedEventArgs<int> e)
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isUnloaded)
				{
					SyncTooltipSettings();
				}
			});
		}

		private void SyncTooltipSettings()
		{
			if (_profileTooltipLinesDropdown != null)
			{
				int normalizedLimit = SparkSettings.ProfileTooltipLimit(_settings.ProfileTooltipLinesPerSection.get_Value());
				if (_settings.ProfileTooltipLinesPerSection.get_Value() != normalizedLimit)
				{
					_settings.ProfileTooltipLinesPerSection.set_Value(normalizedLimit);
				}
				string limitText = normalizedLimit.ToString();
				if (!string.Equals(_profileTooltipLinesDropdown.get_SelectedItem()?.ToString(), limitText, StringComparison.Ordinal))
				{
					_profileTooltipLinesDropdown.set_SelectedItem(limitText);
				}
				((Control)_profileTooltipLinesDropdown).set_Enabled(_settings.TrimLongProfileTooltips.get_Value());
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
			_showKnownForTooltipsCheckbox = null;
			_showCurrentlyTooltipsCheckbox = null;
			_showOocTooltipsCheckbox = null;
			_trimLongTooltipsCheckbox = null;
			_profileTooltipLinesDropdown = null;
			_matureProfilesConfirm.Dispose();
			_regionDropdown = null;
			_matureProfilesButton = null;
		}
	}
}
