using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;
using Soeed.GuildGeoGuesser.Feature.Shared.Controls;
using Soeed.GuildGeoGuesser.Settings.Enums;

namespace Soeed.GuildGeoGuesser.Settings.Services
{
	public class SettingService : IDisposable
	{
		public SettingEntry<bool> DebugInformationEnabled { get; }

		public SettingEntry<int> CornerIconPriority { get; }

		public SettingEntry<int> LastMotD { get; }

		public SettingEntry<string> HelpScreenVersion { get; }

		public SettingEntry<bool> SendHideGw2Ui { get; private set; }

		public SettingEntry<bool> SendHideBlishUi { get; private set; }

		public SettingEntry<bool> SendHideArcDpsUi { get; private set; }

		public SettingEntry<bool> SendHideNexusUi { get; private set; }

		public SettingEntry<bool> SignedSocialContract { get; private set; }

		public SettingEntry<KeyBinding> MainWindowToggle { get; private set; }

		public SettingEntry<KeyBinding> HideGw2Ui { get; private set; }

		public SettingEntry<KeyBinding> HideBlishUi { get; private set; }

		public SettingEntry<KeyBinding> HideArcDpsUi { get; private set; }

		public SettingEntry<KeyBinding> HideNexusUi { get; private set; }

		public SettingEntry<PaginationSizeEnum> PuzzlesPerPage { get; private set; }

		public SettingEntry<PuzzleTypeFilterEnum> PuzzleTypeFilter { get; private set; }

		public SettingEntry<CameraModeFilterEnum> CameraModeFilter { get; private set; }

		public SettingEntry<bool> ShowCameraModeFilter { get; private set; }

		public SettingEntry<PuzzleSortEnum> PuzzleSort { get; private set; }

		public SettingEntry<bool> ProtectedGuessButton { get; private set; }

		public SettingService(SettingCollection settings)
		{
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Expected O, but got Unknown
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Expected O, but got Unknown
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e9: Expected O, but got Unknown
			//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Expected O, but got Unknown
			//IL_044c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Expected O, but got Unknown
			DebugInformationEnabled = settings.DefineSetting<bool>("GGGDebugEnabled", false, (Func<string>)(() => "Debug mode"), (Func<string>)(() => "Enable debug menu in settings to view remote configuration values\nRequires Module Reload"));
			CornerIconPriority = settings.DefineSetting<int>("GGGCornerPriority", 300, (Func<string>)(() => "Top-left icon sort order"), (Func<string>)(() => "Left <----> Right"));
			SettingComplianceExtensions.SetRange(CornerIconPriority, 0, 1000);
			SignedSocialContract = settings.DefineSetting<bool>("GGGSocialContract", false, (Func<string>)(() => ""), (Func<string>)(() => ""));
			LastMotD = settings.DefineSetting<int>("GGGMotD", 0, (Func<string>)(() => ""), (Func<string>)(() => ""));
			HelpScreenVersion = settings.DefineSetting<string>("GGGHelpScreen", "0.0.0", (Func<string>)(() => ""), (Func<string>)(() => ""));
			MainWindowToggle = settings.DefineSetting<KeyBinding>("GGGWindowToggleKey", new KeyBinding(), (Func<string>)(() => "GeoGuess Window Keybind"), (Func<string>)(() => "optional keybind to open/close the module window"));
			SendHideGw2Ui = settings.DefineSetting<bool>("GGGGw2UiDefault", true, (Func<string>)(() => "Hide Guild Wars 2 game UI"), (Func<string>)(() => "Send the 'Hide GW2UI' keybind when creating a new puzzle"));
			SendHideBlishUi = settings.DefineSetting<bool>("GGGBlishUiDefault", false, (Func<string>)(() => "Hide BlishHUD"), (Func<string>)(() => "Send the 'Hide Blish' keybind when creating a new puzzle"));
			SendHideArcDpsUi = settings.DefineSetting<bool>("GGGArcDpsUiDefault", false, (Func<string>)(() => "Hide ArcDPS"), (Func<string>)(() => "Send the 'Hide ArdDPS` keybind when creating a new puzzle"));
			SendHideNexusUi = settings.DefineSetting<bool>("GGGNexusUiDefault", false, (Func<string>)(() => "Hide Nexus"), (Func<string>)(() => "Send the 'Hide Nexus` keybind when creating a new puzzle"));
			HideGw2Ui = settings.DefineSetting<KeyBinding>("GGGUiKey", new KeyBinding((ModifierKeys)5, (Keys)72), (Func<string>)(() => "hide GW2 user interface"), (Func<string>)(() => "Must match the keybind set in game, default Ctrl+Shift+H"));
			HideBlishUi = settings.DefineSetting<KeyBinding>("GGGBlishUiKey", new KeyBinding(), (Func<string>)(() => "hide BlishHUD"), (Func<string>)(() => "!!! Only set if you changed this from the default !!!\nMust match the keybind set in BlishHUD, default <<same as hide gw2 ui>>"));
			HideArcDpsUi = settings.DefineSetting<KeyBinding>("GGGArcDpsKey", new KeyBinding((ModifierKeys)6, (Keys)72), (Func<string>)(() => "hide ArcDPS windows"), (Func<string>)(() => "Must match the keybind set in arcdps, default Alt+Shift+H"));
			HideNexusUi = settings.DefineSetting<KeyBinding>("GGGNexusKey", new KeyBinding(), (Func<string>)(() => "hide Nexus windows"), (Func<string>)(() => "Must match the keybind set in Nexus, default ((unbound))"));
			PuzzlesPerPage = settings.DefineSetting<PaginationSizeEnum>("GGGPuzzlesPerPage", PaginationSizeEnum.Eight, (Func<string>)(() => "Puzzles per page"), (Func<string>)(() => "How many puzzles to show per page on the guild list"));
			PuzzleTypeFilter = settings.DefineSetting<PuzzleTypeFilterEnum>("GGGPuzzleTypeFilter", PuzzleTypeFilterEnum.ALL, (Func<string>)(() => "Status"), (Func<string>)(() => "Filter puzzles based on their completion status"));
			CameraModeFilter = settings.DefineSetting<CameraModeFilterEnum>("GGGCameraModeFilter", CameraModeFilterEnum.ALL, (Func<string>)(() => "Camera"), (Func<string>)(() => "Filter puzzles based on their camera mode, first person or third person"));
			ShowCameraModeFilter = settings.DefineSetting<bool>("GGGShowCameraModeFilter", false, (Func<string>)(() => "Show Camera Mode Filter"), (Func<string>)(() => "Show the camera mode filter in the puzzle list"));
			PuzzleSort = settings.DefineSetting<PuzzleSortEnum>("GGGPuzzleSort", PuzzleSortEnum.NEWEST_FIRST, (Func<string>)(() => "Sort"), (Func<string>)(() => "How puzzles are sorted in the guild list"));
			ProtectedGuessButton = settings.DefineSetting<bool>("GGGProtectedGuessButton", true, (Func<string>)(() => "Prevent accidental guesses"), (Func<string>)(() => "When enabled, hold Ctrl+Shift to activate the guess button and help prevent accidental guesses"));
		}

		public void Dispose()
		{
		}
	}
}
