using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace GilledWars
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class strings
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					resourceMan = new ResourceManager("GilledWars.strings", typeof(strings).Assembly);
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static string StartLoggingBtnText => ResourceManager.GetString("StartLoggingBtnText", resourceCulture);

		internal static string StopLoggingBtnText => ResourceManager.GetString("StopLoggingBtnText", resourceCulture);

		internal static string MeasureFishBtnText => ResourceManager.GetString("MeasureFishBtnText", resourceCulture);

		internal static string CompactModeBtnText => ResourceManager.GetString("CompactModeBtnText", resourceCulture);

		internal static string WhatsBitingBtnText => ResourceManager.GetString("WhatsBitingBtnText", resourceCulture);

		internal static string ProfitTrackerBtnText => ResourceManager.GetString("ProfitTrackerBtnText", resourceCulture);

		internal static string RecentCatchesText => ResourceManager.GetString("RecentCatchesText", resourceCulture);

		internal static string HostTournamentBtnText => ResourceManager.GetString("HostTournamentBtnText", resourceCulture);

		internal static string JoinTournamentBtnText => ResourceManager.GetString("JoinTournamentBtnText", resourceCulture);

		internal static string UseDrfCheckboxText => ResourceManager.GetString("UseDrfCheckboxText", resourceCulture);

		internal static string UseDrfTooltip => ResourceManager.GetString("UseDrfTooltip", resourceCulture);

		internal static string WhatsBitingTooltip => ResourceManager.GetString("WhatsBitingTooltip", resourceCulture);

		internal static string ProfitTrackerTooltip => ResourceManager.GetString("ProfitTrackerTooltip", resourceCulture);

		internal static string CloseLodgeTooltip => ResourceManager.GetString("CloseLodgeTooltip", resourceCulture);

		internal static string SearchFishPlaceholder => ResourceManager.GetString("SearchFishPlaceholder", resourceCulture);

		internal static string CollapseBtnText => ResourceManager.GetString("CollapseBtnText", resourceCulture);

		internal static string RevealBtnText => ResourceManager.GetString("RevealBtnText", resourceCulture);

		internal static string ResetFiltersBtnText => ResourceManager.GetString("ResetFiltersBtnText", resourceCulture);

		internal static string PushPbBtnText => ResourceManager.GetString("PushPbBtnText", resourceCulture);

		internal static string ZoneAnalyzerBtnText => ResourceManager.GetString("ZoneAnalyzerBtnText", resourceCulture);

		internal static string MetaProgressBtnText => ResourceManager.GetString("MetaProgressBtnText", resourceCulture);

		internal static string PushPbTooltip => ResourceManager.GetString("PushPbTooltip", resourceCulture);

		internal static string ZoneAnalyzerTooltip => ResourceManager.GetString("ZoneAnalyzerTooltip", resourceCulture);

		internal static string MetaProgressTooltip => ResourceManager.GetString("MetaProgressTooltip", resourceCulture);

		internal static string HostSetupTitle => ResourceManager.GetString("HostSetupTitle", resourceCulture);

		internal static string StartDelayLabel => ResourceManager.GetString("StartDelayLabel", resourceCulture);

		internal static string DurationLabel => ResourceManager.GetString("DurationLabel", resourceCulture);

		internal static string TrackingModeLabel => ResourceManager.GetString("TrackingModeLabel", resourceCulture);

		internal static string TargetSpeciesLabel => ResourceManager.GetString("TargetSpeciesLabel", resourceCulture);

		internal static string WinFactorLabel => ResourceManager.GetString("WinFactorLabel", resourceCulture);

		internal static string CreateRoomBtn => ResourceManager.GetString("CreateRoomBtn", resourceCulture);

		internal static string JoinRoomTitle => ResourceManager.GetString("JoinRoomTitle", resourceCulture);

		internal static string EnterRoomCodeLabel => ResourceManager.GetString("EnterRoomCodeLabel", resourceCulture);

		internal static string JoinRoomBtn => ResourceManager.GetString("JoinRoomBtn", resourceCulture);

		internal static string ManualVerifyTitle => ResourceManager.GetString("ManualVerifyTitle", resourceCulture);

		internal static string PasteCodePlaceholder => ResourceManager.GetString("PasteCodePlaceholder", resourceCulture);

		internal static string VerifyCodeBtn => ResourceManager.GetString("VerifyCodeBtn", resourceCulture);

		internal static string TrackingModesInfo => ResourceManager.GetString("TrackingModesInfo", resourceCulture);

		internal static string LeaderboardTitle => ResourceManager.GetString("LeaderboardTitle", resourceCulture);

		internal static string SortLabel => ResourceManager.GetString("SortLabel", resourceCulture);

		internal static string FishLabel => ResourceManager.GetString("FishLabel", resourceCulture);

		internal static string AllSpeciesFilterBtn => ResourceManager.GetString("AllSpeciesFilterBtn", resourceCulture);

		internal static string RefreshBtn => ResourceManager.GetString("RefreshBtn", resourceCulture);

		internal static string WeightDropdown => ResourceManager.GetString("WeightDropdown", resourceCulture);

		internal static string LengthDropdown => ResourceManager.GetString("LengthDropdown", resourceCulture);

		internal static string BitingNowTitle => ResourceManager.GetString("BitingNowTitle", resourceCulture);

		internal static string PbWeightLabel => ResourceManager.GetString("PbWeightLabel", resourceCulture);

		internal static string PbLengthLabel => ResourceManager.GetString("PbLengthLabel", resourceCulture);

		internal static string BaitLabel => ResourceManager.GetString("BaitLabel", resourceCulture);

		internal static string ExpandingMainView => ResourceManager.GetString("ExpandingMainView", resourceCulture);

		internal static string LeaderboardRefreshed => ResourceManager.GetString("LeaderboardRefreshed", resourceCulture);

		internal static string NoRecordsFound => ResourceManager.GetString("NoRecordsFound", resourceCulture);

		internal static string NetworkError => ResourceManager.GetString("NetworkError", resourceCulture);

		internal static string NoAchievFound => ResourceManager.GetString("NoAchievFound", resourceCulture);

		internal static string TourneyBegin => ResourceManager.GetString("TourneyBegin", resourceCulture);

		internal static string ResultsSubmitted => ResourceManager.GetString("ResultsSubmitted", resourceCulture);

		internal static string LoadingTop10 => ResourceManager.GetString("LoadingTop10", resourceCulture);

		internal static string Ready => ResourceManager.GetString("Ready", resourceCulture);

		internal static string Measuring => ResourceManager.GetString("Measuring", resourceCulture);

		internal static string DrfActive => ResourceManager.GetString("DrfActive", resourceCulture);

		internal static string DrfPaused => ResourceManager.GetString("DrfPaused", resourceCulture);

		internal static string ApiPaused => ResourceManager.GetString("ApiPaused", resourceCulture);

		internal static string StartingIn => ResourceManager.GetString("StartingIn", resourceCulture);

		internal static string Top5Catches => ResourceManager.GetString("Top5Catches", resourceCulture);

		internal static string ApiErrorBags => ResourceManager.GetString("ApiErrorBags", resourceCulture);

		internal static string FishMeasuredCount => ResourceManager.GetString("FishMeasuredCount", resourceCulture);

		internal static string ApiErrorKey => ResourceManager.GetString("ApiErrorKey", resourceCulture);

		internal static string SnapshotSaved => ResourceManager.GetString("SnapshotSaved", resourceCulture);

		internal static string DrfErrorToken => ResourceManager.GetString("DrfErrorToken", resourceCulture);

		internal static string DrfConnected => ResourceManager.GetString("DrfConnected", resourceCulture);

		internal static string DrfFailed => ResourceManager.GetString("DrfFailed", resourceCulture);

		internal static string CasualLoggingStopped => ResourceManager.GetString("CasualLoggingStopped", resourceCulture);

		internal static string RoomCreated => ResourceManager.GetString("RoomCreated", resourceCulture);

		internal static string ApiErrorRoom => ResourceManager.GetString("ApiErrorRoom", resourceCulture);

		internal static string InvalidRoomCode => ResourceManager.GetString("InvalidRoomCode", resourceCulture);

		internal static string RoomNotFound => ResourceManager.GetString("RoomNotFound", resourceCulture);

		internal static string DrfResumeWait => ResourceManager.GetString("DrfResumeWait", resourceCulture);

		internal static string TourneyStarted => ResourceManager.GetString("TourneyStarted", resourceCulture);

		internal static string BackupCopied => ResourceManager.GetString("BackupCopied", resourceCulture);

		internal static string TourneyCatchAlert => ResourceManager.GetString("TourneyCatchAlert", resourceCulture);

		internal static string CaughtAlert => ResourceManager.GetString("CaughtAlert", resourceCulture);

		internal static string FishCodeCopied => ResourceManager.GetString("FishCodeCopied", resourceCulture);

		internal static string RefreshCooldown => ResourceManager.GetString("RefreshCooldown", resourceCulture);

		internal static string SubmitCooldown => ResourceManager.GetString("SubmitCooldown", resourceCulture);

		internal static string UiToggled => ResourceManager.GetString("UiToggled", resourceCulture);

		internal static string PbClipText => ResourceManager.GetString("PbClipText", resourceCulture);

		internal static string AutoSyncSuccess => ResourceManager.GetString("AutoSyncSuccess", resourceCulture);

		internal static string NoNewSubmissions => ResourceManager.GetString("NoNewSubmissions", resourceCulture);

		internal static string PushingPBs => ResourceManager.GetString("PushingPBs", resourceCulture);

		internal static string ServerRejected => ResourceManager.GetString("ServerRejected", resourceCulture);

		internal static string FailedConnectLB => ResourceManager.GetString("FailedConnectLB", resourceCulture);

		internal static string TournamentOver => ResourceManager.GetString("TournamentOver", resourceCulture);

		internal static string JunkMessages => ResourceManager.GetString("JunkMessages", resourceCulture);

		internal static string TreasureMessages => ResourceManager.GetString("TreasureMessages", resourceCulture);

		internal static string Loading => ResourceManager.GetString("Loading", resourceCulture);

		internal static string MissingLabel => ResourceManager.GetString("MissingLabel", resourceCulture);

		internal static string AllLabel => ResourceManager.GetString("AllLabel", resourceCulture);

		internal static string BitingNowLoading => ResourceManager.GetString("BitingNowLoading", resourceCulture);

		internal static string ToggleBitingTooltip => ResourceManager.GetString("ToggleBitingTooltip", resourceCulture);

		internal static string TargetAllSpecies => ResourceManager.GetString("TargetAllSpecies", resourceCulture);

		internal static string TargetPrefix => ResourceManager.GetString("TargetPrefix", resourceCulture);

		internal static string ApiErrorKeyOrChar => ResourceManager.GetString("ApiErrorKeyOrChar", resourceCulture);

		internal static string LoadingData => ResourceManager.GetString("LoadingData", resourceCulture);

		internal static string FetchingAchievements => ResourceManager.GetString("FetchingAchievements", resourceCulture);

		internal static string SearchSpeciesPlaceholder => ResourceManager.GetString("SearchSpeciesPlaceholder", resourceCulture);

		internal static string Scanning => ResourceManager.GetString("Scanning", resourceCulture);

		internal static string Creating => ResourceManager.GetString("Creating", resourceCulture);

		internal static string Joining => ResourceManager.GetString("Joining", resourceCulture);

		internal static string TourneyEndedTitle => ResourceManager.GetString("TourneyEndedTitle", resourceCulture);

		internal static string TourneyEndedManualTitle => ResourceManager.GetString("TourneyEndedManualTitle", resourceCulture);

		internal static string TourneySubmitting => ResourceManager.GetString("TourneySubmitting", resourceCulture);

		internal static string TourneyResultsInitial => ResourceManager.GetString("TourneyResultsInitial", resourceCulture);

		internal static string TourneyPostedDiscord => ResourceManager.GetString("TourneyPostedDiscord", resourceCulture);

		internal static string TourneyApiFailed => ResourceManager.GetString("TourneyApiFailed", resourceCulture);

		internal static string TourneyNetworkError => ResourceManager.GetString("TourneyNetworkError", resourceCulture);

		internal static string TourneyComplete => ResourceManager.GetString("TourneyComplete", resourceCulture);

		internal static string CornerIconTooltip => ResourceManager.GetString("CornerIconTooltip", resourceCulture);

		internal static string CornerIconSubtitle => ResourceManager.GetString("CornerIconSubtitle", resourceCulture);

		internal static string ProfitTotalDefault => ResourceManager.GetString("ProfitTotalDefault", resourceCulture);

		internal static string GphDefault => ResourceManager.GetString("GphDefault", resourceCulture);

		internal static string ServerError => ResourceManager.GetString("ServerError", resourceCulture);

		internal static string NoCatchesYet => ResourceManager.GetString("NoCatchesYet", resourceCulture);

		internal static string RankLabel => ResourceManager.GetString("RankLabel", resourceCulture);

		internal static string AnglerLabel => ResourceManager.GetString("AnglerLabel", resourceCulture);

		internal static string SpeciesLabel => ResourceManager.GetString("SpeciesLabel", resourceCulture);

		internal static string MetaTrackerTitle => ResourceManager.GetString("MetaTrackerTitle", resourceCulture);

		internal static string CasualFishingLabel => ResourceManager.GetString("CasualFishingLabel", resourceCulture);

		internal static string CasualFishingTooltip => ResourceManager.GetString("CasualFishingTooltip", resourceCulture);

		internal static string TourneyModeTooltip => ResourceManager.GetString("TourneyModeTooltip", resourceCulture);

		internal static string FishLogTooltip => ResourceManager.GetString("FishLogTooltip", resourceCulture);

		internal static string LeaderboardTooltip => ResourceManager.GetString("LeaderboardTooltip", resourceCulture);

		internal static string OpenWebsiteTooltip => ResourceManager.GetString("OpenWebsiteTooltip", resourceCulture);

		internal static string NoneLogged => ResourceManager.GetString("NoneLogged", resourceCulture);

		internal static string CheaterDetected => ResourceManager.GetString("CheaterDetected", resourceCulture);

		internal static string CloseBtnText => ResourceManager.GetString("CloseBtnText", resourceCulture);

		internal static string UploadingText => ResourceManager.GetString("UploadingText", resourceCulture);

		internal static string EndSubmitBtn => ResourceManager.GetString("EndSubmitBtn", resourceCulture);

		internal static string ReCopyCodeBtn => ResourceManager.GetString("ReCopyCodeBtn", resourceCulture);

		internal static string ReCopyCodeTooltip => ResourceManager.GetString("ReCopyCodeTooltip", resourceCulture);

		internal static string ExitTourneyBtn => ResourceManager.GetString("ExitTourneyBtn", resourceCulture);

		internal static string ExitTourneyTooltip => ResourceManager.GetString("ExitTourneyTooltip", resourceCulture);

		internal static string Top5CatchesLabel => ResourceManager.GetString("Top5CatchesLabel", resourceCulture);

		internal static string ExitedTournament => ResourceManager.GetString("ExitedTournament", resourceCulture);

		internal static string TournamentOverWeighing => ResourceManager.GetString("TournamentOverWeighing", resourceCulture);

		internal static string RecentCatchesCompact => ResourceManager.GetString("RecentCatchesCompact", resourceCulture);

		internal static string MaximizeUI => ResourceManager.GetString("MaximizeUI", resourceCulture);

		internal static string NoFishingZone => ResourceManager.GetString("NoFishingZone", resourceCulture);

		internal static string BitingApiSyncing => ResourceManager.GetString("BitingApiSyncing", resourceCulture);

		internal static string NameColumnHeader => ResourceManager.GetString("NameColumnHeader", resourceCulture);

		internal static string BaitColumnHeader => ResourceManager.GetString("BaitColumnHeader", resourceCulture);

		internal static string TimeColumnHeader => ResourceManager.GetString("TimeColumnHeader", resourceCulture);

		internal static string HoleColumnHeader => ResourceManager.GetString("HoleColumnHeader", resourceCulture);

		internal static string ToggleGridView => ResourceManager.GetString("ToggleGridView", resourceCulture);

		internal static string SettingToggleUI => ResourceManager.GetString("SettingToggleUI", resourceCulture);

		internal static string SettingToggleUIDesc => ResourceManager.GetString("SettingToggleUIDesc", resourceCulture);

		internal static string SettingCustomApiKey => ResourceManager.GetString("SettingCustomApiKey", resourceCulture);

		internal static string SettingCustomApiKeyDesc => ResourceManager.GetString("SettingCustomApiKeyDesc", resourceCulture);

		internal static string SettingDrfToken => ResourceManager.GetString("SettingDrfToken", resourceCulture);

		internal static string SettingDrfTokenDesc => ResourceManager.GetString("SettingDrfTokenDesc", resourceCulture);

		internal static string SettingDiscordWebhook => ResourceManager.GetString("SettingDiscordWebhook", resourceCulture);

		internal static string SettingDiscordWebhookDesc => ResourceManager.GetString("SettingDiscordWebhookDesc", resourceCulture);

		internal static string SettingCornerIcon => ResourceManager.GetString("SettingCornerIcon", resourceCulture);

		internal static string SettingCornerIconDesc => ResourceManager.GetString("SettingCornerIconDesc", resourceCulture);

		internal static string SettingAutoStartLogging => ResourceManager.GetString("SettingAutoStartLogging", resourceCulture);

		internal static string SettingAutoStartLoggingDesc => ResourceManager.GetString("SettingAutoStartLoggingDesc", resourceCulture);

		internal static string SettingAutoOpenBiting => ResourceManager.GetString("SettingAutoOpenBiting", resourceCulture);

		internal static string SettingAutoOpenBitingDesc => ResourceManager.GetString("SettingAutoOpenBitingDesc", resourceCulture);

		internal static string SettingShowProfit => ResourceManager.GetString("SettingShowProfit", resourceCulture);

		internal static string SettingShowProfitDesc => ResourceManager.GetString("SettingShowProfitDesc", resourceCulture);

		internal static string SettingShowTimeOfDay => ResourceManager.GetString("SettingShowTimeOfDay", resourceCulture);

		internal static string SettingShowTimeOfDayDesc => ResourceManager.GetString("SettingShowTimeOfDayDesc", resourceCulture);

		internal static string SettingLockTimeOfDay => ResourceManager.GetString("SettingLockTimeOfDay", resourceCulture);

		internal static string SettingLockTimeOfDayDesc => ResourceManager.GetString("SettingLockTimeOfDayDesc", resourceCulture);

		internal static string SettingWidgetIconSize => ResourceManager.GetString("SettingWidgetIconSize", resourceCulture);

		internal static string SettingWidgetIconSizeDesc => ResourceManager.GetString("SettingWidgetIconSizeDesc", resourceCulture);

		internal static string SettingTextLayout => ResourceManager.GetString("SettingTextLayout", resourceCulture);

		internal static string SettingTextLayoutDesc => ResourceManager.GetString("SettingTextLayoutDesc", resourceCulture);

		internal static string SettingUILanguage => ResourceManager.GetString("SettingUILanguage", resourceCulture);

		internal static string SettingUILanguageDesc => ResourceManager.GetString("SettingUILanguageDesc", resourceCulture);

		internal strings()
		{
		}
	}
}
