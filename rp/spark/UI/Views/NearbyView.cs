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
	public class NearbyView : View
	{
		private const int BodyWidth = 590;

		private const int HeaderY = 38;

		private const int ListY = 62;

		private const int ListHeight = 188;

		private const int StatusY = 258;

		private const int RowHeight = 30;

		private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(15.0);

		private static readonly TimeSpan RefreshTimeout = TimeSpan.FromSeconds(15.0);

		private readonly NearbyPresenceService _nearby;

		private readonly SparkSettings _settings;

		private readonly Action<PlayerPresence> _openProfile;

		private readonly SemaphoreSlim _refreshGate = new SemaphoreSlim(1, 1);

		private readonly Action<bool> _setWindowLocked;

		private bool _isUnloaded;

		private CancellationTokenSource _refreshCancellation;

		private Task _autoRefreshTask;

		private Checkbox _showNearbyCheckbox;

		private ProfileScrollList _nearbyList;

		private Label _status;

		public NearbyView(NearbyPresenceService nearby, SparkSettings settings, Action<PlayerPresence> openProfile, Action<bool> setWindowLocked)
			: this()
		{
			_nearby = nearby;
			_settings = settings;
			_openProfile = openProfile;
			_setWindowLocked = setWindowLocked;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Expected O, but got Unknown
			_isUnloaded = false;
			SparkUiActions.BindClick(SparkViewUI.AddButton(buildPanel, "Refresh", 490, 0, 100, 28), () => RefreshAsync(resetScroll: false), SetStatusText, "Couldn't refresh nearby players.");
			_showNearbyCheckbox = SparkViewUI.AddCheckbox(buildPanel, "Show me nearby", _settings.ShowNearbyPresence.get_Value(), 0, 0, 170, 28);
			_showNearbyCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)async delegate
			{
				if (_showNearbyCheckbox != null && _settings.ShowNearbyPresence.get_Value() != _showNearbyCheckbox.get_Checked())
				{
					await SetNearbySharingAsync(_showNearbyCheckbox.get_Checked());
				}
			});
			Checkbox autoRefreshCheckbox = SparkViewUI.AddCheckbox(buildPanel, "Auto-refresh", _settings.AutoRefreshNearbyRpers.get_Value(), 180, 0, 150, 28);
			autoRefreshCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_settings.AutoRefreshNearbyRpers.set_Value(autoRefreshCheckbox.get_Checked());
			});
			Checkbox lockWindowCheckbox = SparkViewUI.AddCheckbox(buildPanel, "Lock position", _settings.NearbyWindowLock.get_Value(), 330, 0, 140, 28);
			lockWindowCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_settings.NearbyWindowLock.set_Value(lockWindowCheckbox.get_Checked());
				_setWindowLocked?.Invoke(lockWindowCheckbox.get_Checked());
			});
			_setWindowLocked?.Invoke(lockWindowCheckbox.get_Checked());
			AddHeader(buildPanel, "Character", 8, 170);
			AddHeader(buildPanel, "Race", 186, 70);
			AddHeader(buildPanel, "Status", 264, 110);
			AddHeader(buildPanel, "Map IP", 382, 70);
			AddHeader(buildPanel, "Distance", 460, 80);
			ProfileScrollList profileScrollList = new ProfileScrollList(590, 188, 30);
			((Control)profileScrollList).set_Location(new Point(0, 62));
			((Control)profileScrollList).set_Parent(buildPanel);
			_nearbyList = profileScrollList;
			Label val = new Label();
			val.set_Text(string.Empty);
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(SparkViewUI.SecondaryTextColor);
			val.set_WrapText(true);
			((Control)val).set_Location(new Point(0, 258));
			((Control)val).set_Size(new Point(590, 42));
			((Control)val).set_Parent(buildPanel);
			_status = val;
			WatchSettings();
			SyncShowNearbyCheckboxFromSettings();
			StartRefresh();
			RefreshAsync(resetScroll: true);
		}

		private void WatchSettings()
		{
			_settings.ShowNearbyPresence.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowNearbyPresenceChanged);
			_settings.ShowKnownForInProfileTooltips.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
			_settings.ShowCurrentlyInProfileTooltips.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
			_settings.ShowOocInfoInProfileTooltips.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
			_settings.TrimLongProfileTooltips.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
			_settings.ProfileTooltipLinesPerSection.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnTooltipLineLimitChanged);
		}

		private void UnwatchSettings()
		{
			_settings.ShowNearbyPresence.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowNearbyPresenceChanged);
			_settings.ShowKnownForInProfileTooltips.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
			_settings.ShowCurrentlyInProfileTooltips.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
			_settings.ShowOocInfoInProfileTooltips.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
			_settings.TrimLongProfileTooltips.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTooltipVisibilityChanged);
			_settings.ProfileTooltipLinesPerSection.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnTooltipLineLimitChanged);
		}

		private void OnShowNearbyPresenceChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isUnloaded)
				{
					SyncShowNearbyCheckboxFromSettings();
				}
			});
		}

		private void OnTooltipVisibilityChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			RefreshNearbyTooltips();
		}

		private void OnTooltipLineLimitChanged(object sender, ValueChangedEventArgs<int> e)
		{
			RefreshNearbyTooltips();
		}

		private void RefreshNearbyTooltips()
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isUnloaded && _nearbyList != null)
				{
					RefreshAsync(resetScroll: false);
				}
			});
		}

		private void SyncShowNearbyCheckboxFromSettings()
		{
			SetChecked(_showNearbyCheckbox, _settings.ShowNearbyPresence.get_Value());
		}

		private static void SetChecked(Checkbox checkbox, bool value)
		{
			if (checkbox != null && checkbox.get_Checked() != value)
			{
				checkbox.set_Checked(value);
			}
		}

		private static void AddHeader(Container parent, string text, int x, int width)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text(text);
			val.set_Font(GameService.Content.get_DefaultFont14());
			val.set_TextColor(new Color(255, 233, 180));
			val.set_StrokeText(true);
			((Control)val).set_Location(new Point(x, 38));
			((Control)val).set_Size(new Point(width, 24));
			((Control)val).set_Parent(parent);
		}

		private async Task SetNearbySharingAsync(bool enabled)
		{
			_settings.ShowNearbyPresence.set_Value(enabled);
			try
			{
				SetStatusText(enabled ? "Sharing nearby presence..." : "Hiding nearby presence...");
				if (!enabled)
				{
					await _nearby.RemoveAsync(_refreshCancellation?.Token ?? CancellationToken.None);
				}
				else
				{
					await _nearby.PublishNowAsync(_refreshCancellation?.Token ?? CancellationToken.None);
				}
				SetStatusText(_nearby.LastStatus);
				if (enabled)
				{
					await RefreshAsync(resetScroll: false);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch
			{
				SetStatusText(enabled ? "Couldn't share nearby presence." : "Couldn't hide nearby presence.");
			}
		}

		private void StartRefresh()
		{
			StopRefresh();
			_refreshCancellation = new CancellationTokenSource();
			_autoRefreshTask = AutoRefreshAsync(_refreshCancellation.Token);
		}

		private void StopRefresh()
		{
			CancellationTokenSource cancellation = _refreshCancellation;
			Task task = _autoRefreshTask;
			_refreshCancellation = null;
			_autoRefreshTask = null;
			if (cancellation != null)
			{
				cancellation.Cancel();
				TaskCleanup.DisposeWhenComplete(task, cancellation);
			}
		}

		private async Task AutoRefreshAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					await Task.Delay(RefreshInterval, cancellationToken);
					if (_settings.AutoRefreshNearbyRpers.get_Value())
					{
						await RefreshAsync(resetScroll: false, cancellationToken);
					}
				}
				catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
				{
					return;
				}
			}
		}

		private Task RefreshAsync(bool resetScroll)
		{
			return RefreshAsync(resetScroll, _refreshCancellation?.Token ?? CancellationToken.None);
		}

		private async Task RefreshAsync(bool resetScroll, CancellationToken cancellationToken)
		{
			if (_isUnloaded || _nearbyList == null || cancellationToken.IsCancellationRequested)
			{
				return;
			}
			bool hasRefreshLock = false;
			try
			{
				hasRefreshLock = await _refreshGate.WaitAsync(0, cancellationToken);
				if (!hasRefreshLock)
				{
					return;
				}
				SetStatusText("Refreshing nearby players...");
				string sharingNotice;
				IReadOnlyList<NearbyPresence> rows;
				using (CancellationTokenSource timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
				{
					timeout.CancelAfter(RefreshTimeout);
					sharingNotice = await _nearby.GetSharingNoticeAsync(timeout.Token);
					rows = await _nearby.SearchAsync(timeout.Token);
				}
				SparkUiThread.Queue(delegate
				{
					if (!_isUnloaded && _nearbyList != null)
					{
						ShowRows(rows, resetScroll, sharingNotice);
					}
				});
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested || _isUnloaded)
			{
			}
			catch
			{
				SparkUiThread.Queue(delegate
				{
					if (!_isUnloaded && _nearbyList != null)
					{
						_nearbyList.ShowEmptyMessage("Could not load nearby players.");
						SetStatusText("Nearby list unavailable.");
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

		private void ShowRows(IReadOnlyList<NearbyPresence> nearbyRows, bool resetScroll, string sharingNotice)
		{
			_nearbyList.ClearRows(resetScroll);
			List<NearbyPresence> rows = (from row in nearbyRows ?? new List<NearbyPresence>()
				where row?.Presence != null
				where row.Presence.Status != RPStatus.Invisible
				orderby (!_nearby.IsCurrentMapIp(row)) ? 1 : 0, (!(row.DistanceMeters < 0.0)) ? row.DistanceMeters : double.MaxValue
				select row).ThenBy((NearbyPresence row) => MapIpText(row.ServerAddress), StringComparer.OrdinalIgnoreCase).ThenBy((NearbyPresence row) => row.VisibleName(), StringComparer.OrdinalIgnoreCase).ToList();
			if (rows.Count == 0)
			{
				_nearbyList.ShowEmptyMessage("No nearby players found.");
				SetStatusText(WithSharingNotice(_settings.AutoRefreshNearbyRpers.get_Value() ? "0 nearby players." : "0 nearby players. Auto-refresh is off.", sharingNotice));
				return;
			}
			for (int index = 0; index < rows.Count; index++)
			{
				AddRow(rows[index], index);
			}
			if (resetScroll)
			{
				_nearbyList.ResetScroll();
			}
			SetStatusText(WithSharingNotice((rows.Count == 1) ? "1 nearby player." : $"{rows.Count} nearby players.", sharingNotice));
		}

		private void AddRow(NearbyPresence nearby, int index)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			PlayerPresence presence = nearby?.Presence ?? new PlayerPresence();
			Panel row = _nearbyList.AddRow(index, string.Empty);
			bool num = _nearby.IsCurrentMapIp(nearby);
			Color secondary = SparkViewUI.SecondaryTextColor;
			Color mapIpColor = (Color)(num ? new Color(140, 220, 140) : secondary);
			string distanceText = (num ? DistanceText(nearby?.DistanceMeters ?? (-1.0)) : "-");
			_nearbyList.AddCell((Container)(object)row, presence.VisibleName(), 8, 5, 170, Color.get_White());
			_nearbyList.AddCell((Container)(object)row, ProfileText.PresenceRace(presence), 186, 5, 70, secondary);
			_nearbyList.AddCell((Container)(object)row, ProfileLabels.StatusLabel(presence.Status), 264, 5, 110, ProfileStatusColors.Get(presence.Status));
			_nearbyList.AddCell((Container)(object)row, MapIpText(nearby?.ServerAddress), 382, 5, 70, mapIpColor);
			_nearbyList.AddCell((Container)(object)row, distanceText, 460, 5, 80, secondary);
			ProfileScrollList.AddInteractionLayer((Container)(object)row, MakeTooltip(nearby), delegate
			{
				_openProfile?.Invoke(presence);
			});
		}

		private Tooltip MakeTooltip(NearbyPresence nearby)
		{
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Expected O, but got Unknown
			PlayerPresence presence = nearby?.Presence ?? new PlayerPresence();
			bool showKnownFor = _settings?.ShowKnownForInProfileTooltips.get_Value() ?? true;
			bool showCurrently = _settings?.ShowCurrentlyInProfileTooltips.get_Value() ?? true;
			bool showOutOfCharacter = _settings?.ShowOocInfoInProfileTooltips.get_Value() ?? true;
			bool trimLongTooltips = _settings?.TrimLongProfileTooltips.get_Value() ?? true;
			int maximumLinesPerSection = _settings?.ProfileTooltipLinesPerSection.get_Value() ?? 12;
			return new Tooltip((ITooltipView)(object)new ProfilePresenceTooltipView(presence.VisibleName(), ProfileText.PresenceCharacterDetails(presence), ProfileLabels.StatusLabel(presence.Status), ProfileText.PresenceLocation(presence), presence.KnownFor, presence.Currently, presence.OutOfCharacterInfo, showKnownFor, showCurrently, showOutOfCharacter, trimLongTooltips, maximumLinesPerSection, new string[1] { "Distance: " + DistanceText(nearby?.DistanceMeters ?? (-1.0)) }));
		}

		private static string DistanceText(double meters)
		{
			if (meters < 0.0)
			{
				return "-";
			}
			if (meters >= 1000.0)
			{
				return $"{meters / 1000.0:0.0}km";
			}
			return $"{Math.Round(meters):0}m";
		}

		private static string MapIpText(string serverAddress)
		{
			string text = serverAddress?.Trim() ?? string.Empty;
			if (string.IsNullOrWhiteSpace(text))
			{
				return "-";
			}
			string[] parts = text.Split('.');
			string lastPart = ((parts.Length != 0) ? parts[parts.Length - 1].Trim() : text);
			if (string.IsNullOrWhiteSpace(lastPart))
			{
				return "-";
			}
			if (lastPart.Length > 3)
			{
				return lastPart.Substring(lastPart.Length - 3);
			}
			return lastPart;
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

		private static string WithSharingNotice(string status, string sharingNotice)
		{
			if (string.IsNullOrWhiteSpace(sharingNotice))
			{
				return status ?? string.Empty;
			}
			if (string.IsNullOrWhiteSpace(status))
			{
				return sharingNotice.Trim();
			}
			return status + " " + sharingNotice.Trim();
		}

		protected override void Unload()
		{
			_isUnloaded = true;
			UnwatchSettings();
			StopRefresh();
			_showNearbyCheckbox = null;
		}
	}
}
