using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using rp.spark.Models;
using rp.spark.Services;

namespace rp.spark.UI.Views
{
	public class ProfileManagementView : View
	{
		private const int FormWidth = 760;

		private const int FormHeight = 360;

		private const int StatusY = 390;

		private readonly ProfileEditorSession _session;

		private Label _status;

		private Label _activeStatus;

		private Label _profileTip;

		private Dropdown _profileDropdown;

		private Dropdown _importChar;

		private Dropdown _importProfile;

		private StandardButton _importButton;

		private TextBox _profileName;

		private Dictionary<string, string> _profileOptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private Dictionary<string, string> _importOptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private bool _isRefreshing;

		private bool _deleteConfirmArmed;

		private CancellationTokenSource _importRefreshCancel;

		public ProfileManagementView(ProfileEditorSession session)
			: this()
		{
			_session = session;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			if (!_session.State.CanEditProfile)
			{
				ProfileEditorUI.ShowUnavailableMessage(buildPanel);
				return;
			}
			FlowPanel form = SparkFormLayout.AddVerticalStack(buildPanel, 0, 0, 760, 360, 15);
			FlowPanel profileGroup = SparkFormLayout.AddAutoStack((Container)(object)form, 760, 0);
			SparkFormLayout.AddLabel((Container)(object)profileGroup, "Profile", 760);
			_profileDropdown = SparkFormLayout.AddDropdown((Container)(object)profileGroup, new string[0], null, 300);
			_profileDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (!_isRefreshing)
				{
					string key = _profileDropdown.get_SelectedItem()?.ToString() ?? string.Empty;
					if (_profileOptions.TryGetValue(key, out var value))
					{
						_session.SelectProfile(value);
					}
				}
			});
			FlowPanel parent = SparkFormLayout.AddAutoStack((Container)(object)form, 760, 0);
			SparkFormLayout.AddLabel((Container)(object)parent, "Profile Name", 760);
			FlowPanel nameRow = SparkFormLayout.AddRow((Container)(object)parent, 760, 35, 25);
			_profileName = SparkFormLayout.AddTextBox((Container)(object)nameRow, string.Empty, string.Empty, 360, 35, 30);
			((TextInputBase)_profileName).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				if (!_isRefreshing)
				{
					_session.Profile.ProfileName = ((TextInputBase)_profileName).get_Text()?.Trim() ?? string.Empty;
				}
			});
			_activeStatus = SparkFormLayout.AddLabel((Container)(object)nameRow, string.Empty, 320, 30, GameService.Content.get_DefaultFont16(), (Color?)new Color(220, 220, 220), strokeText: false);
			BuildActions((Container)(object)form);
			BuildFooter(buildPanel);
			_session.StatusChanged += HandleStatusChanged;
			_session.ProfileChanged += HandleProfileChanged;
			_session.ImportsChanged += HandleImportsChanged;
			RefreshFromSession();
			StartImportRefresh();
		}

		private void BuildActions(Container buildPanel)
		{
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel parent = SparkFormLayout.AddRow(buildPanel, 760, 35, 15);
			((Control)SparkFormLayout.AddButton((Container)(object)parent, "New", 95)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RunProfileAction(delegate
				{
					_session.CreateProfile();
				});
			});
			((Control)SparkFormLayout.AddButton((Container)(object)parent, "Duplicate", 115)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RunProfileAction(delegate
				{
					_session.DuplicateProfile();
				});
			});
			SparkUiActions.BindClick(SparkFormLayout.AddButton((Container)(object)parent, "Save", 95), async delegate
			{
				_deleteConfirmArmed = false;
				await _session.SaveAsync();
			}, _session.SetStatus, "Couldn't save profile.");
			SparkUiActions.BindClick(SparkFormLayout.AddButton((Container)(object)parent, "Set Active", 115), async delegate
			{
				_deleteConfirmArmed = false;
				await _session.SetActiveAsync();
			}, _session.SetStatus, "Couldn't set active profile.");
			((Control)SparkFormLayout.AddButton((Container)(object)parent, "Delete", 95)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!_deleteConfirmArmed)
				{
					_deleteConfirmArmed = true;
					_session.SetStatus("Click Delete again to permanently delete " + GetProfileName(_session.Profile) + ".");
				}
				else
				{
					RunProfileAction(delegate
					{
						_session.DeleteProfile();
					});
				}
			});
			BuildImport(buildPanel);
			_profileTip = SparkFormLayout.AddLabel(buildPanel, string.Empty, 760, 50, GameService.Content.get_DefaultFont14(), SparkViewUI.SecondaryTextColor);
			_profileTip.set_WrapText(true);
		}

		private void BuildImport(Container buildPanel)
		{
			FlowPanel parent = SparkFormLayout.AddAutoStack(buildPanel, 760);
			SparkFormLayout.AddLabel((Container)(object)parent, "Import from another character", 760);
			FlowPanel importRow = SparkFormLayout.AddRow((Container)(object)parent, 760, 35);
			_importChar = SparkFormLayout.AddDropdown((Container)(object)importRow, new string[0], null, 250);
			_importProfile = SparkFormLayout.AddDropdown((Container)(object)importRow, new string[0], null, 250);
			((Control)_importProfile).set_Enabled(false);
			_importButton = SparkFormLayout.AddButton((Container)(object)importRow, "Import Profile", 140, 35, enabled: false);
			_importChar.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (!_isRefreshing)
				{
					RefreshImportProfiles();
				}
			});
			_importProfile.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (!_isRefreshing)
				{
					UpdateImportButton();
				}
			});
			SparkUiActions.BindClick(_importButton, async delegate
			{
				_deleteConfirmArmed = false;
				await _session.ImportAsync(GetSelectedImportId());
			}, _session.SetStatus, "Couldn't import profile.");
		}

		private void BuildFooter(Container buildPanel)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_status = ProfileEditorUI.AddStatusLabel(buildPanel, _session.StatusText, new Point(0, 390), new Point(760, 30));
			ProfileEditorUI.AddHeaderLabel(buildPanel, _session);
		}

		private void HandleStatusChanged(string statusText)
		{
			SparkUiThread.Queue(delegate
			{
				Label status = _status;
				if (((status != null) ? ((Control)status).get_Parent() : null) != null)
				{
					_status.set_Text(statusText ?? string.Empty);
				}
			});
		}

		private void HandleProfileChanged()
		{
			SparkUiThread.Queue(delegate
			{
				Dropdown profileDropdown = _profileDropdown;
				if (((profileDropdown != null) ? ((Control)profileDropdown).get_Parent() : null) != null)
				{
					_deleteConfirmArmed = false;
					RefreshFromSession();
				}
			});
		}

		private void HandleImportsChanged()
		{
			SparkUiThread.Queue(delegate
			{
				Dropdown importChar = _importChar;
				if (((importChar != null) ? ((Control)importChar).get_Parent() : null) != null)
				{
					RefreshFromSession();
				}
			});
		}

		private void StartImportRefresh()
		{
			_importRefreshCancel?.Cancel();
			_importRefreshCancel = new CancellationTokenSource();
			RefreshImportsSoonAsync(_importRefreshCancel.Token);
		}

		private async Task RefreshImportsSoonAsync(CancellationToken cancellationToken)
		{
			_ = 1;
			try
			{
				for (int attempt = 0; attempt < 24; attempt++)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						break;
					}
					if (_session.HasImportState)
					{
						break;
					}
					await _session.RefreshImportsAsync();
					if (_session.HasImportState)
					{
						break;
					}
					await Task.Delay(5000, cancellationToken);
				}
			}
			catch (OperationCanceledException)
			{
			}
		}

		private void RefreshFromSession()
		{
			_isRefreshing = true;
			try
			{
				RefreshProfileDropdown();
				RefreshImportDropdowns();
				((TextInputBase)_profileName).set_Text(GetProfileName(_session.Profile));
				_activeStatus.set_Text(_session.IsSelectedProfileActive ? "Active for broadcast" : "Not active for broadcast");
				_profileTip.set_Text(GetProfileTip());
			}
			finally
			{
				_isRefreshing = false;
			}
		}

		private void RefreshProfileDropdown()
		{
			_profileOptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			_profileDropdown.get_Items().Clear();
			List<string> duplicateNames = (from @group in _session.Profiles.GroupBy((CharacterProfile profile) => GetProfileName(profile), StringComparer.OrdinalIgnoreCase)
				where @group.Count() > 1
				select @group.Key).ToList();
			string selectedLabel = null;
			foreach (CharacterProfile profile2 in _session.Profiles)
			{
				string label = GetProfileOptionLabel(profile2, duplicateNames);
				_profileOptions[label] = profile2.ProfileId;
				_profileDropdown.get_Items().Add(label);
				if (string.Equals(profile2.ProfileId, _session.Profile.ProfileId, StringComparison.OrdinalIgnoreCase))
				{
					selectedLabel = label;
				}
			}
			if (!string.IsNullOrWhiteSpace(selectedLabel))
			{
				_profileDropdown.set_SelectedItem(selectedLabel);
			}
		}

		private void RefreshImportDropdowns()
		{
			if (_importChar == null || _importProfile == null || _importButton == null)
			{
				return;
			}
			string selectedChar = _importChar.get_SelectedItem()?.ToString();
			_importChar.get_Items().Clear();
			if (_session.ImportGroups.Count == 0)
			{
				string emptyText = (_session.HasImportState ? "No profiles found" : "Waiting for SPARK sync");
				_importChar.get_Items().Add(emptyText);
				_importChar.set_SelectedItem(emptyText);
				((Control)_importChar).set_Enabled(false);
				_importProfile.get_Items().Clear();
				((Control)_importProfile).set_Enabled(false);
				((Control)_importButton).set_Enabled(false);
				_importOptions.Clear();
				return;
			}
			foreach (ProfileImportGroup group2 in _session.ImportGroups)
			{
				_importChar.get_Items().Add(group2.CharacterName);
			}
			if (string.IsNullOrWhiteSpace(selectedChar) || !_session.ImportGroups.Any((ProfileImportGroup group) => string.Equals(group.CharacterName, selectedChar, StringComparison.OrdinalIgnoreCase)))
			{
				selectedChar = _session.ImportGroups[0].CharacterName;
			}
			((Control)_importChar).set_Enabled(true);
			_importChar.set_SelectedItem(selectedChar);
			RefreshImportProfiles();
		}

		private void RefreshImportProfiles()
		{
			_importOptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			_importProfile.get_Items().Clear();
			string selectedChar = _importChar.get_SelectedItem()?.ToString() ?? string.Empty;
			ProfileImportGroup group = _session.ImportGroups.FirstOrDefault((ProfileImportGroup importGroup) => string.Equals(importGroup.CharacterName, selectedChar, StringComparison.OrdinalIgnoreCase));
			if (group == null || group.Profiles.Count == 0)
			{
				((Control)_importProfile).set_Enabled(false);
				((Control)_importButton).set_Enabled(false);
				return;
			}
			foreach (CharacterProfile profile in group.Profiles)
			{
				string label = GetProfileName(profile);
				_importOptions[label] = profile.ProfileId;
				_importProfile.get_Items().Add(label);
			}
			((Control)_importProfile).set_Enabled(true);
			_importProfile.set_SelectedItem((group.Profiles.Count > 0) ? GetProfileName(group.Profiles[0]) : null);
			UpdateImportButton();
		}

		private void UpdateImportButton()
		{
			if (_importButton != null)
			{
				((Control)_importButton).set_Enabled(!string.IsNullOrWhiteSpace(GetSelectedImportId()));
			}
		}

		private string GetSelectedImportId()
		{
			Dropdown importProfile = _importProfile;
			string selectedItem = ((importProfile == null) ? null : importProfile.get_SelectedItem()?.ToString()) ?? string.Empty;
			if (!_importOptions.TryGetValue(selectedItem, out var profileId))
			{
				return string.Empty;
			}
			return profileId;
		}

		private void RunProfileAction(Action action)
		{
			_deleteConfirmArmed = false;
			try
			{
				action();
			}
			catch (Exception ex)
			{
				_session.SetStatus(ex.Message);
			}
		}

		protected override void Unload()
		{
			_importRefreshCancel?.Cancel();
			_importRefreshCancel?.Dispose();
			_importRefreshCancel = null;
			_session.StatusChanged -= HandleStatusChanged;
			_session.ProfileChanged -= HandleProfileChanged;
			_session.ImportsChanged -= HandleImportsChanged;
		}

		private string GetProfileOptionLabel(CharacterProfile profile, IEnumerable<string> duplicateNames)
		{
			string name = GetProfileName(profile);
			string label = (duplicateNames.Contains(name, StringComparer.OrdinalIgnoreCase) ? (name + " [" + profile.ProfileId.Substring(0, Math.Min(4, profile.ProfileId.Length)) + "]") : name);
			if (!string.IsNullOrWhiteSpace(_session.ActiveProfileId) && string.Equals(profile.ProfileId, _session.ActiveProfileId, StringComparison.OrdinalIgnoreCase))
			{
				label += " (active)";
			}
			return label;
		}

		private static string GetProfileName(CharacterProfile profile)
		{
			if (!string.IsNullOrWhiteSpace(profile?.ProfileName))
			{
				return profile.ProfileName.Trim();
			}
			return "Default";
		}

		private string GetProfileTip()
		{
			if (_session.Profiles.Count == 0)
			{
				return "You have no profiles! Click the 'New' button to start!";
			}
			if (string.IsNullOrWhiteSpace(_session.ActiveProfileId))
			{
				return "You need to set your profile to active for it to be broadcast. Your active profile is the one shown to others.";
			}
			return "Tip: You can make multiple profiles per character and set whichever one to active depending on who you want to RP as today!";
		}
	}
}
