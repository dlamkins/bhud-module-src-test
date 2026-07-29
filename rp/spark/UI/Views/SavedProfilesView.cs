using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using rp.spark.Models;
using rp.spark.Services;
using rp.spark.UI.Controls;

namespace rp.spark.UI.Views
{
	public class SavedProfilesView : View
	{
		private const int RowHeight = 40;

		private const string SortViewed = "Viewed";

		private const string SortBookmarked = "Bookmarked";

		private static readonly TimeSpan LivePresenceWindow = TimeSpan.FromSeconds(45.0);

		private readonly Func<IReadOnlyList<SavedProfileSummary>> _loadSavedProfiles;

		private readonly Action<SavedProfileSummary, Action<string>> _openProfile;

		private readonly Action<SavedProfileSummary> _removeBookmark;

		private readonly Func<string, bool> _isBlockedAccount;

		private readonly Func<bool> _showMatureProfiles;

		private readonly Action<Action> _watchSavedProfiles;

		private readonly Action<Action> _unwatchSavedProfiles;

		private readonly SavedProfilesMode _mode;

		private readonly SparkSettings _settings;

		private readonly PageList _page = new PageList();

		private PageListControls _pageControls;

		private bool _isUnloaded;

		private TextBox _searchBox;

		private Dropdown _searchFieldDropdown;

		private Dropdown _sortDropdown;

		private ProfileFilterMenu _discoveryFilters;

		private ProfileScrollList _profileList;

		private Label _status;

		private string _statusOverride = string.Empty;

		public SavedProfilesView(Func<IReadOnlyList<SavedProfileSummary>> getSavedProfiles, Action<SavedProfileSummary, Action<string>> openProfile, SavedProfilesMode mode, SparkSettings settings, Func<string, bool> isBlockedAccount = null, Action<SavedProfileSummary> removeBookmark = null, Action<Action> watchSavedProfilesChanged = null, Action<Action> unwatchSavedProfilesChanged = null, Func<bool> showMatureProfiles = null)
			: this()
		{
			_loadSavedProfiles = getSavedProfiles;
			_openProfile = openProfile;
			_mode = mode;
			_settings = settings;
			_isBlockedAccount = isBlockedAccount;
			_showMatureProfiles = showMatureProfiles;
			_removeBookmark = removeBookmark;
			_watchSavedProfiles = watchSavedProfilesChanged;
			_unwatchSavedProfiles = unwatchSavedProfilesChanged;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			_isUnloaded = false;
			ProfileListViewUI.AddTitle(buildPanel, (_mode == SavedProfilesMode.Bookmarks) ? "Bookmarked Profiles" : "Recent Profiles", 320);
			((Control)ProfileListViewUI.AddRefreshButton(buildPanel)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RefreshRows(resetPage: false);
			});
			BuildSearchControls(buildPanel);
			BuildHeader(buildPanel);
			_watchSavedProfiles?.Invoke(HandleSavedProfilesChanged);
			WatchTooltipSettings();
			ProfileScrollList profileScrollList = new ProfileScrollList(760, 400, 40);
			((Control)profileScrollList).set_Location(new Point(0, 128));
			((Control)profileScrollList).set_Parent(buildPanel);
			_profileList = profileScrollList;
			_pageControls = new PageListControls(buildPanel, _page, 760, delegate
			{
				RefreshRows(resetPage: false);
			})
			{
				Location = new Point(0, 532)
			};
			_status = ProfileListViewUI.AddStatusLabel(buildPanel);
			RefreshRows(resetPage: true);
		}

		private void BuildSearchControls(Container parent)
		{
			string[] sortOptions = ((_mode != SavedProfilesMode.Bookmarks) ? new string[6] { "Viewed", "Recently Seen", "Name", "Race", "Account", "Bookmarked" } : new string[6] { "Bookmarked", "Recently Seen", "Name", "Race", "Account", "Viewed" });
			ProfileListSearchControls controls = ProfileListViewUI.AddSearchControls(parent, "Search saved profiles", new string[5] { "All fields", "Name", "Account", "Race", "Profession" }, sortOptions, sortOptions[0], delegate
			{
				RefreshRows(resetPage: true);
			});
			_searchBox = controls.SearchBox;
			_searchFieldDropdown = controls.SearchFieldDropdown;
			_sortDropdown = controls.SortDropdown;
			_discoveryFilters = new ProfileFilterMenu(parent, delegate
			{
				RefreshRows(resetPage: true);
			});
		}

		private void BuildHeader(Container parent)
		{
			if (_mode == SavedProfilesMode.Bookmarks)
			{
				ProfileListViewUI.AddHeader(parent, "Character", 0, 215);
				ProfileListViewUI.AddHeader(parent, "Race", 225, 95);
				ProfileListViewUI.AddHeader(parent, "Account", 330, 160);
				ProfileListViewUI.AddHeader(parent, "Bookmarked", 500, 135);
				ProfileListViewUI.AddHeader(parent, "Action", 650, 100);
			}
			else
			{
				ProfileListViewUI.AddHeader(parent, "Character", 28, 212);
				ProfileListViewUI.AddHeader(parent, "Race", 250, 110);
				ProfileListViewUI.AddHeader(parent, "Account", 370, 170);
				ProfileListViewUI.AddHeader(parent, "Saved", 550, 205);
			}
		}

		private void HandleSavedProfilesChanged()
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isUnloaded)
				{
					RefreshRows(resetPage: false, resetScroll: false);
				}
			});
		}

		private void WatchTooltipSettings()
		{
			if (_settings != null)
			{
				_settings.ShowKnownForInProfileTooltips.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
				_settings.ShowCurrentlyInProfileTooltips.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
				_settings.ShowOocInfoInProfileTooltips.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
				_settings.TrimLongProfileTooltips.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
				_settings.ProfileTooltipLinesPerSection.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnTooltipLineLimitChanged);
			}
		}

		private void UnwatchTooltipSettings()
		{
			if (_settings != null)
			{
				_settings.ShowKnownForInProfileTooltips.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
				_settings.ShowCurrentlyInProfileTooltips.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
				_settings.ShowOocInfoInProfileTooltips.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
				_settings.TrimLongProfileTooltips.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
				_settings.ProfileTooltipLinesPerSection.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnTooltipLineLimitChanged);
			}
		}

		private void OnTooltipVisibilityChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			RebuildTooltips();
		}

		private void OnTooltipLineLimitChanged(object sender, ValueChangedEventArgs<int> e)
		{
			RebuildTooltips();
		}

		private void RebuildTooltips()
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isUnloaded && _profileList != null)
				{
					RefreshRows(resetPage: false, resetScroll: false);
				}
			});
		}

		private void RefreshRows(bool resetPage, bool resetScroll = true)
		{
			if (_isUnloaded || _profileList == null)
			{
				return;
			}
			_profileList.ClearRows(resetScroll);
			IEnumerable<SavedProfileSummary> filteredRows = from savedProfile in (from savedProfile in _loadSavedProfiles?.Invoke() ?? new List<SavedProfileSummary>()
					where savedProfile != null
					where !IsHidden(savedProfile)
					select savedProfile).Where(MatchesSearch)
				where _discoveryFilters == null || _discoveryFilters.Matches(savedProfile.Experience, savedProfile.DiscoveryTags)
				select savedProfile;
			List<SavedProfileSummary> rows = SortRows(filteredRows).ToList();
			string statusOverride = _statusOverride;
			if (resetPage)
			{
				_page.Reset();
			}
			_page.Clamp(rows.Count);
			_pageControls?.Update(rows.Count);
			if (rows.Count == 0)
			{
				_profileList.ShowEmptyMessage((_mode == SavedProfilesMode.Bookmarks) ? GetEmptyBookmarksMessage() : GetEmptyRecentMessage());
				_status.set_Text((!string.IsNullOrWhiteSpace(statusOverride)) ? statusOverride : ((_mode == SavedProfilesMode.Bookmarks) ? "0 matching bookmarks." : "0 matching recent profiles."));
				return;
			}
			IReadOnlyList<SavedProfileSummary> pageRows = _page.GetPage(rows);
			for (int index = 0; index < pageRows.Count; index++)
			{
				AddRow(pageRows[index], index);
			}
			string searchSuffix = ProfileListViewUI.GetSearchSuffix(_searchBox);
			_status.set_Text((!string.IsNullOrWhiteSpace(statusOverride)) ? statusOverride : ((_mode == SavedProfilesMode.Bookmarks) ? $"{rows.Count} bookmarked profile(s){searchSuffix}." : $"{rows.Count} recent profile(s){searchSuffix}."));
		}

		private void AddRow(SavedProfileSummary savedProfile, int index)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			Panel row = _profileList.AddRow(index, string.Empty);
			Color secondary = default(Color);
			((Color)(ref secondary))._002Ector(220, 220, 220);
			if (_mode == SavedProfilesMode.Bookmarks)
			{
				_profileList.AddCell((Container)(object)row, ProfileText.SavedCharacterName(savedProfile), 8, 8, 210, Color.get_White());
				_profileList.AddCell((Container)(object)row, ProfileText.SavedRace(savedProfile), 225, 8, 95, secondary);
				_profileList.AddCell((Container)(object)row, ProfileText.SavedAccountName(savedProfile), 330, 8, 160, secondary);
				_profileList.AddCell((Container)(object)row, GetSavedTime(savedProfile), 500, 8, 135, secondary);
				AddRemoveButton((Container)(object)row, savedProfile);
			}
			else
			{
				AddBookmarkMarker((Container)(object)row, savedProfile);
				_profileList.AddCell((Container)(object)row, ProfileText.SavedCharacterName(savedProfile), 30, 8, 210, Color.get_White());
				_profileList.AddCell((Container)(object)row, ProfileText.SavedRace(savedProfile), 250, 8, 110, secondary);
				_profileList.AddCell((Container)(object)row, ProfileText.SavedAccountName(savedProfile), 370, 8, 170, secondary);
				_profileList.AddCell((Container)(object)row, GetSavedTime(savedProfile), 550, 8, 205, secondary);
			}
			ProfileScrollList.AddInteractionLayer((Container)(object)row, MakeTooltip(savedProfile), delegate
			{
				_openProfile?.Invoke(savedProfile, ShowStatusOverride);
			}, (_mode == SavedProfilesMode.Bookmarks) ? 110 : 0);
		}

		private static void AddBookmarkMarker(Container row, SavedProfileSummary savedProfile)
		{
			if (savedProfile != null && savedProfile.IsBookmarked)
			{
				ProfileListViewUI.AddBookmarkMarker(row);
			}
		}

		private void AddRemoveButton(Container row, SavedProfileSummary savedProfile)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			StandardButton val = new StandardButton();
			val.set_Text("Remove");
			((Control)val).set_Location(new Point(650, 5));
			((Control)val).set_Size(new Point(90, 30));
			((Control)val).set_Parent(row);
			((Control)val).set_ZIndex(101);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_removeBookmark == null)
				{
					_status.set_Text("Bookmark cache unavailable.");
				}
				else
				{
					_removeBookmark(savedProfile);
					RefreshRows(resetPage: false);
					_status.set_Text("Bookmark removed.");
				}
			});
		}

		private void MakeClickable(Control control, SavedProfileSummary savedProfile, string tooltipText)
		{
			ProfileScrollList.WireInteraction(control, tooltipText, delegate
			{
				_openProfile?.Invoke(savedProfile, ShowStatusOverride);
			});
		}

		private void ShowStatusOverride(string message)
		{
			_statusOverride = message ?? string.Empty;
			if (_status != null && !_isUnloaded)
			{
				_status.set_Text(_statusOverride);
			}
		}

		private bool IsHidden(SavedProfileSummary savedProfile)
		{
			bool num = _isBlockedAccount != null && _isBlockedAccount(ProfileText.SavedAccountName(savedProfile));
			bool matureHidden = savedProfile != null && savedProfile.IsMature && !(_showMatureProfiles?.Invoke() ?? false);
			return num || matureHidden;
		}

		private bool MatchesSearch(SavedProfileSummary savedProfile)
		{
			TextBox searchBox = _searchBox;
			string query = ((searchBox == null) ? null : ((TextInputBase)searchBox).get_Text()?.Trim()) ?? string.Empty;
			if (string.IsNullOrWhiteSpace(query))
			{
				return true;
			}
			return GetSearchText(savedProfile).IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private string GetSearchText(SavedProfileSummary savedProfile)
		{
			Dropdown searchFieldDropdown = _searchFieldDropdown;
			return (((searchFieldDropdown == null) ? null : searchFieldDropdown.get_SelectedItem()?.ToString()) ?? "All fields") switch
			{
				"Name" => ProfileText.JoinSearchText(ProfileText.SavedCharacterName(savedProfile), savedProfile?.ProfileName), 
				"Account" => ProfileText.SavedAccountName(savedProfile), 
				"Race" => ProfileText.SavedRace(savedProfile), 
				"Profession" => ProfileText.SavedProfession(savedProfile), 
				_ => ProfileText.JoinSearchText(ProfileText.SavedCharacterName(savedProfile), savedProfile?.ProfileName, ProfileText.SavedAccountName(savedProfile), ProfileText.SavedRace(savedProfile), ProfileText.SavedProfession(savedProfile)), 
			};
		}

		private IEnumerable<SavedProfileSummary> SortRows(IEnumerable<SavedProfileSummary> savedProfiles)
		{
			Dropdown sortDropdown = _sortDropdown;
			return (((sortDropdown == null) ? null : sortDropdown.get_SelectedItem()?.ToString()) ?? ((_mode == SavedProfilesMode.Bookmarks) ? "Bookmarked" : "Viewed")) switch
			{
				"Name" => savedProfiles.OrderBy(ProfileText.SavedCharacterName, StringComparer.OrdinalIgnoreCase).ThenBy((SavedProfileSummary savedProfile) => ProfileText.SavedAccountName(savedProfile), StringComparer.OrdinalIgnoreCase), 
				"Race" => savedProfiles.OrderBy((SavedProfileSummary savedProfile) => ProfileText.SavedRace(savedProfile), StringComparer.OrdinalIgnoreCase).ThenBy(ProfileText.SavedCharacterName, StringComparer.OrdinalIgnoreCase), 
				"Account" => savedProfiles.OrderBy((SavedProfileSummary savedProfile) => ProfileText.SavedAccountName(savedProfile), StringComparer.OrdinalIgnoreCase).ThenBy(ProfileText.SavedCharacterName, StringComparer.OrdinalIgnoreCase), 
				"Recently Seen" => savedProfiles.OrderByDescending(ProfileText.SavedLastSeen).ThenBy(ProfileText.SavedCharacterName, StringComparer.OrdinalIgnoreCase), 
				"Bookmarked" => savedProfiles.OrderByDescending((SavedProfileSummary savedProfile) => savedProfile.BookmarkedAt ?? DateTime.MinValue).ThenBy(ProfileText.SavedCharacterName, StringComparer.OrdinalIgnoreCase), 
				_ => savedProfiles.OrderByDescending((SavedProfileSummary savedProfile) => savedProfile.CachedAt).ThenBy(ProfileText.SavedCharacterName, StringComparer.OrdinalIgnoreCase), 
			};
		}

		private string GetEmptyBookmarksMessage()
		{
			ProfileFilterMenu discoveryFilters = _discoveryFilters;
			if (discoveryFilters != null && discoveryFilters.ActiveCount > 0)
			{
				return "No bookmarks match these filters.";
			}
			TextBox searchBox = _searchBox;
			if (!string.IsNullOrWhiteSpace((searchBox != null) ? ((TextInputBase)searchBox).get_Text() : null))
			{
				return "No bookmarks match this search.";
			}
			return "No bookmarked profiles yet.";
		}

		private string GetEmptyRecentMessage()
		{
			ProfileFilterMenu discoveryFilters = _discoveryFilters;
			if (discoveryFilters != null && discoveryFilters.ActiveCount > 0)
			{
				return "No recent profiles match these filters.";
			}
			TextBox searchBox = _searchBox;
			if (!string.IsNullOrWhiteSpace((searchBox != null) ? ((TextInputBase)searchBox).get_Text() : null))
			{
				return "No recent profiles match this search.";
			}
			return "No recently viewed profiles yet.";
		}

		private Tooltip MakeTooltip(SavedProfileSummary savedProfile)
		{
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Expected O, but got Unknown
			bool showKnownFor = _settings?.ShowKnownForInProfileTooltips.get_Value() ?? true;
			bool showCurrently = _settings?.ShowCurrentlyInProfileTooltips.get_Value() ?? true;
			bool showOutOfCharacter = _settings?.ShowOocInfoInProfileTooltips.get_Value() ?? true;
			bool trimLongTooltips = _settings?.TrimLongProfileTooltips.get_Value() ?? true;
			int maximumLinesPerSection = _settings?.ProfileTooltipLinesPerSection.get_Value() ?? 12;
			string savedTimeLabel = ((_mode == SavedProfilesMode.Bookmarks) ? ("Bookmarked: " + ProfileText.FormatShortTime(savedProfile.BookmarkedAt ?? savedProfile.CachedAt, "-")) : ("Viewed: " + ProfileText.FormatShortTime(savedProfile.CachedAt, "-")));
			return new Tooltip((ITooltipView)(object)new ProfilePresenceTooltipView(ProfileText.SavedCharacterName(savedProfile), ProfileText.SavedCharacterDetails(savedProfile), GetCachedStatusText(savedProfile), GetCachedLocationText(savedProfile), savedProfile?.KnownFor, savedProfile?.Currently, savedProfile?.OutOfCharacterInfo, showKnownFor, showCurrently, showOutOfCharacter, trimLongTooltips, maximumLinesPerSection, new string[2]
			{
				"Account: " + ProfileText.SavedAccountName(savedProfile),
				savedTimeLabel
			}));
		}

		private string GetSavedTime(SavedProfileSummary savedProfile)
		{
			if (_mode != SavedProfilesMode.Bookmarks)
			{
				return ProfileText.FormatShortTime(savedProfile.CachedAt, "-");
			}
			return ProfileText.FormatShortTime(savedProfile.BookmarkedAt ?? savedProfile.CachedAt, "-");
		}

		private static string GetCachedStatusText(SavedProfileSummary savedProfile)
		{
			if (!IsRecentPresence(savedProfile) || savedProfile.Status == RPStatus.Invisible)
			{
				return ProfileLabels.StatusLabel(RPStatus.Offline);
			}
			return ProfileLabels.StatusLabel(savedProfile.Status);
		}

		private static string GetCachedLocationText(SavedProfileSummary savedProfile)
		{
			if (!IsRecentPresence(savedProfile) || string.IsNullOrWhiteSpace(savedProfile.LocationName))
			{
				return "Unknown";
			}
			return savedProfile.LocationName.Trim();
		}

		private static bool IsRecentPresence(SavedProfileSummary savedProfile)
		{
			if (savedProfile != null && savedProfile.LastSeen != default(DateTime))
			{
				return DateTime.UtcNow - savedProfile.LastSeen.ToUniversalTime() <= LivePresenceWindow;
			}
			return false;
		}

		protected override void Unload()
		{
			_isUnloaded = true;
			UnwatchTooltipSettings();
			_unwatchSavedProfiles?.Invoke(HandleSavedProfilesChanged);
			_discoveryFilters?.Dispose();
			_discoveryFilters = null;
		}
	}
}
