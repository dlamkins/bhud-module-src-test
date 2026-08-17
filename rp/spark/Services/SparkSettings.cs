using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using rp.spark.Models;

namespace rp.spark.Services
{
	public class SparkSettings
	{
		private const string SharingSettingsKey = "profile-sharing";

		private const string DiscoverySettingsKey = "profile-discovery";

		private const string PrivacySettingsKey = "privacy";

		private const string UiSettingsKey = "ui";

		private const string BroadcastProfileKey = "BroadcastProfile";

		private const string AutoHideGameUiKey = "AutoHideGameUi";

		private const string HideLocationKey = "HideLocation";

		private const string CurrentStatusKey = "CurrentStatus";

		private const string RegionFilterKey = "RegionFilter";

		private const string BlockedAccountsKey = "BlockedAccounts";

		private const string AutoRefreshOnlineProfilesKey = "AutoRefreshOnlineProfiles";

		private const string ShowMatureProfilesKey = "ShowMatureProfiles";

		private const string ShowNearbyPresenceKey = "ShowNearbyPresence";

		private const string AutoRefreshNearbyRpersKey = "AutoRefreshNearbyRpers";

		private const string NearbyWindowLocationKey = "NearbyWindowLocation";

		private const string NearbyWindowLockKey = "NearbyWindowLock";

		private const string ShowCornerIconKey = "ShowCornerIcon";

		private const string ShowKnownForInProfileTooltipsKey = "ShowKnownForInProfileTooltips";

		private const string ShowCurrentlyInProfileTooltipsKey = "ShowCurrentlyInProfileTooltips";

		private const string ShowOocInfoInProfileTooltipsKey = "ShowOocInfoInProfileTooltips";

		private const string TrimLongProfileTooltipsKey = "TrimLongProfileTooltips";

		private const string ProfileTooltipLinesPerSectionKey = "ProfileTooltipLinesPerSection";

		private static readonly Regex AccountNameRegex = new Regex("^[^.\\r\\n]+\\.\\d{4}$", RegexOptions.Compiled);

		public SettingCollection SharingSettings { get; }

		public SettingCollection DiscoverySettings { get; }

		public SettingEntry<bool> ShowMatureProfiles { get; }

		public SettingCollection PrivacySettings { get; }

		public SettingCollection UiSettings { get; }

		internal ChatSplitterSettings ChatSplitter { get; }

		public SettingEntry<bool> BroadcastProfile { get; }

		public SettingEntry<bool> HideLocation { get; }

		public SettingEntry<bool> AutoHideGameUi { get; }

		public SettingEntry<RPStatus> CurrentStatus { get; }

		public SettingEntry<ProfileRegion> RegionFilter { get; }

		public SettingEntry<bool> AutoRefreshOnlineProfiles { get; }

		public SettingEntry<string> BlockedAccounts { get; }

		public SettingEntry<bool> ShowNearbyPresence { get; }

		public SettingEntry<bool> AutoRefreshNearbyRpers { get; }

		public SettingEntry<string> NearbyWindowLocation { get; }

		public SettingEntry<bool> NearbyWindowLock { get; }

		public SettingEntry<bool> ShowCornerIcon { get; }

		public SettingEntry<bool> ShowKnownForInProfileTooltips { get; }

		public SettingEntry<bool> ShowCurrentlyInProfileTooltips { get; }

		public SettingEntry<bool> ShowOocInfoInProfileTooltips { get; }

		public SettingEntry<bool> TrimLongProfileTooltips { get; }

		public SettingEntry<int> ProfileTooltipLinesPerSection { get; }

		public SparkSettings(SettingCollection settings)
		{
			SharingSettings = settings.AddSubCollection("profile-sharing", true, (Func<string>)(() => "Profile sharing"));
			DiscoverySettings = settings.AddSubCollection("profile-discovery", true, (Func<string>)(() => "Profile discovery"));
			ShowMatureProfiles = DiscoverySettings.DefineSetting<bool>("ShowMatureProfiles", false, (Func<string>)(() => "Show mature/18+ profiles"), (Func<string>)(() => "Allows profiles marked as mature/18+ to appear in online and saved profile lists."));
			PrivacySettings = settings.AddSubCollection("privacy", true, (Func<string>)(() => "Privacy"));
			UiSettings = settings.AddSubCollection("ui", true, (Func<string>)(() => "Interface"));
			ChatSplitter = new ChatSplitterSettings(settings);
			BroadcastProfile = SharingSettings.DefineSetting<bool>("BroadcastProfile", false, (Func<string>)(() => "Share my profile online"), (Func<string>)(() => "When enabled, SPARK will periodically publish your public profile to other players and show you on the online list."));
			HideLocation = SharingSettings.DefineSetting<bool>("HideLocation", false, (Func<string>)(() => "Hide my location"), (Func<string>)(() => "When enabled, SPARK will show your location as Hidden instead of showing where you are."));
			ShowNearbyPresence = SharingSettings.DefineSetting<bool>("ShowNearbyPresence", false, (Func<string>)(() => "Show me to nearby players"), (Func<string>)(() => "When enabled, SPARK will publish your nearby RP presence to other opted-in SPARK users."));
			AutoHideGameUi = UiSettings.DefineSetting<bool>("AutoHideGameUi", true, (Func<string>)(() => "Auto-hide SPARK windows during in-game UI"), (Func<string>)(() => "When enabled, SPARK closes profile windows during the fullscreen map and other GW2 UI states where overlays are not relevant."));
			CurrentStatus = SharingSettings.DefineSetting<RPStatus>("CurrentStatus", RPStatus.Online, (Func<string>)(() => "Status"), (Func<string>)(() => "Your RP status for profile sharing."));
			RegionFilter = DiscoverySettings.DefineSetting<ProfileRegion>("RegionFilter", ProfileRegion.NA, (Func<string>)(() => "Region filter"), (Func<string>)(() => "Filters online profiles to your chosen region."));
			AutoRefreshOnlineProfiles = DiscoverySettings.DefineSetting<bool>("AutoRefreshOnlineProfiles", true, (Func<string>)(() => "Auto-refresh online profiles"), (Func<string>)(() => "Refreshes the open online profile list every 30 seconds."));
			AutoRefreshNearbyRpers = DiscoverySettings.DefineSetting<bool>("AutoRefreshNearbyRpers", true, (Func<string>)(() => "Auto-refresh nearby player list"), (Func<string>)(() => "Refreshes the nearby players window automatically."));
			BlockedAccounts = PrivacySettings.DefineSetting<string>("BlockedAccounts", string.Empty, (Func<string>)(() => "Block list"), (Func<string>)(() => "One account per line. Blocked accounts will be hidden from results."));
			NearbyWindowLocation = UiSettings.DefineSetting<string>("NearbyWindowLocation", string.Empty, (Func<string>)(() => "Nearby Players window location"), (Func<string>)(() => "Stores the last screen position of the Nearby Players window."));
			NearbyWindowLock = UiSettings.DefineSetting<bool>("NearbyWindowLock", false, (Func<string>)(() => "Lock Nearby Players window"), (Func<string>)(() => "Prevents dragging the Nearby Players window while still allowing profile clicks, refresh, and scrolling."));
			ShowCornerIcon = UiSettings.DefineSetting<bool>("ShowCornerIcon", true, (Func<string>)(() => "Show corner icon"), (Func<string>)(() => "Shows the SPARK shortcut menu in the Blish HUD corner icon area."));
			ShowKnownForInProfileTooltips = UiSettings.DefineSetting<bool>("ShowKnownForInProfileTooltips", true, (Func<string>)(() => "Show Known For in profile tooltips"), (Func<string>)(() => "Shows the Known For section when it is available."));
			ShowCurrentlyInProfileTooltips = UiSettings.DefineSetting<bool>("ShowCurrentlyInProfileTooltips", true, (Func<string>)(() => "Show Currently in profile tooltips"), (Func<string>)(() => "Shows the Currently section when it is available."));
			ShowOocInfoInProfileTooltips = UiSettings.DefineSetting<bool>("ShowOocInfoInProfileTooltips", true, (Func<string>)(() => "Show OOC info in profile tooltips"), (Func<string>)(() => "Shows the Out of Character section when it is available."));
			TrimLongProfileTooltips = UiSettings.DefineSetting<bool>("TrimLongProfileTooltips", true, (Func<string>)(() => "Trim long profile tooltips"), (Func<string>)(() => "Limits each enabled profile-tooltip section to the configured number of wrapped lines."));
			ProfileTooltipLinesPerSection = UiSettings.DefineSetting<int>("ProfileTooltipLinesPerSection", 12, (Func<string>)(() => "Profile tooltip lines per section"), (Func<string>)(() => "The maximum number of wrapped lines shown for each enabled profile-tooltip section when trimming is enabled."));
		}

		public IReadOnlyCollection<string> GetBlockedAccountNames()
		{
			return (from account in (BlockedAccounts.get_Value() ?? string.Empty).Split(new string[2] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
				select account.Trim()).Where(IsValidAccountName).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
		}

		public bool AddBlockedAccount(string accountName)
		{
			if (!IsValidAccountName(accountName))
			{
				return false;
			}
			List<string> accounts = GetBlockedAccountNames().ToList();
			if (accounts.Contains(accountName.Trim(), StringComparer.OrdinalIgnoreCase))
			{
				return false;
			}
			accounts.Add(accountName.Trim());
			SetBlockedAccountNames(accounts);
			return true;
		}

		public bool RemoveBlockedAccount(string accountName)
		{
			if (string.IsNullOrWhiteSpace(accountName))
			{
				return false;
			}
			List<string> accounts = GetBlockedAccountNames().ToList();
			bool num = accounts.RemoveAll((string account) => string.Equals(account, accountName.Trim(), StringComparison.OrdinalIgnoreCase)) > 0;
			if (num)
			{
				SetBlockedAccountNames(accounts);
			}
			return num;
		}

		public bool IsBlockedAccount(string accountName)
		{
			if (string.IsNullOrWhiteSpace(accountName))
			{
				return false;
			}
			return GetBlockedAccountNames().Contains(accountName.Trim(), StringComparer.OrdinalIgnoreCase);
		}

		public void SetBlockedAccountNames(IEnumerable<string> accountNames)
		{
			List<string> accounts = (accountNames ?? Enumerable.Empty<string>()).Select((string account) => account?.Trim() ?? string.Empty).Where(IsValidAccountName).Distinct(StringComparer.OrdinalIgnoreCase)
				.OrderBy((string account) => account, StringComparer.OrdinalIgnoreCase)
				.ToList();
			BlockedAccounts.set_Value(string.Join(Environment.NewLine, accounts));
		}

		public string GetServerBaseUrl()
		{
			return "https://spark.a-bat.com";
		}

		public Point GetNearbyWindowLocation(Point fallback)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			string value = NearbyWindowLocation.get_Value()?.Trim() ?? string.Empty;
			if (string.IsNullOrWhiteSpace(value))
			{
				return fallback;
			}
			string[] parts = value.Split(',');
			if (parts.Length != 2)
			{
				return fallback;
			}
			if (!int.TryParse(parts[0], out var x) || !int.TryParse(parts[1], out var y))
			{
				return fallback;
			}
			return new Point(x, y);
		}

		public void SetNearbyWindowLocation(Point location)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			NearbyWindowLocation.set_Value($"{location.X},{location.Y}");
		}

		public static bool IsValidAccountName(string accountName)
		{
			if (!string.IsNullOrWhiteSpace(accountName))
			{
				return AccountNameRegex.IsMatch(accountName.Trim());
			}
			return false;
		}

		public static int ProfileTooltipLimit(int value)
		{
			switch (value)
			{
			case 2:
			case 4:
			case 6:
			case 8:
			case 10:
			case 12:
			case 14:
			case 16:
			case 18:
			case 20:
				return value;
			default:
				return 12;
			}
		}
	}
}
