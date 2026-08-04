using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using rp.spark.Models;
using rp.spark.Services;
using rp.spark.UI.Controls;

namespace rp.spark.UI.Views
{
	public class OnlineProfilesView : View
	{
		private const int RowHeight = 36;

		private const string SearchStatus = "Status";

		private const string SearchLocation = "Location";

		private const string SortStatus = "Status";

		private const string SortLocation = "Location";

		private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(30.0);

		private static readonly TimeSpan RefreshTimeout = TimeSpan.FromSeconds(15.0);

		private readonly Func<bool> _isAutoRefreshEnabled;

		private readonly Action<bool> _setAutoRefreshEnabled;

		private readonly SparkSettings _settings;

		private readonly PageList _page = new PageList();

		private PageListControls _pageControls;

		private readonly Func<CancellationToken, Task<IReadOnlyList<PlayerPresence>>> _loadRows;

		private readonly Func<IReadOnlyList<PlayerPresence>> _loadCachedRows;

		private readonly Action<PlayerPresence> _openProfile;

		private readonly Func<PlayerPresence, bool> _isBookmarked;

		private readonly Action<Action> _watchBookmarks;

		private readonly Action<Action> _unwatchBookmarks;

		private readonly SemaphoreSlim _refreshGate = new SemaphoreSlim(1, 1);

		private bool _isUnloaded;

		private CancellationTokenSource _refreshCancellation;

		private IReadOnlyList<PlayerPresence> _rows = new List<PlayerPresence>();

		private TextBox _searchBox;

		private Dropdown _searchFieldDropdown;

		private Dropdown _sortDropdown;

		private ProfileFilterMenu _discoveryFilters;

		private ProfileScrollList _profileList;

		private Label _status;

		public OnlineProfilesView(Func<CancellationToken, Task<IReadOnlyList<PlayerPresence>>> getPresenceRows, Func<IReadOnlyList<PlayerPresence>> getCachedPresenceRows, Action<PlayerPresence> openProfile, Func<PlayerPresence, bool> isBookmarked = null, Action<Action> watchBookmarksChanged = null, Action<Action> unwatchBookmarksChanged = null, Func<bool> isAutoRefreshEnabled = null, Action<bool> setAutoRefreshEnabled = null, SparkSettings settings = null)
			: this()
		{
			_loadRows = getPresenceRows;
			_loadCachedRows = getCachedPresenceRows;
			_openProfile = openProfile;
			_isBookmarked = isBookmarked;
			_watchBookmarks = watchBookmarksChanged;
			_unwatchBookmarks = unwatchBookmarksChanged;
			_isAutoRefreshEnabled = isAutoRefreshEnabled;
			_setAutoRefreshEnabled = setAutoRefreshEnabled;
			_settings = settings;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Expected O, but got Unknown
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			_isUnloaded = false;
			WatchTooltipSettings();
			ProfileListViewUI.AddTitle(buildPanel, "Online Profiles", 300);
			SparkUiActions.BindClick(ProfileListViewUI.AddRefreshButton(buildPanel), () => RefreshAsync(resetPage: false), SetStatusText, "Couldn't refresh online profiles.");
			BuildSearchControls(buildPanel);
			BuildHeader(buildPanel);
			ProfileScrollList profileScrollList = new ProfileScrollList(760, 400, 36);
			((Control)profileScrollList).set_Location(new Point(0, 128));
			((Control)profileScrollList).set_Parent(buildPanel);
			_profileList = profileScrollList;
			Checkbox val = new Checkbox();
			val.set_Text("Auto-refresh");
			val.set_Checked(IsAutoRefreshEnabled());
			((Control)val).set_Location(new Point(510, 1));
			((Control)val).set_Size(new Point(130, 28));
			((Control)val).set_Parent(buildPanel);
			Checkbox autoRefreshCheckbox = val;
			autoRefreshCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_setAutoRefreshEnabled?.Invoke(autoRefreshCheckbox.get_Checked());
				RefreshVisibleRows(resetScroll: false);
			});
			_pageControls = new PageListControls(buildPanel, _page, 760, delegate
			{
				RefreshVisibleRows(resetScroll: false);
			})
			{
				Location = new Point(0, 532)
			};
			_status = ProfileListViewUI.AddStatusLabel(buildPanel);
			_watchBookmarks?.Invoke(HandleBookmarksChanged);
			StartRefresh();
			RefreshAsync(resetPage: true);
		}

		private static void BuildHeader(Container parent)
		{
			ProfileListViewUI.AddHeader(parent, "Character", 28, 165);
			ProfileListViewUI.AddHeader(parent, "Race", 200, 95);
			ProfileListViewUI.AddHeader(parent, "Account", 305, 140);
			ProfileListViewUI.AddHeader(parent, "Status", 455, 105);
			ProfileListViewUI.AddHeader(parent, "Location", 570, 180);
		}

		private void BuildSearchControls(Container parent)
		{
			ProfileListSearchControls controls = ProfileListViewUI.AddSearchControls(parent, "Search online profiles", new string[7] { "All fields", "Name", "Account", "Race", "Profession", "Status", "Location" }, new string[6] { "Name", "Recently Seen", "Race", "Account", "Status", "Location" }, "Name", delegate
			{
				RefreshVisibleRows(resetScroll: true);
			});
			_searchBox = controls.SearchBox;
			_searchFieldDropdown = controls.SearchFieldDropdown;
			_sortDropdown = controls.SortDropdown;
			_discoveryFilters = new ProfileFilterMenu(parent, delegate
			{
				RefreshVisibleRows(resetScroll: true);
			});
		}

		private void HandleBookmarksChanged()
		{
			SparkUiThread.Queue(delegate
			{
				RefreshVisibleRows(resetScroll: false);
			});
		}

		private void StartRefresh()
		{
			StopRefresh();
			_refreshCancellation = new CancellationTokenSource();
			AutoRefreshAsync(_refreshCancellation.Token);
		}

		private void StopRefresh()
		{
			CancellationTokenSource refreshCancellation = _refreshCancellation;
			_refreshCancellation = null;
			refreshCancellation?.Cancel();
		}

		private async Task AutoRefreshAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					await Task.Delay(RefreshInterval, cancellationToken);
					if (IsAutoRefreshEnabled())
					{
						await RefreshAsync(resetPage: false, cancellationToken);
					}
				}
				catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
				{
					return;
				}
			}
		}

		private bool IsAutoRefreshEnabled()
		{
			return _isAutoRefreshEnabled?.Invoke() ?? true;
		}

		private Task RefreshAsync(bool resetPage)
		{
			return RefreshAsync(resetPage, _refreshCancellation?.Token ?? CancellationToken.None);
		}

		private async Task RefreshAsync(bool resetPage, CancellationToken cancellationToken)
		{
			if (_isUnloaded || _profileList == null || cancellationToken.IsCancellationRequested)
			{
				return;
			}
			bool hasRefreshLock = false;
			bool showedCachedRows = false;
			bool hadExistingRows = _rows != null && _rows.Count > 0;
			try
			{
				hasRefreshLock = await _refreshGate.WaitAsync(0, cancellationToken);
				if (!hasRefreshLock || _isUnloaded || _profileList == null || cancellationToken.IsCancellationRequested)
				{
					return;
				}
				if (!hadExistingRows)
				{
					showedCachedRows = ShowCachedRows(resetPage);
				}
				SetStatusText((hadExistingRows || showedCachedRows) ? "Refreshing profiles..." : "Loading profiles...");
				IReadOnlyList<PlayerPresence> rows;
				using (CancellationTokenSource refreshTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
				{
					refreshTimeout.CancelAfter(RefreshTimeout);
					rows = ((_loadRows != null) ? (await _loadRows(refreshTimeout.Token)) : new List<PlayerPresence>());
				}
				cancellationToken.ThrowIfCancellationRequested();
				if (_isUnloaded)
				{
					return;
				}
				SparkUiThread.Queue(delegate
				{
					if (_profileList != null && _status != null && !_isUnloaded)
					{
						_rows = rows ?? new List<PlayerPresence>();
						RefreshVisibleRows(resetPage);
					}
				});
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested || _isUnloaded)
			{
			}
			catch
			{
				if (_isUnloaded)
				{
					return;
				}
				SparkUiThread.Queue(delegate
				{
					if (_profileList != null && _status != null && !_isUnloaded)
					{
						if (hadExistingRows)
						{
							_status.set_Text("Showing previous results. Server list unavailable.");
						}
						else if (showedCachedRows)
						{
							_status.set_Text("Showing local profile. Server list unavailable.");
						}
						else
						{
							_profileList.ShowEmptyMessage("Could not load online profiles.");
							_status.set_Text("Server list unavailable.");
						}
					}
				});
			}
			finally
			{
				if (hasRefreshLock)
				{
					_refreshGate.Release();
				}
			}
		}

		private bool ShowCachedRows(bool resetScroll)
		{
			if (_loadCachedRows == null || _isUnloaded || _profileList == null)
			{
				return false;
			}
			IReadOnlyList<PlayerPresence> cachedRows;
			try
			{
				cachedRows = _loadCachedRows();
			}
			catch
			{
				return false;
			}
			if (cachedRows == null || cachedRows.Count == 0)
			{
				return false;
			}
			SparkUiThread.Queue(delegate
			{
				if (_profileList != null && _status != null && !_isUnloaded)
				{
					_rows = cachedRows;
					RefreshVisibleRows(resetScroll);
				}
			});
			return true;
		}

		private void ShowRows(IReadOnlyList<PlayerPresence> presenceRows, bool resetScroll)
		{
			if (_profileList == null || _status == null || _isUnloaded)
			{
				return;
			}
			_profileList.ClearRows();
			IEnumerable<PlayerPresence> filteredRows = from row in (presenceRows ?? new List<PlayerPresence>()).Where((PlayerPresence row) => row != null && row.Status != RPStatus.Invisible).Where(MatchesSearch)
				where _discoveryFilters == null || _discoveryFilters.Matches(row.Experience, row.DiscoveryTags)
				select row;
			List<PlayerPresence> rows = SortRows(filteredRows).ToList();
			if (resetScroll)
			{
				_page.Reset();
			}
			_page.Clamp(rows.Count);
			_pageControls?.Update(rows.Count);
			if (rows.Count == 0)
			{
				_profileList.ShowEmptyMessage(GetEmptyMessage());
				_status.set_Text(IsAutoRefreshEnabled() ? "0 matching profiles." : "0 matching profiles. Auto-refresh is off. Manually refresh instead.");
				return;
			}
			IReadOnlyList<PlayerPresence> pageRows = _page.GetPage(rows);
			for (int index = 0; index < pageRows.Count; index++)
			{
				AddRow(pageRows[index], index);
			}
			if (resetScroll)
			{
				_profileList.ResetScroll();
			}
			string visibleRows = ((rows.Count == 1) ? "1 visible profile" : $"{rows.Count} visible profiles");
			string searchSuffix = ProfileListViewUI.GetSearchSuffix(_searchBox);
			string refreshSuffix = (IsAutoRefreshEnabled() ? string.Empty : " Auto-refresh is off. Manually refresh instead.");
			_status.set_Text(visibleRows + searchSuffix + "." + refreshSuffix);
		}

		private void RefreshVisibleRows(bool resetScroll)
		{
			ShowRows(_rows, resetScroll);
		}

		private void AddRow(PlayerPresence presence, int index)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			Panel row = _profileList.AddRow(index, string.Empty);
			Color secondary = default(Color);
			((Color)(ref secondary))._002Ector(220, 220, 220);
			AddBookmarkMarker((Container)(object)row, presence);
			_profileList.AddCell((Container)(object)row, presence.VisibleName(), 30, 7, 163, Color.get_White());
			_profileList.AddCell((Container)(object)row, ProfileText.PresenceRace(presence), 200, 7, 90, secondary);
			_profileList.AddCell((Container)(object)row, presence.AccountName, 305, 7, 95, secondary);
			_profileList.AddCell((Container)(object)row, ProfileLabels.StatusLabel(presence.Status), 455, 7, 105, ProfileStatusColors.Get(presence.Status));
			_profileList.AddCell((Container)(object)row, ProfileText.PresenceLocation(presence), 570, 7, 180, secondary);
			ProfileScrollList.AddInteractionLayer((Container)(object)row, MakeTooltip(presence), delegate
			{
				_openProfile?.Invoke(presence);
			});
		}

		private void AddBookmarkMarker(Container row, PlayerPresence presence)
		{
			Func<PlayerPresence, bool> isBookmarked = _isBookmarked;
			if (isBookmarked != null && isBookmarked(presence))
			{
				ProfileListViewUI.AddBookmarkMarker(row);
			}
		}

		private bool MatchesSearch(PlayerPresence presence)
		{
			TextBox searchBox = _searchBox;
			string query = ((searchBox == null) ? null : ((TextInputBase)searchBox).get_Text()?.Trim()) ?? string.Empty;
			if (string.IsNullOrWhiteSpace(query))
			{
				return true;
			}
			return GetSearchText(presence).IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private string GetSearchText(PlayerPresence presence)
		{
			Dropdown searchFieldDropdown = _searchFieldDropdown;
			return (((searchFieldDropdown == null) ? null : searchFieldDropdown.get_SelectedItem()?.ToString()) ?? "All fields") switch
			{
				"Name" => ProfileText.JoinSearchText(presence?.VisibleName(), presence?.ActiveProfileName), 
				"Account" => presence?.AccountName ?? string.Empty, 
				"Race" => ProfileText.PresenceRace(presence), 
				"Profession" => presence?.VisibleProfession() ?? string.Empty, 
				"Status" => ProfileLabels.StatusLabel(presence?.Status ?? RPStatus.Offline), 
				"Location" => ProfileText.PresenceLocation(presence), 
				_ => ProfileText.JoinSearchText(presence?.VisibleName(), presence?.ActiveProfileName, presence?.AccountName, ProfileText.PresenceRace(presence), presence?.VisibleProfession(), ProfileLabels.StatusLabel(presence?.Status ?? RPStatus.Offline), ProfileText.PresenceLocation(presence)), 
			};
		}

		private IEnumerable<PlayerPresence> SortRows(IEnumerable<PlayerPresence> rows)
		{
			Dropdown sortDropdown = _sortDropdown;
			return (((sortDropdown == null) ? null : sortDropdown.get_SelectedItem()?.ToString()) ?? "Name") switch
			{
				"Recently Seen" => rows.OrderByDescending((PlayerPresence row) => row.LastSeen).ThenBy((PlayerPresence row) => row.VisibleName(), StringComparer.OrdinalIgnoreCase), 
				"Race" => rows.OrderBy((PlayerPresence row) => ProfileText.PresenceRace(row), StringComparer.OrdinalIgnoreCase).ThenBy((PlayerPresence row) => row.VisibleName(), StringComparer.OrdinalIgnoreCase), 
				"Account" => rows.OrderBy((PlayerPresence row) => row.AccountName, StringComparer.OrdinalIgnoreCase).ThenBy((PlayerPresence row) => row.VisibleName(), StringComparer.OrdinalIgnoreCase), 
				"Status" => rows.OrderBy((PlayerPresence row) => ProfileLabels.StatusLabel(row.Status), StringComparer.OrdinalIgnoreCase).ThenBy((PlayerPresence row) => row.VisibleName(), StringComparer.OrdinalIgnoreCase), 
				"Location" => rows.OrderBy(ProfileText.PresenceLocation, StringComparer.OrdinalIgnoreCase).ThenBy((PlayerPresence row) => row.VisibleName(), StringComparer.OrdinalIgnoreCase), 
				_ => rows.OrderBy((PlayerPresence row) => row.VisibleName(), StringComparer.OrdinalIgnoreCase).ThenBy((PlayerPresence row) => row.AccountName, StringComparer.OrdinalIgnoreCase), 
			};
		}

		private string GetEmptyMessage()
		{
			ProfileFilterMenu discoveryFilters = _discoveryFilters;
			if (discoveryFilters != null && discoveryFilters.ActiveCount > 0)
			{
				return "No online profiles match these filters.";
			}
			TextBox searchBox = _searchBox;
			if (!string.IsNullOrWhiteSpace((searchBox != null) ? ((TextInputBase)searchBox).get_Text() : null))
			{
				return "No online profiles match this search.";
			}
			return "No visible profiles yet.";
		}

		private Tooltip MakeTooltip(PlayerPresence presence)
		{
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			bool showKnownFor = _settings?.ShowKnownForInProfileTooltips.get_Value() ?? true;
			bool showCurrently = _settings?.ShowCurrentlyInProfileTooltips.get_Value() ?? true;
			bool showOutOfCharacter = _settings?.ShowOocInfoInProfileTooltips.get_Value() ?? true;
			bool trimLongTooltips = _settings?.TrimLongProfileTooltips.get_Value() ?? true;
			int maximumLinesPerSection = _settings?.ProfileTooltipLinesPerSection.get_Value() ?? 12;
			return new Tooltip((ITooltipView)(object)new ProfilePresenceTooltipView(presence.VisibleName(), ProfileText.PresenceCharacterDetails(presence), ProfileLabels.StatusLabel(presence.Status), ProfileText.PresenceLocation(presence), presence.KnownFor, presence.Currently, presence.OutOfCharacterInfo, showKnownFor, showCurrently, showOutOfCharacter, trimLongTooltips, maximumLinesPerSection));
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
			QueueTooltipRefresh();
		}

		private void OnTooltipLineLimitChanged(object sender, ValueChangedEventArgs<int> e)
		{
			QueueTooltipRefresh();
		}

		private void QueueTooltipRefresh()
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isUnloaded && _profileList != null)
				{
					RefreshVisibleRows(resetScroll: false);
				}
			});
		}

		protected override void Unload()
		{
			_isUnloaded = true;
			UnwatchTooltipSettings();
			_unwatchBookmarks?.Invoke(HandleBookmarksChanged);
			StopRefresh();
			_discoveryFilters?.Dispose();
			_discoveryFilters = null;
		}

		private void SetStatusText(string text)
		{
			if (_status == null || _isUnloaded)
			{
				return;
			}
			SparkUiThread.Queue(delegate
			{
				if (_status != null && !_isUnloaded)
				{
					_status.set_Text(text ?? string.Empty);
				}
			});
		}
	}
}
