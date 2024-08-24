using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Ideka.RacingMeter
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Strings
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
					resourceMan = new ResourceManager("Ideka.RacingMeter.Strings", typeof(Strings).Assembly);
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

		internal static string AlreadySetTooltip => ResourceManager.GetString("AlreadySetTooltip", resourceCulture);

		internal static string AreYouSure => ResourceManager.GetString("AreYouSure", resourceCulture);

		internal static string AvatarSnapXY => ResourceManager.GetString("AvatarSnapXY", resourceCulture);

		internal static string AvatarSnapXYZ => ResourceManager.GetString("AvatarSnapXYZ", resourceCulture);

		internal static string BackToEditing => ResourceManager.GetString("BackToEditing", resourceCulture);

		internal static string BackToRacing => ResourceManager.GetString("BackToRacing", resourceCulture);

		internal static string ByInfo => ResourceManager.GetString("ByInfo", resourceCulture);

		internal static string Cancel => ResourceManager.GetString("Cancel", resourceCulture);

		internal static string ChangeToCurrent => ResourceManager.GetString("ChangeToCurrent", resourceCulture);

		internal static string ConfirmRaceDeletion => ResourceManager.GetString("ConfirmRaceDeletion", resourceCulture);

		internal static string Connect => ResourceManager.GetString("Connect", resourceCulture);

		internal static string ConnectGuest => ResourceManager.GetString("ConnectGuest", resourceCulture);

		internal static string CopyOf => ResourceManager.GetString("CopyOf", resourceCulture);

		internal static string CopyToClipboard => ResourceManager.GetString("CopyToClipboard", resourceCulture);

		internal static string CsvImportWarning => ResourceManager.GetString("CsvImportWarning", resourceCulture);

		internal static string Delete => ResourceManager.GetString("Delete", resourceCulture);

		internal static string DeleteRace => ResourceManager.GetString("DeleteRace", resourceCulture);

		internal static string Deselect => ResourceManager.GetString("Deselect", resourceCulture);

		internal static string Disconnect => ResourceManager.GetString("Disconnect", resourceCulture);

		internal static string DiscordButton => ResourceManager.GetString("DiscordButton", resourceCulture);

		internal static string DiscordTooltip => ResourceManager.GetString("DiscordTooltip", resourceCulture);

		internal static string ErrorDisconnect => ResourceManager.GetString("ErrorDisconnect", resourceCulture);

		internal static string ErrorFailedToConnect => ResourceManager.GetString("ErrorFailedToConnect", resourceCulture);

		internal static string ErrorFormat => ResourceManager.GetString("ErrorFormat", resourceCulture);

		internal static string ErrorGhostLoad => ResourceManager.GetString("ErrorGhostLoad", resourceCulture);

		internal static string ErrorGhostUpload => ResourceManager.GetString("ErrorGhostUpload", resourceCulture);

		internal static string ErrorLeaderboardLoad => ResourceManager.GetString("ErrorLeaderboardLoad", resourceCulture);

		internal static string ErrorMismatchedVersion => ResourceManager.GetString("ErrorMismatchedVersion", resourceCulture);

		internal static string ErrorRaceImport => ResourceManager.GetString("ErrorRaceImport", resourceCulture);

		internal static string ErrorRaceLoad => ResourceManager.GetString("ErrorRaceLoad", resourceCulture);

		internal static string ErrorRacesLoad => ResourceManager.GetString("ErrorRacesLoad", resourceCulture);

		internal static string ErrorSendFailed => ResourceManager.GetString("ErrorSendFailed", resourceCulture);

		internal static string ExceptionApiAccount => ResourceManager.GetString("ExceptionApiAccount", resourceCulture);

		internal static string ExceptionEmptyRaceName => ResourceManager.GetString("ExceptionEmptyRaceName", resourceCulture);

		internal static string ExceptionGeneric => ResourceManager.GetString("ExceptionGeneric", resourceCulture);

		internal static string ExceptionInvalidPointType => ResourceManager.GetString("ExceptionInvalidPointType", resourceCulture);

		internal static string ExceptionInvalidRaceMap => ResourceManager.GetString("ExceptionInvalidRaceMap", resourceCulture);

		internal static string ExceptionInvalidRaceName => ResourceManager.GetString("ExceptionInvalidRaceName", resourceCulture);

		internal static string ExceptionInvalidRaceType => ResourceManager.GetString("ExceptionInvalidRaceType", resourceCulture);

		internal static string ExceptionMinimumCheckpoints => ResourceManager.GetString("ExceptionMinimumCheckpoints", resourceCulture);

		internal static string ExceptionNoAccountApiPermission => ResourceManager.GetString("ExceptionNoAccountApiPermission", resourceCulture);

		internal static string ExceptionOfflineMode => ResourceManager.GetString("ExceptionOfflineMode", resourceCulture);

		internal static string ExceptionPointRadius => ResourceManager.GetString("ExceptionPointRadius", resourceCulture);

		internal static string ExceptionStillConnecting => ResourceManager.GetString("ExceptionStillConnecting", resourceCulture);

		internal static string ExceptionUnauthenticated => ResourceManager.GetString("ExceptionUnauthenticated", resourceCulture);

		internal static string Ghost => ResourceManager.GetString("Ghost", resourceCulture);

		internal static string GhostDescribeBy => ResourceManager.GetString("GhostDescribeBy", resourceCulture);

		internal static string GhostDescribeLocal => ResourceManager.GetString("GhostDescribeLocal", resourceCulture);

		internal static string GhostDescribeRemote => ResourceManager.GetString("GhostDescribeRemote", resourceCulture);

		internal static string GhostLabel => ResourceManager.GetString("GhostLabel", resourceCulture);

		internal static string GhostLocal => ResourceManager.GetString("GhostLocal", resourceCulture);

		internal static string GhostLocalRacerTooltip => ResourceManager.GetString("GhostLocalRacerTooltip", resourceCulture);

		internal static string GhostLocalUploadedTooltip => ResourceManager.GetString("GhostLocalUploadedTooltip", resourceCulture);

		internal static string Guest => ResourceManager.GetString("Guest", resourceCulture);

		internal static string ImportCsv => ResourceManager.GetString("ImportCsv", resourceCulture);

		internal static string Inches => ResourceManager.GetString("Inches", resourceCulture);

		internal static string InsertAfter => ResourceManager.GetString("InsertAfter", resourceCulture);

		internal static string InsertBefore => ResourceManager.GetString("InsertBefore", resourceCulture);

		internal static string KoFiButton => ResourceManager.GetString("KoFiButton", resourceCulture);

		internal static string KoFiTooltip => ResourceManager.GetString("KoFiTooltip", resourceCulture);

		internal static string LabelBest => ResourceManager.GetString("LabelBest", resourceCulture);

		internal static string LabelCheckpoint => ResourceManager.GetString("LabelCheckpoint", resourceCulture);

		internal static string LabelCurrentGhost => ResourceManager.GetString("LabelCurrentGhost", resourceCulture);

		internal static string LabelCurrentRace => ResourceManager.GetString("LabelCurrentRace", resourceCulture);

		internal static string LabelEditing => ResourceManager.GetString("LabelEditing", resourceCulture);

		internal static string LabelFinishTime => ResourceManager.GetString("LabelFinishTime", resourceCulture);

		internal static string LabelGhostRacer => ResourceManager.GetString("LabelGhostRacer", resourceCulture);

		internal static string LabelGhostTime => ResourceManager.GetString("LabelGhostTime", resourceCulture);

		internal static string LabelGhostUploaded => ResourceManager.GetString("LabelGhostUploaded", resourceCulture);

		internal static string LabelGuest => ResourceManager.GetString("LabelGuest", resourceCulture);

		internal static string LabelLap => ResourceManager.GetString("LabelLap", resourceCulture);

		internal static string LabelLast => ResourceManager.GetString("LabelLast", resourceCulture);

		internal static string LabelLobbyRacers => ResourceManager.GetString("LabelLobbyRacers", resourceCulture);

		internal static string LabelLobbyUsersNotReady => ResourceManager.GetString("LabelLobbyUsersNotReady", resourceCulture);

		internal static string LabelPing => ResourceManager.GetString("LabelPing", resourceCulture);

		internal static string LabelRace => ResourceManager.GetString("LabelRace", resourceCulture);

		internal static string LabelRaceAuthor => ResourceManager.GetString("LabelRaceAuthor", resourceCulture);

		internal static string LabelRaceCheckpoints => ResourceManager.GetString("LabelRaceCheckpoints", resourceCulture);

		internal static string LabelRaceLaps => ResourceManager.GetString("LabelRaceLaps", resourceCulture);

		internal static string LabelRaceLength => ResourceManager.GetString("LabelRaceLength", resourceCulture);

		internal static string LabelRaceLooping => ResourceManager.GetString("LabelRaceLooping", resourceCulture);

		internal static string LabelRaceMap => ResourceManager.GetString("LabelRaceMap", resourceCulture);

		internal static string LabelRaceType => ResourceManager.GetString("LabelRaceType", resourceCulture);

		internal static string LabelRaceUpdated => ResourceManager.GetString("LabelRaceUpdated", resourceCulture);

		internal static string Leaderboard => ResourceManager.GetString("Leaderboard", resourceCulture);

		internal static string LoadGhost => ResourceManager.GetString("LoadGhost", resourceCulture);

		internal static string Loading => ResourceManager.GetString("Loading", resourceCulture);

		internal static string LobbyCreate => ResourceManager.GetString("LobbyCreate", resourceCulture);

		internal static string LobbyId => ResourceManager.GetString("LobbyId", resourceCulture);

		internal static string LobbyIdInput => ResourceManager.GetString("LobbyIdInput", resourceCulture);

		internal static string LobbyJoin => ResourceManager.GetString("LobbyJoin", resourceCulture);

		internal static string LobbyLeaderboard => ResourceManager.GetString("LobbyLeaderboard", resourceCulture);

		internal static string LobbyLeave => ResourceManager.GetString("LobbyLeave", resourceCulture);

		internal static string LobbyMaxUsers => ResourceManager.GetString("LobbyMaxUsers", resourceCulture);

		internal static string LobbyRaceCancel => ResourceManager.GetString("LobbyRaceCancel", resourceCulture);

		internal static string LobbyRaceLaps => ResourceManager.GetString("LobbyRaceLaps", resourceCulture);

		internal static string LobbyRaceSelected => ResourceManager.GetString("LobbyRaceSelected", resourceCulture);

		internal static string LobbyRaceSet => ResourceManager.GetString("LobbyRaceSet", resourceCulture);

		internal static string LobbyRaceStart => ResourceManager.GetString("LobbyRaceStart", resourceCulture);

		internal static string LobbySettings => ResourceManager.GetString("LobbySettings", resourceCulture);

		internal static string LobbyUserIsHost => ResourceManager.GetString("LobbyUserIsHost", resourceCulture);

		internal static string LobbyUserIsRacer => ResourceManager.GetString("LobbyUserIsRacer", resourceCulture);

		internal static string LobbyUserKick => ResourceManager.GetString("LobbyUserKick", resourceCulture);

		internal static string LobbyUsers => ResourceManager.GetString("LobbyUsers", resourceCulture);

		internal static string LocalGhosts => ResourceManager.GetString("LocalGhosts", resourceCulture);

		internal static string LocalRaces => ResourceManager.GetString("LocalRaces", resourceCulture);

		internal static string Meters => ResourceManager.GetString("Meters", resourceCulture);

		internal static string NewRace => ResourceManager.GetString("NewRace", resourceCulture);

		internal static string NewRaceName => ResourceManager.GetString("NewRaceName", resourceCulture);

		internal static string Next => ResourceManager.GetString("Next", resourceCulture);

		internal static string NicknameInput => ResourceManager.GetString("NicknameInput", resourceCulture);

		internal static string NicknameTooltip => ResourceManager.GetString("NicknameTooltip", resourceCulture);

		internal static string NicknameUpdate => ResourceManager.GetString("NicknameUpdate", resourceCulture);

		internal static string No => ResourceManager.GetString("No", resourceCulture);

		internal static string None => ResourceManager.GetString("None", resourceCulture);

		internal static string Nothing => ResourceManager.GetString("Nothing", resourceCulture);

		internal static string NoticeCopied => ResourceManager.GetString("NoticeCopied", resourceCulture);

		internal static string NotifyConnectingTo => ResourceManager.GetString("NotifyConnectingTo", resourceCulture);

		internal static string NotifyGhostAlreadyBetter => ResourceManager.GetString("NotifyGhostAlreadyBetter", resourceCulture);

		internal static string NotifyLogInRequired => ResourceManager.GetString("NotifyLogInRequired", resourceCulture);

		internal static string NotifyNoRaceSelected => ResourceManager.GetString("NotifyNoRaceSelected", resourceCulture);

		internal static string NotifyNotEnoughCheckpoints => ResourceManager.GetString("NotifyNotEnoughCheckpoints", resourceCulture);

		internal static string NotifyRaceSaved => ResourceManager.GetString("NotifyRaceSaved", resourceCulture);

		internal static string NotifyRaceStartFailed => ResourceManager.GetString("NotifyRaceStartFailed", resourceCulture);

		internal static string OnlineNoticeGo => ResourceManager.GetString("OnlineNoticeGo", resourceCulture);

		internal static string OnlineNoticeLaps => ResourceManager.GetString("OnlineNoticeLaps", resourceCulture);

		internal static string OnlineNoticeRaceCanceled => ResourceManager.GetString("OnlineNoticeRaceCanceled", resourceCulture);

		internal static string OnlineNoticeRaceFinished => ResourceManager.GetString("OnlineNoticeRaceFinished", resourceCulture);

		internal static string OnlineRacing => ResourceManager.GetString("OnlineRacing", resourceCulture);

		internal static string PointDescription => ResourceManager.GetString("PointDescription", resourceCulture);

		internal static string PointType => ResourceManager.GetString("PointType", resourceCulture);

		internal static string PointTypeCheckpoint => ResourceManager.GetString("PointTypeCheckpoint", resourceCulture);

		internal static string PointTypeGuide => ResourceManager.GetString("PointTypeGuide", resourceCulture);

		internal static string PointTypeLoopStart => ResourceManager.GetString("PointTypeLoopStart", resourceCulture);

		internal static string PointTypeReset => ResourceManager.GetString("PointTypeReset", resourceCulture);

		internal static string Prev => ResourceManager.GetString("Prev", resourceCulture);

		internal static string Race => ResourceManager.GetString("Race", resourceCulture);

		internal static string RaceEditor => ResourceManager.GetString("RaceEditor", resourceCulture);

		internal static string RaceId => ResourceManager.GetString("RaceId", resourceCulture);

		internal static string RaceIdTooltip => ResourceManager.GetString("RaceIdTooltip", resourceCulture);

		internal static string RaceLocal => ResourceManager.GetString("RaceLocal", resourceCulture);

		internal static string RaceLocalAuthorTooltip => ResourceManager.GetString("RaceLocalAuthorTooltip", resourceCulture);

		internal static string RaceLocalLeaderboardTooltip => ResourceManager.GetString("RaceLocalLeaderboardTooltip", resourceCulture);

		internal static string RaceMap => ResourceManager.GetString("RaceMap", resourceCulture);

		internal static string RaceName => ResourceManager.GetString("RaceName", resourceCulture);

		internal static string RacePoints => ResourceManager.GetString("RacePoints", resourceCulture);

		internal static string RacePreview => ResourceManager.GetString("RacePreview", resourceCulture);

		internal static string Races => ResourceManager.GetString("Races", resourceCulture);

		internal static string RaceType => ResourceManager.GetString("RaceType", resourceCulture);

		internal static string RaceTypeCustom => ResourceManager.GetString("RaceTypeCustom", resourceCulture);

		internal static string RaceTypeOfficial => ResourceManager.GetString("RaceTypeOfficial", resourceCulture);

		internal static string RaceTypeUnknown => ResourceManager.GetString("RaceTypeUnknown", resourceCulture);

		internal static string RacingMeter => ResourceManager.GetString("RacingMeter", resourceCulture);

		internal static string Radius => ResourceManager.GetString("Radius", resourceCulture);

		internal static string Redo => ResourceManager.GetString("Redo", resourceCulture);

		internal static string RunRace => ResourceManager.GetString("RunRace", resourceCulture);

		internal static string Save => ResourceManager.GetString("Save", resourceCulture);

		internal static string SelectNearest => ResourceManager.GetString("SelectNearest", resourceCulture);

		internal static string ServerStatusYes => ResourceManager.GetString("ServerStatusYes", resourceCulture);

		internal static string SettingAutoLocalGhost => ResourceManager.GetString("SettingAutoLocalGhost", resourceCulture);

		internal static string SettingAutoLocalGhostText => ResourceManager.GetString("SettingAutoLocalGhostText", resourceCulture);

		internal static string SettingBeetleDriftKey => ResourceManager.GetString("SettingBeetleDriftKey", resourceCulture);

		internal static string SettingBeetleDriftKeyText => ResourceManager.GetString("SettingBeetleDriftKeyText", resourceCulture);

		internal static string SettingCheckpointAlpha => ResourceManager.GetString("SettingCheckpointAlpha", resourceCulture);

		internal static string SettingCheckpointAlphaText => ResourceManager.GetString("SettingCheckpointAlphaText", resourceCulture);

		internal static string SettingCheckpointArrow => ResourceManager.GetString("SettingCheckpointArrow", resourceCulture);

		internal static string SettingCheckpointArrowText => ResourceManager.GetString("SettingCheckpointArrowText", resourceCulture);

		internal static string SettingCheckpointLineThickness => ResourceManager.GetString("SettingCheckpointLineThickness", resourceCulture);

		internal static string SettingCheckpointLineThicknessText => ResourceManager.GetString("SettingCheckpointLineThicknessText", resourceCulture);

		internal static string SettingGriffonMeter => ResourceManager.GetString("SettingGriffonMeter", resourceCulture);

		internal static string SettingMaxGhostData => ResourceManager.GetString("SettingMaxGhostData", resourceCulture);

		internal static string SettingMaxGhostDataText => ResourceManager.GetString("SettingMaxGhostDataText", resourceCulture);

		internal static string SettingMumblePollingRate => ResourceManager.GetString("SettingMumblePollingRate", resourceCulture);

		internal static string SettingMumblePollingRateText => ResourceManager.GetString("SettingMumblePollingRateText", resourceCulture);

		internal static string SettingNormalizedOfficialCheckpoints => ResourceManager.GetString("SettingNormalizedOfficialCheckpoints", resourceCulture);

		internal static string SettingNormalizedOfficialCheckpointsText => ResourceManager.GetString("SettingNormalizedOfficialCheckpointsText", resourceCulture);

		internal static string SettingOnlineDefaultNickname => ResourceManager.GetString("SettingOnlineDefaultNickname", resourceCulture);

		internal static string SettingOnlineDefaultNicknameText => ResourceManager.GetString("SettingOnlineDefaultNicknameText", resourceCulture);

		internal static string SettingOnlineMarkRacers => ResourceManager.GetString("SettingOnlineMarkRacers", resourceCulture);

		internal static string SettingOnlineMarkRacersText => ResourceManager.GetString("SettingOnlineMarkRacersText", resourceCulture);

		internal static string SettingOnlineUrl => ResourceManager.GetString("SettingOnlineUrl", resourceCulture);

		internal static string SettingOnlineUrlText => ResourceManager.GetString("SettingOnlineUrlText", resourceCulture);

		internal static string SettingRacerAnchorX => ResourceManager.GetString("SettingRacerAnchorX", resourceCulture);

		internal static string SettingRacerAnchorXText => ResourceManager.GetString("SettingRacerAnchorXText", resourceCulture);

		internal static string SettingRacerAnchorY => ResourceManager.GetString("SettingRacerAnchorY", resourceCulture);

		internal static string SettingRacerAnchorYText => ResourceManager.GetString("SettingRacerAnchorYText", resourceCulture);

		internal static string SettingRacerHAlignment => ResourceManager.GetString("SettingRacerHAlignment", resourceCulture);

		internal static string SettingRacerHAlignmentText => ResourceManager.GetString("SettingRacerHAlignmentText", resourceCulture);

		internal static string SettingRacerVAlignment => ResourceManager.GetString("SettingRacerVAlignment", resourceCulture);

		internal static string SettingRacerVAlignmentText => ResourceManager.GetString("SettingRacerVAlignmentText", resourceCulture);

		internal static string SettingRollerBeetleMeter => ResourceManager.GetString("SettingRollerBeetleMeter", resourceCulture);

		internal static string SettingSFXVolumeMultiplier => ResourceManager.GetString("SettingSFXVolumeMultiplier", resourceCulture);

		internal static string SettingSFXVolumeMultiplierText => ResourceManager.GetString("SettingSFXVolumeMultiplierText", resourceCulture);

		internal static string SettingShowGuides => ResourceManager.GetString("SettingShowGuides", resourceCulture);

		internal static string SettingShowGuidesText => ResourceManager.GetString("SettingShowGuidesText", resourceCulture);

		internal static string SettingShownCheckpoints => ResourceManager.GetString("SettingShownCheckpoints", resourceCulture);

		internal static string SettingShownCheckpointsText => ResourceManager.GetString("SettingShownCheckpointsText", resourceCulture);

		internal static string SettingShowSpeedometer => ResourceManager.GetString("SettingShowSpeedometer", resourceCulture);

		internal static string SettingShowSpeedometerText => ResourceManager.GetString("SettingShowSpeedometerText", resourceCulture);

		internal static string SettingSiegeTurtleMeter => ResourceManager.GetString("SettingSiegeTurtleMeter", resourceCulture);

		internal static string SettingSkiffMeter => ResourceManager.GetString("SettingSkiffMeter", resourceCulture);

		internal static string SettingSkimmerMeter => ResourceManager.GetString("SettingSkimmerMeter", resourceCulture);

		internal static string SettingSpeedometerAnchorY => ResourceManager.GetString("SettingSpeedometerAnchorY", resourceCulture);

		internal static string SettingSpeedometerAnchorYText => ResourceManager.GetString("SettingSpeedometerAnchorYText", resourceCulture);

		internal static string SettingToggleDebug => ResourceManager.GetString("SettingToggleDebug", resourceCulture);

		internal static string SettingToggleDebugText => ResourceManager.GetString("SettingToggleDebugText", resourceCulture);

		internal static string SettingToggleSpeedometer => ResourceManager.GetString("SettingToggleSpeedometer", resourceCulture);

		internal static string SettingToggleSpeedometerText => ResourceManager.GetString("SettingToggleSpeedometerText", resourceCulture);

		internal static string SettingWarclawMeter => ResourceManager.GetString("SettingWarclawMeter", resourceCulture);

		internal static string SupportStatusFaulted => ResourceManager.GetString("SupportStatusFaulted", resourceCulture);

		internal static string SupportStatusNo => ResourceManager.GetString("SupportStatusNo", resourceCulture);

		internal static string SupportStatusTooltipFaulted => ResourceManager.GetString("SupportStatusTooltipFaulted", resourceCulture);

		internal static string SupportStatusTooltipNo => ResourceManager.GetString("SupportStatusTooltipNo", resourceCulture);

		internal static string SupportStatusTooltipUnchecked => ResourceManager.GetString("SupportStatusTooltipUnchecked", resourceCulture);

		internal static string SupportStatusUnchecked => ResourceManager.GetString("SupportStatusUnchecked", resourceCulture);

		internal static string SwapNext => ResourceManager.GetString("SwapNext", resourceCulture);

		internal static string SwapPrev => ResourceManager.GetString("SwapPrev", resourceCulture);

		internal static string TestFromHere => ResourceManager.GetString("TestFromHere", resourceCulture);

		internal static string TestingModeNotice => ResourceManager.GetString("TestingModeNotice", resourceCulture);

		internal static string TestingRace => ResourceManager.GetString("TestingRace", resourceCulture);

		internal static string TestRace => ResourceManager.GetString("TestRace", resourceCulture);

		internal static string TooltipRaceLooping => ResourceManager.GetString("TooltipRaceLooping", resourceCulture);

		internal static string TooltipRaceNotLooping => ResourceManager.GetString("TooltipRaceNotLooping", resourceCulture);

		internal static string Unavailable => ResourceManager.GetString("Unavailable", resourceCulture);

		internal static string UnavailableOffline => ResourceManager.GetString("UnavailableOffline", resourceCulture);

		internal static string Undo => ResourceManager.GetString("Undo", resourceCulture);

		internal static string UnloadGhost => ResourceManager.GetString("UnloadGhost", resourceCulture);

		internal static string UnloadRace => ResourceManager.GetString("UnloadRace", resourceCulture);

		internal static string UnsavedChanges => ResourceManager.GetString("UnsavedChanges", resourceCulture);

		internal static string UpdateLeaderboard => ResourceManager.GetString("UpdateLeaderboard", resourceCulture);

		internal static string UpdateRaces => ResourceManager.GetString("UpdateRaces", resourceCulture);

		internal static string UploadGhost => ResourceManager.GetString("UploadGhost", resourceCulture);

		internal static string Yes => ResourceManager.GetString("Yes", resourceCulture);

		internal Strings()
		{
		}
	}
}
