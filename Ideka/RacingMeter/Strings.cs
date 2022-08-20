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

		internal static string BestLabel => ResourceManager.GetString("BestLabel", resourceCulture);

		internal static string ByInfo => ResourceManager.GetString("ByInfo", resourceCulture);

		internal static string Cancel => ResourceManager.GetString("Cancel", resourceCulture);

		internal static string ChangeToCurrent => ResourceManager.GetString("ChangeToCurrent", resourceCulture);

		internal static string Checkpoint => ResourceManager.GetString("Checkpoint", resourceCulture);

		internal static string CheckpointLabel => ResourceManager.GetString("CheckpointLabel", resourceCulture);

		internal static string ConfirmRaceDeletion => ResourceManager.GetString("ConfirmRaceDeletion", resourceCulture);

		internal static string CopyOf => ResourceManager.GetString("CopyOf", resourceCulture);

		internal static string CreatedBy => ResourceManager.GetString("CreatedBy", resourceCulture);

		internal static string CsvImportWarning => ResourceManager.GetString("CsvImportWarning", resourceCulture);

		internal static string CurrentGhostLabel => ResourceManager.GetString("CurrentGhostLabel", resourceCulture);

		internal static string CurrentRaceLabel => ResourceManager.GetString("CurrentRaceLabel", resourceCulture);

		internal static string Delete => ResourceManager.GetString("Delete", resourceCulture);

		internal static string DeleteRace => ResourceManager.GetString("DeleteRace", resourceCulture);

		internal static string Deselect => ResourceManager.GetString("Deselect", resourceCulture);

		internal static string EditingLabel => ResourceManager.GetString("EditingLabel", resourceCulture);

		internal static string ErrorFormat => ResourceManager.GetString("ErrorFormat", resourceCulture);

		internal static string ErrorGhostLoad => ResourceManager.GetString("ErrorGhostLoad", resourceCulture);

		internal static string ErrorGhostUpload => ResourceManager.GetString("ErrorGhostUpload", resourceCulture);

		internal static string ErrorLeaderboardLoad => ResourceManager.GetString("ErrorLeaderboardLoad", resourceCulture);

		internal static string ErrorLogIn => ResourceManager.GetString("ErrorLogIn", resourceCulture);

		internal static string ErrorRaceImport => ResourceManager.GetString("ErrorRaceImport", resourceCulture);

		internal static string ErrorRacesLoad => ResourceManager.GetString("ErrorRacesLoad", resourceCulture);

		internal static string ErrorRefreshToken => ResourceManager.GetString("ErrorRefreshToken", resourceCulture);

		internal static string ErrorVersionVerification => ResourceManager.GetString("ErrorVersionVerification", resourceCulture);

		internal static string ExceptionAccountVerification => ResourceManager.GetString("ExceptionAccountVerification", resourceCulture);

		internal static string ExceptionApiAccount => ResourceManager.GetString("ExceptionApiAccount", resourceCulture);

		internal static string ExceptionEmptyRaceName => ResourceManager.GetString("ExceptionEmptyRaceName", resourceCulture);

		internal static string ExceptionGeneric => ResourceManager.GetString("ExceptionGeneric", resourceCulture);

		internal static string ExceptionGw2AuthLogIn => ResourceManager.GetString("ExceptionGw2AuthLogIn", resourceCulture);

		internal static string ExceptionHttpListener => ResourceManager.GetString("ExceptionHttpListener", resourceCulture);

		internal static string ExceptionInvalidPointType => ResourceManager.GetString("ExceptionInvalidPointType", resourceCulture);

		internal static string ExceptionInvalidRaceMap => ResourceManager.GetString("ExceptionInvalidRaceMap", resourceCulture);

		internal static string ExceptionInvalidRaceName => ResourceManager.GetString("ExceptionInvalidRaceName", resourceCulture);

		internal static string ExceptionInvalidRaceType => ResourceManager.GetString("ExceptionInvalidRaceType", resourceCulture);

		internal static string ExceptionLogInExpired => ResourceManager.GetString("ExceptionLogInExpired", resourceCulture);

		internal static string ExceptionMinimumCheckpoints => ResourceManager.GetString("ExceptionMinimumCheckpoints", resourceCulture);

		internal static string ExceptionMoveResetPoint => ResourceManager.GetString("ExceptionMoveResetPoint", resourceCulture);

		internal static string ExceptionNoAccountApiPermission => ResourceManager.GetString("ExceptionNoAccountApiPermission", resourceCulture);

		internal static string ExceptionPointRadius => ResourceManager.GetString("ExceptionPointRadius", resourceCulture);

		internal static string ExceptionRemoveResetPoint => ResourceManager.GetString("ExceptionRemoveResetPoint", resourceCulture);

		internal static string FinishTimeLabel => ResourceManager.GetString("FinishTimeLabel", resourceCulture);

		internal static string Ghost => ResourceManager.GetString("Ghost", resourceCulture);

		internal static string GhostDescribeBy => ResourceManager.GetString("GhostDescribeBy", resourceCulture);

		internal static string GhostDescribeLocal => ResourceManager.GetString("GhostDescribeLocal", resourceCulture);

		internal static string GhostDescribeRemote => ResourceManager.GetString("GhostDescribeRemote", resourceCulture);

		internal static string GhostLabel => ResourceManager.GetString("GhostLabel", resourceCulture);

		internal static string GhostLocal => ResourceManager.GetString("GhostLocal", resourceCulture);

		internal static string GhostLocalRacerTooltip => ResourceManager.GetString("GhostLocalRacerTooltip", resourceCulture);

		internal static string GhostLocalUploadedTooltip => ResourceManager.GetString("GhostLocalUploadedTooltip", resourceCulture);

		internal static string GhostRacerLabel => ResourceManager.GetString("GhostRacerLabel", resourceCulture);

		internal static string GhostTimeLabel => ResourceManager.GetString("GhostTimeLabel", resourceCulture);

		internal static string GhostUploadedLabel => ResourceManager.GetString("GhostUploadedLabel", resourceCulture);

		internal static string ImportCsv => ResourceManager.GetString("ImportCsv", resourceCulture);

		internal static string Inches => ResourceManager.GetString("Inches", resourceCulture);

		internal static string InsertAfter => ResourceManager.GetString("InsertAfter", resourceCulture);

		internal static string InsertBefore => ResourceManager.GetString("InsertBefore", resourceCulture);

		internal static string KoFiButton => ResourceManager.GetString("KoFiButton", resourceCulture);

		internal static string LastLabel => ResourceManager.GetString("LastLabel", resourceCulture);

		internal static string Leaderboard => ResourceManager.GetString("Leaderboard", resourceCulture);

		internal static string LoadGhost => ResourceManager.GetString("LoadGhost", resourceCulture);

		internal static string Loading => ResourceManager.GetString("Loading", resourceCulture);

		internal static string LocalGhosts => ResourceManager.GetString("LocalGhosts", resourceCulture);

		internal static string LocalRaces => ResourceManager.GetString("LocalRaces", resourceCulture);

		internal static string LoggedIn => ResourceManager.GetString("LoggedIn", resourceCulture);

		internal static string LoggingIn => ResourceManager.GetString("LoggingIn", resourceCulture);

		internal static string LogIn => ResourceManager.GetString("LogIn", resourceCulture);

		internal static string LogInError => ResourceManager.GetString("LogInError", resourceCulture);

		internal static string LogInPrompt => ResourceManager.GetString("LogInPrompt", resourceCulture);

		internal static string LogInSuccess => ResourceManager.GetString("LogInSuccess", resourceCulture);

		internal static string LogOut => ResourceManager.GetString("LogOut", resourceCulture);

		internal static string Meters => ResourceManager.GetString("Meters", resourceCulture);

		internal static string NewRace => ResourceManager.GetString("NewRace", resourceCulture);

		internal static string NewRaceName => ResourceManager.GetString("NewRaceName", resourceCulture);

		internal static string Next => ResourceManager.GetString("Next", resourceCulture);

		internal static string None => ResourceManager.GetString("None", resourceCulture);

		internal static string Nothing => ResourceManager.GetString("Nothing", resourceCulture);

		internal static string NotifyGhostAlreadyBetter => ResourceManager.GetString("NotifyGhostAlreadyBetter", resourceCulture);

		internal static string NotifyLogInRequired => ResourceManager.GetString("NotifyLogInRequired", resourceCulture);

		internal static string NotifyNoRaceSelected => ResourceManager.GetString("NotifyNoRaceSelected", resourceCulture);

		internal static string NotifyNotEnoughCheckpoints => ResourceManager.GetString("NotifyNotEnoughCheckpoints", resourceCulture);

		internal static string NotifyOfflineMode => ResourceManager.GetString("NotifyOfflineMode", resourceCulture);

		internal static string NotifyRaceMapMismatch => ResourceManager.GetString("NotifyRaceMapMismatch", resourceCulture);

		internal static string NotifyRaceSaved => ResourceManager.GetString("NotifyRaceSaved", resourceCulture);

		internal static string NotifyRaceStartFailed => ResourceManager.GetString("NotifyRaceStartFailed", resourceCulture);

		internal static string PointDescription => ResourceManager.GetString("PointDescription", resourceCulture);

		internal static string PointType => ResourceManager.GetString("PointType", resourceCulture);

		internal static string PointTypeCheckpoint => ResourceManager.GetString("PointTypeCheckpoint", resourceCulture);

		internal static string PointTypeGuide => ResourceManager.GetString("PointTypeGuide", resourceCulture);

		internal static string PointTypeLoopStart => ResourceManager.GetString("PointTypeLoopStart", resourceCulture);

		internal static string PointTypeReset => ResourceManager.GetString("PointTypeReset", resourceCulture);

		internal static string Prev => ResourceManager.GetString("Prev", resourceCulture);

		internal static string Race => ResourceManager.GetString("Race", resourceCulture);

		internal static string RaceAuthorLabel => ResourceManager.GetString("RaceAuthorLabel", resourceCulture);

		internal static string RaceCheckpointsLabel => ResourceManager.GetString("RaceCheckpointsLabel", resourceCulture);

		internal static string RaceEditor => ResourceManager.GetString("RaceEditor", resourceCulture);

		internal static string RaceId => ResourceManager.GetString("RaceId", resourceCulture);

		internal static string RaceIdTooltip => ResourceManager.GetString("RaceIdTooltip", resourceCulture);

		internal static string RaceLengthLabel => ResourceManager.GetString("RaceLengthLabel", resourceCulture);

		internal static string RaceLocal => ResourceManager.GetString("RaceLocal", resourceCulture);

		internal static string RaceLocalAuthorTooltip => ResourceManager.GetString("RaceLocalAuthorTooltip", resourceCulture);

		internal static string RaceLocalLeaderboardTooltip => ResourceManager.GetString("RaceLocalLeaderboardTooltip", resourceCulture);

		internal static string RaceMap => ResourceManager.GetString("RaceMap", resourceCulture);

		internal static string RaceMapLabel => ResourceManager.GetString("RaceMapLabel", resourceCulture);

		internal static string RaceName => ResourceManager.GetString("RaceName", resourceCulture);

		internal static string RacePoints => ResourceManager.GetString("RacePoints", resourceCulture);

		internal static string RacePreview => ResourceManager.GetString("RacePreview", resourceCulture);

		internal static string Races => ResourceManager.GetString("Races", resourceCulture);

		internal static string RaceType => ResourceManager.GetString("RaceType", resourceCulture);

		internal static string RaceTypeCustom => ResourceManager.GetString("RaceTypeCustom", resourceCulture);

		internal static string RaceTypeLabel => ResourceManager.GetString("RaceTypeLabel", resourceCulture);

		internal static string RaceTypeOfficial => ResourceManager.GetString("RaceTypeOfficial", resourceCulture);

		internal static string RaceTypeUnknown => ResourceManager.GetString("RaceTypeUnknown", resourceCulture);

		internal static string RaceUpdatedLabel => ResourceManager.GetString("RaceUpdatedLabel", resourceCulture);

		internal static string RacingMeter => ResourceManager.GetString("RacingMeter", resourceCulture);

		internal static string Radius => ResourceManager.GetString("Radius", resourceCulture);

		internal static string Redo => ResourceManager.GetString("Redo", resourceCulture);

		internal static string ResetPoint => ResourceManager.GetString("ResetPoint", resourceCulture);

		internal static string RunRace => ResourceManager.GetString("RunRace", resourceCulture);

		internal static string Save => ResourceManager.GetString("Save", resourceCulture);

		internal static string SelectNearest => ResourceManager.GetString("SelectNearest", resourceCulture);

		internal static string ServerStatusYes => ResourceManager.GetString("ServerStatusYes", resourceCulture);

		internal static string SettingAutoLocalGhost => ResourceManager.GetString("SettingAutoLocalGhost", resourceCulture);

		internal static string SettingAutoLocalGhostText => ResourceManager.GetString("SettingAutoLocalGhostText", resourceCulture);

		internal static string SettingGriffonMeter => ResourceManager.GetString("SettingGriffonMeter", resourceCulture);

		internal static string SettingMaxGhostData => ResourceManager.GetString("SettingMaxGhostData", resourceCulture);

		internal static string SettingMaxGhostDataText => ResourceManager.GetString("SettingMaxGhostDataText", resourceCulture);

		internal static string SettingMumblePollingRate => ResourceManager.GetString("SettingMumblePollingRate", resourceCulture);

		internal static string SettingMumblePollingRateText => ResourceManager.GetString("SettingMumblePollingRateText", resourceCulture);

		internal static string SettingNormalizedOfficialCheckpoints => ResourceManager.GetString("SettingNormalizedOfficialCheckpoints", resourceCulture);

		internal static string SettingNormalizedOfficialCheckpointsText => ResourceManager.GetString("SettingNormalizedOfficialCheckpointsText", resourceCulture);

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

		internal static string SettingSkiffMeter => ResourceManager.GetString("SettingSkiffMeter", resourceCulture);

		internal static string SettingSkimmerMeter => ResourceManager.GetString("SettingSkimmerMeter", resourceCulture);

		internal static string SettingSpeedometerAnchorY => ResourceManager.GetString("SettingSpeedometerAnchorY", resourceCulture);

		internal static string SettingSpeedometerAnchorYText => ResourceManager.GetString("SettingSpeedometerAnchorYText", resourceCulture);

		internal static string SettingToggleDebug => ResourceManager.GetString("SettingToggleDebug", resourceCulture);

		internal static string SettingToggleDebugText => ResourceManager.GetString("SettingToggleDebugText", resourceCulture);

		internal static string SettingToggleSpeedometer => ResourceManager.GetString("SettingToggleSpeedometer", resourceCulture);

		internal static string SettingToggleSpeedometerText => ResourceManager.GetString("SettingToggleSpeedometerText", resourceCulture);

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

		internal static string Unavailable => ResourceManager.GetString("Unavailable", resourceCulture);

		internal static string Undo => ResourceManager.GetString("Undo", resourceCulture);

		internal static string UnloadGhost => ResourceManager.GetString("UnloadGhost", resourceCulture);

		internal static string UnloadRace => ResourceManager.GetString("UnloadRace", resourceCulture);

		internal static string UnsavedChanges => ResourceManager.GetString("UnsavedChanges", resourceCulture);

		internal static string UpdateLeaderboard => ResourceManager.GetString("UpdateLeaderboard", resourceCulture);

		internal static string UpdateRaces => ResourceManager.GetString("UpdateRaces", resourceCulture);

		internal static string UploadGhost => ResourceManager.GetString("UploadGhost", resourceCulture);

		internal Strings()
		{
		}
	}
}
