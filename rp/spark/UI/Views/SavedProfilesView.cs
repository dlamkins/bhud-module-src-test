using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using rp.spark.Models;
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

		private readonly PageList _page = new PageList();

		private PageListControls _pageControls;

		private bool _isUnloaded;

		private TextBox _searchBox;

		private Dropdown _searchFieldDropdown;

		private Dropdown _sortDropdown;

		private ProfileScrollList _profileList;

		private Label _status;

		private string _statusOverride = string.Empty;

		public SavedProfilesView(Func<IReadOnlyList<SavedProfileSummary>> getSavedProfiles, Action<SavedProfileSummary, Action<string>> openProfile, SavedProfilesMode mode, Func<string, bool> isBlockedAccount = null, Action<SavedProfileSummary> removeBookmark = null, Action<Action> watchSavedProfilesChanged = null, Action<Action> unwatchSavedProfilesChanged = null, Func<bool> showMatureProfiles = null)
			: this()
		{
			_loadSavedProfiles = getSavedProfiles;
			_openProfile = openProfile;
			_mode = mode;
			_isBlockedAccount = isBlockedAccount;
			_showMatureProfiles = showMatureProfiles;
			_removeBookmark = removeBookmark;
			_watchSavedProfiles = watchSavedProfilesChanged;
			_unwatchSavedProfiles = unwatchSavedProfilesChanged;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			_isUnloaded = false;
			ProfileListViewUI.AddTitle(buildPanel, (_mode == SavedProfilesMode.Bookmarks) ? "Bookmarked Profiles" : "Recent Profiles", 320);
			((Control)ProfileListViewUI.AddRefreshButton(buildPanel)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RefreshRows(resetPage: false);
			});
			BuildSearchControls(buildPanel);
			BuildHeader(buildPanel);
			_watchSavedProfiles?.Invoke(HandleSavedProfilesChanged);
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

		private void RefreshRows(bool resetPage, bool resetScroll = true)
		{
			if (_isUnloaded || _profileList == null)
			{
				return;
			}
			_profileList.ClearRows(resetScroll);
			IEnumerable<SavedProfileSummary> filteredRows = (from savedProfile in _loadSavedProfiles?.Invoke() ?? new List<SavedProfileSummary>()
				where savedProfile != null
				where !IsHidden(savedProfile)
				select savedProfile).Where(MatchesSearch);
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
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			string tooltipText = TooltipText(savedProfile);
			Panel row = _profileList.AddRow(index, tooltipText);
			if (_mode == SavedProfilesMode.Bookmarks)
			{
				MakeClickable((Control)(object)_profileList.AddCell((Container)(object)row, ProfileText.SavedCharacterName(savedProfile), 8, 8, 210, Color.get_White()), savedProfile, tooltipText);
				MakeClickable((Control)(object)_profileList.AddCell((Container)(object)row, ProfileText.SavedRace(savedProfile), 225, 8, 95, new Color(220, 220, 220)), savedProfile, tooltipText);
				MakeClickable((Control)(object)_profileList.AddCell((Container)(object)row, ProfileText.SavedAccountName(savedProfile), 330, 8, 160, new Color(220, 220, 220)), savedProfile, tooltipText);
				MakeClickable((Control)(object)_profileList.AddCell((Container)(object)row, GetSavedTime(savedProfile), 500, 8, 135, new Color(220, 220, 220)), savedProfile, tooltipText);
				AddRemoveButton((Container)(object)row, savedProfile);
			}
			else
			{
				MakeClickable((Control)(object)row, savedProfile, tooltipText);
				AddBookmarkMarker((Container)(object)row, savedProfile, tooltipText);
				MakeClickable((Control)(object)_profileList.AddCell((Container)(object)row, ProfileText.SavedCharacterName(savedProfile), 30, 8, 210, Color.get_White()), savedProfile, tooltipText);
				MakeClickable((Control)(object)_profileList.AddCell((Container)(object)row, ProfileText.SavedRace(savedProfile), 250, 8, 110, new Color(220, 220, 220)), savedProfile, tooltipText);
				MakeClickable((Control)(object)_profileList.AddCell((Container)(object)row, ProfileText.SavedAccountName(savedProfile), 370, 8, 170, new Color(220, 220, 220)), savedProfile, tooltipText);
				MakeClickable((Control)(object)_profileList.AddCell((Container)(object)row, GetSavedTime(savedProfile), 550, 8, 205, new Color(220, 220, 220)), savedProfile, tooltipText);
			}
		}

		private void AddBookmarkMarker(Container row, SavedProfileSummary savedProfile, string tooltipText)
		{
			if (savedProfile != null && savedProfile.IsBookmarked)
			{
				AssetIcon marker = ProfileListViewUI.AddBookmarkMarker(row);
				MakeClickable((Control)(object)marker, savedProfile, tooltipText);
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
			StandardButton val = new StandardButton();
			val.set_Text("Remove");
			((Control)val).set_Location(new Point(650, 5));
			((Control)val).set_Size(new Point(90, 30));
			((Control)val).set_Parent(row);
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
			TextBox searchBox = _searchBox;
			if (!string.IsNullOrWhiteSpace((searchBox != null) ? ((TextInputBase)searchBox).get_Text() : null))
			{
				return "No bookmarks match this search.";
			}
			return "No bookmarked profiles yet.";
		}

		private string GetEmptyRecentMessage()
		{
			TextBox searchBox = _searchBox;
			if (!string.IsNullOrWhiteSpace((searchBox != null) ? ((TextInputBase)searchBox).get_Text() : null))
			{
				return "No recent profiles match this search.";
			}
			return "No recently viewed profiles yet.";
		}

		private string TooltipText(SavedProfileSummary savedProfile)
		{
			List<string> lines = new List<string>
			{
				ProfileText.SavedCharacterName(savedProfile),
				ProfileText.SavedCharacterDetails(savedProfile),
				"Account: " + ProfileText.SavedAccountName(savedProfile),
				"Status: " + GetCachedStatusText(savedProfile),
				"Location: " + GetCachedLocationText(savedProfile),
				(_mode == SavedProfilesMode.Bookmarks) ? ("Bookmarked: " + ProfileText.FormatShortTime(savedProfile.BookmarkedAt ?? savedProfile.CachedAt, "-")) : ("Viewed: " + ProfileText.FormatShortTime(savedProfile.CachedAt, "-"))
			};
			if (!string.IsNullOrWhiteSpace(savedProfile.Currently))
			{
				lines.Add("----------------");
				lines.Add("Currently: " + savedProfile.Currently.Trim());
			}
			if (!string.IsNullOrWhiteSpace(savedProfile.OutOfCharacterInfo))
			{
				lines.Add("----------------");
				lines.Add(savedProfile.OutOfCharacterInfo.Trim());
			}
			return string.Join(Environment.NewLine, lines.Where((string line) => !string.IsNullOrWhiteSpace(line)));
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
			_unwatchSavedProfiles?.Invoke(HandleSavedProfilesChanged);
		}
	}
}
